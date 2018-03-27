using Newtonsoft.Json;
using PropertySysAPI.Accessor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Services;
using YK.BackgroundMgr.DomainInterface;

namespace YK.BackgroundMgr.MVCWeb.ZNMS
{

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class ZNMSService : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetUserDeptInfoById(string userOwnerId)
        {
            Guid guid;
            List<YK.Framework.ApplicationDTO.InterfaceDTO.UserOwnerEntrance> userOwnerEntranceList = new List<YK.Framework.ApplicationDTO.InterfaceDTO.UserOwnerEntrance>();
            try
            {
                guid = new Guid(userOwnerId);
                userOwnerEntranceList = PublicAPIHelper.GetUserOwnerEntrances(guid);
                if (userOwnerEntranceList == null)
                {
                    return "没有查到用户的小区楼栋信息!";
                }
                else
                {
                    return JsonConvert.SerializeObject(userOwnerEntranceList);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        [WebMethod]
        public string GetKeysByOwnerId(string userOwnerId)
        {
            string openPermission = ConfigurationManager.AppSettings["openPermission"].ToString();
            string msg = string.Empty;
            string maxKeyExpireTime = "2999-12-30 00:00:00";
            Dictionary<int, string> dic = new Dictionary<int, string>() {
                { 0, "未知的错误" },
                { 1, "没有数据" },
                { 2, "用户ID格式错误" },
                { 3, "获取成功" },
                { -1, "数据异常" }
            };
            Guid guid;
            Key key = new Key();
            List<Key> keyList = new List<Key>();
            List<Key> keyOwnerList = new List<Key>();
            DataTable dt = new DataTable();
            EntranceAccessor db = new EntranceAccessor();
            JsonResult jsonResult = new JsonResult() { Code = "0" };
            List<int?> villageDeptList = new List<int?>();
            List<string> unitsDeptList = new List<string>();
            List<int> buildsDeptList = new List<int>();
            List<YK.Framework.ApplicationDTO.InterfaceDTO.UserOwnerEntrance> userOwnerEntranceList = new List<YK.Framework.ApplicationDTO.InterfaceDTO.UserOwnerEntrance>();

            StringBuilder userIds = new StringBuilder();
            try
            {
                guid = new Guid(userOwnerId);
            }
            catch (Exception ex)
            {
                Utils.ErrorLog(ex.Message);
                jsonResult.Code = "2";
                jsonResult.Msg = dic[2];
                jsonResult.Keys = new List<EntranceKey>();
                return JsonConvert.SerializeObject(jsonResult);
            }
            try
            {
                userOwnerEntranceList = PublicAPIHelper.GetUserOwnerEntrances(guid);
                //add by donald 当查不到用户给取空权限 2017-2-24
                if (userOwnerEntranceList == null)
                {
                    jsonResult.Code = "1";
                    jsonResult.Msg = dic[1];
                    jsonResult.Keys = new List<EntranceKey>();
                    return JsonConvert.SerializeObject(jsonResult);
                }
                /*是否开启自动授权,若没有开启那只能是业主才能授权*/
                if (openPermission.ToUpper() == "FALSE")
                {
                    userOwnerEntranceList = userOwnerEntranceList.Where(o => o.PersonState == (int)PersonState.业主).ToList();
                }

                if (null != userOwnerEntranceList && userOwnerEntranceList.Count > 0)
                {
                    var userOwnerEntranceIsPermissionTrueList = userOwnerEntranceList.Where(o => o.IsPermission != 0).ToList();
                    string deptId = string.Empty;
                    string unitNames = string.Empty;
                    string buidIds = string.Empty;
                    string tab = "Entrances";
                    string row = "Id,KeyID,'" + maxKeyExpireTime + "' as KeyExpireTime,Name,VillageID";
                    string where = string.Empty;

                    /*01-自动权限*/
                    foreach (YK.Framework.ApplicationDTO.InterfaceDTO.UserOwnerEntrance item in userOwnerEntranceIsPermissionTrueList)
                    {
                        if (!string.IsNullOrEmpty(item.CommunityDeptId.ToString()) && !villageDeptList.Contains(item.CommunityDeptId))
                        {
                            deptId = deptId + " VillageID=" + item.CommunityDeptId + " or ";
                            villageDeptList.Add(item.CommunityDeptId);
                        }
                        if (!string.IsNullOrEmpty(item.UnitName.ToString()))
                        {
                            unitNames += " (UnitName='" + item.UnitName + "'" + " AND BuildId=" + item.BuildingDeptId + ") OR ";
                        }
                        if (!string.IsNullOrEmpty(item.BuildingDeptId.ToString()) && !buildsDeptList.Contains(item.BuildingDeptId))
                        {
                            buidIds += "'" + item.BuildingDeptId + "'" + ",";
                            buildsDeptList.Add(item.BuildingDeptId);

                        }
                    }
                    buidIds = buidIds.TrimEnd(',');
                    if (!string.IsNullOrEmpty(deptId))
                    {
                        deptId = deptId.Substring(0, deptId.Length - 3);
                    }
                    if (!string.IsNullOrEmpty(unitNames))
                    {
                        unitNames = unitNames.Substring(0, unitNames.Length - 3);
                    }
                    string buildWhere = " BuildId in (" + buidIds + ")";
                    // string unitNameWhere = "UnitName in (" + unitNames + ")";
                    /*小区的锁*/
                    if (!string.IsNullOrEmpty(deptId))
                    {
                        where += "  ((" + deptId + ")" + " and (BuildId is null or BuildId='') and (UnitName is null or UnitName=''))";
                    }
                    /*楼栋的锁*/
                    if (!string.IsNullOrEmpty(buidIds))
                    {
                        where += "or (" + buildWhere + " and ((UnitName is null or UnitName='')))";
                    }
                    /*单元的锁*/
                    if (!string.IsNullOrEmpty(unitNames))
                    {
                        where += "or (" + unitNames + ")";
                    }
                    if (!string.IsNullOrEmpty(where))
                    {
                        if (!string.IsNullOrEmpty(where))
                        {
                            where = "   State = 1 and " + where;
                        }
                        dt = db.CommonSearch(tab, row, where);
                        keyList = ModelHelper.ConvertToModel<Key>(dt);
                    }
                }
                /*02-手动权限*/
                List<Key> handOperateList = new List<Key>();
                foreach (var item in userOwnerEntranceList)
                {
                    userIds.Append(string.Format("'{0}',", item.UserOwnerId));
                }
                if (!string.IsNullOrEmpty(userIds.ToString()))
                {
                    handOperateList = ModelHelper.ConvertToModel<Key>(db.GetEntranceUserByUserId(userIds.ToString().TrimEnd(',')).Tables[0]);
                }
                foreach (Key item in handOperateList)
                {
                    if (Convert.ToDateTime(item.KeyExpireTime) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")))
                    {
                        if (!keyList.Any(o => o.Id == item.Id))
                        {
                            item.KeyExpireTime = Convert.ToDateTime(item.KeyExpireTime).ToString("yyyy-MM-dd 00:00:00");
                            keyList.Add(item);
                        }
                    }
                }
                /*03-根据单元锁赋值楼栋锁 BEGIN*/
                if (handOperateList.Count > 0)
                {
                    DataTable dtUnit = DSKeyUnit(userIds.ToString().TrimEnd(','));
                    DataTable dtBuild = new DataTable();
                    string builds = string.Empty;
                    foreach (DataRow dr in dtUnit.Rows)
                    {
                        builds += "'" + dr["BuildId"].ToString() + "'" + ",";
                    }
                    if (!string.IsNullOrEmpty(builds))
                    {
                        builds = builds.TrimEnd(',');
                        try
                        {
                            dtBuild = DSKeyBuild(builds);
                        }
                        catch (Exception)
                        {
                            jsonResult.Code = "-1";
                            jsonResult.Msg = dic[-1];
                            jsonResult.Keys = new List<EntranceKey>();
                            return JsonConvert.SerializeObject(jsonResult);
                        }
                    }
                    if (dtBuild.Rows.Count > 0)
                    {
                        foreach (Key itemKey in ModelHelper.ConvertToModel<Key>(dtBuild))
                        {
                            if (Convert.ToDateTime(itemKey.KeyExpireTime) >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")))
                            {
                                if (!keyList.Any(o => o.Id == itemKey.Id))
                                {
                                    keyList.Add(itemKey);
                                }
                            }
                        }
                    }
                }
                /*根据单元锁赋值楼栋锁 END*/
                if (keyList.Count > 0)
                {
                    villageDeptList.Clear();
                    keyList.ForEach(o =>
                    {
                        if (!villageDeptList.Contains(o.VillageID))
                        {
                            villageDeptList.Add(o.VillageID);
                        }
                    });
                    var villageList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptsByIds(villageDeptList);
                    StringBuilder sb = new StringBuilder();

                    foreach (var o in keyList)
                    {
                        if (villageList.Any(m => m.Id == o.VillageID))
                        {
                            o.VillageName = villageList.SingleOrDefault(n => n.Id == o.VillageID).Name;
                            if (string.IsNullOrEmpty(o.VillageName))
                            {
                                o.VillageName = "";
                            }
                        }
                        else
                        {
                            o.VillageName = "";
                        }
                    }

                    jsonResult.Code = "3";
                    jsonResult.Msg = dic[3];
                    jsonResult.Keys = keyList.Select(o => new EntranceKey { KeyExpireTime = o.KeyExpireTime, Name = o.Name, KeyID = o.KeyID, VillageName = o.VillageName }).ToList();
                    return JsonConvert.SerializeObject(jsonResult);
                }
                else
                {
                    jsonResult.Code = "1";
                    jsonResult.Msg = dic[1];
                    jsonResult.Keys = keyList.Select(o => new EntranceKey { KeyExpireTime = o.KeyExpireTime, Name = o.Name, KeyID = o.KeyID, VillageName = o.VillageName }).ToList();
                    return JsonConvert.SerializeObject(jsonResult);
                }

            }
            catch (Exception ex)
            {
                Utils.ErrorLog(ex.Message);
                jsonResult.Code = "-1";
                jsonResult.Msg = dic[-1];
                jsonResult.Keys = new List<EntranceKey>();
                return JsonConvert.SerializeObject(jsonResult);
            }
        }


        /// <summary>
        /// /获取单元锁
        /// </summary>
        public DataTable DSKeyUnit(string userId)
        {
            EntranceAccessor entranceAcc = new EntranceAccessor();
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            string tab = "[dbo].[EntranceUsers] A inner join Entrances B on  A.EntranceID=B.Id AND  (UnitName is not null ) AND UserOwnerInfoId in(" + userId + ") AND A.KeyExpireTime>='" + time + "'";
            string row = "B.ID, A.KeyExpireTime,B.KeyID,B.BuildId,B.UnitName,B.Name,B.VillageID ";
            return entranceAcc.CommonSearch(tab, row, string.Empty);
        }

        /// <summary>
        /// 根据小区单元获得小区楼栋 
        /// </summary>
        public DataTable DSKeyBuild(string builds)
        {
            try
            {
                DataTable dt = new DataTable();
                string maxTime = "2999-12-30 00:00:00";
                EntranceAccessor entranceAcc = new EntranceAccessor();
                string tab = "[Entrances]";
                string row = "Id,KeyID,'" + maxTime + "' as KeyExpireTime,Name,VillageID";
                string where = "BuildId in(" + builds + ") AND State=1 AND (UnitName is Null or UnitName='')";
                if (!string.IsNullOrEmpty(where))
                {
                    return entranceAcc.CommonSearch(tab, row, where);
                }
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }


    public class JsonResult
    {
        public string Code { get; set; }
        public string Msg { get; set; }
        public List<EntranceKey> Keys { get; set; }
    }
    public class Key
    {
        public int Id { get; set; }
        public string KeyID { get; set; }
        public string KeyExpireTime { get; set; }
        public string Name { get; set; }
        public int VillageID { get; set; }
        public string VillageName { get; set; }
    }

    public class EntranceKey
    {
        public string KeyID { get; set; }
        public string KeyExpireTime { get; set; }
        public string Name { get; set; }
        public string VillageName { get; set; }

    }

    public enum PersonState
    {
        业主 = 1,
        会员 = 2,
        家人 = 3
    }
}