using Newtonsoft.Json;
using PropertySysAPI.Accessor;
using PropertySysAPI.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using YK.Framework.ApplicationDTO;
using ZY_WebLibrary;

namespace YK.BackgroundMgr.MVCWeb.ZNMS
{
    /// <summary>
    /// ShareKeyService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class ShareKeyService : System.Web.Services.WebService
    {

        /// <summary>
        /// 新增分享钥匙
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="keys">用户钥匙KeyId 用分号隔开 eg:1723,1724 </param>
        /// <param name="setNums">开门次数</param>
        /// <param name="keyDate">过期时间</param>
        /// <returns></returns>
        [WebMethod]
        public ReturnResult AddShareKeys(string userId, string keys, int setNums, DateTime keyDate)
        {
            try
            {
                keys = keys.TrimEnd(',');
                if (string.IsNullOrEmpty(userId.ToString()))
                {
                    return new ReturnResult()
                    {
                        IsResult = false,
                        Msg = "请输入用户ID!",
                        Data = new object()
                    };
                }
                string privateKey = ConfigurationManager.AppSettings["PrivateKey"];
                YK.Framework.ApplicationDTO.UserOwnerInfo userInfo = PublicAPIHelper.GetUserOwnerInfo(userId);
                if (userInfo == null)
                {
                    return new ReturnResult()
                    {
                        IsResult = false,
                        Msg = "没有当前用户信息!",
                        Data = new object()
                    };
                }

                if (string.IsNullOrEmpty(setNums.ToString()))
                {
                    return new ReturnResult()
                    {
                        IsResult = false,
                        Msg = "请输入设置次数!",
                        Data = new object()
                    };
                }

                if (string.IsNullOrEmpty(keys))
                {
                    return new ReturnResult()
                    {
                        IsResult = false,
                        Msg = "请输入KeyId!",
                        Data = new object()
                    };
                }
                if (string.IsNullOrEmpty(keyDate.ToString()))
                {
                    return new ReturnResult()
                    {
                        IsResult = false,
                        Msg = "请输入时间!",
                        Data = new object()
                    };
                }
                if (!DateTime.TryParse(keyDate.ToString(), out keyDate))
                {
                    return new ReturnResult()
                    {
                        IsResult = false,
                        Msg = "请输入正确的时间!",
                        Data = new object()
                    };
                }
                if (!(setNums > 0))
                {
                    return new ReturnResult()
                    {
                        IsResult = false,
                        Msg = "请正确输入可开门次数!",
                        Data = new object()
                    };
                }
                if (!(keyDate > DateTime.Now))
                {
                    return new ReturnResult()
                    {
                        IsResult = false,
                        Msg = "过期时间必须大于当前时间!",
                        Data = new object()
                    };
                }
                ShareKeys model = new ShareKeys()
                {
                    UserId = userId,
                    Keys = keys,
                    SetNums = setNums,
                    UseNums = 0,
                    KeyDate = keyDate,
                    UpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now
                };
                ShareKeyAccessor shareKeyAcc = new ShareKeyAccessor();
                int res = shareKeyAcc.AddShareKey(model);
                if (!(res > 0))
                {

                    return new ReturnResult()
                    {
                        IsResult = false,
                        Msg = "处理失败!",
                        Data = new object()
                    };
                }
                else
                {

                    string encryptKey = string.Format("{0}_{1}_{2}_{4}_{3}", "yunkai-door", userInfo.BindingPhonerNumber, string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now), Guid.NewGuid(), res.ToString());
                    encryptKey = Utils.EncryptDES(encryptKey, privateKey);
                    return new ReturnResult()
                    {
                        IsResult = true,
                        Msg = "处理成功",
                        Data = encryptKey
                    };
                }
            }
            catch (Exception ex)
            {
                Utils.ErrorLog(ex.Message);
                return new ReturnResult()
                {
                    IsResult = false,
                    Msg = "数据异常" + ex,
                    Data = new object()
                };
            }
        }
        [WebMethod]
        public ReturnResult GetShareKeys(string shareKey)
        {

            try
            {
                string privateKey = ConfigurationManager.AppSettings["PrivateKey"];
                string decryptKey = Utils.DecryptDES(shareKey, privateKey);
                if (decryptKey.Split('_').Length != 5)
                {
                    Utils.ErrorLog("密钥解密错误");
                    return new ReturnResult()
                    {

                        IsResult = false,
                        Msg = "无效的二维码",
                        Data = "{" + string.Format("\"Name\":\"{0}\",\"Door\":\"{1}\",\"CommunityName\":\"{3}\",\"Phone\":\"{4}\",\"Keys\":{2}", "", "", "[]", "", "") + "}"

                    };
                }
                ShareKeyAccessor shareKeyAcc = new ShareKeyAccessor();
                return shareKeyAcc.GetShareKeys(WebTool.QeryNumber(decryptKey.Split('_')[3]));

            }
            catch (Exception ex)
            {
                List<Entrances> list = new List<Entrances>();
                Utils.ErrorLog(DateTime.Now.ToString() + ex.StackTrace + "</br>" + ex.Message);
                return new ReturnResult()
                {
                    IsResult = false,
                    Msg = "无效的二维码",
                    Data = "{" + string.Format("\"Name\":\"{0}\",\"Door\":\"{1}\",\"CommunityName\":\"{3}\",\"Phone\":\"{4}\",\"Keys\":{2}", "", "", "[]", "", "") + "}"
                };
            }
        }
        [WebMethod]
        public ReturnResult GetKeysByVillageId(string VillageId)
        {
            try
            {
                YK.Framework.ApplicationDTO.DeptInfo deptInfo = PublicAPIHelper.GetDeptInfo(Convert.ToInt32(VillageId));
                if (deptInfo == null)
                {
                    return new ReturnResult()
                    {
                        IsResult = false,
                        Msg = "没有当前小区信息",
                        Data = string.Empty
                    };
                }
                else
                {

                    EntranceAccessor entranceAcc = new EntranceAccessor();
                    DataTable dt = entranceAcc.CommonSearch("Entrances", "*", "VillageID=" + VillageId + " and State=1");
                    List<Entrances> list = new List<Entrances>();
                    list = ModelHelper.ConvertToModel<Entrances>(dt);
                    return new ReturnResult()
                    {
                        IsResult = true,
                        Msg = "获取成功!",
                        Data = JsonConvert.SerializeObject(list.Select(o => new { KeyId = o.KeyID, KeyName = o.Name, KeyAddress = o.Address }))
                    };
                }

            }
            catch (Exception ex)
            {
                Utils.ErrorLog(DateTime.Now.ToString() + ex.StackTrace + "</br>" + ex.Message);
                return new ReturnResult()
                {
                    IsResult = false,
                    Msg = "获取失败、数据异常!",
                    Data = string.Empty
                };
            }

        }

        [WebMethod]
        public ReturnResult EnrDeptInfo(int deptId)
        {
            try
            {
                DeptInfo deptInfo = PublicAPIHelper.GetDeptInfo(deptId);
                string key = "AB3DBA1B";
                string desString = Utils.EncryptDES(deptId.ToString() + "_" + string.Format("{0:yyyyMMddHHmmss}", DateTime.Now), key);
                desString = HttpUtility.UrlEncode(desString);
                if (null != deptInfo)
                {
                    return new ReturnResult() { Msg = "Key:" + desString + "名称:" + deptInfo.Name };
                }
                else
                {
                    return new ReturnResult() { Msg = "没有查到数据信息" };
                }
            }
            catch (Exception ex)
            {
                WebTool.GetBUG(ex);
                return new ReturnResult() { Msg = "数据异常" };

            }
        }

    }
}
