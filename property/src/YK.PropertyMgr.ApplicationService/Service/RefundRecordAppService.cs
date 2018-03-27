using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;

namespace YK.PropertyMgr.ApplicationService
{
    public partial class RefundRecordAppService
    {
        public IEnumerable<TemplateModel> GetRefundRecordViewTemplate()
        {
            int i = 1;
            //RefundRecordDTO dd = new RefundRecordDTO();
            TemplateColumn[] showColumns = new TemplateColumn[] 
            {
                new TemplateColumn(){ ColumnName = "HouseDoorNo", ColumnDesc = "房间", Seq = i++, ElementType="TextBox"},
                new TemplateColumn(){ ColumnName = "Customer", ColumnDesc = "客户", Seq = i++, ElementType="TextBox", IsRequred = true, MaxLength = 50, ValidateType="stringLength"},
                new TemplateColumn(){ ColumnName = "ReceiptNum", ColumnDesc = "票据号", Seq = i++, ElementType="TextBox"},
                new TemplateColumn(){ ColumnName = "Amount", ColumnDesc = "退款金额", Seq = i++,ElementType="TextBox"},
                new TemplateColumn(){ ColumnName = "PayType", ColumnDesc = "付款方式", Seq = i++, DictId = PropertyEnumType.PayType.ToString(), IsRequred = true},
                new TemplateColumn(){ ColumnName = "Reason", ColumnDesc = "退款原因", Seq = i++, ElementType="TextArea", IsRequred = true, MaxLength = 200, ValidateType="stringLength"}
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(RefundRecordDTO), showColumns);
            var item = template.Where(t => t.Field == "PayType").FirstOrDefault();
            if (item != null)
            {
                //支付方式暂时不支持内部转账
                item.DictionaryModels = item.DictionaryModels
                    .Where(d => int.Parse(d.Code) != PayTypeEnum.InternalTransfer.GetHashCode()
                    && int.Parse(d.Code) != PayTypeEnum.Wallet.GetHashCode()
                    && int.Parse(d.Code) != PayTypeEnum.OneNetcom.GetHashCode()).ToList();//添加屏蔽一网通
            }
            
            return template;
        }


        public IEnumerable<TemplateModel> GetForegiRefundRecordViewTemplate()
        {
            int i = 1;
            //RefundRecordDTO dd = new RefundRecordDTO();
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "Customer", ColumnDesc = "客户", Seq = i++, ElementType="TextBox"},
                new TemplateColumn(){ ColumnName = "ReceiptNum", ColumnDesc = "票据号", Seq = i++, ElementType="TextBox"},
                new TemplateColumn(){ ColumnName = "Amount", ColumnDesc = "退款金额", Seq = i++,ElementType="TextBox"},
                new TemplateColumn(){ ColumnName = "PayType", ColumnDesc = "付款方式", Seq = i++, DictId = PropertyEnumType.PayType.ToString(), IsRequred = true},
                new TemplateColumn(){ ColumnName = "Reason", ColumnDesc = "退款原因", Seq = i++, ElementType="TextArea", IsRequred = true, MaxLength = 200, ValidateType="stringLength"}
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(RefundRecordDTO), showColumns);
            var item = template.Where(t => t.Field == "PayType").FirstOrDefault();
            if (item != null)
            {
                //支付方式暂时不支持内部转账
                item.DictionaryModels = item.DictionaryModels
                    .Where(d => int.Parse(d.Code) != PayTypeEnum.InternalTransfer.GetHashCode()
                    && int.Parse(d.Code) != PayTypeEnum.Wallet.GetHashCode()).ToList();
            }

            return template;
        }
    }
}
