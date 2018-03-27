using Aspose.Cells;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.CompositeDomainService;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using YK.PropertyMgr.DomainService;
using YK.PropertyMgr.ApplicationMapper;
using System.Linq.Expressions;

namespace YK.PropertyMgr.ApplicationService
{
   public partial class ReceiptBookDetailAppService
    {

        public bool ExistReceiptBookNum(string ReceiptBookNum,int ComDeptId,int ReceiptBookType)
        {
            Condition < ReceiptBookDetail> condition = new Condition<ReceiptBookDetail>(c => c.IsDel == false &&  c.Number== ReceiptBookNum);
            ReceiptBookDetailDomainService _ReceiptBookDetailDomainService = new ReceiptBookDetailDomainService();

            var list = _ReceiptBookDetailDomainService.GetReceiptBookDetailList(condition.ExpressionBody, ComDeptId, ReceiptBookType);
            if (list.Count > 0)
                return true;
            else
                return false;
        }


        public IEnumerable<TemplateModel> GetReceiptBookDetailShowListTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "Number", ColumnDesc = "票据号", Seq = i++,DictId="-1",IsExport=true},
                new TemplateColumn(){ ColumnName = "ReceResourcesNum", ColumnDesc = "收费资源", Seq = i++,DictId="-1",IsExport=true},
                new TemplateColumn(){ ColumnName = "Amount", ColumnDesc = "收费金额", Seq = i++,DictId="-1",IsExport=true},
                new TemplateColumn(){ ColumnName = "DiscountAmount", ColumnDesc = "优惠金额", Seq = i++,DictId="-1",IsExport=true},
                new TemplateColumn(){ ColumnName = "PayDateTimeStr", ColumnDesc = "收费日期", Seq = i++,DictId="-1",IsExport=true},
                new TemplateColumn(){ ColumnName = "OperatorName", ColumnDesc = "操作人", Seq = i++,DictId="-1",IsExport=true},
                new TemplateColumn(){ ColumnName = "ChargeType", ColumnDesc = "状态", Seq = i++,DictId = PropertyEnumType.ChargeType.ToString(),IsExport=true},
                new TemplateColumn(){ ColumnName = "PayMthodId", ColumnDesc = "付款方式", Seq = i++,DictId = PropertyEnumType.PayType.ToString(),IsExport=true},
                new TemplateColumn(){ ColumnName = "RefundRecordReason", ColumnDesc = "退款原因", Seq = i++,DictId="-1",IsExport=true},
                new TemplateColumn(){ ColumnName = "Remark", ColumnDesc = "备注", Seq = i++,DictId="-1",IsExport=true},
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(ReceiptBookDetailShowDTO), showColumns);
            return template;
        }


        public IList<ReceiptBookDetailShowDTO> GetReceiptBookDetailShowList(ReceiptBookDetailSeachDTO Seach, out int totalCount,bool IsExport=false)
        {
            if (Seach.ReceiptBookId == null || Seach.ReceiptBookId <= 0)
            {
                totalCount = 0;
                return new List<ReceiptBookDetailShowDTO>();
            }
            string expressions = " Number desc";
            Condition<ReceiptBookDetail> condition_detail = new Condition<ReceiptBookDetail>(o=>o.ReceiptBookId==Seach.ReceiptBookId);
            Condition<ChargeRecord> condition_chargeRecord = new Condition<ChargeRecord>(o => true);

            if (!string.IsNullOrEmpty(Seach.ReceiptNum))
            {
                condition_detail = condition_detail & new Condition<ReceiptBookDetail>(o => o.Number.Contains(Seach.ReceiptNum));
            }

            if (!string.IsNullOrEmpty(Seach.ResourcesName))
            {
                condition_chargeRecord = condition_chargeRecord & new Condition<ChargeRecord>(o => o.ResourcesNames.Contains(Seach.ResourcesName));

            }

            if (Seach.ChargeStartDate > DateTime.MinValue)
            {
                condition_chargeRecord = condition_chargeRecord & new Condition<ChargeRecord>(o => o.PayDate>=Seach.ChargeStartDate);
            }

            if (Seach.ChargeEndDate > DateTime.MinValue)
            {
                Seach.ChargeEndDate = Seach.ChargeEndDate.Value.AddDays(1).AddMilliseconds(-1);
               condition_chargeRecord = condition_chargeRecord & new Condition<ChargeRecord>(o => o.PayDate <= Seach.ChargeEndDate);
            }
            ReceiptBookDetailDomainService _ReceiptBookDetailDomainService = new ReceiptBookDetailDomainService();
            return _ReceiptBookDetailDomainService.GetReceiptBookDetailShowList(condition_detail.ExpressionBody, condition_chargeRecord.ExpressionBody, expressions, out totalCount, Seach.PageIndex, Seach.PageSize, IsExport);
          
        }




    }
}
