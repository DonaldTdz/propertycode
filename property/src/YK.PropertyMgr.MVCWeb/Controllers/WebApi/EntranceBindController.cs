using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.ApplicationService.Service;
using YK.PropertyMgr.CompositeDomainService;
using YK.PropertyMgr.Crosscuting;

namespace YK.PropertyMgr.MVCWeb.Controllers.WebApi
{
    public class EntranceBindController : ApiController
    {
        #region Page_Load
        private ResultModel Login(string userName, string password)
        {
            ResultModel resModel = new ResultModel();
            var userInfo = PresentationServiceHelper.LookUp<IPropertyService>().Login(userName, password);
            if (userInfo == null)
            {
                resModel.IsSuccess = false;
                resModel.Msg = "没有当前用户信息";
            }
            else
            {

            }
            return resModel;
        }
        #endregion

        #region 获取小区信息
        /// <summary>
        /// 获取小区信息
        /// </summary>
        /// <param name="villageName">小区名称</param>
        /// <returns></returns>
        private JsonResult<ResultModel> GetVillageByName(string villageName)
        {
            ResultModel resModel = new ResultModel();
            try
            {
                EntranceChangeAppService service = new EntranceChangeAppService();
                var list = service.GetSECVillage(villageName);
                if (list.Count > 0)
                {
                    resModel.IsSuccess = true;
                    resModel.Data = list;
                }
                else
                {
                    resModel.IsSuccess = false;
                    resModel.Data = null;
                    resModel.Msg = "没有数据信息";
                }
                return Json(resModel);
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile("GetVillageByName 异常：" + ex.Message, "WebApi/EntranceBindController", FileLogType.Exception);
                resModel.IsSuccess = false;
                resModel.Data = null;
                resModel.Msg = "数据异常";
                return Json(resModel);
            }
        }
        #endregion

        #region 获取小区信息
        /// <summary>
        /// 获取小区信息
        /// </summary>
        /// <param name="villageName">小区名称</param>
        /// <returns></returns>
        public JsonResult<ResultModel> GetVillageByCity(string cityName)
        {
            ResultModel resModel = new ResultModel();
            try
            {
                if (string.IsNullOrEmpty(cityName))
                {
                    resModel.IsSuccess = false;
                    resModel.Msg = "请输入城市的名称";
                    resModel.Data = null;
                    return Json(resModel);
                }

                EntranceChangeAppService service = new EntranceChangeAppService();
                var list = service.GetSECVillageByCity(cityName);
                if (list.Count > 0)
                {
                    resModel.IsSuccess = true;
                    resModel.Data = list;
                }
                else
                {
                    resModel.IsSuccess = false;
                    resModel.Data = null;
                    resModel.Msg = "没有数据信息";
                }
                return Json(resModel);
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile("GetVillageByCity 异常：" + ex.Message, "WebApi/EntranceBindController", FileLogType.Exception);
                resModel.IsSuccess = false;
                resModel.Data = null;
                resModel.Msg = "数据异常";
                return Json(resModel);
            }
        }

        #endregion

        #region 获取大门信息
        /// <summary>
        /// 获取大门信息
        /// </summary>
        /// <param name="villageId"></param>
        /// <returns></returns>
        public JsonResult<ResultModel> GetEntranceDoors(int villageId)
        {
            ResultModel resModel = new ResultModel();
            try
            {
                if (!(villageId > 0))
                {
                    resModel.IsSuccess = false;
                    resModel.Msg = "请选择小区";
                    return Json(resModel);
                }
                EntranceChangeAppService service = new EntranceChangeAppService();
                var res = service.GetEntranceDTOList(villageId);

                if (res.Count > 0)
                {
                    resModel.IsSuccess = true;
                    resModel.Data = res;
                    resModel.Msg = "成功获取信息";
                }
                else
                {
                    resModel.IsSuccess = false;
                    resModel.Msg = "没有数据信息";
                }
                return Json(resModel);

            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile("GetEntranceDoors 异常：" + ex.Message, "WebApi/EntranceBindController", FileLogType.Exception);
                resModel.IsSuccess = false;
                resModel.Msg = "数据异常:"+ex.Message;
                return Json(resModel);
            }
        }

        /// <summary>
        /// 获取大门信息
        /// </summary>
        /// <param name="villageId"></param>
        /// <returns></returns>
        public JsonResult<ResultModel> GetBindEntranceDoors(int villageId)
        {
            ResultModel resModel = new ResultModel();
            try
            {
                if (!(villageId > 0))
                {
                    resModel.IsSuccess = false;
                    resModel.Msg = "请选择小区";
                    return Json(resModel);
                }
                EntranceChangeAppService service = new EntranceChangeAppService();
                var res = service.GetBindEntranceDTOList(villageId);

                if (res.Count > 0)
                {
                    resModel.IsSuccess = true;
                    resModel.Data = res;
                    resModel.Msg = "成功获取信息";
                }
                else
                {
                    resModel.IsSuccess = false;
                    resModel.Msg = "没有数据信息";
                }
                return Json(resModel);

            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile("GetBindEntranceDoors 异常：" + ex.Message, "WebApi/EntranceBindController", FileLogType.Exception);
                resModel.IsSuccess = false;
                resModel.Msg = "数据异常:" + ex.Message;
                return Json(resModel);
            }
        }




        #endregion  

        #region 绑定设备KEY
        /// <summary>
        /// 绑定设备KEY
        /// </summary>
        /// <param name="entranceId"></param>
        /// <param name="newKeyId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult<ResultModel> ChangeEntranceKey([FromBody]APIEntrancs parms)
        {
            
            ResultModel res = new ResultModel();
            res.IsSuccess = true;
            try
            {
                List<APIEntrancParameter> list = JsonConvert.DeserializeObject<List<APIEntrancParameter>>(parms.APIEntrancList);
                if (list == null)
                {
                    res.IsSuccess = false;
                    res.Msg = "没有获取到对象值";
                    return Json(res);
                }
                EntranceChangeAppService changeService = new EntranceChangeAppService();
                return Json(changeService.ChangeEntranceKey(list));
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile("ChangeEntranceKey 异常：" + ex, "WebApi/EntranceBindController", FileLogType.Exception);
                res.IsSuccess = false;
                res.Msg = "数据异常"+ ex.Message;
                return Json(res);
            }
        }
        #endregion

        #region 解除绑定
        /// <summary>
        /// 解除绑定
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult<ResultModel> NoBindKey([FromBody]APIEntrancs parms)
        {

            ResultModel res = new ResultModel();
            res.IsSuccess = true;
            try
            {
                List<APIEntrancParameter> list = JsonConvert.DeserializeObject<List<APIEntrancParameter>>(parms.APIEntrancList);
                if (list == null)
                {
                    res.IsSuccess = false;
                    res.Msg = "没有获取到对象值";
                    return Json(res);
                }
                EntranceChangeAppService changeService = new EntranceChangeAppService();
                return Json(changeService.NoBindEntrance(list));
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile("NoBindKey 异常：" + ex, "WebApi/EntranceBindController", FileLogType.Exception);
                res.IsSuccess = false;
                res.Msg = "数据异常" + ex.Message ;
                return Json(res);
            }
        }
        #endregion

        #region 获取服务器时间
        public JsonResult<ResultModel> GetServerDateTime()
        {
            ResultModel resModel = new ResultModel()
            {
                IsSuccess = true,
                Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Msg = "成功获取服务器时间"
            };

            return Json(resModel);
        }

        #endregion
    }

}
