using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.PresentationService;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class VacationController : BaseController
    {
        public List<VacationInfo> VacationInfos
        {
            get
            {
                List<VacationInfo> vacationInfos = new List<VacationInfo>();
                vacationInfos.Add(new VacationInfo()
                {
                    Id = 1,
                    BeginTime = "2015-09-02",
                    DeptId = 1,
                    DeptName = "逸社区",
                    EndTime = "2015-09-05",
                    Reason = "个人有事需要请假",
                    UserName = "Kuangqifu",
                    VacationType = 1
                });

                vacationInfos.Add(new VacationInfo()
                {
                    Id = 2,
                    BeginTime = "2015-09-02",
                    DeptId = 1,
                    DeptName = "逸社区1",
                    EndTime = "2015-09-05",
                    Reason = "个人有事需要请假1",
                    UserName = "Kuangqifu1",
                    VacationType = 1
                });

                return vacationInfos;
            }
        }

        public ActionResult Index()
        {
            VacationListData vacationListData = new VacationListData();
            vacationListData.Language = Language;
            var templateModels = GetTemplateModels();
            vacationListData.TemplateModels = templateModels;
            return View(vacationListData);
        }

        public ActionResult VacationViewAdd()
        {
            VacationInfo vacationInfo = new VacationInfo();
            return CreateVacationView("Add", vacationInfo);
        }

        public ActionResult VacationViewEdit(int vacationId)
        {
            return CreateVacationView("Edit", VacationInfos.SingleOrDefault(r=>r.Id == vacationId));
        }

        public ActionResult VacationViewLook(int vacationId)
        {
            return CreateVacationView("Look", VacationInfos.SingleOrDefault(r => r.Id == vacationId));
        }

        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="vacationInfo">用户信息</param>
        [HttpPost]
        public ActionResult AddVacation(VacationInfo vacationInfo)
        {
            return CreateResultView();
        }

        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="VacationAllInfo">用户信息</param>
        [HttpPost]
        public ActionResult EditVacation(VacationInfo vacationInfo)
        {
            return CreateResultView();
        }

        public ActionResult DeleteVacation(int vacationId)
        {
            return CreateResultView();
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="queryParams">查询参数</param>
        /// <returns>返回查询结果</returns>
        [HttpPost]
        public ActionResult GetVacations([ModelBinder(typeof(DTModelBinder))]DTParameterModel queryParams)
        {
            SearchResultData<VacationInfo> queryResult = new SearchResultData<VacationInfo>()
            {
                draw = queryParams.Draw,
                recordsFiltered = VacationInfos.Count,
                recordsTotal = VacationInfos.Count,
                data = VacationInfos
            };

            return Json(queryResult);
        }

        #region Private Method

        private ActionResult CreateResultView()
        {
            var jResult = new JsonResult();
            jResult.Data = new ActionResultModel() { ActionInfo = "操作成功", IsSuccess = true };
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jResult;
        }

        private ActionResult CreateVacationView(string viewType, VacationInfo VacationInfo)
        {
            VacationViewData vacationViewData = new VacationViewData();
            vacationViewData.ViewType = viewType;
            vacationViewData.Language = Language;
            vacationViewData.TemplateModels = GetTemplateModels();
            vacationViewData.VacationInfo = VacationInfo;
            return View("VacationView", vacationViewData);
        }

        private IEnumerable<TemplateModel> GetTemplateModels()
        {
            return PresentationServiceHelper.LookUp<ITemplateService>().GetTemplateModels("OAProjectTemplate.xml", "Vacation", true);
        }

        #endregion
    }

    public class VacationListData
    {
        public string Language { get; set; }
        public IEnumerable<TemplateModel> TemplateModels { get; set; }
    }

    public class VacationViewData : VacationListData
    {
        public string ViewType { get; set; }

        public VacationInfo VacationInfo { get; set; }
    }
}
