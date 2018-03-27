using Newtonsoft.Json;
using PropertySysAPI.Accessor;
using PropertySysAPI.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using YK.BackgroundMgr.PresentationService;
using YK.Framework.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO;

namespace YK.PropertyMgr.MVCWeb.Controllers.WebApi
{
    public class ShareKeyServiceController : ApiController
    {
        #region 新增分享钥匙

        /// <summary>
        /// 新增分享钥匙
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="keys">用户钥匙KeyId 用分号隔开 eg:1723,1724 </param>
        /// <param name="setNums">开门次数</param>
        /// <param name="keyDate">过期时间</param>
        /// <returns></returns>
        public JsonResult<APIResultDTO> AddShareKeys([FromBody]ShareKeyModel smodel)
        {
            try
            {
                smodel.keys = smodel.keys.TrimEnd(',');
                if (string.IsNullOrEmpty(smodel.userId.ToString()))
                {
                    return Json(new APIResultDTO()
                    {
                        Code = 701,
                        Message = "请输入用户ID!",
                        Data = new object()
                    });
                }
                string privateKey = ConfigurationManager.AppSettings["PrivateKey"];
                YK.Framework.ApplicationDTO.UserOwnerInfo userInfo = PublicAPIHelper.GetUserOwnerInfo(smodel.userId);
                if (userInfo == null)
                {
                    return Json(new APIResultDTO()
                    {
                        Code = 702,
                        Message = "没有当前用户信息!",
                        Data = new object()
                    });
                }

                if (string.IsNullOrEmpty(smodel.setNums.ToString()))
                {
                    return Json(new APIResultDTO()
                    {
                        Code = 703,
                        Message = "请输入设置次数!",
                        Data = new object()
                    });
                }

                if (string.IsNullOrEmpty(smodel.keys))
                {
                    return Json(new APIResultDTO()
                    {
                        Code = 704,
                        Message = "请输入KeyId!",
                        Data = new object()
                    });
                }
                if (string.IsNullOrEmpty(smodel.keyDate.ToString()))
                {
                    return Json(new APIResultDTO()
                    {
                        Code = 705,
                        Message = "请输入时间!",
                        Data = new object()
                    });
                }
                if (!(smodel.setNums > 0))
                {
                    return Json(new APIResultDTO()
                    {
                        Code = 706,
                        Message = "请正确输入可开门次数!",
                        Data = new object()
                    });
                }
                if (!(smodel.keyDate > DateTime.Now))
                {
                    return Json(new APIResultDTO()
                    {
                        Code = 707,
                        Message = "过期时间必须大于当前时间!",
                        Data = new object()
                    });
                }
                ShareKeys model = new ShareKeys()
                {
                    UserId = smodel.userId,
                    Keys = smodel.keys,
                    SetNums = smodel.setNums,
                    UseNums = 0,
                    KeyDate = smodel.keyDate,
                    UpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now
                };
                ShareKeyAccessor shareKeyAcc = new ShareKeyAccessor();
                int res = shareKeyAcc.AddShareKey(model);
                if (!(res > 0))
                {

                    return Json(new APIResultDTO()
                    {
                        Code = 708,
                        Message = "处理失败!",
                        Data = new object()
                    });
                }
                else
                {

                    string encryptKey = string.Format("{0}_{1}_{2}_{4}_{3}", "yunkai-door", userInfo.BindingPhonerNumber, string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now), Guid.NewGuid(), res.ToString());
                    encryptKey = Utils.EncryptDES(encryptKey, privateKey);
                    return Json(new APIResultDTO()
                    {
                        Code = 0,
                        Message = "处理成功",
                        Data = encryptKey
                    });
                }
            }
            catch (Exception ex)
            {
                Utils.ErrorLog(ex.Message);
                return Json(new APIResultDTO()
                {
                    Code = 901,
                    Message = "数据异常" + ex,
                    Data = new object()
                });
            }
        }

        #endregion

        #region 获取钥匙     
        [HttpPost]
        public JsonResult<APIResultDTO> GetShareKeys([FromBody]KeyModel kmodel)
        {

            try
            {
                string privateKey = ConfigurationManager.AppSettings["PrivateKey"];
                string decryptKey = Utils.DecryptDES(kmodel.shareKey, privateKey);
                if (decryptKey.Split('_').Length != 5)
                {
                    Utils.ErrorLog("密钥解密错误");
                    return Json(new APIResultDTO()
                    {

                        Code = 701,
                        Message = "无效的二维码",
                        //Data = "{" + string.Format("\"Name\":\"{0}\",\"Door\":\"{1}\",\"CommunityName\":\"{3}\",\"Phone\":\"{4}\",\"Keys\":{2}", "", "", "[]", "", "") + "}"
                        Data = new { Name = "", Door = new string[0], CommunityName = "", Phone = "", Keys = ""}
                    });
                }
                ShareKeyAccessor shareKeyAcc = new ShareKeyAccessor();
                var result = shareKeyAcc.GetShareKeys(int.Parse(decryptKey.Split('_')[3]));
                APIResultDTO apiResult = new APIResultDTO();
                apiResult.Code = result.IsResult ? 0 : 702;
                apiResult.Message = result.Msg;
                apiResult.Data = result.Data;
                return Json(apiResult);
            }
            catch (Exception ex)
            {
                List<Entrances> list = new List<Entrances>();
                Utils.ErrorLog(DateTime.Now.ToString() + ex.StackTrace + "</br>" + ex.Message);
                return Json(new APIResultDTO()
                {
                    Code = 901,
                    Message = "无效的二维码",
                    //Data = "{" + string.Format("\"Name\":\"{0}\",\"Door\":\"{1}\",\"CommunityName\":\"{3}\",\"Phone\":\"{4}\",\"Keys\":{2}", "", "", "[]", "", "") + "}"
                    Data = new { Name = "", Door = new string[0], CommunityName = "", Phone = "", Keys = "" }
                });
            }
        }

        #endregion

        #region 获取小区钥匙
        [HttpPost]
        public JsonResult<APIResultDTO> GetKeysByVillageId([FromBody]KeyModel kmodel)
        {
            try
            {
                YK.Framework.ApplicationDTO.DeptInfo deptInfo = PublicAPIHelper.GetDeptInfo(Convert.ToInt32(kmodel.VillageId));
                if (deptInfo == null)
                {
                    return Json(new APIResultDTO()
                    {
                        Code = 701,
                        Message = "没有当前小区信息",
                        Data = string.Empty
                    });
                }
                else
                {

                    EntranceAccessor entranceAcc = new EntranceAccessor();
                    DataTable dt = entranceAcc.CommonSearch("Entrances", "*", "VillageID=" + kmodel.VillageId + " and State=1");
                    List<Entrances> list = new List<Entrances>();
                    list = ModelHelper.ConvertToModel<Entrances>(dt);
                    return Json(new APIResultDTO()
                    {
                        Code = 0,
                        Message = "获取成功!",
                        Data = JsonConvert.SerializeObject(list.Select(o => new { KeyId = o.KeyID, KeyName = o.Name, KeyAddress = o.Address }))
                    });
                }

            }
            catch (Exception ex)
            {
                Utils.ErrorLog(DateTime.Now.ToString() + ex.StackTrace + "</br>" + ex.Message);
                return Json(new APIResultDTO()
                {
                    Code = 901,
                    Message = "获取失败、数据异常!",
                    Data = string.Empty
                });
            }

        }

        #endregion

        [HttpPost]
        public JsonResult<APIResultDTO> EnrDeptInfo([FromBody]KeyModel kmodel)
        {
            try
            {
                DeptInfo deptInfo = PublicAPIHelper.GetDeptInfo(kmodel.deptId);
                string key = ConfigurationManager.AppSettings["PrivateKey"];
                string desString = Utils.EncryptDES(kmodel.deptId.ToString() + "_" + string.Format("{0:yyyyMMddHHmmss}", DateTime.Now), key);
                desString = HttpUtility.UrlEncode(desString);
                if (null != deptInfo)
                {
                    return Json(new APIResultDTO() { Code = 0, Message = "Key:" + desString + "名称:" + deptInfo.Name, Data = deptInfo });
                }
                else
                {
                    return Json(new APIResultDTO() { Code = 501, Message = "没有查到数据信息" });
                }
            }
            catch (Exception ex)
            {
                Utils.ErrorLog(DateTime.Now.ToString() + ex.StackTrace + "</br>" + ex.Message);
                return Json(new APIResultDTO() { Code = 901, Message = "数据异常" });

            }
        }

    }

    public class ShareKeyModel
    {
        /// <summary>
        /// 用户的OwnerID来源于基础组
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 用户钥匙KeyId 用分号隔开 eg:1723,1724
        /// </summary>
        public string keys { get; set; }
        /// <summary>
        /// 开门次数
        /// </summary>
        public int setNums { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime keyDate { get; set; }
    }

    public class KeyModel
    {
        /// <summary>
        /// 二维码的加密信息
        /// </summary>
        public string shareKey { get; set; }
        /// <summary>
        /// 小区ID
        /// </summary>
        public string VillageId { get; set; }
        /// <summary>
        /// 房屋ID
        /// </summary>
        public int deptId { get; set; }
    }
}