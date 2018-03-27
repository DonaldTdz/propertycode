using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.ApplicationService.Service;
using YK.PropertyMgr.Crosscuting;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class EntranceController : BaseController
    {

        public ActionResult Index()
        {
            EntranceAppService entranceAppService = new EntranceAppService();
            EntranceListData entranceListDataListData = new EntranceListData();
            entranceListDataListData.Language = Language;
            entranceListDataListData.Provinces = new ProvinceAppService().GetProvinces();//
            var templateModels = GetTemplateModels();
            entranceListDataListData.TemplateModels = templateModels;

            entranceListDataListData.TemplateModels = entranceAppService.GetEntranceViewTemplate();
            return View(entranceListDataListData);
        }

        #region 查询信息处理
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="queryParams">查询参数</param>
        /// <returns>返回查询结果</returns>
        [HttpPost]
        public ActionResult GetEntrances(EntranceSearchDTO search)
        {
            try
            {
                int outCount = 0;
                EntranceAppService entranceAppService = new EntranceAppService();
                IList<EntranceViewDTO> dataList = entranceAppService.GetEntranceDTOList(search, out outCount);
                SearchResultData<EntranceViewDTO> queryResult = new SearchResultData<EntranceViewDTO>()
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
            return PresentationServiceHelper.LookUp<ITemplateService>().GetTemplateModels("PropertyMgrTemplate.xml", "Entrance", true);
        }
        #endregion

        #region 编辑信息处理

        public ActionResult EntranceViewAdd(int? villageId)
        {
            EntranceDTO Entrance = new EntranceDTO();
            return CreateEntranceView("Add", Entrance, villageId);
        }


        public ActionResult EntranceViewEdit(int entranceId)
        {
            EntranceAppService entranceAppService = new EntranceAppService();
            return CreateEntranceView("Edit", entranceAppService.GetEntranceByKey(entranceId), 0);
        }

        public ActionResult EntranceLook(int subjectId, int? villageId)
        {
            EntranceAppService subjectService = new EntranceAppService();
            return CreateEntranceView("Look", subjectService.GetEntranceByKey(subjectId), villageId);
        }
        [HttpPost]
        public ActionResult EditEntrance(EntranceDTO entrance, string villageName)
        {

            EntranceAppService entranceAppService = new EntranceAppService();
            entrance.Name = entrance.Name;
            entrance.DoorName = entrance.Address;
            var DataBaseDTO = entranceAppService.GetEntranceByKey(entrance.Id);
            var Extension2= ObjectHelper.CompareTypeString<EntranceDTO>(DataBaseDTO, entrance);
            ResultModel res = entranceAppService.UpdateEntranceCus(entrance, villageName);

            if (res.IsSuccess)
                LogProperty.WirteFrameworkLog(CurrentAdminUser.Id.ToString(), CurrentAdminUser.RealName, "EntranceController", "EditEntrance", "修改门禁信息", "小区" + villageName + "修改门禁" + entrance.Name,Extension2);


            return CreateResultView(res);
        }

        [HttpPost]
        public ActionResult AddEntrance(EntranceDTO entrance, string villageName)
        {


            entrance.Name = entrance.Name;
            entrance.DoorName = entrance.Address;
            var json = new JsonResult();
            DateTime nowTime = DateTime.Now;
            
            entrance.CreateTime = nowTime;
            entrance.BindSockState = (int)BindSockStateEnum.NotBindLock;
            EntranceAppService entranceAppService = new EntranceAppService();
            ResultModel res = entranceAppService.InsertEntranceCus(entrance, villageName);
            if(res.IsSuccess)
            LogProperty.WirteFrameworkLog(CurrentAdminUser.Id.ToString(), CurrentAdminUser.RealName, "EntranceController", "AddEntrance", "新增门禁", "小区" + villageName + "新增门禁"+ entrance.Name);


            return CreateResultView(res);

        }
        public ActionResult DeleteEntrance(int subjectId)
        {

            EntranceAppService chargeSubAppService = new EntranceAppService();
            ResultModel res = new ResultModel();
            return CreateResultView(res);
        }

        private ActionResult CreateResultView(ResultModel res)
        {
            var jResult = new JsonResult();
            jResult.Data = new ActionResultModel() { ActionInfo = res.Msg, IsSuccess = res.IsSuccess };
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jResult;
        }




        private ActionResult CreateEntranceView(string viewType, EntranceDTO Entrance, int? villageId)
        {
            EntranceAppService entranceAppService = new EntranceAppService();
            DeptAppService deptService = new DeptAppService();
            CountyAppService countryAppServer = new CountyAppService();
            ProvinceAppService provinceAppService = new ProvinceAppService();
            CityAppService cityAppServie = new CityAppService();
            string strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu;
            string strFilterBuilds = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";" + (int)EDeptType.LouYu;
            SEC_DeptDTO deptVillage = new SEC_DeptDTO();
            if (Entrance.Id > 0)
            {
                villageId = Entrance.VillageID;
            }
            deptVillage = deptService.GetDeptInfoById(villageId.ToString());
            EntranceViewData EntranceViewData = new EntranceViewData()
            {
                ViewType = viewType,
                Language = Language,
                TemplateModels = GetTemplateModels(),
                Entrance = Entrance,
                /*小区、楼栋、单元*/
                DeptVillages = new List<DropTemp>() { new DropTemp() {
                    Id=Convert.ToInt32(deptVillage.Id),
                    Text=deptVillage.Name
                } },
                Builds = deptService.GetBuildsByComDeptId(Convert.ToInt32(villageId)).Select(o => new DropTemp { Id = o.Id, Text = o.Name }).ToList(),
                Units = entranceAppService.LoadUnit(Entrance.BuildId),
                /*省市县*/
                //Provinces = provinceAppService.GetProvinces(),
                Cities = cityAppServie.GetCityList(Convert.ToInt32(Entrance.ProvinceID)),
                Countries = countryAppServer.GetCountyList(Convert.ToInt32(Entrance.CityID))

            };
            return View("EntranceView", EntranceViewData);
        }

        /// <summary>
        /// 获取楼宇
        /// </summary>
        /// <param name="provinceId">省ID</param>
        /// <returns></returns>
        public ActionResult LoadBuild(int? villageId)
        {

            if (string.IsNullOrEmpty(villageId.ToString()))
            {
                villageId = -1;
            }
            var jResult = new JsonResult();
            if (villageId > 0)
            {
                string strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";" + (int)EDeptType.LouYu;
                DeptAppService deptAppServie = new DeptAppService();
                List<DropTemp> listBuilds = new List<DropTemp>();
                listBuilds = deptAppServie.GetDeptTree(CurrentAdminUser, Convert.ToInt32(villageId), strFilter).Select(o => new DropTemp { Id = Convert.ToInt32(o.id.Split('_')[0]), Text = o.text }).ToList();
                jResult.Data = listBuilds;
            }
            else
            {
                jResult.Data = new List<DropTemp>();
            }
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jResult;
        }
        /// <summary>
        /// 获取单元信息
        /// </summary>
        /// <param name="buildId">楼宇ID</param>
        /// <returns></returns>
        public ActionResult LoadUnit(int? buildId)
        {
            var jResult = new JsonResult();
            List<string> listUnits = new List<string>();

            if (!string.IsNullOrEmpty(buildId.ToString()))
            {
                EntranceAppService entranceAppService = new EntranceAppService();
                listUnits = entranceAppService.LoadUnit(buildId);
            }
            jResult.Data = listUnits;
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jResult;
        }
        /// <summary>
        /// 加载市
        /// </summary>
        /// <param name="provinceId">省ID</param>
        /// <returns></returns>
        public ActionResult LoadCity(int? provinceId)
        {
            if (string.IsNullOrEmpty(provinceId.ToString()))
            {
                provinceId = 0;
            }
            var jResult = new JsonResult();
            CityAppService cityAppServie = new CityAppService();
            List<CityDTO> list = cityAppServie.GetCityList(Convert.ToInt32(provinceId));
            jResult.Data = list;
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jResult;
        }
        /// <summary>
        /// 加载区县
        /// </summary>
        /// <param name="cityId">市ID</param>
        /// <returns></returns>
        public ActionResult LoadCountry(int? cityId)
        {
            if (string.IsNullOrEmpty(cityId.ToString()))
            {
                cityId = 0;
            }
            var jResult = new JsonResult();

            CountyAppService countryAppServie = new CountyAppService();
            List<CountyDTO> list = countryAppServie.GetCountyList(Convert.ToInt32(cityId));
            jResult.Data = list;
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jResult;
        }

        #endregion

        public JsonResult NotBind(int entranceId)
        {
            EntranceAppService entranceAppService = new EntranceAppService();
            EntranceDTO model = entranceAppService.GetEntranceById(entranceId);
            if (model == null)
            {
                var jResult = new JsonResult();
                jResult.Data = new ResultModel() { Msg = "没有当前大门信息!", IsSuccess = false };
                jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jResult;
            }
            else
            {
                if (model.KeyID == 0)
                {
                    return Json(new ResultModel() { IsSuccess = false, Msg = "当前大门没有绑定锁信息!" });
                }
                else
                {
                    model.KeyID = 0;
                    model.BindSockState = (int)BindSockStateEnum.NotBindLock;
                }
            }


            ResultModel res = entranceAppService.NotBindKey(model);
            if (res.IsSuccess)
                LogProperty.WirteFrameworkLog(CurrentAdminUser.Id.ToString(), CurrentAdminUser.RealName, "EntranceController", "NotBind", "解绑门禁", "小区" + model.VillageID + "解绑门禁" + model.Name);


            return Json(res);
        }
    }
    public class EntranceListData
    {
        public string Language { get; set; }
        public IEnumerable<TemplateModel> TemplateModels { get; set; }
        /*加载省信息*/
        public List<ProvinceDTO> Provinces { get; set; }
    }

    public class DropTemp
    {
        public int? Id { get; set; }
        public string Text { get; set; }
    }


    public class EntranceViewData : EntranceListData
    {
        public string ViewType { get; set; }
        /*加载对象相信*/
        public EntranceDTO Entrance { get; set; }
        /*加载小区信息*/
        public List<DropTemp> DeptVillages { get; set; }
        /*加载楼宇*/
        public List<DropTemp> Builds { get; set; }
        /*加载单元*/
        public List<string> Units { get; set; }

        ///*加载省信息*/
        //public List<ProvinceDTO> Provinces { get; set; }
        /*加载市*/
        public List<CityDTO> Cities { get; set; }
        /*加载区县*/
        public List<CountyDTO> Countries { get; set; }

    }
}