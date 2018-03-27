using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class RefundRecordController : BaseController
    {
        //
        // GET: /RefundRecord/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RefundModal(ChargeRecordDTO chargeRecordInfo) 
        {
            if (string.IsNullOrEmpty(chargeRecordInfo.ReceiptId))
            {
                RecordPageType ptype = chargeRecordInfo.PageType;
                chargeRecordInfo = (new ChargeRecordAppService()).GetChargeRecordById(chargeRecordInfo.Id);
                chargeRecordInfo.PageType = ptype;
            }
            RefundRecordViewData refundRecordViewData = new RefundRecordViewData();
            //退款金额 = 收费金额-优惠金额 2017-03-31 如果 < 0,则退款金额为0
            var ramount = chargeRecordInfo.Amount.Value - chargeRecordInfo.DiscountAmount.Value;
            if (ramount < 0)
            {
                ramount = 0;
            }
            refundRecordViewData.RefundRecordInfo = new RefundRecordDTO() 
            { 
                  RefChargeRecordId = chargeRecordInfo.Id,
                  HouseDeptId = chargeRecordInfo.HouseDeptId,
                  Amount = ramount,
                  HouseDoorNo = chargeRecordInfo.HouseDoorNo,
                  ReceiptNum = chargeRecordInfo.ReceiptNum,
                  PayType = PayTypeEnum.Cash.GetHashCode()//默认现金
            };
            refundRecordViewData.Language = this.Language;
            RefundRecordAppService service = new RefundRecordAppService();
            refundRecordViewData.TemplateModels = service.GetRefundRecordViewTemplate();
            refundRecordViewData.PageType = chargeRecordInfo.PageType;
            return View(refundRecordViewData);
        }

        public ActionResult ForegiRefundModal(ChargeRecordDTO chargeRecordInfo)
        {
            if (string.IsNullOrEmpty(chargeRecordInfo.ReceiptId))
            {
                RecordPageType ptype = chargeRecordInfo.PageType;
                chargeRecordInfo = (new ChargeRecordAppService()).GetChargeRecordById(chargeRecordInfo.Id);
                chargeRecordInfo.PageType = ptype;
            }
            RefundRecordViewData refundRecordViewData = new RefundRecordViewData();
            refundRecordViewData.RefundRecordInfo = new RefundRecordDTO()
            {
                RefChargeRecordId = chargeRecordInfo.Id,
                HouseDeptId = chargeRecordInfo.HouseDeptId,
                Customer = chargeRecordInfo.CustomerName,
                Amount = chargeRecordInfo.Amount.Value - chargeRecordInfo.DiscountAmount.Value,//退款金额 = 收费金额-优惠金额 2017-03-31
                HouseDoorNo = chargeRecordInfo.HouseDoorNo,
                ReceiptNum = chargeRecordInfo.ReceiptNum,
                PayType = PayTypeEnum.Cash.GetHashCode()//默认现金
            };
            refundRecordViewData.Language = this.Language;
            RefundRecordAppService service = new RefundRecordAppService();
            refundRecordViewData.TemplateModels = service.GetForegiRefundRecordViewTemplate();
            refundRecordViewData.PageType = chargeRecordInfo.PageType;
            return View("RefundModal", refundRecordViewData);
        }


        
    }





    public class RefundRecordViewData 
    {
        public string Language { get; set; }
        public IEnumerable<TemplateModel> TemplateModels { get; set; }
        public RefundRecordDTO RefundRecordInfo { get; set; }

        public RecordPageType PageType { get; set; }
    }
}