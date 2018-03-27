using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class PreAccountManageController : BaseController
    {
        // GET: PreAccountManage
        public ActionResult PreAccountManageIndex()
        {
            OperationRecordListData recordData = new OperationRecordListData();
            PrepayAccountAppService servic = new PrepayAccountAppService();
            recordData.TemplateModels = servic.GetOperationRecordTemplate();
            return View(recordData);
        }

        #region 获取预存账户信息

        public ActionResult GetPreAccountList(int? deptId, EDeptType? deptType)
        {
            if (!deptId.HasValue || deptType != EDeptType.FangWu)
            {
                return Json(new List<PrepayAccountTransferDTO>(),JsonRequestBehavior.AllowGet);
            }
            PrepayAccountAppService servic = new PrepayAccountAppService();
            var paccountList = servic.GetPrepayAccountTransferList(deptId.Value);
            return Json(paccountList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 预存费转移

        public ActionResult PreAccountCostTransferView(int deptId, EDeptType deptType, PrepayAccountTransferDTO info)
        {
            PreAccountCostTransferData data = new PreAccountCostTransferData();
            data.PreAccountTransfer = info;
            ChargBillAppService service = new ChargBillAppService();
            data.HouseSubjectList = service.GetHouseSubjectList(deptId, false, deptType, true)
                .Where(s => s.Id != info.ChargeSubjectID)//排除自身
                .ToList();
            return View(data);
        }

        /// <summary>
        /// 预存费转移
        /// </summary>
        [HttpPost]
        public ActionResult PreAccountCostTransfer(PreAccountCostTransferDTO transfer)
        {
            PrepayAccountAppService servic = new PrepayAccountAppService();
            transfer.Operator = CurrentAdminUser.Id.Value;
            transfer.OperatorName = CurrentAdminUser.RealName;
            var result = servic.PreAccountCostTransfer(transfer);
            return Json(result);
        }

        #endregion

        #region 操作记录

        [HttpPost]
        public ActionResult GetOperationRecordList(PreAccountORSearchDTO search)
        {
            int outCount = 0;
            PrepayAccountAppService service = new PrepayAccountAppService();
            IList<PrepayAccountLogDTO> dataList = service.GetOperationRecordList(search, out outCount);
            SearchResultData<PrepayAccountLogDTO> queryResult = new SearchResultData<PrepayAccountLogDTO>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };
            return Json(queryResult);
        }

        #endregion

        #region 预存费批量抵扣

        public ActionResult PreCostBatchDeductionIndex()
        {
            PreCostBatchDeductionData data = new PreCostBatchDeductionData();
            PrepayAccountAppService service = new PrepayAccountAppService();
            data.TemplateModels = service.GetBatchDeductionTemplate();
            return View(data);
        }

        [HttpPost]
        public ActionResult GetBatchDeductionBillList(BatchDeductionSearchDTO search)
        {
            int outCount = 0;
            PrepayAccountAppService service = new PrepayAccountAppService();
            IList<BatchDeductionBillSumDTO> dataList = service.GetBatchDeductionList(search, out outCount);
            SearchResultData<BatchDeductionBillSumDTO> queryResult = new SearchResultData<BatchDeductionBillSumDTO>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };
            return Json(queryResult);
        }

        [HttpPost]
        public ActionResult PreCostBatchDeduction(string[] houseDeptSubjectIds)
        {
            PrepayAccountAppService service = new PrepayAccountAppService();
            var result = service.PreCostBatchDeduction(houseDeptSubjectIds, this.CurrentAdminUser.Id.Value, this.CurrentAdminUser.RealName);
            return Json(result);
        }

        #endregion
    }

    public class PreAccountCostTransferData
    {
        public PrepayAccountTransferDTO PreAccountTransfer { get; set; }
        public IList<ChargeSubjectDTO> HouseSubjectList { get; set; }
    }

    public class OperationRecordListData
    {
        public string Language { get; set; }
        public IEnumerable<TemplateModel> TemplateModels { get; set; }
    }

    public class PreCostBatchDeductionData
    {
        public IEnumerable<TemplateModel> TemplateModels { get; set; }
    }
}