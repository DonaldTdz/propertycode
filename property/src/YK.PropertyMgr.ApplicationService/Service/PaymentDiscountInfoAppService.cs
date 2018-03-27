using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using YK.PropertyMgr.CompositeAppService;
using YK.PropertyMgr.ApplicationService;
using YK.BackgroundMgr.DomainInterface;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.DomainService;
using YK.PropertyMgr.CompositeDomainService;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.ParkingSys.DomainEntity;
using System.Data;

namespace YK.PropertyMgr.ApplicationService
{
    public partial class PaymentDiscountInfoAppService
    {
        public List<ReportPayDisInf> GetPaymentDiscountInfoReport(ReportPayDisInfSearchDTO searchDto, out int totalCount, out decimal outSum)
        {
            Condition<PaymentDiscountInfo> condition = GetCondition(searchDto);

            string expressions = "CreateTime desc";
            var domainPaymentDiscountInfoReport = PaymentDiscountInfoService
                .GetPaymentDiscountInfoReport(condition.ExpressionBody, expressions, out totalCount, searchDto.PageStart, searchDto.PageSize, out outSum);

            return domainPaymentDiscountInfoReport;
        }
        /// <summary>
        /// 拼接查询条件
        /// </summary>
        /// <param name="searchDto">查询条件</param>
        /// <returns></returns>
        Condition<PaymentDiscountInfo> GetCondition(ReportPayDisInfSearchDTO searchDto)
        {
            Condition<PaymentDiscountInfo> condition = new Condition<PaymentDiscountInfo>(c => c.ChargeRecord.ComDeptId == searchDto.ComDeptId && c.IsDel != true);
            if (!string.IsNullOrEmpty(searchDto.ResourceName))
            {
                condition = condition & new Condition<PaymentDiscountInfo>(c => c.ChargeRecord.ResourcesNames.Contains(searchDto.ResourceName));
            }
            if (!string.IsNullOrEmpty(searchDto.OwnerName))
            {
                condition = condition & new Condition<PaymentDiscountInfo>(c => c.CustomerName.Contains(searchDto.OwnerName));
            }
            if (!string.IsNullOrEmpty(searchDto.ReceiptNum))
            {
                condition = condition & new Condition<PaymentDiscountInfo>(c => c.ChargeRecord.Receipt.Number.Contains(searchDto.ReceiptNum));
            }
            if (!string.IsNullOrEmpty(searchDto.DiscountDesc))
            {
                condition = condition & new Condition<PaymentDiscountInfo>(c => c.DiscountDesc.Contains(searchDto.DiscountDesc));
            }
            if (searchDto.Status > 0)
            {
                condition = condition & new Condition<PaymentDiscountInfo>(c => c.Status == searchDto.Status);
            }
            if (searchDto.BeginDate.HasValue)
            {
                condition = condition & new Condition<PaymentDiscountInfo>(c => c.CreateTime >= searchDto.BeginDate);
            }
            if (searchDto.EndDate.HasValue)
            {
                searchDto.EndDate = searchDto.EndDate.Value.AddDays(1).AddSeconds(-1);
                condition = condition & new Condition<PaymentDiscountInfo>(c => c.CreateTime <= searchDto.EndDate);
            }
            if (searchDto.LouyuIdStr != null && searchDto.LouyuIdStr.Length > 0)
            {

                var LouyuList = searchDto.LouyuIdStr.Split(',').ToList().ConvertAll(i => int.Parse(i));
                var bulidList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetBuildsInfoByBuildDeptId(LouyuList);
                string BuildCode = bulidList[0].Building_code;

                Condition<PaymentDiscountInfo> condition_bill_OR = new Condition<PaymentDiscountInfo>(o => o.ChargeRecord.HouseDeptNos.StartsWith(BuildCode));
                foreach (var c in bulidList)
                {
                    condition_bill_OR = condition_bill_OR | new Condition<PaymentDiscountInfo>(o => o.ChargeRecord.HouseDeptNos.StartsWith(c.Building_code));

                }
                condition = condition & condition_bill_OR;
            }


            return condition;
        }
        /// <summary>
        /// 优惠明细表列
        /// </summary>
        /// <param name="IsExport">是否导出</param>
        /// <returns></returns>
        public IEnumerable<TemplateModel> GetPayDisInfRepListTemplate(bool IsExport = false)
        {
            int i = 1;
            List<TemplateColumn> showColumns = new List<TemplateColumn>();

            showColumns.Add(new TemplateColumn() { ColumnName = "ResourcesNames", ColumnDesc = "资源", Seq = i++, IsExport = IsExport });
            showColumns.Add(new TemplateColumn() { ColumnName = "CustomerName", ColumnDesc = "业主姓名", Seq = i++, IsExport = IsExport });
            showColumns.Add(new TemplateColumn() { ColumnName = "Number", ColumnDesc = "票据号", Seq = i++, IsExport = IsExport });
            showColumns.Add(new TemplateColumn() { ColumnName = "DiscountType", ColumnDesc = "优惠类型", Seq = i++, IsExport = IsExport, DictId = PropertyEnumType.DiscountType.ToString() });
            showColumns.Add(new TemplateColumn() { ColumnName = "DiscountDesc", ColumnDesc = "优惠名称", Seq = i++, IsExport = IsExport });
            showColumns.Add(new TemplateColumn() { ColumnName = "CreateTimeFormat", ColumnDesc = "使用日期", Seq = i++, IsExport = IsExport, Type = "date" });
            showColumns.Add(new TemplateColumn() { ColumnName = "DiscountAmount", ColumnDesc = "优惠金额", Seq = i++, IsExport = IsExport, Type= "Decimal" });
            showColumns.Add(new TemplateColumn() { ColumnName = "Status", ColumnDesc = "状态", Seq = i++, IsExport = IsExport, DictId = PropertyEnumType.DiscountStatus.ToString() });

            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ReportPayDisInf), showColumns.ToArray());
            return template;
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="exportDatas"></param>
        /// <param name="templateModels"></param>
        /// <returns></returns>
        public ReportArrearsExportModel PayDisInfRepExport(ReportPayDisInfSearchDTO search)
        {
            ReportArrearsExportModel exportModel = new ReportArrearsExportModel();
            Condition<PaymentDiscountInfo> condition = GetCondition(search);
            string expressions = "CreateTime desc";
            var dataList = PaymentDiscountInfoService.GetPaymentDiscountInfoReport(condition.ExpressionBody, expressions);

            //返回数据
            DataTable dt = new DataTable();
            var tempModel = GetPayDisInfRepListTemplate(true);
            foreach (var tm in tempModel)
            {
                if (tm.IsExport)
                {
                    switch (tm.Type)
                    {
                        case "Decimal":
                            dt.Columns.Add(tm.EnName, typeof(decimal));
                            break;
                        default:
                            dt.Columns.Add(tm.EnName, typeof(string));
                            break;
                    }
                }
            }
            decimal sum = 0;
            foreach (var item in dataList)
            {
                //赋值
                DataRow dr = dt.NewRow();
                var Properties = typeof(ReportPayDisInf).GetProperties();
                foreach (DataColumn dc in dt.Columns)
                {
                    var propertie = Properties.FirstOrDefault(o => o.Name == dc.ColumnName);
                    object value = null;
                    if (propertie != null)
                        value= propertie.GetValue(item);

                    if (value != null)
                    {
                        TemplateModel tmpMod = tempModel.FirstOrDefault(o => o.EnName == dc.ColumnName);
                        if (tmpMod != null && tmpMod.DictId != "-1" && tmpMod.DictionaryModels != null)
                        {
                            DictionaryModel dm = (DictionaryModel)tmpMod.DictionaryModels.FirstOrDefault(o => o.Code == value.ToString());
                            value = dm == null ? value : dm.CnName;
                        }
                    }

                    dr[dc.ColumnName] = value;
                }
                dt.Rows.Add(dr);
                sum += (item.DiscountAmount ?? 0);
            }
            
            DataRow drsum = dt.NewRow();
            drsum[0] = "合计";
            drsum["DiscountAmount"] = sum;
            
            dt.Rows.Add(drsum);

            exportModel.ExportData = dt;
            exportModel.TemPlateList = tempModel;

            return exportModel;
        }
    }
}
