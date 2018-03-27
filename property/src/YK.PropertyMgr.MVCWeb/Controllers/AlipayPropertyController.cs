using PropertyAlipay.Entity.model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.ApplicationService.Service;


namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class AlipayPropertyController : BaseController
    {

        #region 授权

        // GET: AlipayProperty
        public ActionResult Index()
        {
            AlipayPropertyIndexData alipayPropertyIndexData = new AlipayPropertyIndexData();
            AlipayPropertyAppService alipaycommunity = new AlipayPropertyAppService();
            alipayPropertyIndexData.Language = this.Language;
          //  alipayPropertyIndexData.TemplateModels = alipaycommunity.GetAlipayCommunityTemplate();
            return View(alipayPropertyIndexData);
        }
       
     

        [HttpPost]
        public ActionResult GetAlipayOAuth(int DeptId)
        {
            var jResult = new JsonResult();

           string CallBackUrl= ConfigurationManager.AppSettings["PropertyAlipayOAuthCallBackUrl"].ToString();
           string AlipayOAuthkUrl = ConfigurationManager.AppSettings["PropertyAlipayOAuthUrl"].ToString();
           string AlipayAppId = ConfigurationManager.AppSettings["PropertyAlipayAppId"].ToString();

            var CallBackUrlStr = CallBackUrl + "?DeptId=" + DeptId;

            CallBackUrl= HttpUtility.UrlEncode(CallBackUrlStr);

            var OAuthURL = AlipayOAuthkUrl + "?app_id=" + AlipayAppId + "&redirect_uri=" + CallBackUrl;

            jResult.Data = new ActionResultModel() {   IsSuccess = true, DataInfo= OAuthURL };
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jResult;
        }


        [HttpPost]
        public ActionResult CheckIsOAuth(int DeptId)
        {

            AlipayAPPAuthTokenAppService _AlipayAPPAuthTokenAppService = new AlipayAPPAuthTokenAppService();

           var result=   _AlipayAPPAuthTokenAppService.IsCheckIsOAuth(DeptId);

            var jResult = new JsonResult();
            jResult.Data = result;
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jResult;
        }


      



        // GET: AlipayProperty
        public ActionResult AlipayAppAuthTokenIndex()
        {
            AlipayPropertyIndexData alipayPropertyIndexData = new AlipayPropertyIndexData();
            AlipayPropertyAppService alipaycommunity = new AlipayPropertyAppService();
            alipayPropertyIndexData.Language = this.Language;
            //  alipayPropertyIndexData.TemplateModels = alipaycommunity.GetAlipayCommunityTemplate();
            return View(alipayPropertyIndexData);
        }


        [HttpPost]
        public ActionResult GetAppAuthTokenQuery(int DeptId)
        {

            AlipayAPPAuthTokenAppService _AlipayAPPAuthTokenAppService = new AlipayAPPAuthTokenAppService();

            var result = _AlipayAPPAuthTokenAppService.GetAppAuthTokenQuery(DeptId);

            var jResult = new JsonResult();
            jResult.Data = result;
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jResult;
        }

        [HttpPost]
        public ActionResult RefreshAppAuthToken(int DeptId)
        {

            AlipayAPPAuthTokenAppService _AlipayAPPAuthTokenAppService = new AlipayAPPAuthTokenAppService();
            var result = _AlipayAPPAuthTokenAppService.SaveAppAuthToken(DeptId, false);
            var jResult = new JsonResult();
            jResult.Data = result;
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return Json(jResult);
        }



        #endregion

        #region 对接小区

            public ActionResult AlipayCommunityList()
            {
                AlipayPropertyIndexData alipayPropertyIndexData = new AlipayPropertyIndexData();
                AlipayPropertyAppService alipaycommunity = new AlipayPropertyAppService();
                alipayPropertyIndexData.Language = this.Language;
                return View(alipayPropertyIndexData);
            }

        [HttpPost]
        public ActionResult GetAlipayCommunityList(int DeptId)
        {
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();
            var AlipayCommunityList= _AlipayPropertyAppService.GetAlipayCommunityList(DeptId);
            return Json(AlipayCommunityList);
        }


        public ActionResult AlipaiCommunityCreateViewAdd(int ComDeptId)
        {
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();
            var ResponseModel = _AlipayPropertyAppService.GetNewCommunityResponseModelByComDeptId(ComDeptId);
            var ProvinceList = _AlipayPropertyAppService.GetSec_AreaList(1).ToList();
            AlipaiCommunityViewData model = new AlipaiCommunityViewData()
            {
                CommunityModel = ResponseModel,
                ProvinceList = ProvinceList,
                IsShow=false
            };
            return CreateAlipaiCommunityView(model);
        }

        public ActionResult AlipaiCommunityCreateViewEdit(int AlipayCommunityId)
        {
            //获取修改信息

            AlipayCommunityAppService _AlipayCommunityAppService = new AlipayCommunityAppService();
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();

            var AlipayCommunityModel = _AlipayCommunityAppService.GetAlipayCommunityByKey(AlipayCommunityId);
            var CommunityModel = _AlipayPropertyAppService.GetCommunityById(AlipayCommunityModel.AlipayCommunityId, AlipayCommunityModel.ProDeptId.Value);
            var ProvinceList = _AlipayPropertyAppService.GetSec_AreaList(1).ToList();
            var CityList = _AlipayPropertyAppService.GetSec_AreaList(2, Convert.ToInt32(CommunityModel.province_code)).ToList();
            var DistrictList = _AlipayPropertyAppService.GetSec_AreaList(3, Convert.ToInt32(CommunityModel.city_code)).ToList();
            AlipaiCommunityViewData model = new AlipaiCommunityViewData()
            {
                CommunityModel = CommunityModel,
                ProvinceList = ProvinceList,
                CityList = CityList,
                DistrictList = DistrictList,
                IsShow=false

            };

            return CreateAlipaiCommunityView(model);

        }

        public ActionResult AlipaiCommunityCreateViewShow(int AlipayCommunityId)
        {
            //获取修改信息

            AlipayCommunityAppService _AlipayCommunityAppService = new AlipayCommunityAppService();
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();

            var AlipayCommunityModel = _AlipayCommunityAppService.GetAlipayCommunityByKey(AlipayCommunityId);
            var CommunityModel = _AlipayPropertyAppService.GetCommunityById(AlipayCommunityModel.AlipayCommunityId, AlipayCommunityModel.ProDeptId.Value);
            var ProvinceList = _AlipayPropertyAppService.GetSec_AreaList(1).ToList();
            var CityList = _AlipayPropertyAppService.GetSec_AreaList(2, Convert.ToInt32(CommunityModel.province_code)).ToList();
            var DistrictList = _AlipayPropertyAppService.GetSec_AreaList(3, Convert.ToInt32(CommunityModel.city_code)).ToList();
            var QRCodeImageurl = _AlipayPropertyAppService.GetQRCodeImage(AlipayCommunityModel.AlipayCommunityId, AlipayCommunityModel.ProDeptId.Value);
            bool IsQrCodeShow = false;
            if (QRCodeImageurl.Length > 0)
                IsQrCodeShow = true;

            AlipaiCommunityViewData model = new AlipaiCommunityViewData()
            {
                CommunityModel = CommunityModel,
                ProvinceList = ProvinceList,
                CityList = CityList,
                DistrictList = DistrictList,
                IsShow = true,
                QrCodeUrl = QRCodeImageurl,
                IsShowQrCode = IsQrCodeShow



            };

            return CreateAlipaiCommunityView(model);

        }





        private ActionResult CreateAlipaiCommunityView(AlipaiCommunityViewData Model)
        {
            return View("AlipayCommunityView", Model);
        }

        [HttpPost]
        public ActionResult LoadCity(int? ProvinceID)
        {
            if (ProvinceID == null)
                return Json(new List<SEC_AreaDTO>());
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();
            var ProvinceList = _AlipayPropertyAppService.GetSec_AreaList(2, ProvinceID.Value).ToList();
            return Json(ProvinceList);
        }
        [HttpPost]
        public ActionResult LoadDistrict(int? CityID)
        {

            if (CityID == null)
                return Json(new List<SEC_Area>());
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();
            var ProvinceList = _AlipayPropertyAppService.GetSec_AreaList(3, CityID.Value).ToList();
            return Json(ProvinceList);
        }


        [HttpPost]
        public ActionResult SaveAlipayCommunity(Community SaveModel)
        {

            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();
            var resultModel = _AlipayPropertyAppService.SaveAlipayCommunity(SaveModel, CurrentAdminUser.RealName);
            
            return Json(resultModel);
        }



        #endregion

        #region 小区服务初始化
        public ActionResult AlipayBasicserviceList()
        {
            AlipayPropertyIndexData alipayPropertyIndexData = new AlipayPropertyIndexData();
            alipayPropertyIndexData.Language = this.Language;
            return View(alipayPropertyIndexData);
        }
        [HttpPost]
        public ActionResult GetAlipayCommunityBaseServerList(int DeptId)
        {
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();
            var AlipayCommunity=_AlipayPropertyAppService.GetAlipayCommunityBasicService(DeptId);
            return Json(AlipayCommunity);
        }


        public ActionResult AlipaiCommunityBasicserviceAddView(int AlipayCommunityId)
        {
            //获取修改信息

            AlipayCommunityAppService _AlipayCommunityAppService = new AlipayCommunityAppService();
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();

            var AlipayCommunityModel = _AlipayCommunityAppService.GetAlipayCommunityByKey(AlipayCommunityId);

           var PropertyAlipayInformCallBackUrl = ConfigurationManager.AppSettings["PropertyAlipayInformCallBackUrl"].ToString(); 
            AlipayCommunityBasicserviceDTO Model = new AlipayCommunityBasicserviceDTO()
            {
                community_id = AlipayCommunityModel.AlipayCommunityId,
                external_invoke_address = PropertyAlipayInformCallBackUrl

            };
            //  var Model = _AlipayPropertyAppService.GetBasicserviceInfomation(AlipayCommunityModel.AlipayCommunityId, AlipayCommunityModel.ProDeptId.Value);

            AlipayBasicserviceViewData model = new AlipayBasicserviceViewData()
            {
                BaseserviceModel = Model,
                IsReadOnly = false,
                IsAdd = false,
                BasicServiceStatusList = _AlipayPropertyAppService.GetBasicServiceStatus(),
                BasicServiceTypeList = _AlipayPropertyAppService.GetBasicServiceType()

            };
            return CreateAlipaiCommunityBaseserviceView(model);

        }
        public ActionResult AlipaiCommunityBasicserviceEditView(int AlipayCommunityId)
        {
            //获取修改信息

            AlipayCommunityAppService _AlipayCommunityAppService = new AlipayCommunityAppService();
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();

            var AlipayCommunityModel = _AlipayCommunityAppService.GetAlipayCommunityByKey(AlipayCommunityId);


            var Model=  _AlipayPropertyAppService.GetBasicserviceInfomation(AlipayCommunityModel.AlipayCommunityId, AlipayCommunityModel.ProDeptId.Value);

            AlipayBasicserviceViewData model = new AlipayBasicserviceViewData()
            {
                BaseserviceModel = Model,
                IsReadOnly = false,
                IsAdd =true,
                BasicServiceStatusList = _AlipayPropertyAppService.GetBasicServiceStatus(),
                BasicServiceTypeList = _AlipayPropertyAppService.GetBasicServiceType()

            };
            return CreateAlipaiCommunityBaseserviceView(model);

        }

        public ActionResult AlipaiCommunityBasicserviceShowView(int AlipayCommunityId)
        {
            //获取修改信息

            AlipayCommunityAppService _AlipayCommunityAppService = new AlipayCommunityAppService();
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();

            var AlipayCommunityModel = _AlipayCommunityAppService.GetAlipayCommunityByKey(AlipayCommunityId);


            var Model = _AlipayPropertyAppService.GetBasicserviceInfomation(AlipayCommunityModel.AlipayCommunityId, AlipayCommunityModel.ProDeptId.Value);

            AlipayBasicserviceViewData model = new AlipayBasicserviceViewData()
            {
                BaseserviceModel = Model,
                IsReadOnly = true,
                IsAdd =true,
                BasicServiceStatusList = _AlipayPropertyAppService.GetBasicServiceStatus(),
                BasicServiceTypeList = _AlipayPropertyAppService.GetBasicServiceType()

            };
            return CreateAlipaiCommunityBaseserviceView(model);

        }



        private ActionResult CreateAlipaiCommunityBaseserviceView(AlipayBasicserviceViewData Model)
        {
            return View("AlipayBasicserviceView", Model);
        }



        [HttpPost]
        public ActionResult SaveAlipayBasicService(AlipayCommunityBasicserviceDTO models)
        {
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();
           var result= _AlipayPropertyAppService.SaveAlipayCommunityBasicservice(models, CurrentAdminUser.RealName);

            return Json(result);
        }


        #endregion

        #region 小区房屋
        public ActionResult AlipayRoomList(int? ProDeptId)
        {
            AlipayPropertyIndexData alipayPropertyIndexData = new AlipayPropertyIndexData();
            var ComDeptList = this.GetComDeptListByPropertyDeptId(ProDeptId);
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();
            alipayPropertyIndexData.CommunityDeptList = ComDeptList;
            alipayPropertyIndexData.Language = this.Language;
            if (ComDeptList.Count > 0)
                alipayPropertyIndexData.DefaultComDeptId = ComDeptList[0].Id;
            alipayPropertyIndexData.TemplateModels = _AlipayPropertyAppService.GetALipayRoomTemplate();
            return View(alipayPropertyIndexData);
        }

        [HttpPost]
        public ActionResult GetAlipayRoomList(AlipayPropertySearchDTO search)
        {

            int outCount;

            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();

            var dataList = _AlipayPropertyAppService.GetALipayRoomList(search, out outCount);
            var jdata = new
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };
            return Json(jdata);

        }

      
        public List<DeptInfo> GetComDeptListByPropertyDeptId(int? ProDeptId)
        {
            DeptAppService _DeptAppService = new DeptAppService();
            var Deptinfo = _DeptAppService.GetCommunityDeptInfoByPropertyId(ProDeptId);
            return Deptinfo;
        }

        public ActionResult AlipayRoomView(int ComDeptId)
        {
            AlipayRoomViewData alipayRoomViewData = new AlipayRoomViewData();
            alipayRoomViewData.Language = this.Language;
            alipayRoomViewData.ComDeptId = ComDeptId;
            return View(alipayRoomViewData);
        }
        [HttpGet]
        public ActionResult GetAlipayHouseSelectTree(int ComDeptId)
        {
            var jResult = new JsonResult();
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jResult.MaxJsonLength = Int32.MaxValue;
            ChargBillAppService chargBillAppService = new ChargBillAppService();
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();
            jResult.Data = _AlipayPropertyAppService.GetAlipayRoomSelectHouseDeptList(ComDeptId);
            return jResult;
        }

        [HttpPost]
        public async  Task<ActionResult> SaveAlipayRoomUpload(AlipayRoomViewData savemodel)
        {
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();
            var Result= await _AlipayPropertyAppService.SaveAlipayRoomUpload(savemodel.TreeNodeList, savemodel.ComDeptId, CurrentAdminUser.RealName, CurrentAdminUser.Id.Value);
            return Json(Result);
        }

        [HttpPost]
        public ActionResult DeleteAlipayRoom(string Ids,int? ComDeptId)
        {
           AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();
           var Result= _AlipayPropertyAppService.DeleteAlipayRoom(Ids, ComDeptId);
           return Json(Result);
        }
        [HttpPost]
        public ActionResult  SynchronizationRoomInfo(int? ComDeptId)
        {
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();
            _AlipayPropertyAppService.SynchronizationRoomInfo(ComDeptId,CurrentAdminUser.RealName,CurrentAdminUser.Id.Value);
            ResultModel Result = new ResultModel() {  IsSuccess=true,Msg="提交成功，请稍后查询查看结果"};
            return Json(Result);
        }




        #endregion

        #region 账单
        public ActionResult AlipayChargeBillList(int? ProDeptId)
        {
            var ComDeptList = this.GetComDeptListByPropertyDeptId(ProDeptId);
            AlipayPropertyIndexData alipayPropertyIndexData = new AlipayPropertyIndexData();
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();
            alipayPropertyIndexData.Language = this.Language;
            alipayPropertyIndexData.CommunityDeptList = ComDeptList;
            if (ComDeptList.Count > 0)
                alipayPropertyIndexData.DefaultComDeptId = ComDeptList[0].Id;
            alipayPropertyIndexData.TemplateModels = _AlipayPropertyAppService.GetALipayChargeBillTemplate();
            return View(alipayPropertyIndexData);
        }

        [HttpPost]
        public ActionResult GetAlipayChargeBillList(AlipayPropertySearchDTO search)
        {
            int outCount=0;
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();
            var dataList = _AlipayPropertyAppService.GetAlipayChargeBillList(search, out outCount);
            var jdata = new
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };
            return Json(jdata);

        }

        public ActionResult AlipayChargeBillView(int? ComDeptId)
        {
            AlipayChargeBillViewData alipayChargeBillViewData = new AlipayChargeBillViewData();
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();
            if (ComDeptId != null&&ComDeptId>0)
            {
                ChargeSubjectAppService _ChargeSubjectAppService = new ChargeSubjectAppService();
                alipayChargeBillViewData.ChargeSubjectlist = _ChargeSubjectAppService.GetAllChargeSubjectsByComDeptId(ComDeptId.Value);
            }


            alipayChargeBillViewData.DefaultComDeptId = ComDeptId;
            alipayChargeBillViewData.Language = this.Language;
            alipayChargeBillViewData.TemplateModels = _AlipayPropertyAppService.GetALipayChargeBillViewTemplate();
            return View(alipayChargeBillViewData);
        }

        [HttpPost]
        public ActionResult GetAlipayChargeBillViewList(AlipayPropertySearchDTO search)
        {
            int outCount = 0;
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();
            var dataList =_AlipayPropertyAppService.GetAlipayChargeBillViewList(search, out outCount);
            var jdata = new
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };
            return Json(jdata);

        }

        [HttpPost]
        public ActionResult SaveAlipayChargeBillUpload(string Ids, int? ComDeptId)
        {
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();
            var idsList = Ids.Split(',').ToList();
            var ChargeBillIdList = idsList.Select(o=>o).ToList();
            var Result = _AlipayPropertyAppService.SaveAlipayChargeBillUpload(ChargeBillIdList, ComDeptId, CurrentAdminUser.RealName, CurrentAdminUser.Id.Value);
            return Json(Result);
        }

        [HttpPost]
        public ActionResult DeleteAlipayChargeBill(string Ids, int? ComDeptId)
        {
            AlipayPropertyAppService _AlipayPropertyAppService = new AlipayPropertyAppService();
            var idsList = Ids.Split(',').ToList();
            var AliayBillChargeBillIdList = idsList.Select(o => o).ToList();
            var Result = _AlipayPropertyAppService.DeleteAlipayChargeBill(idsList, ComDeptId);
            return Json(Result);
        }


        #endregion



    }


    public class AlipaiCommunityViewData
    {
        public Community CommunityModel { get; set; }
        public string QrCodeUrl { get; set; }
        public List<SEC_AreaDTO> ProvinceList { get; set; }
        public List<SEC_AreaDTO> CityList { get; set; }
        public List<SEC_AreaDTO> DistrictList { get; set; }
        public bool IsShow { get; set; }
        public bool IsShowQrCode { get; set; }
    }

    public class AlipayBasicserviceViewData
    {
        public AlipayCommunityBasicserviceDTO BaseserviceModel { get; set; }
        public List<Status> BasicServiceStatusList { get; set; }

        public List<Status> BasicServiceTypeList { get; set; }

        public bool IsReadOnly { get; set; }
        public bool IsAdd { get; set; }

    }

    public class AlipayRoomViewData
    {
        public int?[] TreeNodeList { get; set; }
        public int? ComDeptId { get; set; }
        public string Language { get; set; }
    }

    public class AlipayChargeBillViewData
    {
        public string[] ChargeBillList { get; set; }
        public int? DefaultComDeptId { get; set; }
        public List<ChargeSubjectDTO> ChargeSubjectlist { get; set; }
        public IEnumerable<TemplateModel> TemplateModels { get; set; }
        public string Language { get; set; }
    }
    



    public class AlipayPropertyIndexData
    {
        public string Language { get; set; }
        public List<DeptInfo> CommunityDeptList { get; set; }
        public int? DefaultComDeptId { get; set; }
        public IEnumerable<TemplateModel> TemplateModels { get; set; }



    }
}