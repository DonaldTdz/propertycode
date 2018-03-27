using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.CompositeDomainService;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.DomainEntity;

namespace YK.PropertyMgr.ApplicationService
{
    public partial class EntranceUserAppService
    {
        public List<YK.Framework.ApplicationDTO.SQUserOwnerInfo> GetUserOwnerInfoByBuildingDeptId(int BuildingDeptId)
        {
            return HttpClientService.GetUserOwnerInfoByBuildingDeptId(BuildingDeptId);
        }
        public List<YK.Framework.ApplicationDTO.SQUserOwnerInfo> GetSQUserOwnerInfoByCommunityDeptId(int communityDeptId)
        {
            return HttpClientService.GetSQUserOwnerInfoByCommunityDeptId(communityDeptId);
        }

        public List<Entrance> GetEntrance()
        {
            return new List<Entrance>();
        }

        public IEnumerable<TemplateModel> GetEntrancePowerViewTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "HouseDeptId", ColumnDesc = "房屋ID", Seq = i++, IsListColumn = false },
                new TemplateColumn(){ ColumnName = "GuidId", ColumnDesc = "Id", Seq = i++, IsListColumn = false},
                new TemplateColumn(){ ColumnName = "Name", ColumnDesc = "姓名", Seq = i++},
                new TemplateColumn(){ ColumnName = "AllRoomNo", ColumnDesc = "房号", Seq = i++},
                new TemplateColumn(){ ColumnName = "Telephone", ColumnDesc = "电话", Seq = i++},
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(YK.Framework.ApplicationDTO.SQUserOwnerInfo), showColumns);
            return template;
        }

        /// <summary>
        /// add 2017-04-06
        /// 发送授权通知
        /// </summary>
        /// <param name="comDepId">小区Id</param>
        /// <param name="phone">用户电话号码</param>
        public void SendAuthorizationNotice(int? houseDeptId, string phone, string doorNo)
        {
            //物业授权房间XXX成功，已获得手机开门特权

            TxtMsg msgInfo = new TxtMsg()
            {
                Title = "物业授权成功",
                Content = string.Format("物业授权房间{0}成功，已获得手机开门特权", doorNo),
                MsgType = "107",
                TemplateType = "Notice"
            };

            if (houseDeptId.HasValue && houseDeptId != 0)
            {
                Task.Run(() =>
                {
                    int comDeptid = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCommunityDeptIdByHouseDeptId(houseDeptId.Value);
                    HttpClientService service = new HttpClientService();
                    if (comDeptid != 0 && !string.IsNullOrEmpty(phone))
                    {
                        try
                        {
                            service.SendPushWithJson(phone, msgInfo, comDeptid.ToString());
                        }
                        catch (Exception ex)
                        {
                            LogProperty.WriteLoginToFile(string.Format("[授权通知]comDeptId:{0} Error:{1}", comDeptid, ex.Message), "JPushMsg", FileLogType.Exception);
                        }
                    }
                });
            }
        }

        public ResultModel BatchUserRightPower(string[] entrancesIds, string[] userIds, DateTime KeyExpireTime, IList<EntranceSendMsgModel> EntranceSendMsgList)
        {
            List<EntranceUser> entancePowerList = new List<EntranceUser>();
            EntranceUser entranceUserExists = null;
            int entrancesIdKey = 0;
            try
            {
                foreach (string entrancesId in entrancesIds)
                {
                    entrancesIdKey = Convert.ToInt32(entrancesId);
                    foreach (string userid in userIds)
                    {
                        entranceUserExists = EntranceUserService.GetEntranceUser(o => o.UserOwnerInfoId == userid && o.EntranceID == entrancesIdKey);
                        if (entranceUserExists != null)
                        {
                            var entranceUser = entancePowerList.Where(e => e.Id == entranceUserExists.Id).FirstOrDefault();
                            if (entranceUser != null)
                            {
                                entranceUser.KeyExpireTime = KeyExpireTime;
                            }
                            else
                            {
                                entranceUserExists.KeyExpireTime = KeyExpireTime;
                                entancePowerList.Add(entranceUserExists);
                            }
                        }
                        else
                        {
                            EntranceUser model = new EntranceUser()
                            {
                                Id = 0,
                                CreateTime = DateTime.Now,
                                KeyExpireTime = KeyExpireTime,
                                EntranceID = Convert.ToInt32(entrancesId),
                                UserOwnerInfoId = userid
                            };
                            entancePowerList.Add(model);
                        }
                    }
                }
                if (EntranceUserService.BatchUserRightPower(entancePowerList))
                {
                    //如果成功 发送推送信息给用户
                    if (EntranceSendMsgList != null)
                    {
                        foreach (var item in EntranceSendMsgList)
                        {
                            SendAuthorizationNotice(item.HouseDeptId, item.Phone, item.DoorNo);
                        }
                    }
                    //SendAuthorizationNotice(houseDeptId, phone, doorNo);
                    return new ResultModel() { IsSuccess = true, Msg = "授权处理成功!" };
                }
                else
                {
                    return new ResultModel() { IsSuccess = false, Msg = "授权处理失败!" };
                }
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile("BatchUserRightPower 异常：" + ex, "YK.PropertyMgr.ApplicationService/EntranceUserAppService", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, Msg = "数据异常、授权处理失败!" };
            }
        }
    }
}
