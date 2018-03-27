using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.CompositeAppService;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class PaymentTasksController : BaseController
    {
        public ActionResult Index()
        {

            PaymentTasksAppService paymentTasksAppService = new PaymentTasksAppService();
            PaymentTasksListData paymentTasksListData = new PaymentTasksListData();
            paymentTasksListData.Language = Language;
            var templateModels = paymentTasksAppService.GetPaymentTasksViewTemplate();
            paymentTasksListData.TemplateModels = templateModels;
            return View("PaymentTasksList", paymentTasksListData);
        }


        public ActionResult DetailIndex(int ComDeptId, int PaymentTaskId)
        {
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            PaymentTasksAppService paymentTasksAppService = new PaymentTasksAppService();
            PaymentTasksListData paymentTasksListData = new PaymentTasksListData();
            //paymentTasksListData.TotalMoney = chargeRecordAppService.GetPaymentTasksDetailListById(null, PaymentTaskId, null);
            paymentTasksListData.Language = Language;
            if (PaymentTaskId > 0)
                paymentTasksListData.paymentTasksDTO = paymentTasksAppService.GetPaymentTasksByKey(PaymentTaskId);
            else
            {
                paymentTasksListData.paymentTasksDTO = new PaymentTasksDTO { Status = 0, Id = 0 };
                paymentTasksListData.LastPaymentTaskDate = paymentTasksAppService.GetLastPaymentTaskDate(ComDeptId);

            }
            paymentTasksListData.PaymentTaskId = PaymentTaskId;

            return View("PaymentTaskDetailIndex", paymentTasksListData);
        }

        public ActionResult PaymentTaskViewAddList(int ComDeptId)
        {
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            PaymentTasksListData paymentTasksListData = new PaymentTasksListData();
            paymentTasksListData.Language = Language;
            paymentTasksListData.ComDeptId = ComDeptId;

            var templateModels = chargeRecordAppService.GetBillChargeRecordViewTemplate();
            paymentTasksListData.TemplateModels = templateModels;
            return View("PaymentTasksAddListView", paymentTasksListData);
        }

        public ActionResult PaymentTaskDetailBySubjectViewList()
        {
            PaymentTasksAppService paymentTasksAppService = new PaymentTasksAppService();
            PaymentTasksListData paymentTasksListData = new PaymentTasksListData();
            paymentTasksListData.Language = Language;

            //var templateModels = paymentTasksAppService.GetPaymentTasksBySubjectViewTemplate();
            //paymentTasksListData.TemplateModels = templateModels;
            return View("PaymentTaskDetailBySubjectListView", paymentTasksListData);
        }

        public ActionResult PaymentTaskDetailViewList()
        {
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            PaymentTasksListData paymentTasksListData = new PaymentTasksListData();
            paymentTasksListData.Language = Language;

            var templateModels = chargeRecordAppService.GetPaymentTaskChargeRecordListTemplate();
            paymentTasksListData.TemplateModels = templateModels;
            return View("PaymentTaskDetailListView", paymentTasksListData);
        }

         public ActionResult PaymentTaskByPayMthodViewList()
        {
          
            PaymentTasksListData paymentTasksListData = new PaymentTasksListData();
            paymentTasksListData.Language = Language;
            return View("PaymentTaskByPayMthodViewList", paymentTasksListData);
        }

        public ActionResult PaymentTaskDetailBySubjectNewViewList()
        {

            PaymentTasksListData paymentTasksListData = new PaymentTasksListData();
            paymentTasksListData.Language = Language;
            return View("PaymentTaskDetailBySubjectListNewView", paymentTasksListData);
        }





        /// <summary>
        /// 收费记录异步查询
        /// </summary>
        [HttpPost]
        public ActionResult GetBillChargeRecordViewList(BillChargeRecordSearchDTO search)
        {
            int outCount = 0;
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            IList<BillChargeRecord> dataList = chargeRecordAppService.GetPaymentTasksBillChargeRecordList(search, out outCount);
            SearchResultData<BillChargeRecord> queryResult = new SearchResultData<BillChargeRecord>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };

            return Json(queryResult);
        }

        /// <summary>
        /// 交款新增
        /// </summary>
        [HttpPost]
        public ActionResult PaymentTasksAdd(PaymentTasksDTO PageModel)
        {
            PaymentTasksAppService paymentTasksAppService = new PaymentTasksAppService();
            string[] ids = PageModel.IdStr.Split(',');
            ids = ids.Where(o => o != "").ToArray();
            ResultModel resultModel = paymentTasksAppService.PaymentTasksAdd(ids, (int)CurrentAdminUser.Id, CurrentAdminUser.RealName, PageModel.Remark, PageModel.PaymentDate.Value);
            return Json(resultModel);
        }

        /// <summary>
        /// 交款新增
        /// </summary>
        [HttpPost]
        public ActionResult PaymentTasksContributionsAdd(DateTime PaymentDateMax, int ComDeptId, string Remark)
        {
            PaymentTasksAppService paymentTasksAppService = new PaymentTasksAppService();
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            IList<BillChargeRecord> list = chargeRecordAppService.GetPaymentTasksDetailList(PaymentDateMax, ComDeptId);
            List<string> strlist = new List<string>();
            foreach (BillChargeRecord br in list)
            {
                strlist.Add(br.Id.ToString());
            }

            string[] ids = strlist.ToArray();
            ResultModel resultModel = paymentTasksAppService.PaymentTasksAdd(ids, (int)CurrentAdminUser.Id, CurrentAdminUser.RealName, Remark, PaymentDateMax);

            return Json(resultModel);
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="queryParams">查询参数</param>
        /// <returns>返回查询结果</returns>
        [HttpPost]
        public ActionResult GetPaymentTasksList(PaymentTasksSearchDTO search)
        {
            try
            {
                int outCount = 0;
                PaymentTasksAppService paymentTasksAppService = new PaymentTasksAppService();
                IList<PaymentTasksDTO> dataList = paymentTasksAppService.GetPaymentTasksDTOList(search, out outCount);
                SearchResultData<PaymentTasksDTO> queryResult = new SearchResultData<PaymentTasksDTO>()
                {
                    draw = search.Draw,
                    recordsFiltered = outCount,
                    recordsTotal = outCount,
                    data = dataList
                };

                return Json(queryResult);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        private IEnumerable<TemplateModel> GetTemplateModels()
        {
            return PresentationServiceHelper.LookUp<ITemplateService>().GetTemplateModels("PropertyMgrTemplate.xml", "PaymentTasks", true);
        }

        /// <summary>
        /// 查询交款—科目汇总
        /// </summary>
        /// <param name="queryParams">查询参数</param>
        /// <returns>返回查询结果</returns>
        [HttpPost]
        public ActionResult GetPaymentTasksBySubjectList(PaymentTasksSearchDTO search)
        {
            
            int outCount = 0;
            PaymentTasksAppService paymentTasksAppService = new PaymentTasksAppService();

            var returnModel = paymentTasksAppService.GetPaymentTasksBySubjectList(Convert.ToInt32(search.PaymentTaskId), search.PaymentDateMax, search.DeptId, out outCount);
            returnModel.ReportArrearsDTOList.Add(returnModel.ReportArrearsSum);
            var HeadList = returnModel.ReportHeadList.Select(o => new { name = o.Name }).ToArray();
            var aaData = returnModel.ReportArrearsDTOList.Select(o => o.RowDataList.Select(a => a.Text)).ToArray();
            var result = new
            {
                thead = HeadList,
                data = new
                {
                    aaData = aaData,
                    iTotalDisplayRecords = outCount,
                    iTotalRecords = outCount,
                    draw = search.Draw++
                }
            };
            return Json(result);
        }


        /// <summary>
        ///查询交款-明细汇总
        /// </summary>
        [HttpPost]
        public ActionResult GetPaymentTaskDetailViewList(BillChargeRecordSearchDTO search)
        {
            int outCount = 0;
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            IList<ChargeRecordDTO> dataList = chargeRecordAppService.GetPaymentTasksDetailList(search, out outCount);
            SearchResultData<ChargeRecordDTO> queryResult = new SearchResultData<ChargeRecordDTO>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };

            return Json(queryResult);
        }

        /// <summary>
        /// 交款弃审
        /// </summary>
        [HttpPost]
        public ActionResult PaymentTasksAbandonRviewed(PaymentTasksDTO PageModel)
        {
            ResultModel resultModel = PaymentAppService.PaymentTasksAbandonRviewed((int)PageModel.Id, (int)CurrentAdminUser.Id, CurrentAdminUser.RealName);

            return Json(resultModel);
        }

        /// <summary>
        /// 交款撤销审核
        /// </summary>
        /// <param name="PageModel"></param>
        /// <returns></returns>
        public ActionResult PaymentTasksDelete(PaymentTasksDTO PageModel)
        {
            ResultModel resultModel = PaymentAppService.PaymentTasksDelete((int)PageModel.Id, (int)CurrentAdminUser.Id, CurrentAdminUser.RealName);

            return Json(resultModel);
        }

        /// <summary>
        /// 交款审核
        /// </summary>
        /// <param name="PageModel"></param>
        /// <returns></returns>
        public ActionResult PaymentTasksRviewed(PaymentTasksDTO PageModel)
        {
            ResultModel resultModel = PaymentAppService.PaymentTasksRviewed((int)PageModel.Id, (int)CurrentAdminUser.Id, CurrentAdminUser.RealName, PageModel.CheckRemark);

            return Json(resultModel);
        }

        /// <summary>
        /// 撤销审核
        /// </summary>
        /// <param name="PageModel"></param>
        /// <returns></returns>
        public ActionResult PaymentTasksRevokeRviewed(PaymentTasksDTO PageModel)
        {
            ResultModel resultModel = PaymentAppService.PaymentTasksRevokeRviewed((int)PageModel.Id, (int)CurrentAdminUser.Id, CurrentAdminUser.RealName, PageModel.CheckRemark);

            return Json(resultModel);
        }

        #region 查询交款总金额

        /// <summary>
        /// 查询交款总金额
        /// </summary>
        /// <param name="search">查询参数</param>
        /// <returns>返回查询结果</returns>
        [HttpGet]
        public JsonResult GetPaymentTasksTotalMoney(PaymentTasksSearchDTO search)
        {
            int outCount = 0;
            search.PageSize = -1;
            search.PageStart = 0;
            PaymentTasksAppService paymentTasksAppService = new PaymentTasksAppService();
            IList<PaymentTasksDTO> dataList = paymentTasksAppService.GetPaymentTasksDTOList(search, out outCount);
            var totalMoney = dataList.Sum(p => p.Money);
            var returnValue = new ResultModel
            {
                IsSuccess = true,
                Data = new { TotalMoney = totalMoney == null ? "0" : totalMoney.Value.ToString() }
            };

            return Json(returnValue, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 查询交款—科目-金额汇总

        /// <summary>
        /// 查询交款—科目-金额汇总
        /// </summary>
        /// <param name="queryParams">查询参数</param>
        /// <returns>返回查询结果</returns>
        [HttpGet]
        public JsonResult GetPaymentTasksBySubjectTotalMoney(PaymentTasksSearchDTO search)
        {
            //int outCount = 0;
            //PaymentTasksAppService paymentTasksAppService = new PaymentTasksAppService();
            //IList<PaymentTaskBySubjetDTO> dataList = paymentTasksAppService.GetPaymentTasksBySubjectList(Convert.ToInt32(search.PaymentTaskId), search.PaymentDateMax, search.DeptId, out outCount);

            //var internalTransferTotal = dataList.Sum(p => p.InternalTransfer);
            //var cashTotal = dataList.Sum(p => p.Cash);
            //var bankCardTotal = dataList.Sum(p => p.BankCard);
            //var alipayTotal = dataList.Sum(p => p.Alipay);
            //var weChatTotal = dataList.Sum(p => p.WeChat);
            //var billAmountTotal = dataList.Sum(p => p.BillAmount);
            //var amountTotal = dataList.Sum(p => p.Amount);
            //var walletTotal = dataList.Sum(p => p.Wallet);
            //var internalDebitToal = dataList.Sum(p => p.InternalDebit);
            //var returnValue = new ResultModel
            //{
            //    IsSuccess = true,
            //    Data = new {
            //        InternalTransferTotal = internalTransferTotal == null ? "0" : internalTransferTotal.Value.ToString(),
            //        CashTotal = cashTotal == null ? "0" : cashTotal.Value.ToString(),
            //        BankCardTotal = bankCardTotal == null ? "0" : bankCardTotal.Value.ToString(),
            //        AlipayTotal = alipayTotal == null ? "0" : alipayTotal.Value.ToString(),
            //        WeChatTotal = weChatTotal == null ? "0" : weChatTotal.Value.ToString(),
            //        BillAmountTotal = billAmountTotal == null ? "0" : billAmountTotal.Value.ToString(),
            //        AmountTotal = amountTotal == null ? "0" : amountTotal.Value.ToString(),
            //        WalletTotal = walletTotal == null ? "0" : walletTotal.Value.ToString(),
            //        InternalDebitToal = internalDebitToal == null ? "0" : internalDebitToal.Value.ToString()
            //    }
            //};

            //return Json(returnValue, JsonRequestBehavior.AllowGet);
            return null;
        }

        #endregion

        #region 查询交款-明细-金额汇总

        /// <summary>
        ///查询交款-明细-金额汇总
        /// </summary>
        [HttpGet]
        public JsonResult GetPaymentTaskDetailViewListTotalMoney(BillChargeRecordSearchDTO search)
        {
            var CheckAdminId = CheckSEC_RoleApplicableUserId();
            int outCount = 0;
            search.PageSize = -1;
            search.PageStart = 0;
            search.SECRole_AdminId = CheckAdminId;
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            IList<ChargeRecordDTO> dataList = chargeRecordAppService.GetPaymentTasksDetailList(search, out outCount);
            var totalMoney = dataList.Sum(c => c.Amount);
            var returnValue = new ResultModel
            {
                IsSuccess = true,
                Data = new { TotalMoney = totalMoney == null ? "0" : totalMoney.Value.ToString() }
            };
            return Json(returnValue, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 查询交款--支付明细汇总

        [HttpPost]
        public ActionResult GetPaymentTaskPayMthodIdList(BillChargeRecordSearchDTO search)
        {
            var CheckAdminId = CheckSEC_RoleApplicableUserId();
            
            PaymentTasksAppService paymentTasksAppService = new PaymentTasksAppService();
            var returnModel = paymentTasksAppService.GetPaymentTaskPayMthodIdList(Convert.ToInt32(search.PaymentTaskId), search.PaymentDateMax, search.DeptId, CheckAdminId);
            return Json(returnModel);
        }


        #endregion

        #region 查询交款--收费项目汇总V2.5新
        [HttpPost]
        public ActionResult GetPaymentTaskSubjectList(BillChargeRecordSearchDTO search)
        {
            var CheckAdminId = CheckSEC_RoleApplicableUserId();
            PaymentTasksAppService paymentTasksAppService = new PaymentTasksAppService();
            var returnModel = paymentTasksAppService.GetPaymentTaskSubjectList(Convert.ToInt32(search.PaymentTaskId), search.PaymentDateMax, search.DeptId, CheckAdminId);
            return Json(returnModel);
        }
        #endregion


        private List<SEC_Role> GetSEC_RoleListByAdminUserId()
        {
            var AdminUserId = this.CurrentAdminUser.Id;
            var SEC_RoleList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetRoleListByAdminUsers(AdminUserId);
            return SEC_RoleList.ToList();

        }

        /// <summary>
        ///  检查操作者岗位能交款的数据权限 0所有交款  UserId:只能交自己收费的款项
        /// </summary>
        /// <returns></returns>
        private int? CheckSEC_RoleApplicableUserId()
        {
            var SEC_RoleList = GetSEC_RoleListByAdminUserId();
            int? ReturnNum = -1;
            //获取是否为收费前台权限
            var RoleOne= SEC_RoleList.Where(o => o.Code== "PropertyChargesCashier").ToList();
            if(RoleOne!=null&&RoleOne.Count>0)
                ReturnNum = this.CurrentAdminUser.Id;
            var RoleTwo = SEC_RoleList.Where(o => o.Code == "PropertyChargesManagers"||o.Code== "PropertyChargesFinancialStaff").ToList();
            if (RoleTwo != null && RoleTwo.Count > 0)
                ReturnNum = 0;
            return ReturnNum;
        }




    }

    public class PaymentTasksListData
    {
        public string Language { get; set; }
        public int ComDeptId { get; set; }
        /// <summary>
        /// 上次交款时间
        /// </summary>
        public string LastPaymentTaskDate { get; set; }
        public PaymentTasksDTO paymentTasksDTO { get; set; }
        public int PaymentTaskId { get; set; }
        public decimal TotalMoney { get; set; }
        public IEnumerable<TemplateModel> TemplateModels { get; set; }
    }

}