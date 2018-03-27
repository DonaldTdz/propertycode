using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.PresentationService;
using YK.Opendoor.IOpendoor;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.ApplicationService.Service;
using YK.PropertyMgr.CompositeDomainService;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using static YK.PropertyMgr.CompositeDomainService.HttpClientService;

namespace YK.PropertyMgr.ApplicationService
{




    public partial class EntranceAppService
    {

        /// <summary>
        /// 解除锁的绑定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel NotBindKey(EntranceDTO modelDTO)
        {
            Entrance model = EntranceMappers.ChangeDTOToEntranceNew(modelDTO);
            bool res = EntranceService.UpdateEntrance(model);
            if (res)
            {
                LogProperty.WriteLoginToFile("NotBindKey 解除绑定。 大门ID:" + model.Id + ",大门地址:" + model.Address + ",旧KEYID：" + model.KeyID, "YK.PropertyMgr.ApplicationService/EntranceAppService", FileLogType.Info);
                return new ResultModel() { IsSuccess = true, Msg = "解除绑定成功!" };
            }
            else
            {
                return new ResultModel() { IsSuccess = true, Msg = "解除绑定失败!" };
            }
        }


        public string EditFrameworkLog(EntranceDTO modelDTO)
        {


            return string.Empty;
        }


        #region 获取KEYID和DOORID 原来版本使用
        public ResultModel GetDoorIdAndKeyId(ref EntranceDTO entranceDto, string villageName)
        {
            try
            {
                /*1.KeyId,2.DoorId*/
                Entrance modelEntrance = EntranceMappers.ChangeDTOToEntranceNew(entranceDto);
                string res = HttpClientService.GetKeyIdNew(modelEntrance, villageName).ToString();
                ResultAPI reModels = JsonConvert.DeserializeObject<ResultAPI>(res);
                if (!reModels.Result)
                {
                    return new ResultModel() { IsSuccess = false, Msg = reModels.Msg };
                }
                else
                {
                    string[] resData = reModels.Data.ToString().Split(',');
                    int keyId = Convert.ToInt32(resData[0].Substring(1, resData[0].Length - 1).Trim());
                    int doorId = Convert.ToInt32(resData[1].Substring(0, resData[0].Length - 2).Trim());
                    if (!(entranceDto.Id > 0))
                    {

                        if (Convert.ToInt32(doorId) > 0 && Convert.ToInt32(doorId) > 0)
                        {
                            entranceDto.KeyID = keyId;
                            entranceDto.DoorId = doorId;
                        }
                        else
                        {
                            return new ResultModel() { IsSuccess = false, Msg = "未能获取正常的KeyId或DoorId" };
                        }
                    }

                    return new ResultModel() { IsSuccess = true, Msg = string.Empty };
                }
            }
            catch (Exception ex)
            {
                return new ResultModel() { IsSuccess = false, Msg = "接口数据异常" };
            }
        }
        #endregion

        #region 新增大门信息

        public ResultModel InsertEntranceCus(EntranceDTO entranceDto, string villageName)
        {
            int isSuccess = 0;
            try
            {
                entranceDto.DoorId = 0;
                entranceDto.KeyID = 0;
                Entrance modelEntrance = EntranceMappers.ChangeDTOToEntranceNew(entranceDto);

                var village = PresentationServiceHelper.LookUp<IPropertyService>().GetSecDeptInfo(Convert.ToInt32(entranceDto.VillageID));
                if ((int)EDeptType.XiaoQu != village.DeptType)
                {
                    return new ResultModel() { IsSuccess = true, Msg = "请选择小区!" };
                }
                Entrance model = EntranceMappers.ChangeDTOToEntranceNew(entranceDto);
                model.CommunityDeptId = model.VillageID;
                //门禁卡升级添加 20170622 Msy
                try
                {
                    var eaopService = PresentationServiceHelper.LookUp<IEntranceAop>();
                    if (eaopService != null)
                    {
                        eaopService.BeforInsert(model,false);
                    }
                }
                catch (Exception ex)
                {
                    LogProperty.WriteLoginToFile("InsertEntranceCus " + ex, "YK.PropertyMgr.ApplicationService/EntranceAppService", FileLogType.Exception);
                    
                }
                isSuccess = EntranceService.InsertEntranceReturnId(model);
                if (isSuccess > 0)
                {
                    string strApiRes = HttpClientService.AddDoor(model, village.Name);
                    APIResult resModel = JsonConvert.DeserializeObject<APIResult>(strApiRes);
                    if (resModel.Result)
                    {
                        return new ResultModel() { IsSuccess = true, Msg = "新增设备成功!" };
                    }
                    else
                    {
                        model = EntranceMappers.ChangeDTOToEntranceNew(GetEntranceById(isSuccess));
                        model.State = 0;
                        EntranceService.UpdateEntrance(model);
                        LogProperty.WriteLoginToFile("InsertEntranceCus " + resModel.Msg, "YK.PropertyMgr.ApplicationService/EntranceAppService", FileLogType.Exception);
                        return new ResultModel() { IsSuccess = false, Msg = "接口数据异常!" };
                    }
                }
                else
                {
                    return new ResultModel() { IsSuccess = false, Msg = "新增设备失败!" };
                }
            }
            catch (Exception ex)
            {
                if (isSuccess > 0)
                {
                    Entrance model = EntranceMappers.ChangeDTOToEntranceNew(GetEntranceById(isSuccess));
                    model.State = 0;
                    model.Name = model.Name + "[异常大门无法正常使用]";
                    EntranceService.UpdateEntrance(model);
                }
                LogProperty.WriteLoginToFile("InsertEntranceCus " + ex, "YK.PropertyMgr.ApplicationService/EntranceAppService", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, Msg = "数据异常、新增设备失败!" };
            }
        }
        public ResultModel InsertEntranceCus_OLD(EntranceDTO entranceDto, string villageName)
        {
            try
            {
                entranceDto.DoorId = 0;
                entranceDto.KeyID = 0;
                Entrance modelEntrance = EntranceMappers.ChangeDTOToEntranceNew(entranceDto);

                ResultModel resultModel = GetDoorIdAndKeyId(ref entranceDto, villageName);
                if (!(entranceDto.DoorId > 0) && !(entranceDto.KeyID > 0))
                {
                    return new ResultModel() { IsSuccess = false, Msg = "没有获取正常的KeyId和DoorId" };
                }
                if (resultModel.IsSuccess)
                {
                    Entrance model = EntranceMappers.ChangeDTOToEntranceNew(entranceDto);
                    int isSuccess = EntranceService.InsertEntranceReturnId(model);
                    if (isSuccess > 0)
                    {
                        return new ResultModel() { IsSuccess = true, Msg = "新增设备成功!" };
                    }
                    else
                    {
                        return new ResultModel() { IsSuccess = false, Msg = "新增设备失败!" };
                    }
                }
                else
                {
                    return resultModel;
                }

            }
            catch (Exception ex)
            {
                return new ResultModel() { IsSuccess = false, Msg = "数据异常、新增设备失败!" };
            }
        }
        #endregion

        #region 更新大门信息
        /// <summary>
        /// 更新大门信息
        /// </summary>
        /// <param name="entranceDto">大门对象</param>
        /// <param name="villageName">大门名称</param>
        /// <returns></returns>
        public ResultModel UpdateEntranceCus(EntranceDTO entranceDto, string villageName)
        {
            try
            {
                /*更新时不能更新KEYID的值*/
                Entrance modelEntrance = EntranceMappers.ChangeDTOToEntranceNew(entranceDto);
                Entrance model = EntranceMappers.ChangeDTOToEntranceNew(entranceDto);

                try
                {
                    var eaopService = PresentationServiceHelper.LookUp<IEntranceAop>();
                    if (eaopService != null)
                    {
                        eaopService.BeforInsert(model, true);
                    }
                }
                catch (Exception ex)
                {
                    LogProperty.WriteLoginToFile("InsertEntranceCus " + ex, "YK.PropertyMgr.ApplicationService/EntranceAppService", FileLogType.Exception);

                }

                bool isSuccess = EntranceService.UpdateEntrance(model);
                if (isSuccess)
                {
                    string strApiRes = HttpClientService.UpdateDoor(Convert.ToInt32(model.Id), model.Name, model.Address);
                    APIResult resModel = JsonConvert.DeserializeObject<APIResult>(strApiRes);
                    if (resModel.Result)
                    {
                        return new ResultModel() { IsSuccess = true, Msg = "更新设备成功!" };
                    }
                    else
                    {
                        LogProperty.WriteLoginToFile("UpdateEntranceCus " + resModel.Msg, "YK.PropertyMgr.ApplicationService/EntranceAppService", FileLogType.Exception);
                        return new ResultModel() { IsSuccess = true, Msg = "更新接口数据异常" };
                    }
                }
                else
                {
                    return new ResultModel() { IsSuccess = false, Msg = "更新设备失败!" };
                }
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile("UpdateEntranceCus " + ex, "YK.PropertyMgr.ApplicationService/EntranceAppService", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, Msg = "数据异常、更新设备失败!" };
            }
        }
        #endregion

        #region 大门信息列表
        public IList<EntranceViewDTO> GetEntranceDTOList(EntranceSearchDTO searchDto, out int totalCount)
        {
            try
            {

                totalCount = 0;
                Condition<Entrance> condition = new Condition<Entrance>(c => true);
                if (!string.IsNullOrEmpty(searchDto.Name))
                {
                    condition = condition & new Condition<Entrance>(c => c.Name.Contains(searchDto.Name) || c.Address.Contains(searchDto.Name));
                }
                if (!string.IsNullOrEmpty(searchDto.DeptId.ToString()))
                {
                    var secDept = PresentationServiceHelper.LookUp<IPropertyService>().GetSecDeptInfo(Convert.ToInt32(searchDto.DeptId));
                    if (secDept.DeptType == (int)EDeptType.LouYu)
                    {
                        int buildId = Convert.ToInt32(secDept.Code.Split('.')[1]);

                        condition = condition & new Condition<Entrance>(c => c.BuildId == buildId);
                    }
                    else
                    {
                        condition = condition & new Condition<Entrance>(c => c.VillageID == searchDto.DeptId);
                    }
                }

                if (!string.IsNullOrEmpty(searchDto.State.ToString()))
                {
                    condition = condition & new Condition<Entrance>(c => c.State == searchDto.State);
                }
                string expressions = "Id asc";
                var domainList = EntranceService.GetEntranceDTOList(searchDto.PageIndex, searchDto.PageSize, condition.ExpressionBody, expressions, out totalCount);
                var dtoList = EntranceMappers.ChangeEntranceToDTOs(domainList).ToList().Select(o => new EntranceViewDTO()
                {
                    Address = o.Address,
                    BuildId = o.BuildId,
                    CityID = o.CityID,
                    ProvinceName = domainList.Where(m => m.ProvinceID == o.ProvinceID).FirstOrDefault().Province.Name,
                    CityName = domainList.Where(m => m.CityID == o.CityID).FirstOrDefault().City.Name,
                    CountryName = domainList.Where(m => m.CountyID == o.CountyID).FirstOrDefault().County.Name,
                    StateName = o.State == 1 ? "启用" : "禁用",
                    CountyID = o.CountyID,
                    CreateTime = o.CreateTime,
                    DeviceType = !string.IsNullOrEmpty(o.UnitName) ? "单元锁" : (string.IsNullOrEmpty(o.BuildId.ToString()) ? "小区锁" : "楼宇锁"),
                    DoorId = o.DoorId,
                    DoorName = o.DoorName,
                    Id = o.Id,
                    KeyID = o.KeyID,
                    Name = o.Name,
                    ProvinceID = o.ProvinceID,
                    State = o.State,
                    UnitName = o.UnitName,
                    VillageID = o.VillageID,
                    BindLock = o.KeyID > 0 ? "已绑定锁" : "未绑定锁"
                }).ToList();
                return dtoList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取用户的授权设备
        /// <summary>
        /// 获取用户的授权设备
        /// </summary>
        /// <param name="searchDto"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<EntrancePersonal> GetEntrancePersonal(EntranceSearchDTO searchDto)
        {
            try
            {


                Condition<EntrancePersonal> condition = new Condition<EntrancePersonal>(c => true);
                if (!string.IsNullOrEmpty(searchDto.Name))
                {
                    condition = condition & new Condition<EntrancePersonal>(c => c.Name.Contains(searchDto.Name) || c.Address.Contains(searchDto.Name));
                }
                if (!string.IsNullOrEmpty(searchDto.DeptId.ToString()))
                {
                    condition = condition & new Condition<EntrancePersonal>(c => c.VillageID == searchDto.DeptId);
                }
                if (!string.IsNullOrEmpty(searchDto.State.ToString()))
                {
                    condition = condition & new Condition<EntrancePersonal>(c => c.State == searchDto.State);
                }

                if (!string.IsNullOrEmpty(searchDto.Guid.ToString()))
                {
                    condition = condition & new Condition<EntrancePersonal>(c => c.UserId == searchDto.Guid);
                }
                DateTime KeyExpireTime = DateTime.Now.Date;
                condition = condition & new Condition<EntrancePersonal>(c => c.KeyExpireTime >= KeyExpireTime);
                condition = condition & new Condition<EntrancePersonal>(c => c.State == 1);/*启用的设备*/

                List<YK.Framework.ApplicationDTO.InterfaceDTO.UserOwnerEntrance> userOwnerEntranceList = new List<YK.Framework.ApplicationDTO.InterfaceDTO.UserOwnerEntrance>();
                var domainList = EntranceService.GetEntrancePersonal(condition.ExpressionBody);
                List<int?> listBuildEntarnceByUnit = new List<int?>();
                domainList.ForEach(o =>
                {
                    o.DeviceType = (!string.IsNullOrEmpty(o.UnitName) ? "单元锁" : (o.BuildId.HasValue ? "楼宇锁" : "小区锁")) + "(手动授权)";
                    if (o.BuildId.HasValue && !string.IsNullOrEmpty(o.UnitName))
                    {
                        if (!listBuildEntarnceByUnit.Contains(o.BuildId))
                        {   /*根据手动授权的单元锁获，获取自动授权的楼栋锁*/
                            listBuildEntarnceByUnit.Add(o.BuildId);
                        }
                    }
                });
                /*自动授权*/
                string openPermission = ConfigurationManager.AppSettings["OpenPermission"].ToString();
                Guid guid = new Guid(searchDto.Guid);
                DateTime KeyExpireTimeAutomatic = Convert.ToDateTime("2999-12-30");
                userOwnerEntranceList = HttpClientService.GetUserOwnerEntrances(guid);
                /*是否开启自动授权,若没有开启那只能是业主才能授权*/
                if (userOwnerEntranceList != null)
                {
                    userOwnerEntranceList = userOwnerEntranceList.Where(o => o.CommunityDeptId == searchDto.DeptId).ToList();
                    if (openPermission.ToUpper() == "FALSE")
                    {
                        userOwnerEntranceList = userOwnerEntranceList.Where(o => o.PersonState == (int)PersonStateEnums.业主).ToList();
                    }
                }
                List<Entrance> entancePowerAutomatics = new List<Entrance>();
                List<Entrance> listEntrance = new List<Entrance>();
                List<Entrance> villageEntrances = new List<Entrance>();
                if (null != userOwnerEntranceList && userOwnerEntranceList.Count > 0)
                {
                    var userOwnerIsPermissionEntranceList = userOwnerEntranceList.Where(o => o.IsPermission != 0).ToList();/*不为零的才有权限*/
                  //userOwnerIsPermissionEntranceList = new List<Framework.ApplicationDTO.InterfaceDTO.UserOwnerEntrance>();
                    foreach (var item in userOwnerIsPermissionEntranceList)
                    {
                        /*小区下的所有,有用设备*/
                        entancePowerAutomatics = EntranceService.GetEntrances(o => o.VillageID == item.CommunityDeptId && o.State == 1).ToList();
                        villageEntrances.AddRange(entancePowerAutomatics);
                        /*小区锁*/
                        listEntrance = entancePowerAutomatics.Where(m => m.BuildId.HasValue == false && string.IsNullOrEmpty(m.UnitName) == true).ToList();
                        domainList = EntranceToEntrancePersonal(domainList, listEntrance, KeyExpireTimeAutomatic, "小区锁(自动授权)", searchDto.Guid);
                        /*楼宇锁*/
                        listEntrance = entancePowerAutomatics.Where(m => m.BuildId == item.BuildingDeptId && string.IsNullOrEmpty(m.UnitName) == true).ToList();
                        domainList = EntranceToEntrancePersonal(domainList, listEntrance, KeyExpireTimeAutomatic, "楼宇锁(自动授权)", searchDto.Guid);
                        /*单元锁*/
                        listEntrance = entancePowerAutomatics.Where(m => m.BuildId == item.BuildingDeptId && m.UnitName == item.UnitName).ToList();
                        domainList = EntranceToEntrancePersonal(domainList, listEntrance, KeyExpireTimeAutomatic, "单元锁(自动授权)", searchDto.Guid);
                    }

                    /*根据手动授权的单元锁获取自动授权的楼宇锁k*/
                    listEntrance = villageEntrances.Where(o => listBuildEntarnceByUnit.Contains(o.BuildId) && string.IsNullOrEmpty(o.UnitName)).ToList();
                    domainList = EntranceToEntrancePersonal(domainList, listEntrance, KeyExpireTimeAutomatic, "楼宇锁(自动授权)", searchDto.Guid);

                }
                if (!string.IsNullOrEmpty(searchDto.Name))
                {
                    domainList = domainList.Where(o => o.Name.Contains(searchDto.Name.Trim())).ToList();
                }
                return domainList.OrderByDescending(o => o.KeyExpireTime).ToList();
            }
            catch (Exception ex)
            {

                return new List<EntrancePersonal>();
            }
        }

        private List<EntrancePersonal> EntranceToEntrancePersonal(List<EntrancePersonal> listEntrancePersonal, List<Entrance> listEntrance, DateTime KeyExpireTimeAutomatic, string deviceType, string userId)
        {
            listEntrance.ForEach(o =>
            {
                if (!listEntrancePersonal.Any(n => n.Id == o.Id))
                {
                    listEntrancePersonal.Add(new EntrancePersonal()
                    {
                        Id = o.Id,
                        Name = o.Name,
                        Address = o.Address,
                        KeyExpireTime = KeyExpireTimeAutomatic,
                        CreateTime = o.CreateTime,
                        KeyID = o.KeyID,
                        State = o.State,
                        UserId = userId,
                        VillageID = o.VillageID,
                        DeviceType = deviceType
                    });
                }
            });
            return listEntrancePersonal;
        }

        #endregion

        #region 门禁授权大门模板信息展示
        public IEnumerable<TemplateModel> GetEntranceViewTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "Id", ColumnDesc = "编号", Seq = i++},
                new TemplateColumn(){ ColumnName = "Name", ColumnDesc = "设备名称", Seq = i++},
                new TemplateColumn(){ ColumnName = "DeviceType", ColumnDesc = "门锁类型", Seq = i++},
                new TemplateColumn(){ ColumnName = "ProvinceName", ColumnDesc = "省", Seq = i++},
                new TemplateColumn(){ ColumnName = "CityName", ColumnDesc = "市", Seq = i++},
                new TemplateColumn(){ ColumnName = "CountryName", ColumnDesc = "区/县", Seq = i++},
                new TemplateColumn(){ ColumnName = "Address", ColumnDesc = "地址", Seq = i++},
                new TemplateColumn(){ ColumnName = "StateName", ColumnDesc = "状态", Seq = i++},
                new TemplateColumn(){ ColumnName = "BindLock", ColumnDesc = "是否绑定锁", Seq = i++},
                new TemplateColumn(){ ColumnName = "CreateTime", ColumnDesc = "创建时间", Seq = i++,Type="date"},
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(EntranceViewDTO), showColumns);
            return template;
        }
        /// <summary>
        /// 门禁授权大门模板信息展示
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TemplateModel> GetEntrancePowerViewTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "Id", ColumnDesc = "编号", Seq = i++},
                new TemplateColumn(){ ColumnName = "Name", ColumnDesc = "设备名称", Seq = i++},
                new TemplateColumn(){ ColumnName = "CreateTime", ColumnDesc = "创建时间", Seq = i++,Type="date"},
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(EntranceViewDTO), showColumns);
            return template;
        }

        /// <summary>
        /// 个人授权设备信息展示
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TemplateModel> GetEntrancePowerPersonalViewTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "Id", ColumnDesc = "编号", Seq = i++},
                new TemplateColumn(){ ColumnName = "Name", ColumnDesc = "名称", Seq = i++},
                new TemplateColumn(){ ColumnName = "DeviceType", ColumnDesc = "类型", Seq = i++},
                new TemplateColumn(){ ColumnName = "KeyExpireTime", ColumnDesc = "过期时间", Seq = i++,Type="date"},
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(EntranceViewDTO), showColumns);
            return template;
        }
        #endregion

        #region 获取单元信息
        /// <summary>
        /// 获取单元信息
        /// </summary>
        /// <param name="buildId">楼宇ID</param>
        /// <returns></returns>
        public List<string> LoadUnit(int? buildId)
        {
            List<string> listUnits = new List<string>();
            if (!string.IsNullOrEmpty(buildId.ToString()))
            {
                List<int?> builds = new List<int?>();
                builds.Add(buildId);
                DeptAppService service = new DeptAppService();
                var listHouse = service.GetDeptHouseList(builds);
                foreach (DeptInfo item in listHouse)
                {
                    listUnits.Add(item.Name.Split('-')[1]);/*单元信息*/
                }
                listUnits = listUnits.Distinct().ToList();
            }
            return listUnits;
        }
        #endregion

        #region 获取大门信息

        /// <summary>
        /// 获取大门信息
        /// </summary>
        /// <param name="keyId">设备的ID</param>
        /// <returns></returns>
        public EntranceDTO GetEntrance(int keyId)
        {
            return EntranceMappers.ChangeEntranceToDTO(EntranceService.GetEntrance(keyId));
        }
        /// <summary>
        /// 获取大门信息
        /// </summary>
        /// <param name="entranceId">大门ID</param>
        /// <returns></returns>
        public EntranceDTO GetEntranceById(int entranceId)
        {
            return EntranceMappers.ChangeEntranceToDTO(EntranceService.GetEntranceById(entranceId));
        }

        #endregion

    }
}
