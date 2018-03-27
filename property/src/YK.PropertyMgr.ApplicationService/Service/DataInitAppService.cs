using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.Crosscuting;
using YK.BackgroundMgr.PresentationService;
using YK.FrameworkTools.ExcelService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.CompositeAppService;

namespace YK.PropertyMgr.ApplicationService.Service
{
    public class DataInitAppService
    {
        #region 欠费导入模板

        public IEnumerable<TemplateModel> GetArrearageListTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "ResourceTypeName", ColumnDesc = "资源类型", Seq = i++, IsExport = true, IsRequred = true},
                new TemplateColumn(){ ColumnName = "ResourceNo", ColumnDesc = "资源", Seq = i++, IsExport = true, IsRequred = true},
                new TemplateColumn(){ ColumnName = "SubjectName", ColumnDesc = "收费项目", Seq = i++, IsExport = true, IsRequred = true},
                new TemplateColumn(){ ColumnName = "BeginDate", ColumnDesc = "开始日期", Seq = i++, IsExport = true, IsRequred = true},
                new TemplateColumn(){ ColumnName = "EndDate", ColumnDesc = "结束日期", Seq = i++, IsExport = true, IsRequred = true},
                new TemplateColumn(){ ColumnName = "Amount", ColumnDesc = "欠费金额", Seq = i++, IsExport = true, IsRequred = true},
                new TemplateColumn(){ ColumnName = "IsDeveloper", ColumnDesc = "开发商代缴", Seq = i++, IsExport = true},
                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++, IsExport = true}
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ArrearageInfo), showColumns);
            return template;
        }

        private CustomerValidateResult ValidateHouseColumn(DataRow validateRow, DataTable importTable, DataTable successTable)
        {
            CustomerValidateResult customerValidateResult = new CustomerValidateResult();
            DateTime dt;
            if (!DateTime.TryParse(validateRow["开始日期"].ToString(), out dt))
            {
                customerValidateResult.IsSuccess = false;
                customerValidateResult.ErrorMsg = "开始日期格式错误";
                return customerValidateResult;
            }
            if (!DateTime.TryParse(validateRow["结束日期"].ToString(), out dt))
            {
                customerValidateResult.IsSuccess = false;
                customerValidateResult.ErrorMsg = "结束日期格式错误";
                return customerValidateResult;
            }
            decimal amount;
            if (!decimal.TryParse(validateRow["欠费金额"].ToString(), out amount))
            {
                customerValidateResult.IsSuccess = false;
                customerValidateResult.ErrorMsg = "欠费金额格式错误";
                return customerValidateResult;
            }
            DateTime bdt = DateTime.Parse(validateRow["开始日期"].ToString());
            DateTime edt = DateTime.Parse(validateRow["结束日期"].ToString());
            if (bdt > edt)
            {
                customerValidateResult.IsSuccess = false;
                customerValidateResult.ErrorMsg = "开始日期不能大于结束日期";
                return customerValidateResult;
            }
            customerValidateResult.IsSuccess = true;
            return customerValidateResult;
        }

        public ResultModel ImportArrearage(string filePath, IEnumerable<TemplateModel> templateModels, int ComDeptId, int Operator, string OperatorName)
        {
            var importResult = ExcelHelper.ImportFromExcels(filePath, templateModels, this.ValidateHouseColumn, null, false);
            IList<ArrearageInfo> ArrearageInfoList = new List<ArrearageInfo>();

            foreach (DataRow successRow in importResult.SuccessTable.Rows)
            {
                ArrearageInfo item = new ArrearageInfo()
                {
                    Amount = decimal.Parse(successRow["Amount"].ToString()),
                    BeginDate = DateTime.Parse(successRow["BeginDate"].ToString()),
                    EndDate = DateTime.Parse(successRow["EndDate"].ToString()),
                    Remark = successRow["Remark"].ToString(),
                    ResourceNo = successRow["ResourceNo"].ToString(),
                    ResourceTypeName = successRow["ResourceTypeName"].ToString(),
                    SubjectName = successRow["SubjectName"].ToString(),
                    IsDeveloper = successRow["IsDeveloper"].ToString(),
                    RowNum = int.Parse(successRow["行号"].ToString())
                };
                ArrearageInfoList.Add(item);
            }
            int totalCount = importResult.SuccessTable.Rows.Count + importResult.ErrorTable.Rows.Count;
            ResultModel result = DataInitCompositeAppService.ImportArrearage(ComDeptId, ArrearageInfoList, Operator, OperatorName);
            //导入逻辑错误
            if (!result.IsSuccess)
            {
                if (result.Data != null)
                {
                    IList<ArrearageInfo> errorList = result.Data as IList<ArrearageInfo>;
                    foreach (var item in errorList)
                    {
                        DataRow errorRow = importResult.ErrorTable.NewRow();
                        errorRow["欠费金额"] = item.Amount;
                        errorRow["开始日期"] = item.BeginDate;
                        errorRow["结束日期"] = item.EndDate;
                        errorRow["备注"] = item.Remark;
                        errorRow["资源"] = item.ResourceNo;
                        errorRow["资源类型"] = item.ResourceTypeName;
                        errorRow["收费项目"] = item.SubjectName;
                        errorRow["开发商代缴"] = item.IsDeveloper;
                        errorRow["错误提示"] = item.ErrorMsg;
                        errorRow["错误行号"] = item.RowNum;
                        importResult.ErrorTable.Rows.Add(errorRow);
                    }
                }          
                string strError = "统计：导入" + totalCount + "条数据，其中导入成功" + (totalCount - importResult.ErrorTable.Rows.Count) + "条数据，导入失败" + importResult.ErrorTable.Rows.Count + "条数据，请修改之后重新导入！";
                result.Msg = strError;
                var totalErrorRow = importResult.ErrorTable.NewRow();
                totalErrorRow["错误行号"] = result.Msg;
                importResult.ErrorTable.Rows.Add(totalErrorRow);
                result.Data = importResult.ErrorTable;
            }
            else
            {
                //导入基本验证有错误
                if (importResult.ErrorTable.Rows.Count > 0)
                {
                    result.IsSuccess = false;
                    result.Msg = importResult.ErrorMsg;
                    var totalErrorRow = importResult.ErrorTable.NewRow();
                    totalErrorRow["错误行号"] = result.Msg;
                    importResult.ErrorTable.Rows.Add(totalErrorRow);
                    result.Data = importResult.ErrorTable;
                }
            }
            return result;
        }

        #endregion

        #region 历史缴费记录导入

        public IEnumerable<TemplateModel> GetHistoryCostListTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "ResourceTypeName", ColumnDesc = "资源类型", Seq = i++, IsExport = true, IsRequred = true},
                new TemplateColumn(){ ColumnName = "ResourceNo", ColumnDesc = "资源", Seq = i++, IsExport = true, IsRequred = true},
                new TemplateColumn(){ ColumnName = "SubjectName", ColumnDesc = "收费项目", Seq = i++, IsExport = true, IsRequred = true},
                new TemplateColumn(){ ColumnName = "ChargeTypeName", ColumnDesc = "收费类型", Seq = i++, IsExport = true, IsRequred = true},
                new TemplateColumn(){ ColumnName = "BeginDate", ColumnDesc = "开始日期", Seq = i++, IsExport = true, IsRequred = true},
                new TemplateColumn(){ ColumnName = "EndDate", ColumnDesc = "结束日期", Seq = i++, IsExport = true, IsRequred = true},
                new TemplateColumn(){ ColumnName = "PayDate", ColumnDesc = "交易时间", Seq = i++, IsExport = true, IsRequred = true},
                new TemplateColumn(){ ColumnName = "Amount", ColumnDesc = "交易金额", Seq = i++, IsExport = true, IsRequred = true},
                new TemplateColumn(){ ColumnName = "ReceiptNo", ColumnDesc = "票据号", Seq = i++, IsExport = true },
                new TemplateColumn(){ ColumnName = "CustomerName", ColumnDesc = "客户", Seq = i++, IsExport = true },
                new TemplateColumn(){ ColumnName = "OperatorName", ColumnDesc = "操作人", Seq = i++, IsExport = true, IsRequred = true},
                new TemplateColumn(){ ColumnName = "PayTypeName", ColumnDesc = "付款方式", Seq = i++, IsExport = true, IsRequred = true},
                new TemplateColumn(){ ColumnName = "IsDeveloper", ColumnDesc = "开发商代缴", Seq = i++, IsExport = true},
                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++, IsExport = true}
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(HistoryCostInfo), showColumns);
            return template;
        }

        private CustomerValidateResult ValidateHistoryCostColumn(DataRow validateRow, DataTable importTable, DataTable successTable)
        {
            CustomerValidateResult customerValidateResult = new CustomerValidateResult();
            DateTime dt;
            if (!DateTime.TryParse(validateRow["开始日期"].ToString(), out dt))
            {
                customerValidateResult.IsSuccess = false;
                customerValidateResult.ErrorMsg = "开始日期格式错误";
                return customerValidateResult;
            }
            if (!DateTime.TryParse(validateRow["结束日期"].ToString(), out dt))
            {
                customerValidateResult.IsSuccess = false;
                customerValidateResult.ErrorMsg = "结束日期格式错误";
                return customerValidateResult;
            }
            if (!DateTime.TryParse(validateRow["交易时间"].ToString(), out dt))
            {
                customerValidateResult.IsSuccess = false;
                customerValidateResult.ErrorMsg = "交易时间格式错误";
                return customerValidateResult;
            }
            decimal amount;
            if (!decimal.TryParse(validateRow["交易金额"].ToString(), out amount))
            {
                customerValidateResult.IsSuccess = false;
                customerValidateResult.ErrorMsg = "交易金额格式错误";
                return customerValidateResult;
            }
            DateTime bdt = DateTime.Parse(validateRow["开始日期"].ToString());
            DateTime edt = DateTime.Parse(validateRow["结束日期"].ToString());
            if (bdt > edt)
            {
                customerValidateResult.IsSuccess = false;
                customerValidateResult.ErrorMsg = "开始日期不能大于结束日期";
                return customerValidateResult;
            }
            customerValidateResult.IsSuccess = true;
            return customerValidateResult;
        }

        public ResultModel ImportHistoryCost(string filePath, IEnumerable<TemplateModel> templateModels, int ComDeptId, int Operator, string OperatorName)
        {
            var importResult = ExcelHelper.ImportFromExcels(filePath, templateModels, this.ValidateHistoryCostColumn, new Dictionary<string, object>(), false);
            IList<HistoryCostInfo> HistoryCostInfoList = new List<HistoryCostInfo>();

            foreach (DataRow successRow in importResult.SuccessTable.Rows)
            {
                HistoryCostInfo item = new HistoryCostInfo()
                {
                    Amount = decimal.Parse(successRow["Amount"].ToString()),
                    BeginDate = DateTime.Parse(successRow["BeginDate"].ToString()),
                    EndDate = DateTime.Parse(successRow["EndDate"].ToString()),
                    Remark = successRow["Remark"].ToString(),
                    ResourceNo = successRow["ResourceNo"].ToString(),
                    ResourceTypeName = successRow["ResourceTypeName"].ToString(),
                    SubjectName = successRow["SubjectName"].ToString(),
                    IsDeveloper = successRow["IsDeveloper"].ToString(),
                    ChargeTypeName = successRow["ChargeTypeName"].ToString(),
                    PayDate = DateTime.Parse(successRow["PayDate"].ToString()),
                    ReceiptNo = successRow["ReceiptNo"].ToString(),
                    CustomerName = successRow["CustomerName"].ToString(),
                    OperatorName = successRow["OperatorName"].ToString(),
                    PayTypeName = successRow["PayTypeName"].ToString(),
                    RowNum = int.Parse(successRow["行号"].ToString())
                };
                HistoryCostInfoList.Add(item);
            }
            int totalCount = importResult.SuccessTable.Rows.Count + importResult.ErrorTable.Rows.Count;
            ResultModel result = DataInitCompositeAppService.ImportHistoryCost(ComDeptId, HistoryCostInfoList, Operator, OperatorName);
            //导入逻辑错误
            if (!result.IsSuccess)
            {
                if (result.Data != null)
                {
                    IList<HistoryCostInfo> errorList = result.Data as IList<HistoryCostInfo>;
                    foreach (var item in errorList)
                    {
                        DataRow errorRow = importResult.ErrorTable.NewRow();
                        errorRow["交易金额"] = item.Amount;
                        errorRow["开始日期"] = item.BeginDate;
                        errorRow["结束日期"] = item.EndDate;
                        errorRow["备注"] = item.Remark;
                        errorRow["资源"] = item.ResourceNo;
                        errorRow["资源类型"] = item.ResourceTypeName;
                        errorRow["收费项目"] = item.SubjectName;
                        errorRow["开发商代缴"] = item.IsDeveloper;
                        errorRow["收费类型"] = item.ChargeTypeName;
                        errorRow["交易时间"] = item.PayDate;
                        errorRow["票据号"] = item.ReceiptNo;
                        errorRow["客户"] = item.CustomerName;
                        errorRow["操作人"] = item.OperatorName;
                        errorRow["付款方式"] = item.PayTypeName;
                        errorRow["错误提示"] = item.ErrorMsg;
                        errorRow["错误行号"] = item.RowNum;
                        importResult.ErrorTable.Rows.Add(errorRow);
                    }
                }
                
                string strError = "统计：导入" + totalCount + "条数据，其中导入成功" + (totalCount - importResult.ErrorTable.Rows.Count) + "条数据，导入失败" + importResult.ErrorTable.Rows.Count + "条数据，请修改之后重新导入！";
                result.Msg = strError;
                var totalErrorRow = importResult.ErrorTable.NewRow();
                totalErrorRow["错误行号"] = result.Msg;
                importResult.ErrorTable.Rows.Add(totalErrorRow);
                result.Data = importResult.ErrorTable;
            }
            else
            {
                //导入基本验证有错误
                if (importResult.ErrorTable.Rows.Count > 0)
                {
                    result.IsSuccess = false;
                    result.Msg = importResult.ErrorMsg;
                    var totalErrorRow = importResult.ErrorTable.NewRow();
                    totalErrorRow["错误行号"] = result.Msg;
                    importResult.ErrorTable.Rows.Add(totalErrorRow);
                    result.Data = importResult.ErrorTable;
                }
            }
            return result;
        }

        #endregion

    }
}
