using Newtonsoft.Json;
using PropertySysAPI.Accessor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using YK.BackgroundMgr.MVCWeb.ZNMS;
using YK.PropertyMgr.ApplicationDTO;

namespace YK.PropertyMgr.MVCWeb.Controllers.WebApi
{
    public class EntrancePowerAPIController : ApiController
    {
        /// TODO:
        /// <summary>
        /// 获取用户的钥匙权限
        /// </summary>
        /// <param name="userOwnerId"></param>
        /// <returns></returns>
        //[System.Web.Mvc.HttpGet]
        //public string GetKeysByOwnerId(string userOwnerId)
        //{
        //    string OpenPermission = "FALSE";
        //    JsonResult jsonResult = new JsonResult() { Code = 0 };
        //    Guid guid;
        //    SetDataInfo setDataInfo = new SetDataInfo()
        //    {
        //        OpenPermission = ConfigurationManager.AppSettings["openPermission"].ToString(),
        //        MaxKeyExpireTime = "2999-12-30 00:00:00",
        //    };
        //    List<YK.Framework.ApplicationDTO.InterfaceDTO.UserOwnerEntrance> userOwnerEntranceList = new List<YK.Framework.ApplicationDTO.InterfaceDTO.UserOwnerEntrance>();
        //    try
        //    {
        //        guid = new Guid(userOwnerId);
        //        userOwnerEntranceList = PublicAPIHelper.GetUserOwnerEntrances(guid);
        //        /*是否开启自动授权,若没有开启那只能是业主才能授权*/
        //        if (setDataInfo.OpenPermission.ToUpper() == OpenPermission)
        //        {
        //            if (userOwnerEntranceList != null)
        //            {
        //                userOwnerEntranceList = userOwnerEntranceList.Where(o => o.PersonState == (int)PersonState.业主).ToList();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        jsonResult.Code = 2;
        //        jsonResult.Msg = setDataInfo.DicNoticMsg[jsonResult.Code];
        //        jsonResult.Keys = new List<EntranceKey>();
        //        return JsonConvert.SerializeObject(jsonResult);
        //    }
        //    return string.Empty;
        //}
    }
    //public class SetDataInfo
    //{
    //    public string OpenPermission { get; set; }
    //    public string MaxKeyExpireTime { get; set; }
    //    public Dictionary<int, string> DicNoticMsg
    //    {
    //        get
    //        {
    //            return new Dictionary<int, string>() {
    //            { -1, "数据异常" },
    //            { 0, "未知的错误" },
    //            { 1, "没有数据" },
    //            { 2, "用户ID格式错误" },
    //            { 3, "获取成功" } };
    //        }
    //    }
    //}
    //public class JsonResult
    //{
    //    public int Code { get; set; }
    //    public string Msg { get; set; }
    //    public List<EntranceKey> Keys { get; set; }
    //}
    //public class EntranceKey
    //{
    //    public string KeyID { get; set; }
    //    public string KeyExpireTime { get; set; }
    //    public string Name { get; set; }
    //    public string VillageName { get; set; }

    //}
}
