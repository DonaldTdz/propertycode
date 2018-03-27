using Newtonsoft.Json;
using PropertySysAPI.Entity;
using System.Linq;
using System.Data;
using ZY_DAL;
using System.Collections.Generic;
using System;
using YK.PropertyMgr.ApplicationDTO;

namespace PropertySysAPI.Accessor
{
    public class ShareKeyAccessor
    {
        /// <summary>
        /// 新增分享钥匙
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddShareKey(ShareKeys model)
        {
            return Insert.InsertReturnid<ShareKeys>(model);
        }

        /// <summary>
        /// 查询分享钥匙
        /// </summary>
        /// <param name="shareKeyId"></param>
        /// <returns></returns>
        public ReturnResult GetShareKeys(int shareKeyId)
        {

            object lcokModel = new object();
            string data = string.Empty;
            DataTable dt = new DataTable();
            ShareKeys model = new SqlDAL().GetValue<ShareKeys>("ShareKeys", "Id=" + shareKeyId, string.Empty);
            List<Entrances> list = new List<Entrances>();
            string communityName = string.Empty;
            if (model != null)
            {

                YK.Framework.ApplicationDTO.UserOwnerInfo userInfo = PublicAPIHelper.GetUserOwnerInfo(model.UserId);
                if (!string.IsNullOrEmpty(model.UserId))
                {
                    List<YK.Framework.ApplicationDTO.CommunityInfo> communityList = PublicAPIHelper.GetCommunityInfoByOwnerId(new Guid(model.UserId));
                    communityList.ForEach(o =>
                    {
                        communityName += o.Name + ",";
                    });
                    communityName = communityName.TrimEnd(',');
                }

                if (Convert.ToDateTime(model.KeyDate.ToShortDateString() + " 23:59:59") < DateTime.Now)
                {
                    //当前分享的钥匙已经过期
                    return new ReturnResult()
                    {
                        IsResult = false,
                        Msg = "二维码已失效",
                        Data = "{" + string.Format("\"Name\":\"{0}\",\"Door\":\"{1}\",\"CommunityName\":\"{3}\",\"Phone\":\"{4}\",\"Keys\":{2}", userInfo.UserName, userInfo.DoorNo, "[]", communityName, userInfo.BindingPhonerNumber) + "}"
                    };
                }
                if (string.IsNullOrEmpty(model.Keys))
                {
                    return new ReturnResult()
                    {
                        IsResult = false,
                        Msg = "没有keys信息",
                        Data = new object()
                    };
                }
                dt = new SqlDAL().DataQuery("Entrances", "KeyID in(" + model.Keys + ")", string.Empty).Tables[0];
                list = ModelHelper.ConvertToModel<Entrances>(dt);

                if (userInfo != null)
                {
                    data = "{" + string.Format("\"Name\":\"{0}\",\"Door\":\"{1}\",\"CommunityName\":\"{3}\",\"Phone\":\"{4}\",\"Keys\":{2}", userInfo.UserName, userInfo.DoorNo, JsonConvert.SerializeObject(list.Select(o => new { KeyId = o.KeyID, KeyName = o.Name, KeyAddress = o.Address })), communityName, userInfo.BindingPhonerNumber) + "}";

                }
                if (list.Count > 0)
                {
                    lock (lcokModel)
                    {
                        if (model.SetNums > model.UseNums)
                        {

                            int useNums = (int)model.UseNums + 1;
                            model.UseNums = useNums;
                            Update.Updatemodle<ShareKeys>(model, "Id=" + model.Id);
                            return new ReturnResult()
                            {
                                IsResult = true,
                                Msg = "处理成功",
                                Data = data
                            };
                        }
                        else
                        {
                            //分享开门次数已经用完
                            return new ReturnResult()
                            {
                                IsResult = false,
                                Msg = "二维码已失效",
                                Data = "{" + string.Format("\"Name\":\"{0}\",\"Door\":\"{1}\",\"CommunityName\":\"{3}\",\"Phone\":\"{4}\",\"Keys\":{2}", userInfo.UserName, userInfo.DoorNo, "[]", communityName, userInfo.BindingPhonerNumber) + "}"

                            };
                        }
                    }
                }
                else
                {
                    return new ReturnResult()
                    {
                        IsResult = false,
                        Msg = "未查到分享的钥匙数据信息",
                        Data = "{" + string.Format("\"Name\":\"{0}\",\"Door\":\"{1}\",\"CommunityName\":\"{3}\",\"Phone\":\"{4}\",\"Keys\":{2}", userInfo.UserName, userInfo.DoorNo, "[]", communityName, userInfo.BindingPhonerNumber) + "}"
                    };
                }
            }
            else
            {
                return new ReturnResult()
                {
                    IsResult = false,
                    Msg = "未查到分享数据信息",
                    Data = "{" + string.Format("\"Name\":\"{0}\",\"Door\":\"{1}\",\"CommunityName\":\"{3}\",\"Phone\":\"{4}\",\"Keys\":{2}", "", "", "[]", "", "") + "}"

                };
            }

        }
    }
}
