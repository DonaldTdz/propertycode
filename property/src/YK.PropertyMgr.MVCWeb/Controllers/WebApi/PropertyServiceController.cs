using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.Crosscuting;

namespace YK.PropertyMgr.MVCWeb.Controllers.WebApi
{
    public class PropertyServiceController : ApiController
    {
        #region 车位修改通知

        [HttpPost]
        public JsonResult<APIResultDTO> CarportChangeNotice([FromBody]JObject jpara)
        {

            APICarportChangeParameter para = jpara.ToObject<APICarportChangeParameter>();
            //记录日志
            LogProperty.WriteLoginToFile(string.Format("HouseDeptId:{0} CarportId:{1}  ActionStatus:{2}", para.HouseDeptId, para.CarportId, para.ActionStatus), "PropertyService/CarportChangeNotice", FileLogType.Info);
            para.HouseDeptId = para.HouseDeptId ?? 0;
            para.CarportId = para.CarportId ?? 0;
            para.RelieveOperator = para.RelieveOperator ?? -1;
            //解除绑定逻辑
            SubjectHouseRefAppService SubjectHouseRef = new SubjectHouseRefAppService();
            try
            {
                SubjectHouseRef.CarportChangeNotice(para);
                return Json(new APIResultDTO() { Code = 0, Message = "通知成功" });
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("HouseDeptId:{0} CarportId:{1}  Exception:{2}", para.HouseDeptId, para.CarportId, ex), "PropertyService/CarportChangeNotice", FileLogType.Exception);
                return Json(new APIResultDTO() { Code = 901, Message = "通知异常" });
            }
            
        }

        #endregion
    }

}