using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.DomainService;
using YK.PropertyMgr.RepositoryContract;

namespace YK.PropertyMgr.ApplicationService.Service
{
    public class BillDetailAppService
    {
        public IEnumerable<DictionaryModel> GetChargeTypeList()
        {
            var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
            return propertyService.GetDictionaryModels(PropertyEnumType.ChargeType.ToString());
        }

        public IEnumerable<object> GetBillStatusList()
        {
            List<object> dataList = new List<object>();
            dataList.Add(new { Code = 0, Name = "--请选择--" });
            dataList.Add(new { Code = 1, Name = "未付款" });
            dataList.Add(new { Code = 2, Name = "已付款" });
            dataList.Add(new { Code = 3, Name = "退款" });
            return dataList;
        }

        public IEnumerable<DictionaryModel> GetPayTypeList()
        {
            var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
            return propertyService.GetDictionaryModels(PropertyEnumType.PayType.ToString());
        }

        public IEnumerable<ChargeSubjectDTO> GetChargeSubjectList(int? DeptId, EDeptType? DeptType)
        {
            if (!DeptType.HasValue || DeptType != EDeptType.XiaoQu)
            {
                return new List<ChargeSubjectDTO>();
            }
            ChargeSubjectAppService cservice = new ChargeSubjectAppService();
            var dataList = cservice.GetChargeSubjectList(g => g.ComDeptId == DeptId && g.IsDel == false);
            //dataList.Insert(0, new ChargeSubjectDTO() { Name = "--请选择--" });
            return dataList;
        }

        public IEnumerable<TemplateModel> GetBillDetailTemplate(bool SettleAccount)
        {
            int i = 1;
            List<TemplateColumn> showColumns = new List<TemplateColumn>();
            showColumns.Add(new TemplateColumn() { ColumnName = "ResourcesName", ColumnDesc = "收费资源", Seq = i++, IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "BillDesc", ColumnDesc = "交易项", Seq = i++, IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "ChargeType", ColumnDesc = "收费类型", Seq = i++, DictId = PropertyEnumType.ChargeType.ToString(), IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "StartDate", ColumnDesc = "账单开始", Seq = i++, Type = "date", IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "EndDate", ColumnDesc = "账单结束", Seq = i++, Type = "date", IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "BillAmount", ColumnDesc = "账单金额", Seq = i++, IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "Amount", ColumnDesc = "收费金额", Seq = i++, IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "ChargeDate", ColumnDesc = "收费日期", Seq = i++, Type = "date", IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "GenerationDate", ColumnDesc = "生成时间", Seq = i++, Type = "date", IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "ReceiptNum", ColumnDesc = "票据号", Seq = i++, IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "CustomerName", ColumnDesc = "客户", Seq = i++, IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "OperatorName", ColumnDesc = "操作人", Seq = i++, IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "BillStatusName", ColumnDesc = "状态", Seq = i++, IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "PayType", ColumnDesc = "付款方式", Seq = i++, DictId = PropertyEnumType.PayType.ToString(), IsExport = true });
            if (SettleAccount)
            {
                showColumns.Add(new TemplateColumn() { ColumnName = "SerialNumber", ColumnDesc = "序列号", Seq = i++, IsExport = true });
                showColumns.Add(new TemplateColumn() { ColumnName = "AccountingStatus", ColumnDesc = "预结算状态", Seq = i++, DictId = PropertyEnumType.AccountingStatus.ToString(), IsExport = true });
            }
            showColumns.Add(new TemplateColumn() { ColumnName = "RefundReason", ColumnDesc = "退款原因", Seq = i++, IsExport = true });
            showColumns.Add(new TemplateColumn() { ColumnName = "Remark", ColumnDesc = "备注", Seq = i++, IsExport = true });

            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(BillDetailInfo), showColumns.ToArray());
            return template;
        }

        public IList<BillDetailInfo> GetBillDetailList(BillDetailSearchDTO searchDto, out int totalCount)
        {
            if (searchDto.DeptType != EDeptType.XiaoQu)
            {
                totalCount = 0;
                return new List<BillDetailInfo>();
            }

            Condition<BillDetailInfo> condition = new Condition<BillDetailInfo>(c => c.DeptId == searchDto.DeptId && c.BillAmount != 0);
            if (!string.IsNullOrEmpty(searchDto.ResourcesName))
            {
                condition = condition & new Condition<BillDetailInfo>(c => c.ResourcesName.Contains(searchDto.ResourcesName));
            }
            if (!string.IsNullOrEmpty(searchDto.ReceiptNum))
            {
                condition = condition & new Condition<BillDetailInfo>(c => c.ReceiptNum.Contains(searchDto.ReceiptNum));
            }
            if (!string.IsNullOrEmpty(searchDto.CustomerName))
            {
                condition = condition & new Condition<BillDetailInfo>(c => c.CustomerName.Contains(searchDto.CustomerName));
            }
            if (!string.IsNullOrEmpty(searchDto.OperatorName))
            {
                condition = condition & new Condition<BillDetailInfo>(c => c.OperatorName.Contains(searchDto.OperatorName));
            }
            if (searchDto.ChargeSubjectId.HasValue)
            {
                condition = condition & new Condition<BillDetailInfo>(c => c.ChargeSubjectId == searchDto.ChargeSubjectId);
            }
            if (searchDto.PayType.HasValue)
            {
                condition = condition & new Condition<BillDetailInfo>(c => c.PayType == searchDto.PayType);
            }
            if (searchDto.BillStatus.HasValue && searchDto.BillStatus != 0)
            {
                //如果是退款，查费用记录
                if (searchDto.BillStatus == (int)BillStatusEnum.Refunded)
                {
                    condition = condition & new Condition<BillDetailInfo>(c => c.ReceiptStatus == (int)ReceiptStatusEnum.Refunded);
                }
                else
                {
                    condition = condition & new Condition<BillDetailInfo>(c => c.BillStatus == searchDto.BillStatus);
                    //排除费用记录为退款的记录
                    condition = condition & new Condition<BillDetailInfo>(c => c.ReceiptStatus != (int)ReceiptStatusEnum.Refunded);
                }
            }
            if (searchDto.ChargeType.HasValue)
            {
                condition = condition & new Condition<BillDetailInfo>(c => c.ChargeType == searchDto.ChargeType);
            }
            if (searchDto.StartDate.HasValue)
            {
                condition = condition & new Condition<BillDetailInfo>(c => c.GenerationDate >= searchDto.StartDate);
            }
            if (searchDto.EndDate.HasValue)
            {
                searchDto.EndDate = searchDto.EndDate.Value.AddDays(1).AddSeconds(-1);
                condition = condition & new Condition<BillDetailInfo>(c => c.GenerationDate <= searchDto.EndDate);
            }

            string expressions = "GenerationDate desc, ChargeDate desc, ReceiptNum desc";
            ChargBillDomainService service = new ChargBillDomainService();
            var dataList = service.GetBillDetailListPage(condition.ExpressionBody, expressions, out totalCount, searchDto.PageStart, searchDto.PageSize);
            return dataList;
        }
    }
}
