using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.ApplicationService.Service;
using static YK.PropertyMgr.MVCWeb.Controllers.ChargeSubjectController;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class ChargeSubjectController : BaseController
    {

        public ActionResult Index()
        {
            ChargeSubjectListData chargeSubjectListData = new ChargeSubjectListData();
            chargeSubjectListData.Language = Language;
            var templateModels = GetTemplateModels();
            chargeSubjectListData.TemplateModels = templateModels;
            return View(chargeSubjectListData);

        }

        public ActionResult ChargeSubjectViewAdd()
        {
            ChargeSubjectDTO chargeSubject = new ChargeSubjectDTO();
            return CreateChargeSubjectView("Add", chargeSubject);
        }

        public ActionResult ChargeSubjectViewEdit(int subjectId)
        {
            ChargeSubjectAppService subjectService = new ChargeSubjectAppService();
            return CreateChargeSubjectView("Edit", subjectService.GetChargeSubjectByKey(subjectId));
        }

        public ActionResult ChargeSubjectLook(int subjectId)
        {
            ChargeSubjectAppService subjectService = new ChargeSubjectAppService();
            return CreateChargeSubjectView("Look", subjectService.GetChargeSubjectByKey(subjectId));
        }

        public ActionResult DeleteChargeSubject(int subjectId)
        {

            ChargeSubjectAppService chargeSubAppService = new ChargeSubjectAppService();
            ReturnResult res = chargeSubAppService.DeleteChargeSubjectCus(subjectId);
            return CreateResultView(res);
        }


        #region Private Method

        private ActionResult CreateResultView(ReturnResult res)
        {
            var jResult = new JsonResult();
            jResult.Data = new ActionResultModel() { ActionInfo = res.Msg, IsSuccess = res.IsSuccess };
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jResult;
        }

        private ActionResult CreateChargeSubjectView(string viewType, ChargeSubjectDTO chargeSubject)
        {
            ChargBillAppService billAppService = new ChargBillAppService();
            int outCount = 0;
            if (chargeSubject.Id > 0)
            {
                billAppService.GetChargBillDTOList(Convert.ToInt32(chargeSubject.Id), out outCount);
            }
            else
            {
                chargeSubject.AutomaticBill = (int)AutomaticBillEnum.AutomaticBillEnumY;
            }
            var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
            List<DictionaryModel> listBillPeriod = propertyService.GetDictionaryModels("BillPeriod");
            List<DictionaryModelSubjectType> listSubjectType = propertyService.GetDictionaryModels("SubjectType").Select(o => new DictionaryModelSubjectType { }).ToList();
            ChargeSubjectViewData chargeSubjectViewData = new ChargeSubjectViewData();
            chargeSubjectViewData.ViewType = viewType;
            chargeSubjectViewData.Language = Language;
            chargeSubjectViewData.TemplateModels = GetTemplateModels();
            chargeSubjectViewData.ChargeSubject = chargeSubject;
            chargeSubjectViewData.BillBillPeriodList = listBillPeriod;
            chargeSubjectViewData.IsAutoBillDrop = "[{ Id: " + AutomaticBillEnum.AutomaticBillEnumN + ", Name:否 }, { Id: " + AutomaticBillEnum.AutomaticBillEnumY + ", Name: 是 }]";
            if (outCount > 0)
            {
                chargeSubjectViewData.IsAllowdEdit = false;
            }
            else
            {
                chargeSubjectViewData.IsAllowdEdit = true;
            }
            chargeSubjectViewData.SubjectTypeList = propertyService.GetDictionaryModels("SubjectType").Where(o => o.EnName != "SystemPreset").Select(o => new DictionaryModelSubjectType()
            {
                Id = o.Id,
                EnName = o.EnName,
                CnName = o.CnName,
                Code = Convert.ToInt32(o.Code),
                Order = o.Order
            }).ToList();
            return View("ChargeSubjecView", chargeSubjectViewData);
        }

        #endregion


        #region 获取字典
        public ActionResult GetDictionaryModels(string dicId)
        {

            var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
            List<DictionaryModel> list = propertyService.GetDictionaryModels(dicId);
            var jResult = new JsonResult();
            jResult.Data = new ActionResultModel() { ActionInfo = JsonConvert.SerializeObject(list), IsSuccess = true };
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jResult;
        }
        #endregion



        #region 获取科目信息
        /// <summary>
        /// 获取科目信息
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSubject(int subjectId)
        {
            ChargeSubjectAppService subjectAppService = new ChargeSubjectAppService();
            ChargeSubjectDTO model = subjectAppService.GetChargeSubjectByKey(subjectId);
            if (null == model)
            {
                model = new ChargeSubjectDTO();

            }
            var jsonResult = new JsonResult();
            jsonResult.Data = model;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion


        #region 新增编辑数据

        [HttpPost]
        public ActionResult AddChargeSubject(ChargeSubjectDTO chargeSubject)
        {
            string subjectTypes = string.Empty;
            DateTime nowTime = DateTime.Now;
            chargeSubject.UpdateTime = nowTime;
            chargeSubject.CreateTime = nowTime;
            chargeSubject.Operator = CurrentAdminUser.Id;
            chargeSubject.PenaltyRate = 0;
            chargeSubject.BeginDate = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd} 00:00:00", chargeSubject.BeginDate));
            chargeSubject.Remark = chargeSubject.Remark;
            if (chargeSubject.AutomaticBill == (int)AutomaticBillEnum.AutomaticBillEnumN)
            {
                chargeSubject.BillDay = null;
            }
            ChargeSubjectAppService chargeSubAppService = new ChargeSubjectAppService();
            ReturnResult res = chargeSubAppService.InsertChargeSubjectCus(chargeSubject);
            return CreateResultView(res);
        }

        [HttpPost]
        public ActionResult EditChargeSubject(ChargeSubjectDTO chargeSubject)
        {
            ChargeSubjectAppService subjectSevice = new ChargeSubjectAppService();
            ChargeSubjectDTO subjectDTO = subjectSevice.GetChargeSubjectByKey(chargeSubject.Id);
            string subjectTypes = string.Empty;
            DateTime nowTime = DateTime.Now;
            chargeSubject.UpdateTime = nowTime;
            chargeSubject.ComDeptId = subjectDTO.ComDeptId;
            subjectTypes = chargeSubject.ChargeFormula.Replace('+', '|').Replace('-', '|').Replace('*', '|').Replace('/', '|').Replace('(', '|').Replace('+', ')').Replace(',', '|');
            if (chargeSubject.AutomaticBill == (int)AutomaticBillEnum.AutomaticBillEnumY)
            {
                if (!(chargeSubject.BillDay >= 1))
                {
                    return CreateResultView(new ReturnResult() { IsSuccess = false, Msg = "自动生成账单、账单日必须大于或等于1" });
                }
            }
            else
            {
                chargeSubject.BillDay = null;
            }

            ChargeSubjectAppService chargeSubAppService = new ChargeSubjectAppService();
            ReturnResult res = chargeSubAppService.UpdateChargeSubjectCus(chargeSubject);
            return CreateResultView(res);
        }

        #endregion

        #region 查询科目信息
        /// <summary>
        /// 查询科目信息
        /// </summary>
        /// <param name="queryParams">查询科目信息</param>
        /// <returns>返回查询结果</returns>
        [HttpPost]
        public ActionResult GetChargeSubjects([ModelBinder(typeof(DTModelBinder))]DTParameterModel queryParams)
        {
            string search = queryParams.CustomSearch;
            int count = 0;
            int pageIndex = 0;
            ChargeSubjectAppService chargeSubAppService = new ChargeSubjectAppService();
            ChargeSubjectDTO model = chargeSubAppService.GetSearchParms(queryParams, out pageIndex);
            pageIndex = (queryParams.Start / 10) + 1;
            DeptAppService deptService = new DeptAppService();
            string strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
            var treeData = deptService.GetDeptCustomTree(CurrentAdminUser.UserName, strFilter);
            List<int?> listComIds = new List<int?>();
            foreach (var m in treeData)
            {
                foreach (var n in (List<CustomTreeNodeModel>)m.children)
                {
                    listComIds.Add(Convert.ToInt32(n.id.Split('_')[0]));
                }
            }
            IEnumerable<ChargeSubjectDTO> list = chargeSubAppService.Paging(listComIds, pageIndex, queryParams.Length, model, string.Empty, out count);
            SearchResultData<ChargeSubjectDTO> queryResult = new SearchResultData<ChargeSubjectDTO>()
            {
                draw = queryParams.Draw,
                recordsFiltered = count,
                recordsTotal = count,
                data = list
            };

            return Json(queryResult);

        }

        #endregion

        #region XML
        private IEnumerable<TemplateModel> GetTemplateModels()
        {
            return PresentationServiceHelper.LookUp<ITemplateService>().GetTemplateModels("PropertyMgrTemplate.xml", "ChargeSubject", true);
        }
        #endregion
    }
    #region 模板
    public class ChargeSubjectListData
    {
        public string Language { get; set; }
        public IEnumerable<TemplateModel> TemplateModels { get; set; }
    }

    public class ChargeSubjectViewData : ChargeSubjectListData
    {
        public string ViewType { get; set; }
        /// <summary>
        ///收费周期 
        /// </summary>
        public List<DictionaryModel> BillBillPeriodList { get; set; }
        /// <summary>
        ///收费类别 
        /// </summary>
        public List<DictionaryModelSubjectType> SubjectTypeList { get; set; }
        public ChargeSubjectDTO ChargeSubject { get; set; }
        /*如果科目产生账单则不允许修改科目*/
        public bool IsAllowdEdit { get; set; }

        public string IsAutoBillDrop { get; set; }
    }


    public class DictionaryModelSubjectType
    {
        public int Id { get; set; }
        public string EnName { get; set; }
        public string CnName { get; set; }
        public int Code { get; set; }
        public string Order { get; set; }
    }

    #endregion


}
