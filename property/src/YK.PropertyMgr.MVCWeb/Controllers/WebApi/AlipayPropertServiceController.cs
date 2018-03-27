using Newtonsoft.Json;
using PropertyAlipay.Entity.model;
using PropertyAlipay.Service.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.Crosscuting;

namespace YK.PropertyMgr.MVCWeb.Controllers.WebApi
{
    public class AlipayPropertServiceController : ApiController
    {
        [HttpGet]
        public JsonResult<APIResultDTO> AlipayPropertyToken(int? DeptId,string app_id, string source,string app_auth_code)
        {
            //记录日志
            LogProperty.WriteLoginToFile(string.Format("BuildId:{0}", DeptId), "AlipayPropertService/AlipayPropertyToken", FileLogType.Info);
     
            try
            {
                AlipayAPPAuthTokenAppService _AlipayAPPAuthTokenAppService = new AlipayAPPAuthTokenAppService();
                bool boolResult =_AlipayAPPAuthTokenAppService.SaveAppAuthToken(DeptId,true, app_auth_code);

                if (!boolResult)
                {
                    LogProperty.WriteLoginToFile(string.Format("BuildId:{0}", DeptId, "保存到数据库失败，请重新获取"), "AlipayPropertService/AlipayPropertyToken", FileLogType.Exception);
                    return Json(new APIResultDTO() { Code = 901, Message = "获取成功，但保存到数据库失败，请重试或者联系管理员2" });
                }
                
                APIResultDTO aPIResultDTO = new APIResultDTO()
                {
                     
                    Message = "授权令牌获取成功",
                    Code = 10000
                };

                 

                return Json(aPIResultDTO) ;
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("BuildId:{0}  ErrorMessage:{1}", DeptId, ex), "AlipayPropertService/AlipayPropertyToken", FileLogType.Exception);
                return Json(new APIResultDTO() { Code = 901, Message = "获取失败"+ex.Message });
            }
        }

        [HttpPost]
        public void  AlipayInform()
        {

            LogProperty.WriteLoginToFile(string.Format("支付宝回调进入"), "AlipayPropertService/AlipayInform", FileLogType.Info);
            LogProperty.WirteFrameworkLog("0", "admin", "AlipayInform", "AlipayInform", "ALipay", "支付宝回调进入");
            var request = HttpContext.Current.Request;
            NameValueCollection collection = request.Form;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            foreach (var str in collection.AllKeys)
            {
                sArray.Add(str, collection[str]);
            }
               
            LogProperty.WriteLoginToFile(string.Format("返回信息"+ sArray.ToString()), "AlipayPropertService/AlipayInform", FileLogType.Info);


            LogProperty.WirteFrameworkLog("0", "admin", "AlipayInform", "AlipayInform", "admin", collection.ToString());
           
           

        }


    }
}
