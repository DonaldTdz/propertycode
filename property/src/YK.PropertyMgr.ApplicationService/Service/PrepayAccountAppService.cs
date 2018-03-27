using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.PresentationService;
using YK.FrameworkTools.ExcelService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.ApplicationService.Service;
using YK.PropertyMgr.CompositeAppService;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.DomainService;
using YK.PropertyMgr.RepositoryContract;

namespace YK.PropertyMgr.ApplicationService
{
    public partial class PrepayAccountAppService
    {


        #region  构建查询对象
        /// <summary>
        /// 构建查询对象
        /// </summary>
        /// <param name="queryParams"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>

        public PrepayAccountShowDTO GetSearchParms(DTParameterModel queryParams, out int pageIndex)
        {
            PrepayAccountShowDTO model = new PrepayAccountShowDTO();
            string searchParm = queryParams.CustomSearch;
            pageIndex = (queryParams.Start % queryParams.Length) + 1;
            Type type = model.GetType();
            List<SearchParm> searchParms = JsonConvert.DeserializeObject<List<SearchParm>>(queryParams.CustomSearch);
            foreach (var item in searchParms)
            {

                if (item.name == "DeptId")
                {
                    item.name = "ComDeptId";
                }

                PropertyInfo p = type.GetProperty(item.name);
                if (p != null)
                {
                    if (!string.IsNullOrEmpty(item.value.ToString()))
                    {
                        if (!p.PropertyType.IsGenericType)
                        {
                            /*非泛型*/
                            p.SetValue(model, string.IsNullOrEmpty(item.value) ? null : Convert.ChangeType(item.value, p.PropertyType), null);
                        }
                        else
                        {
                            /*泛型Nullable<>*/
                            Type genericTypeDefinition = p.PropertyType.GetGenericTypeDefinition();
                            if (genericTypeDefinition == typeof(Nullable<>))
                            {
                                p.SetValue(model, string.IsNullOrEmpty(item.value) ? null : Convert.ChangeType(item.value, Nullable.GetUnderlyingType(p.PropertyType)), null);
                            }
                        }
                    }
                }
            }
            return model;
        }

        #endregion

        #region 获取对象分页集合
        public IList<PrepayAccountShowDTO> Paging(int PageStart, int PageSize, PrepayAccountShowDTO showModel, string expressions, out int totalCount)
        {

            DeptAppService deptAppService = new DeptAppService();
            //SEC_DeptDTO deptinfo= deptAppService.GetDeptInfoById(showModel.ComDeptId.ToString());
            if (showModel.DeptType != EDeptType.XiaoQu)
            {
                totalCount = 0;
                return new List<PrepayAccountShowDTO>();
            }
            totalCount = 0;

            PrepayAccountDomainService prepayAccountService = new PrepayAccountDomainService();
            return prepayAccountService.GetPrepayAccountList(showModel, PageStart, PageSize, expressions, out totalCount);


        }
        #endregion

        #region 导入账账单余额数据
        public ImportResult ImportPrepayAccounts(string filePath, IEnumerable<TemplateModel> templateModels, int deptId, int Operator, string OperatorName)
        {

            PrepayAccountDomainService prepayAccountDomainService = new PrepayAccountDomainService();
            return prepayAccountDomainService.ImportPrepayAccounts(filePath, templateModels, deptId, Operator, OperatorName);


        }
        #endregion

        #region 预存账户管理

        /// <summary>
        /// 获取预存费转移列表
        /// </summary>
        public IList<PrepayAccountTransferDTO> GetPrepayAccountTransferList(int houseDeptId)
        {
            PrepayAccountDomainService prepayAccountDomainService = new PrepayAccountDomainService();
            return prepayAccountDomainService.GetPrepayAccountTransferList(houseDeptId);
        }

        /// <summary>
        /// 预存费转移
        /// </summary>
        public ResultModel PreAccountCostTransfer(PreAccountCostTransferDTO transfer)
        {
            return BalanceAppService.BalanceTransfer(transfer);
        }

        /// <summary>
        /// 获取操作记录模板
        /// </summary>
        public IEnumerable<TemplateModel> GetOperationRecordTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "Desc", ColumnDesc = "内容", Seq = i++ },
                new TemplateColumn(){ ColumnName = "Operator", ColumnDesc = "操作人", Seq = i++ },
                new TemplateColumn(){ ColumnName = "OperationTimeDesc", ColumnDesc = "操作时间", Seq = i++ },
                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++}
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(PrepayAccountLogDTO), showColumns);
            return template;
        }

        /// <summary>
        /// 获取操作记录列表
        /// </summary>
        public IList<PrepayAccountLogDTO> GetOperationRecordList(PreAccountORSearchDTO searchDto, out int outCount)
        {
            if (searchDto.DeptType == EDeptType.FangWu || searchDto.DeptType == EDeptType.XiaoQu || searchDto.DeptType == EDeptType.LouYu)
            {
                //条件
                Condition<PrepayAccountLog> condition = new Condition<PrepayAccountLog>(c => true);
                //以房屋查
                if (searchDto.DeptType == EDeptType.FangWu)
                {
                    condition = condition & new Condition<PrepayAccountLog>(c => c.HouseDeptId == searchDto.DeptId);
                }
                else if (searchDto.DeptType == EDeptType.LouYu)
                {
                    var louyu = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptListBySingleBuildDeptId(searchDto.DeptId);
                    if (louyu != null)
                    {
                        condition = condition & new Condition<PrepayAccountLog>(c => c.ComDeptId == louyu.ComDeptId);
                        var louyuCode = louyu.BuildingCode + "-";
                       condition = condition & new Condition<PrepayAccountLog>(c => c.Desc.StartsWith(louyuCode));
                    }
                }
                else//以小区查
                {
                    condition = condition & new Condition<PrepayAccountLog>(c => c.ComDeptId == searchDto.DeptId);
                }

                if (!string.IsNullOrEmpty(searchDto.Desc))
                {
                    condition = condition & new Condition<PrepayAccountLog>(c => c.Desc.Contains(searchDto.Desc));
                }
                if (!string.IsNullOrEmpty(searchDto.Operator))
                {
                    condition = condition & new Condition<PrepayAccountLog>(c => c.Operator.Contains(searchDto.Operator));
                }
                if (searchDto.BeginDate.HasValue)
                {
                    condition = condition & new Condition<PrepayAccountLog>(c => c.OperationTime >= searchDto.BeginDate);
                }
                if (searchDto.EndDate.HasValue)
                {
                    searchDto.EndDate = searchDto.EndDate.Value.AddDays(1).AddSeconds(-1);
                    condition = condition & new Condition<PrepayAccountLog>(c => c.OperationTime <= searchDto.EndDate);
                }

                //默认排序
                string expressions = "OperationTime desc";
                PrepayAccountLogDomainService service = new PrepayAccountLogDomainService();
                var domainList = service.Paging(searchDto.PageIndex, searchDto.PageSize, condition.ExpressionBody, expressions, out outCount);
                var dtoList = PrepayAccountLogMappers.ChangePrepayAccountLogToDTOs(domainList);

                //返回数据
                return dtoList.ToList();
            }
            else
            {
                outCount = 0;
                return new List<PrepayAccountLogDTO>();
            }
        }

        /// <summary>
        /// 获取批量抵扣模板
        /// </summary>
        public IEnumerable<TemplateModel> GetBatchDeductionTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "ResourceName", ColumnDesc = "收费资源", Seq = i++ },
                new TemplateColumn(){ ColumnName = "SubjectName", ColumnDesc = "收费项目", Seq = i++ },
                new TemplateColumn(){ ColumnName = "Amount", ColumnDesc = "欠费金额", Seq = i++ },
                new TemplateColumn(){ ColumnName = "PreAmount", ColumnDesc = "收费项目预存", Seq = i++ },
                new TemplateColumn(){ ColumnName = "CommonPreAmount", ColumnDesc = "全部收费项目预存", Seq = i++ }
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(BatchDeductionBillDTO), showColumns);
            return template;
        }

        /// <summary>
        /// 获取批量抵扣列表
        /// </summary>
        public IList<BatchDeductionBillSumDTO> GetBatchDeductionList(BatchDeductionSearchDTO searchDto, out int outCount)
        {
            if (searchDto.DeptType == EDeptType.FangWu || searchDto.DeptType == EDeptType.XiaoQu || searchDto.DeptType == EDeptType.LouYu)
            {
                //条件
                Condition<BatchDeductionBillDTO> condition = new Condition<BatchDeductionBillDTO>(c => true);
                //以房屋查
                if (searchDto.DeptType == EDeptType.FangWu)
                {
                    condition = condition & new Condition<BatchDeductionBillDTO>(c => c.HouseDeptId == searchDto.DeptId);
                }
                else if(searchDto.DeptType == EDeptType.LouYu)
                {
                    var louyu = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptListBySingleBuildDeptId(searchDto.DeptId);
                    if (louyu != null)
                    {
                        condition = condition & new Condition<BatchDeductionBillDTO>(c => c.ComDeptId == louyu.ComDeptId);
                        var louyuCode = louyu.BuildingCode + "-";
                        condition = condition & new Condition<BatchDeductionBillDTO>(c => c.HouseDoorNo.StartsWith(louyuCode));
                    }
                }
                else//以小区查
                {
                    condition = condition & new Condition<BatchDeductionBillDTO>(c => c.ComDeptId == searchDto.DeptId);
                }

                //默认排序
                //string expressions = "ResourceName, SubjectName";
                PrepayAccountDomainService service = new PrepayAccountDomainService();
                var domainList = service.GetBatchDeductionBillList(searchDto.PageStart, searchDto.PageSize, condition.ExpressionBody, out outCount);

                //返回数据
                return domainList.ToList();
            }
            else
            {
                outCount = 0;
                return new List<BatchDeductionBillSumDTO>();
            }
        }

        /// <summary>
        /// 批量抵扣操作
        /// </summary>
        public ResultModel PreCostBatchDeduction(string[] houseDeptSubjectIds, int Operator, string OperatorName)
        {
            if (houseDeptSubjectIds == null || houseDeptSubjectIds.Count() <= 0)
            {
                return new ResultModel() { ErrorCode = "701", IsSuccess = false, Msg = "选择房屋不能为空" };
            }
            ResultModel result = new ResultModel();
            PrepayAccountDomainService service = new PrepayAccountDomainService();
            bool bo = service.PreCostBatchDeduction(houseDeptSubjectIds, Operator, OperatorName);
            result.IsSuccess = bo;
            if (bo)
            {
                result.ErrorCode = "0";
                result.Msg = "批量抵扣操作成功";
            }
            else
            {
                result.ErrorCode = "901";
                result.Msg = "操作异常，请联系管理员";
            }

            return result;
        }
        #endregion

    }
}
