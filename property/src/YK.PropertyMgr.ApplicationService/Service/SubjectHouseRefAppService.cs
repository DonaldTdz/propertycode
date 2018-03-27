using System;
using System.Collections.Generic;
using System.Linq;
using YK.PropertyMgr.DomainEntity;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using System.Text;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using Newtonsoft.Json;
using Microsoft.Practices.Unity;
using YK.PropertyMgr.RepositoryContract;
using System.Data;
using YK.PropertyMgr.DomainService;
using System.Web.Http;

namespace YK.PropertyMgr.ApplicationService
{
    public partial class SubjectHouseRefAppService
    {

        #region 批量新增
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainSubjectHouseRefDTOList"></param>
        /// <param name="isSetTime"></param>
        /// <param name="isBind"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public ResultModel InsertSubjectHouseRefList(int resType, List<SubjectHouseRefDTO> domainSubjectHouseRefDTOList, string chargeFormula, List<SubjectHouseRefDTO> listNotBindSubjectHouseRef, int subjectId, bool isPostBillSave, int operaterId, string opearaterName)
        {
            ResultModel res = new ResultModel();

            try
            {
                int subjectType = 0;
                string msg = string.Empty;
                string meterMsg = string.Empty;
                List<SubjectHouseRef> list = new List<SubjectHouseRef>();
                ChargeSubjectAppService subjectAppService = new ChargeSubjectAppService();
                var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
                ChargeSubjectDTO subject = new ChargeSubjectDTO();
                List<int?> listIds = new List<int?>();
                /*房屋的ID*/
                List<int?> houseDeptIds = new List<int?>();
                StringBuilder sb = new StringBuilder();
                List<SubjectHouseRef> listSubjectRefHaveBind = new List<SubjectHouseRef>();
                Dictionary<int?, int?> dic = new Dictionary<int?, int?>();

                List<SubjectHouseRef> notBindList = new List<SubjectHouseRef>();
                if ((resType == (int)MeterTypeEnum.GasMeter || resType == (int)MeterTypeEnum.WattHourMeter || resType == (int)MeterTypeEnum.WaterMeter))
                {
                    subjectType = (int)SubjectTypeEnum.Meter;
                    domainSubjectHouseRefDTOList.ForEach(o =>
                    {
                        listIds.Add(o.ResourcesId);
                        houseDeptIds.Add(o.HouseDeptId);
                    });

                    var data = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouseListByHouseDeptIds(houseDeptIds);
                    List<HouseInfo> houses = data.Where(o => o.HouseStatus == 1).ToList();
                    houses.ForEach(o =>
                    {
                        sb.Append(string.Format("{0},", o.DoorNo));
                        if (domainSubjectHouseRefDTOList.Any(m => m.HouseDeptId == o.HouseDeptID))
                        {
                            var removeItems = (domainSubjectHouseRefDTOList.Where(m => m.HouseDeptId == o.HouseDeptID).ToList());
                            removeItems.ForEach(m =>
                            {
                                domainSubjectHouseRefDTOList.Remove(m);
                            });
                        }
                    });
                    if (houses != null && houses.Count > 0)
                    {
                        sb.Append("未交房无法绑定收费项目!");
                    }
                    msg = sb.ToString();
                }
                if (resType == (int)EDeptType.CheKu)
                {
                    subjectType = subjectType = (int)SubjectTypeEnum.ParkingSpace;
                }
                if (resType == (int)EDeptType.LouYu)
                {
                    List<int?> listRes = new List<int?>();
                    domainSubjectHouseRefDTOList.ForEach(o =>
                    {
                        listIds.Add(o.ResourcesId);
                        listRes.Add(o.ResourcesId);
                    });
                    subjectType = subjectType = (int)SubjectTypeEnum.House;

                    var data = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouseListByHouseDeptIds(listRes);
                    /*展示数字、后面改枚举*/
                    /*未交房不计费不可绑定科目*/
                    List<HouseInfo> houses = data.Where(o => o.HouseStatus == 1).ToList();
                    houses.ForEach(o =>
                    {
                        sb.Append(string.Format("{0},", o.DoorNo));
                        if (domainSubjectHouseRefDTOList.Any(m => m.ResourcesId == o.HouseDeptID))
                        {
                            domainSubjectHouseRefDTOList.Remove(domainSubjectHouseRefDTOList.FirstOrDefault(m => m.ResourcesId == o.HouseDeptID));
                        }
                    });
                    if (houses != null && houses.Count > 0)
                    {
                        sb.Append("未交房无法绑定收费项目!");
                    }
                    msg = sb.ToString();
                }
                foreach (var item in listNotBindSubjectHouseRef)
                {
                    list.Add(SubjectHouseRefMappers.ChangeDTOToSubjectHouseRefNew(item));

                }
                /*已經綁定的三表不能再进行绑定*/
                if ((resType == (int)MeterTypeEnum.GasMeter || resType == (int)MeterTypeEnum.WattHourMeter || resType == (int)MeterTypeEnum.WaterMeter))
                {

                    listSubjectRefHaveBind = SubjectHouseRefService.GetChargeSubjectList(o => listIds.Contains(o.ResourcesId) && o.SubjectType == subjectType && o.IsDel == false);
                    listSubjectRefHaveBind = listSubjectRefHaveBind.Where(o => o.ChargeSubjecId != subjectId).ToList();
                    listIds.Clear();
                    listSubjectRefHaveBind.ForEach(o =>
                    {
                        listIds.Add(o.ResourcesId);
                    });
                    if (domainSubjectHouseRefDTOList.Where(o => listIds.Contains(o.ResourcesId)).ToList().Count > 0)
                    {
                        switch (resType)
                        {
                            case (int)MeterTypeEnum.WaterMeter:
                                msg = "选中资源中已存在针对“水表”的收费项目,不再做绑定设置";
                                domainSubjectHouseRefDTOList = domainSubjectHouseRefDTOList.Where(o => !listIds.Contains(o.ResourcesId)).ToList();
                                break;
                            case (int)MeterTypeEnum.WattHourMeter:
                                msg = "选中资源中已存在针对“电表”的收费项目,不再做绑定设置";
                                domainSubjectHouseRefDTOList = domainSubjectHouseRefDTOList.Where(o => !listIds.Contains(o.ResourcesId)).ToList();
                                break;
                            case (int)MeterTypeEnum.GasMeter:
                                msg = "选中资源中已存在针对“气表”的收费项目,不再做绑定设置";
                                domainSubjectHouseRefDTOList = domainSubjectHouseRefDTOList.Where(o => !listIds.Contains(o.ResourcesId)).ToList();
                                break;
                        }
                    }
                }


                /*获取资源的名称ReourceName*/
                List<int?> resourceIds = domainSubjectHouseRefDTOList.ConvertAll(o => o.ResourcesId).ToList();
                Dictionary<int, string> dicResources =
                        (dicResources = subjectType == (int)SubjectTypeEnum.ParkingSpace ? GetResourceName(resourceIds, subjectType) :
                        (subjectType == (int)SubjectTypeEnum.Meter ? GetResourceName(resourceIds, subjectType) : GetResourceName(resourceIds, subjectType)));

                list.AddRange(domainSubjectHouseRefDTOList.Select(o => new SubjectHouseRef()
                {
                    Id = o.Id,
                    BeginDateBill=o.BeginDateBill,
                    ChargeSubjecId = o.ChargeSubjecId,
                    HouseDeptId = o.HouseDeptId,
                    ResourcesId = o.ResourcesId,
                    IsDevPay = o.IsDevPay,
                    DevBeginDate = o.DevBeginDate,
                    DevEndDate = o.DevEndDate,
                    CreateTime = o.CreateTime,
                    UpdateTime = o.UpdateTime,
                    IsDel = o.IsDel,
                    Operator = o.Operator,
                    SubjectType = o.SubjectType,
                    ResourceName = dicResources.Any(m => m.Key == o.ResourcesId) ? dicResources.FirstOrDefault(m => m.Key == o.ResourcesId).Value : ""
                }).ToList());

                /*移除绑定时判断是否生成账单*/
                List<int?> subjectIds = new List<int?>();
                notBindList = list.Where(o => o.IsDel == true).ToList();
                subjectIds.Add(subjectId);

                var listSubjectBill = IsCreateBillWhenNoBindSubject(notBindList, resType, subjectIds, 2);
                if (listSubjectBill.Count > 0)
                {
                    if (isPostBillSave == true)
                    {
                        return new ResultModel()
                        {
                            IsSuccess = true,
                            ErrorCode = "100",
                            Data = listSubjectBill//JsonConvert.SerializeObject(listSubjectBill)
                        };
                    }
                }
                bool isSuccess = SubjectHouseRefService.BatchInsertOrUpdateSubjectHouseRefList(list, operaterId,null);
                if (isSuccess)
                {
                    return new ResultModel() { IsSuccess = true, Msg = "项目设置处理成功!" + msg };
                }
                else
                {
                    return new ResultModel() { IsSuccess = false, Msg = "项目设置处理失败" + msg };
                }
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile("InsertSubjectHouseRefList" + ex, "SubjectHouseRefAppService/InsertSubjectHouseRefList", FileLogType.Info);
                return new ResultModel() { IsSuccess = false, Msg = "数据异常、项目设置失败!" };
            }
        }

        #region 设置开发商代缴
        /// <summary>
        /// 设置开发商代缴
        /// </summary>
        /// <param name="model">绑定或解绑的科目信息</param>
        /// <param name="operater">操作人</param>
        /// <param name="isPostBillSave">是否有未生成账单的解除科目</param>
        /// <param name="sourceType">【房屋、车位、三表】</param>
        /// <param name="operaterId">解绑人ID</param>
        /// <param name="opearaterName">操作名字</param>
        /// <returns></returns>

        public ResultModel SaveSetDevloperPay(DeveloperSetTimeListDTO model, int operater, bool isPostBillSave, int sourceType, int operaterId, string opearaterName)
        {
            try
            {

                ChargeSubjectAppService service = new ChargeSubjectAppService();
                List<int?> listBindSubject = new List<int?>();
                List<int?> listNotSet = new List<int?>();
                DeveloperSetTime setTime = new DeveloperSetTime();
                StringBuilder sb = new StringBuilder();


                List<int?> listResIds = new List<int?>();
                List<int?> houseDeptIds = new List<int?>();


                /*01-未交房无法绑定收费项目*/
                listResIds.Add(model.ResId);
                houseDeptIds.Add(model.HouseDeptId);
                var houseList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouseListByHouseDeptIds(houseDeptIds);
                List<HouseInfo> houses = houseList.Where(o => o.HouseStatus == 1).ToList();
                houses.ForEach(o =>
                {
                    sb.Append(string.Format("{0},", o.DoorNo));
                });
                if (houses != null && houses.Count > 0)
                {
                    sb.Append("未交房无法绑定收费项目!");
                }
                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    return new ResultModel()
                    {
                        IsSuccess = false,
                        Msg = sb.ToString()
                    };
                }
                /*02-有开发商的设置开发商代缴*/
                model.DeveloperSetTimelist.Where(o => o.BindSubject == true).ToList().ForEach(o =>
                {
                    listBindSubject.Add(o.SubjectId);
                });


                List<SubjectHouseRefDTO> SubjectHouseReflist = GetSubjectHouseRefList(listBindSubject, model.ResId);
                if (model.SubjectType == (int)SubjectTypeEnum.Meter)
                {
                    var listMeter = model.DeveloperSetTimelist.Where(o => o.BindSubject == true).ToList();
                    if (listMeter.Count > 1)
                    {
                        return new ResultModel() { IsSuccess = false, Msg = "三表资源只能绑定一个科目!" };
                    }
                    var isExistsBindMeter = GetSubjectHouseRef(model.ResId, model.SubjectType);
                    if (isExistsBindMeter != null && isExistsBindMeter.Id > 0)
                    {
                        if (listMeter.Count > 0)
                        {
                            if (listMeter.FirstOrDefault().SubjectId != isExistsBindMeter.ChargeSubjecId)
                            {
                                return new ResultModel() { IsSuccess = false, Msg = "已经绑定过三表项目、无法再为其他项目设置开发商代缴时间 !" };
                            }
                        }
                    }
                }
                /*按周期设置开发商代缴时间 */
                foreach (var o in SubjectHouseReflist)
                {

                    ChargeSubjectDTO subject = new ChargeSubjectAppService().GetChargeSubjectByKey(o.ChargeSubjecId);
                    setTime = model.DeveloperSetTimelist.SingleOrDefault(m => m.SubjectId == o.ChargeSubjecId);
                    if (setTime.BeginDate.HasValue && (subject.BillPeriod == (int)BillPeriodEnum.MonthlyCharge || subject.BillPeriod == (int)BillPeriodEnum.MeterCharge))
                    {
                        o.DevBeginDate = new DateTime(setTime.BeginDate.Value.Year, setTime.BeginDate.Value.Month, 1);
                        o.DevEndDate = new DateTime(setTime.EndDate.Value.Year, setTime.EndDate.Value.Month, 1).AddMonths(1).AddDays(-1);
                    }
                    else
                    {
                        o.DevEndDate = setTime.EndDate;

                    }
                    o.DevBeginDate = setTime.BeginDate;

                    if (!string.IsNullOrEmpty(setTime.BeginDate.ToString()) && !string.IsNullOrEmpty(setTime.EndDate.ToString()))
                    {
                        o.IsDevPay = true;
                    }
                    else
                    {
                        o.IsDevPay = false;
                    }

                    listBindSubject.Remove(o.ChargeSubjecId);
                }
                /*2.没有的设置开发商*/
                var noBillsSetTime = model.DeveloperSetTimelist.Where(o => listBindSubject.Contains(o.SubjectId)).ToList();
                var subjects = new ChargeSubjectAppService().GetChargeSubjectList(o => listBindSubject.Contains(o.Id));
                foreach (var n in noBillsSetTime)
                {
                    if (string.IsNullOrEmpty(n.BeginDateBill.ToString()))
                    {
                        n.BeginDateBill = DateTime.Now;
                    }
                }
                foreach (var o in noBillsSetTime)
                {
                    var singlseSubject = subjects.FirstOrDefault(m => m.Id == o.SubjectId);
                    SubjectHouseRefDTO obj = new SubjectHouseRefDTO()
                    {
                        ChargeSubjecId = o.SubjectId,
                        CreateTime = DateTime.Now,
                        DevBeginDate = o.BeginDate,
                        DevEndDate = o.EndDate,
                        HouseDeptId = model.HouseDeptId,
                        IsDel = false,
                        BeginDateBill = o.BeginDateBill,
                        ResourcesId = model.ResId,
                        UpdateTime = DateTime.Now,
                        SubjectType = model.SubjectType,
                        Operator = operater
                    };
                    if (singlseSubject.BillPeriod == (int)BillPeriodEnum.MonthlyCharge || singlseSubject.BillPeriod == (int)BillPeriodEnum.MeterCharge)
                    {
                        if (obj.DevEndDate.HasValue)
                        {
                            obj.DevEndDate = obj.DevEndDate.Value.AddMonths(1).AddDays(-1);
                        }
                    }

                    if (!string.IsNullOrEmpty(o.BeginDate.ToString()) && !string.IsNullOrEmpty(o.EndDate.ToString()))
                    {
                        obj.IsDevPay = true;
                    }
                    else
                    {
                        obj.IsDevPay = false;
                    }
                    SubjectHouseReflist.Add(obj);
                }
                /*3.不设置开发商代缴*/
                model.DeveloperSetTimelist.Where(o => o.BindSubject == false).ToList().ForEach(o =>
                {
                    listNotSet.Add(o.SubjectId);
                });

                List<SubjectHouseRefDTO> notSetSubjectHouseReflist = GetSubjectHouseRefList(listNotSet, model.ResId);
                /*解除绑定关系、查看账单信息*/
                var data = notSetSubjectHouseReflist.Select(o => new SubjectHouseRef()
                {
                    Id = o.Id,
                    ChargeSubjecId = o.ChargeSubjecId,
                    HouseDeptId = o.HouseDeptId,
                    ResourcesId = o.ResourcesId,
                    IsDevPay = o.IsDevPay,
                    DevBeginDate = o.DevBeginDate,
                    DevEndDate = o.DevEndDate,
                    CreateTime = o.CreateTime,
                    UpdateTime = o.UpdateTime,
                    IsDel = o.IsDel,
                    Operator = o.Operator,
                    SubjectType = o.SubjectType,
                    BeginDateBill = o.BeginDateBill
                }).ToList();
                var listSubjectBill = IsCreateBillWhenNoBindSubject(data, sourceType, notSetSubjectHouseReflist.ConvertAll(o => o.ChargeSubjecId).Distinct().ToList(), 1);
                if (listSubjectBill.Count > 0)
                {
                    if (isPostBillSave == true)
                    {
                        return new ResultModel()
                        {
                            IsSuccess = true,
                            ErrorCode = "100",
                            Data = listSubjectBill//JsonConvert.SerializeObject(listSubjectBill)
                        };
                    }
                }
                /*解除绑定*/
                notSetSubjectHouseReflist.ForEach(o =>
                {
                    o.DevBeginDate = null;
                    o.DevEndDate = null;
                    o.IsDevPay = false;
                    o.IsDel = true;
                    o.UpdateTime = DateTime.Now;
                    SubjectHouseReflist.Add(o);
                });

                bool isSuccessed = true;
                if (SubjectHouseReflist.Count > 0)
                {
                    int subjectType = 0;
                    if (sourceType == (int)EDeptType.CheKu)
                    {
                        subjectType = (int)ReourceTypeEnum.CarPark;
                    }
                    else if (sourceType == (int)MeterTypeEnum.GasMeter || sourceType == (int)MeterTypeEnum.WaterMeter || sourceType == (int)MeterTypeEnum.WattHourMeter)
                    {
                        subjectType = (int)ReourceTypeEnum.ThreeMeter;
                    }
                    else
                    {
                        subjectType = (int)ReourceTypeEnum.House;
                    }


                    /*获取资源的名称ReourceName*/
                    List<int?> resourceIds = SubjectHouseReflist.ConvertAll(o => o.ResourcesId).ToList();
                    Dictionary<int, string> dicResources =
                            (dicResources = subjectType == (int)SubjectTypeEnum.ParkingSpace ? GetResourceName(resourceIds, subjectType) :
                            (subjectType == (int)SubjectTypeEnum.Meter ? GetResourceName(resourceIds, subjectType) : GetResourceName(resourceIds, subjectType)));
                    SubjectHouseReflist.ForEach(o =>
                    {
                        o.ResourceName = dicResources.Any(m => m.Key == o.ResourcesId) ? dicResources.FirstOrDefault(m => m.Key == o.ResourcesId).Value : "";
                    });
                    /*数据提交*/
                    isSuccessed = InsertOrUpdateSubjectHouseRefList(SubjectHouseReflist, operaterId, opearaterName, model);
                }
                if (isSuccessed)
                {
                    return new ResultModel()
                    {
                        IsSuccess = true,
                        // Msg = "开发商代缴设置处理完成" + sb.ToString()
                        Msg = "项目设置处理完成" + sb.ToString()
                    };
                }
                else
                {
                    return new ResultModel()
                    {
                        IsSuccess = true,
                        Msg = "项目设置处理失败" + sb.ToString()
                    };
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 根据资源ID和类型返回资源的名称
        /// </summary>
        /// <param name="resIds">资源ID（房屋、车位、三表）</param>
        /// <param name="type">房屋、车位、三表</param>
        /// <returns></returns>
        public Dictionary<int, string> GetResourceName(List<int?> resIds, int type)
        {

            Dictionary<int, string> dicResources = new Dictionary<int, string>();
            if (type == (int)SubjectTypeEnum.ParkingSpace)
            {
                dicResources = PresentationServiceHelper.LookUp<IPropertyService>().GetParkingSpace(resIds);
            }
            else if (type == (int)SubjectTypeEnum.Meter)
            {
                var data = new MeterAppService().GetMeterDTOS(resIds).ToList();
                data.ForEach(o =>
                {
                    dicResources.Add(o.Id.Value, o.MeterNum);
                });
            }
            else
            {
                var data = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouseListByHouseDeptIds(resIds).Distinct().ToList();
                data.ForEach(o =>
                {
                    dicResources.Add(o.Id.Value, o.DoorNo);
                });
            }
            return dicResources;
        }

        #endregion

        #region 当解除绑定的时候未生成账单的绑定科目是否生成账单
        /// <summary>
        /// 当解除绑定的时候未生成账单的绑定科目是否生成账单
        /// </summary>
        /// <param name="list">资源和科目的关系</param>
        /// <param name="resType">资源类型</param>
        /// <param name="subjectIds">科目Ids</param>
        /// <param name="Type">/*type=1【多项目1个资源】，type=2【多资源解绑一个项目】*/</param>
        /// <returns></returns>
        public List<SubjectBillView> IsCreateBillWhenNoBindSubject(List<SubjectHouseRef> list, int resType, List<int?> subjectIds, int Type)
        {

            List<int?> resIds = new List<int?>();
            List<int?> resNotHaveBill = new List<int?>();/*以前没有生成过账单*/
            List<ChargBillDTO> listHaveBill = new List<ChargBillDTO>();/*以前生成过账单*/
            List<DeptInfo> listHouse = new List<DeptInfo>();/*房屋信息*/
            List<MeterDTO> listMeters = new List<MeterDTO>();/*三表信息*/
            Dictionary<int, string> dicCarport = new Dictionary<int, string>();/*车位*/
            List<SubjectBillView> listSubjectBillView = new List<SubjectBillView>();

            ChargBillAppService service = new ChargBillAppService();
            ChargeSubjectAppService subjectService = new ChargeSubjectAppService();
            MeterAppService meterService = new MeterAppService();

            List<ChargeSubjectDTO> subjetcModels = subjectService.GetChargeSubjectList(o => subjectIds.Contains(o.Id));

            list.ForEach(o =>
            {
                if (!resIds.Contains(o.ResourcesId))
                {
                    resIds.Add(o.ResourcesId);
                }
            });

            if (resType == (int)EDeptType.CheKu)
            {
                resType = (int)ReourceTypeEnum.CarPark;
                dicCarport = PresentationServiceHelper.LookUp<IPropertyService>().GetParkingSpace(resIds);

            }
            else if (resType == (int)EDeptType.LouYu)
            {
                resType = (int)ReourceTypeEnum.House;
                listHouse = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptsByIds(resIds);

            }
            else if (resType == (int)MeterTypeEnum.GasMeter || resType == (int)MeterTypeEnum.WaterMeter || resType == (int)MeterTypeEnum.WattHourMeter)
            {
                resType = (int)ReourceTypeEnum.ThreeMeter;
                listMeters = meterService.GetMeterDTOS(resIds);

            }

            if (resIds.Count > 0)
            {
                SubjectBillView obj = new SubjectBillView();
                /*1.以前生成过的*/
                listHaveBill = service.GetChargBillDTOList(subjectIds, resType, resIds).OrderByDescending(o => o.EndDate).ToList();

                #region   /*根据资源绑定科目、生成账单*/
                if (Type == 1)
                {
                    var listBillGroup = listHaveBill.GroupBy(o => new { o.ChargeSubjectId, o.ResourcesId }).ToList();
                    foreach (var o in listBillGroup)
                    {
                        ChargBillDTO model = listHaveBill.Where(m => m.ChargeSubjectId == o.Key.ChargeSubjectId).OrderByDescending(n => n.EndDate).ToList().FirstOrDefault();
                        var t = subjetcModels.FirstOrDefault(m => m.Id == o.Key.ChargeSubjectId);

                        /*房屋和车位*/
                        if (resType == (int)ReourceTypeEnum.CarPark || resType == (int)ReourceTypeEnum.House)
                        {
                            //获取账单计费开始时间 2017-8-30 bug #5408
                            var houseref = list.Where(l => l.ResourcesId == model.ResourcesId && l.ChargeSubjecId == model.ChargeSubjectId).FirstOrDefault();
                            DateTime? beginDate = null;
                            if (houseref != null)
                            {
                                beginDate = houseref.BeginDateBill;
                            }
                            /*账单结束日期、小于当前时间*/
                            //if (Convert.ToDateTime(model.EndDate) < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                            // 计费开始日有且小于今天
                            if ((beginDate.HasValue && Convert.ToDateTime(beginDate.Value.ToString("yyyy-MM-dd")) < Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd")))
                                && Convert.ToDateTime(model.EndDate) < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                            {
                                obj = new SubjectBillView()
                                {
                                    ResourceId = o.Key.ResourcesId.ToString(),
                                    ResType = resType,
                                    SubjectId = Convert.ToInt32(t.Id),
                                    SubjctName = t.Name,
                                    HouseDeptId = list.FirstOrDefault(m => m.ChargeSubjecId == o.Key.ChargeSubjectId && m.ResourcesId == o.Key.ResourcesId).HouseDeptId.ToString(),
                                    IsChecked = false,
                                    BillEndTime = Convert.ToDateTime(Convert.ToDateTime(model.EndDate).ToString("yyyy-MM-dd")),
                                };

                                switch (resType)
                                {
                                    case (int)ReourceTypeEnum.House:
                                        obj.ResourceName = listHouse.Where(c => c.Id == resIds[0]).FirstOrDefault().Name;
                                        break;
                                    case (int)ReourceTypeEnum.CarPark:
                                        obj.ResourceName = dicCarport[Convert.ToInt32(o.Key.ResourcesId)];
                                        break;
                                }

                                listSubjectBillView.Add(obj);
                            }
                        }
                        /*三表*/
                        else if (resType == (int)ReourceTypeEnum.ThreeMeter)
                        {
                            if (MeterDataCheck(o.Key.ResourcesId))
                            {
                                obj = new SubjectBillView()
                                {
                                    ResourceId = o.Key.ResourcesId.ToString(),
                                    ResType = resType,
                                    SubjectId = Convert.ToInt32(t.Id),
                                    SubjctName = t.Name,
                                    HouseDeptId = list.FirstOrDefault(m => m.ChargeSubjecId == o.Key.ChargeSubjectId && m.ResourcesId == o.Key.ResourcesId).HouseDeptId.ToString(),
                                    IsChecked = false,
                                    BillEndTime = model.EndDate,
                                    ResourceName = listMeters.Where(c => c.Id == o.Key.ResourcesId).FirstOrDefault().MeterNum
                                };
                                listSubjectBillView.Add(obj);
                            }
                        }

                        list.Remove(list.FirstOrDefault(m => m.ChargeSubjecId == o.Key.ChargeSubjectId));
                    }
                }
                #endregion

                #region   /*根据科目绑定资源、生成账单*/
                if (Type == 2)
                {
                    var t = subjetcModels.FirstOrDefault();
                    var listBillGroup = listHaveBill.GroupBy(o => o.ResourcesId).ToList();
                    foreach (var o in listBillGroup)
                    {

                        var model = listHaveBill.Where(m => m.ResourcesId == o.Key).OrderByDescending(m => m.EndDate).FirstOrDefault();
                        /*房屋和车位*/
                        if (resType == (int)ReourceTypeEnum.CarPark || resType == (int)ReourceTypeEnum.House)
                        {
                            //获取账单计费开始时间 2017-8-30 bug #5408
                            var houseref = list.Where(l => l.ResourcesId == model.ResourcesId && l.ChargeSubjecId == model.ChargeSubjectId).FirstOrDefault();
                            DateTime? beginDate = null;
                            if (houseref != null)
                            {
                                beginDate = houseref.BeginDateBill;
                            }
                            /*账单结束日期、小于当前时间*/
                            // 计费开始日有且小于今天
                            if ((beginDate.HasValue && Convert.ToDateTime(beginDate.Value.ToString("yyyy-MM-dd")) < Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"))) 
                                && Convert.ToDateTime(model.EndDate) < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                            {
                                obj = new SubjectBillView()
                                {
                                    ResourceId = o.Key.ToString(),
                                    ResType = resType,
                                    SubjectId = Convert.ToInt32(t.Id),
                                    SubjctName = t.Name,
                                    HouseDeptId = list.Where(m => m.ResourcesId == o.Key).FirstOrDefault().HouseDeptId.ToString(),
                                    IsChecked = false,
                                    BillEndTime = Convert.ToDateTime(Convert.ToDateTime(model.EndDate).ToString("yyyy-MM-dd")),
                                };
                                switch (resType)
                                {
                                    case (int)ReourceTypeEnum.House:
                                        obj.ResourceName = listHouse.Where(c => c.Id == o.Key).FirstOrDefault().Name;
                                        break;
                                    case (int)ReourceTypeEnum.CarPark:
                                        obj.ResourceName = dicCarport[Convert.ToInt32(o.Key)];
                                        break;
                                    case (int)ReourceTypeEnum.ThreeMeter:
                                        obj.ResourceName = listMeters.Where(c => c.Id == o.Key).FirstOrDefault().MeterNum;
                                        break;
                                }
                                listSubjectBillView.Add(obj);
                            }
                        }
                        /*三表*/
                        else if (resType == (int)ReourceTypeEnum.ThreeMeter)
                        {
                            if (MeterDataCheck(o.Key))
                            {
                                obj = new SubjectBillView()
                                {
                                    ResourceId = o.Key.ToString(),
                                    ResType = resType,
                                    SubjectId = Convert.ToInt32(t.Id),
                                    SubjctName = t.Name,
                                    HouseDeptId = list.Where(m => m.ResourcesId == o.Key).FirstOrDefault().HouseDeptId.ToString(),
                                    IsChecked = false,
                                    BillEndTime = model.EndDate,
                                    ResourceName = listMeters.Where(c => c.Id == o.Key).FirstOrDefault().MeterNum
                                };
                                listSubjectBillView.Add(obj);
                            }
                        }
                        list.Remove(list.FirstOrDefault(m => m.ResourcesId == o.Key));

                    }
                }
                #endregion

                #region   /*没有生成过账单的*/
                foreach (var o in list)
                {
                    /*账单开始日和今天对比*/
                    var t = subjetcModels.FirstOrDefault(m => m.Id == o.ChargeSubjecId);
                    //获取账单计费开始时间 2017-8-30 bug #5408
                    var bdate = o.BeginDateBill > t.BeginDate ? o.BeginDateBill : t.BeginDate;

                    SubjectHouseRefDTO model = GetSubjectHouseRefByResId(o.ResourcesId.Value, o.ChargeSubjecId.Value);
                    /*房屋和车位*/
                    if (resType == (int)ReourceTypeEnum.CarPark || resType == (int)ReourceTypeEnum.House)
                    {
                        /*账单开始日小于或等于今天的时间*/
                        //2017-8-30 修改 bug #5408
                        //if (t.BeginDate <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                        if (bdate <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                        {
                            obj = new SubjectBillView()
                            {
                                ResourceId = o.ResourcesId.ToString(),
                                ResType = resType,
                                SubjectId = Convert.ToInt32(t.Id),
                                SubjctName = t.Name,
                                BillEndTime = model.CreateTime,
                                HouseDeptId = o.HouseDeptId.ToString(),
                                IsChecked = false
                            };
                            switch (resType)
                            {
                                case (int)ReourceTypeEnum.House:
                                    obj.ResourceName = listHouse.Where(c => c.Id == o.ResourcesId).FirstOrDefault().Name;
                                    break;
                                case (int)ReourceTypeEnum.CarPark:
                                    obj.ResourceName = dicCarport[Convert.ToInt32(o.ResourcesId)];
                                    break;
                            }
                            listSubjectBillView.Add(obj);
                        }
                    }/*三表*/
                    else if (resType == (int)ReourceTypeEnum.ThreeMeter)
                    {
                        if (MeterDataCheck(o.ResourcesId))
                        {
                            obj = new SubjectBillView()
                            {
                                ResourceId = o.ResourcesId.ToString(),
                                ResType = resType,
                                SubjectId = Convert.ToInt32(t.Id),
                                SubjctName = t.Name,
                                BillEndTime = model.CreateTime,
                                HouseDeptId = o.HouseDeptId.ToString(),
                                IsChecked = false,
                                ResourceName = listMeters.Where(c => c.Id == o.ResourcesId).FirstOrDefault().MeterNum
                            };
                            listSubjectBillView.Add(obj);
                        }
                    }
                }

                #endregion
            }
            return listSubjectBillView;
        }
        /// <summary>
        /// 获取三表是否可以生成账单
        /// </summary>
        /// <param name="meterId">三表ID</param>
        /// <returns></returns>
        public bool MeterDataCheck(int? meterId)
        {
            using (var mrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var meter = mrUnitOfWork.MeterRepository.GetAll().Where(o => o.Id == meterId).FirstOrDefault();
                //查询当前收费项目对应的最后一条记录
                var lastReadRecord = mrUnitOfWork
                    .MeterReadRecordRepository
                    .GetAll()
                    .Where(r => r.MeterId == meterId)
                    .OrderByDescending(r => r.ReadDate)
                    .FirstOrDefault();
                //如果为null继续循环
                if (lastReadRecord == null)
                {
                    return false;
                }
                //如果抄表日期相等 不需要生成账单
                if (lastReadRecord.ReadDate.Value.ToShortDateString() == meter.ReadDate.Value.ToShortDateString())
                {
                    return false;
                }
                return true;
            }
        }
        #endregion

        #region 新增绑定收费项目
        /// <summary>
        /// 绑定收费项目
        /// </summary>
        /// <param name="model">绑定收费项目</param>
        /// <returns></returns>
        public ReturnResult InsertSubjectHouseRefCus(SubjectHouseRefDTO model)
        {
            ReturnResult res = new ReturnResult();
            try
            {
                res = Validation(model);
                if (!res.IsSuccess)
                {
                    return res;
                }
                bool isSuccess = InsertSubjectHouseRef(model);
                if (isSuccess)
                {
                    res.IsSuccess = true;
                    res.Msg = "处理成功!";
                    return res;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Msg = "处理失败!";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Msg = " 数据异常!";
                return res;
            }

        }
        #endregion

        #region 更新绑定收费项目
        /// <summary>
        /// 绑定收费项目
        /// </summary>
        /// <param name="model">绑定收费项目</param>
        /// <returns></returns>
        public ReturnResult UpdateSubjectHouseRefCus(SubjectHouseRefDTO model)
        {
            ReturnResult res = new ReturnResult();
            try
            {
                res = Validation(model);
                if (!res.IsSuccess)
                {
                    return res;
                }
                bool isSuccess = UpdateSubjectHouseRef(model);
                if (isSuccess)
                {
                    res.IsSuccess = true;
                    res.Msg = "处理成功!";
                    return res;
                }
                else
                {
                    res.IsSuccess = false;
                    res.Msg = "处理失败!";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Msg = " 数据异常!";
                return res;
            }

        }
        #endregion

        #region 数据校验

        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ReturnResult Validation(SubjectHouseRefDTO model)
        {

            ReturnResult res = new ReturnResult()
            {
                IsSuccess = true
            };
            if (string.IsNullOrEmpty(model.ChargeSubjecId.ToString()) || !(model.ChargeSubjecId > 0))
            {
                res.Msg = "科目信息不能为空!";
                res.IsSuccess = false;
                return res;
            }
            //if (string.IsNullOrEmpty(model.RefType.ToString()) || !(model.RefType > 0))
            //{
            //    res.Msg = "关联类型不能为空!";
            //    res.IsSuccess = false;
            //    return res;
            //}
            if (string.IsNullOrEmpty(model.ResourcesId.ToString()) || !(model.ResourcesId > 0))
            {
                res.Msg = "资源信息不能为空!";
                res.IsSuccess = false;
                return res;
            }

            if (string.IsNullOrEmpty(model.DevBeginDate.ToString()) || string.IsNullOrEmpty(model.DevEndDate.ToString()))
            {
                if (model.IsDevPay.HasValue)
                {
                    res.Msg = "开发商代缴时间不能为空!";
                    res.IsSuccess = false;
                    return res;
                }
            }
            else
            {
                if (model.DevBeginDate > model.DevEndDate)
                {
                    res.Msg = "代缴初始时间必须大于结束时间!";
                    res.IsSuccess = false;
                    return res;
                }
            }
            return res;
        }
        #endregion

        #region 获取对象分页集合
        public IList<SubjectHouseRefDTO> Paging(int PageIndex, int PageSize, SubjectHouseRefDTO chargeSubjectDTO, string expressions, out int totalCount)
        {
            var dataList = SubjectHouseRefService.Paging(PageIndex, PageSize, c => c.ChargeSubjecId == chargeSubjectDTO.Id && c.IsDel == false, expressions, out totalCount);
            return SubjectHouseRefMappers.ChangeSubjectHouseRefToDTOs(dataList).ToList();
        }
        #endregion

        #region 获取资源绑定科目下的对象
        /// <summary>
        /// 获取资源绑定科目下的对象
        /// </summary>
        /// <param name="subjectId">科目</param>
        /// <param name="HouseDeptId">房屋</param>
        /// <returns></returns>
        public SubjectHouseRefDTO GetSubjectHouseRefDTO(int subjectId, int houseDeptId)
        {
            SubjectHouseRef model = SubjectHouseRefService.GetSubjectHouseRefSingle(o => o.ChargeSubjecId == subjectId && o.HouseDeptId == houseDeptId && o.IsDel == false);
            return SubjectHouseRefMappers.ChangeSubjectHouseRefToDTO(model);
        }
        public SubjectHouseRefDTO GetSubjectHouseRef(int resId, int subjectType)
        {
            SubjectHouseRef model = SubjectHouseRefService.GetSubjectHouseRefSingle(o => o.ResourcesId == resId && o.IsDel == false && o.SubjectType == subjectType);
            return SubjectHouseRefMappers.ChangeSubjectHouseRefToDTO(model);
        }
        public SubjectHouseRefDTO GetSubjectHouseRefByResId(int resId, int subjectId)
        {
            SubjectHouseRef model = SubjectHouseRefService.GetSubjectHouseRefSingle(o => o.ChargeSubjecId == subjectId && o.ResourcesId == resId && o.IsDel == false);
            return SubjectHouseRefMappers.ChangeSubjectHouseRefToDTO(model);
        }
        public List<SubjectHouseRefDTO> GetSubjectHouseRefList(int subjectId, List<int?> houseDeptIds)
        {
            List<SubjectHouseRef> list = SubjectHouseRefService.GetChargeSubjectList(o => o.ChargeSubjecId == subjectId && houseDeptIds.Contains(o.ResourcesId) && o.IsDel == false);

            return SubjectHouseRefMappers.ChangeSubjectHouseRefToDTOs(list);
        }
        public List<SubjectHouseRefDTO> GetSubjectHouseRefList(List<int?> subjectIds, int resId)
        {
            List<SubjectHouseRef> list = SubjectHouseRefService.GetChargeSubjectList(o => subjectIds.Contains(o.ChargeSubjecId) & o.ResourcesId == resId && o.IsDel == false);
            return SubjectHouseRefMappers.ChangeSubjectHouseRefToDTOs(list);
        }
        #endregion



        #region 批量新增或更新
        /// <summary>
        /// 批量新增或更新
        /// </summary>
        /// <param name="list"></param> 


        public bool InsertOrUpdateSubjectHouseRefList(List<SubjectHouseRefDTO> list, int operaterId, string opearaterName, DeveloperSetTimeListDTO model)
        {
            List<SubjectHouseRef> listBatchSet = new List<SubjectHouseRef>();
            list.ForEach(o =>
            {
                listBatchSet.Add(SubjectHouseRefMappers.ChangeDTOToSubjectHouseRefNew(o));
            });
            return SubjectHouseRefService.BatchInsertOrUpdateSubjectHouseRefList(listBatchSet, operaterId, model);
        }
        #endregion

        #region 查找科目
        public List<SubjectHouseRefDTO> GetBindSubject(int resId, int subjectType)
        {
            var dataList = SubjectHouseRefService.GetChargeSubjectList(o => o.IsDel == false && o.ResourcesId == resId && o.SubjectType == subjectType);
            return SubjectHouseRefMappers.ChangeSubjectHouseRefToDTOs(dataList).ToList();
        }

        /// <summary>
        /// 获取绑定的收费项目
        /// </summary>
        public List<SubjectHouseRefDTO> GetBindSubjectInfo(int resId, int subjectType)
        {
            return SubjectHouseRefService.GetChargeSubjectInfoList(resId, subjectType);
        }
        #endregion


        public IEnumerable<TemplateModel> GetSubjectBindLogTemplate()
        {
            int i = 1;
            TemplateColumn[] showColumns = new TemplateColumn[]
            {
                new TemplateColumn(){ ColumnName = "ResourceName", ColumnDesc = "资源名称", Seq = i++},
                new TemplateColumn(){ ColumnName = "SubjectName", ColumnDesc = "科目名称", Seq = i++},
                new TemplateColumn(){ ColumnName = "OperateTypeName", ColumnDesc = "操作类型", Seq = i++},
                new TemplateColumn(){ ColumnName = "OperateTimeFormat", ColumnDesc = "操作时间", Seq = i++},
                new TemplateColumn(){ ColumnName = "OperateName", ColumnDesc = "操作人", Seq = i++},
            };
            IEnumerable<TemplateModel> template = TemplateModelHelper.GetTemplateModels(typeof(SubjectBindLogDTO), showColumns);
            return template;
        }

        #region 绑定科目日志查询


        public IList<SubjectBindLogDTO> GetSubjectHouseRefDTOList(SubjectBindLogSearchDTO searchDto, out int totalCount)
        {
            totalCount = 0;
            Condition<SubjectHouseRef> condition = new Condition<SubjectHouseRef>(c => c.Id > 0);
            if (searchDto.DeptId != 0)
            {
                condition = condition & new Condition<SubjectHouseRef>(c => c.ChargeSubject.ComDeptId == searchDto.DeptId);
            }
            if (!string.IsNullOrEmpty(searchDto.ResourceName))
            {
                condition = condition & new Condition<SubjectHouseRef>(c => c.ResourceName.Contains(searchDto.ResourceName));
            }
            if (!string.IsNullOrEmpty(searchDto.SubjectId))
            {
                int subjectId = Convert.ToInt32(searchDto.SubjectId.Split(':')[1]);
                condition = condition & new Condition<SubjectHouseRef>(c => c.ChargeSubjecId == subjectId);
            }

            //if (string.IsNullOrEmpty(searchDto.BeginTime))
            //{
            //    DateTime now = DateTime.Now;
            //    DateTime beginTime = new DateTime(now.Year, now.Month, 1);
            //    condition = condition & new Condition<SubjectHouseRef>(c => c.CreateTime >= beginTime);
            //}
            //else
            //{
            //    DateTime benginTime = Convert.ToDateTime(searchDto.BeginTime);
            //    condition = condition & new Condition<SubjectHouseRef>(c => c.CreateTime >= benginTime);
            //}
            //if (string.IsNullOrEmpty(searchDto.EndTime))
            //{
            //    DateTime now = DateTime.Now;
            //    condition = condition & new Condition<SubjectHouseRef>(c => c.CreateTime <= now);
            //}
            //else
            //{

            //    DateTime endTime = Convert.ToDateTime(searchDto.EndTime).AddDays(1).AddSeconds(-1);
            //    condition = condition & new Condition<SubjectHouseRef>(c => c.CreateTime <= endTime);
            //}


            string expressions = "Id";
            var domainList = SubjectHouseRefService.GetSubjectBindLog(searchDto.PageStart, searchDto.PageSize, condition.ExpressionBody, searchDto.BeginTime, searchDto.EndTime, expressions, out totalCount);
            List<int?> userIds = new List<int?>();
            //userIds.AddRange(domainList.ToList().ConvertAll(o => o.Operator).ToList());
            userIds.AddRange(domainList.ToList().ConvertAll(o => (!string.IsNullOrEmpty(o.Operator.ToString()) ? o.Operator : 0)).ToList());
            var users = PresentationServiceHelper.LookUp<IPropertyService>().GetAdminUsers(userIds.ToArray());
            domainList.ToList().ForEach(o =>
            {
                string Name = "";
                if (o.Operator == -1)
                {
                    Name = "外部系统";
                }
                if (o.Operator == 0)
                {
                    Name = "内部系统";
                }
                string operateName = users.Any(m => m.Id == o.Operator) ? users.FirstOrDefault(m => m.Id == o.Operator).UserName : Name;
                //string relieveOperator = o.RelieveOperator.HasValue ? (users.Any(m => m.Id == o.Operator) ? users.FirstOrDefault(m => m.Id == o.Operator).UserName : "") : "";
                if (o.IsDel.Value)
                {
                    o.OperateTypeName = "解除绑定";
                    o.OperateName = operateName;

                }
                else
                {
                    o.OperateTypeName = "绑定";
                    o.OperateName = operateName;
                }
            });
            return domainList.OrderBy(o => o.ResourceName).ThenByDescending(o => o.OperateTime).Take(searchDto.PageSize).ToList();
        }
        #endregion

        #region 接口单一解除车位绑定
        public void CarportChangeNotice([FromBody]APICarportChangeParameter para)
        {
            SubjectHouseRefDomainService SubjectHouseRef = new SubjectHouseRefDomainService();
            SubjectHouseRef.CarportChangeNotice(para);
        }
        #endregion

        #region 接口:通过手机号码获取业主信息
        public OwnerInformationDTO GetOwnerByBindingPhonerNumber(string BindingPhonerNumber)
        {
            SubjectHouseRefDomainService SubjectHouseRef = new SubjectHouseRefDomainService();
            return SubjectHouseRef.GetOwnerByBindingPhonerNumber(BindingPhonerNumber);
        }
        #endregion

        #region 接口：通过选着房屋进行自助缴费
        /// <summary>
        /// 通过小区ID获取楼栋信息
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DeptInfoDTO GetBuildingByComDeptId(int? ComDeptId)
        {
            SubjectHouseRefDomainService SubjectHouseRef = new SubjectHouseRefDomainService();
            return SubjectHouseRef.GetBuildingByComDeptId(ComDeptId);
        }
        /// <summary>
        /// 通过房屋选择并验证后获取住户详细信息
        /// </summary>
        /// <param name="RoomId"></param>
        /// <param name="BindingPhonerNumber"></param>
        /// <returns></returns>
        public OwnerInformationDTO GetOwnerByValidatePhonerNumber(int? RoomId, string BindingPhonerNumber)
        {
            SubjectHouseRefDomainService SubjectHouseRef = new SubjectHouseRefDomainService();
            return SubjectHouseRef.GetOwnerByValidatePhonerNumber(RoomId, BindingPhonerNumber);
        }
        #endregion
    }
}
