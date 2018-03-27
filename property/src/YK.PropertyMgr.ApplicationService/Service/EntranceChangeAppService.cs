using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.DomainService;

namespace YK.PropertyMgr.ApplicationService.Service
{
    public class EntranceChangeAppService
    {
        /// <summary>
        /// 获取小区下的未绑定大门
        /// </summary>
        /// <param name="deptVillageId"></param>
        /// <returns></returns>
        public IList<APIEnrance> GetEntranceDTOList(int? deptVillageId)
        {
            try
            {
                EntranceDomainService service = new EntranceDomainService();
                var domainList = service.GetEntrances(o => o.VillageID == deptVillageId && o.State == 1 && o.BindSockState != (int)BindSockStateEnum.BindLock).OrderBy(o => o.KeyID).ToList();
                if (domainList != null && domainList.Count > 0)
                {
                    var dtoList = domainList.Select(o => new APIEnrance()
                    {
                        Id = o.Id,
                        KeyID = o.KeyID,
                        CommunityDeptId = o.CommunityDeptId ,
                        DeviceId =o.DeviceId,
                        IncrementId = o.IncrementId,
                        PwdGroup = o.PwdGroup,
                        Name = o.Name,
                        Address = o.Address
                    }).ToList();
                    return dtoList;
                }
                else
                {
                    return new List<APIEnrance>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IList<APIEnrance> GetBindEntranceDTOList(int? deptVillageId)
        {
            try
            {
                EntranceDomainService service = new EntranceDomainService();
                var domainList = service.GetEntrances(o => o.VillageID == deptVillageId && o.State == 1 && o.BindSockState == (int)BindSockStateEnum.BindLock).OrderBy(o => o.KeyID).ToList();
                if (domainList != null && domainList.Count > 0)
                {
                    var dtoList = domainList.Select(o => new APIEnrance()
                    {
                        Id = o.Id,
                        KeyID = o.KeyID,
                        CommunityDeptId = o.CommunityDeptId,
                        DeviceId = o.DeviceId,
                        IncrementId = o.IncrementId,
                        PwdGroup = o.PwdGroup,
                        Name = o.Name,
                        Address = o.Address
                    }).ToList();
                    return dtoList;
                }
                else
                {
                    return new List<APIEnrance>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 获取小区信息
        /// </summary>
        /// <param name="villageName"></param>
        /// <returns></returns>
        public List<APIVillage> GetSECVillage(string villageName)
        {
            try
            {
                var service = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCommunityList(villageName);
                var list = service.Select(o => new APIVillage
                {
                    DeptId = Convert.ToInt32(o.DeptId),
                    Name = o.Name,
                    Province = o.Province,
                    City = o.City,
                    County = o.County
                });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取小区信息
        /// </summary>
        /// <param name="villageName"></param>
        /// <returns></returns>
        public List<APIVillage> GetSECVillageByCity(string cityName)
        {
            var service = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCommunityListByCity(cityName);
            var list = service.Select(o => new APIVillage
            {
                DeptId = Convert.ToInt32(o.DeptId),
                Name = o.Name,
                Province = o.Province,
                City = o.City,
                County = o.County
            });
            return list.ToList();
        }



        #region 接口-修改绑定的KeyID
        public ResultModel ChangeEntranceKey(List<APIEntrancParameter> parms)
        {
            ResultModel res = new ResultModel();
            List<Entrance> list = new List<Entrance>();
            StringBuilder sb = new StringBuilder();
            foreach (var o in parms)
            {
                EntranceAppService service = new EntranceAppService();
                if (!(o.EntranceId > 0))
                {
                    res.IsSuccess = false;
                    res.Msg = "没有合法的大门信息";
                    return res;
                }
                EntranceDTO model = service.GetEntranceById(o.EntranceId);
                if (model == null || model.State == 0)
                {
                    res.IsSuccess = false;
                    res.Msg = "没有当前大门信息";
                    return res;
                }
                if (!(o.NewKeyId > 0))
                {
                    res.IsSuccess = false;
                    res.Msg = "绑定门的设备KeyID不正确";
                    return res;
                }
                else
                {
                    EntranceDTO modelIsExists = service.GetEntrance(o.NewKeyId);
                    //if (modelIsExists != null && modelIsExists.BindSockState == (int)BindSockStateEnum.BindLock && modelIsExists.State == 1)
                    if (modelIsExists != null)
                    {
                        res.IsSuccess = false;
                        res.Msg = o.NewKeyId + "当前设备KEYID已经绑定过其他门";
                        return res;
                    }
                    /*设置新的KEYID*/
                    sb.Append(" [大门ID：" + model.Id + ",大门地址:" + model.Address + ",   旧KEY：" + model.KeyID + ",新KEY:" + o.NewKeyId + "] ");
                    model.KeyID = o.NewKeyId;
                    model.DeviceId = o.DeviceId;
                    list.Add(EntranceMappers.ChangeDTOToEntranceNew(model));
                }
            }
            if (list.Count > 0)
            {
                EntranceDomainService entranceDomainService = new EntranceDomainService();

                //var bindList = list.Select(o => new Entrance()
                //{
                //    Id = o.Id,
                //    VillageID = o.VillageID,
                //    CommunityDeptId =o.VillageID,
                //    ProvinceID = o.ProvinceID,
                //    CityID = o.CityID,
                //    CountyID = o.CountyID,
                //    Name = o.Name,
                //    Address = o.Address,
                //    KeyID = o.KeyID,
                //    DeviceId=o.DeviceId,
                //    State = o.State,
                //    CreateTime = o.CreateTime,
                //    UnitName = o.UnitName,
                //    BuildId = o.BuildId,
                //    DoorId = o.DoorId,
                //    DoorName = o.DoorName,
                //    BindSockState = (int)BindSockStateEnum.BindLock
                //});
                //bool isSuccess = entranceDomainService.ChangeEntranceKey(bindList.ToList());
                //绑定门锁 会把新增字段更新为null 原来代码已注释 2017-06-30
                foreach (var item in list)
                {
                    item.BindSockState = (int)BindSockStateEnum.BindLock;
                }
                bool isSuccess = entranceDomainService.ChangeEntranceKey(list);
                if (isSuccess)
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                    {
                        LogProperty.WriteLoginToFile("ChangeEntranceKey" + sb, "EntranceChangeAppService/ChangeEntranceKey", FileLogType.Info);
                    }
                    res.IsSuccess = true;
                    res.Msg = "设备绑定完成";
                    return res;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Msg = "设备绑定失败";
                    return res;
                }
            }
            else
            {
                res.IsSuccess = false;
                res.Msg = "没有设备用于绑定";
                return res;
            }


        }


        public ResultModel NoBindEntrance(List<APIEntrancParameter> parms)
        {
            ResultModel res = new ResultModel();
            List<Entrance> list = new List<Entrance>();
            StringBuilder sb = new StringBuilder();
            foreach (var o in parms)
            {
                EntranceAppService service = new EntranceAppService();
                if (!(o.NewKeyId > 0))
                {
                    res.IsSuccess = false;
                    res.Msg = "没有合法的大门信息";
                    return res;
                }
                //EntranceDTO model = service.GetEntrance(o.NewKeyId);
                EntranceDTO model = service.GetEntranceByKey(o.EntranceId);
                if (model == null || model.State == 0)
                {
                    res.IsSuccess = false;
                    res.Msg = "没有当前大门信息";
                    return res;
                }
                /*设置新的KEYID，解除绑定KEYID*/
                sb.Append(" 解除绑定 [大门ID：" + model.Id + ",大门地址:" + model.Address + ",   旧KEY：" + model.KeyID + ",新KEY:0] ");
                model.KeyID = 0;
                model.DeviceId = "";
                list.Add(EntranceMappers.ChangeDTOToEntranceNew(model));
            }
            if (list.Count > 0)
            {
                EntranceDomainService entranceDomainService = new EntranceDomainService();
                //var bindList = list.Select(o => new Entrance()
                //{
                //    Id = o.Id,
                //    VillageID = o.VillageID,
                //    CommunityDeptId =o.CommunityDeptId,
                //    ProvinceID = o.ProvinceID,
                //    CityID = o.CityID,
                //    CountyID = o.CountyID,
                //    Name = o.Name,
                //    Address = o.Address,
                //    KeyID = o.KeyID,
                //    DeviceId =o.DeviceId,
                //    State = o.State,
                //    CreateTime = o.CreateTime,
                //    UnitName = o.UnitName,
                //    BuildId = o.BuildId,
                //    DoorId = o.DoorId,
                //    DoorName = o.DoorName,
                //    BindSockState = (int)BindSockStateEnum.NotBindLock
                //});
                //bool isSuccess = entranceDomainService.ChangeEntranceKey(bindList.ToList());
                //解绑门锁 会把新增字段更新为null 原来代码已注释 2017-06-30
                foreach (var item in list)
                {
                    item.BindSockState = (int)BindSockStateEnum.NotBindLock;
                }
                bool isSuccess = entranceDomainService.ChangeEntranceKey(list);
                if (isSuccess)
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                    {
                        LogProperty.WriteLoginToFile("ChangeEntranceKey" + sb, "EntranceChangeAppService/ChangeEntranceKey", FileLogType.Info);
                    }
                    res.IsSuccess = true;
                    res.Msg = "设备解除绑定完成";
                    return res;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Msg = "设备解除绑定失败";
                    return res;
                }
            }
            else
            {
                res.IsSuccess = false;
                res.Msg = "没有设备用于解绑";
                return res;
            }


        }
        #endregion
    }
}
