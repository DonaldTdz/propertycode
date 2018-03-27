using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.ApplicationService.Service;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.DomainService;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class ResTypeController : BaseController
    {
        #region Page_Load


        public ActionResult CarPlace(int commuDeptId)
        {

            ChargeSubjectAppService chargeSubjectAppService = new ChargeSubjectAppService();
            List<ChargeSubjectDTO> list = chargeSubjectAppService.GetChargeSubjectsByComDeptId(commuDeptId);
            return PartialView("Index");
        }
        public ActionResult MeterWater(int commuDeptId)
        {
            ChargeSubjectAppService chargeSubjectAppService = new ChargeSubjectAppService();
            List<ChargeSubjectDTO> list = chargeSubjectAppService.GetChargeSubjectsByComDeptId(commuDeptId);
            return PartialView("Index", list);
        }
        public ActionResult MeterElec(int commuDeptId)
        {
            ChargeSubjectAppService chargeSubjectAppService = new ChargeSubjectAppService();
            List<ChargeSubjectDTO> list = chargeSubjectAppService.GetChargeSubjectsByComDeptId(commuDeptId);
            return PartialView("Index", list);
        }
        public ActionResult MeterGas(int commuDeptId)
        {
            ChargeSubjectAppService chargeSubjectAppService = new ChargeSubjectAppService();
            List<ChargeSubjectDTO> list = chargeSubjectAppService.GetChargeSubjectsByComDeptId(commuDeptId);
            return PartialView("Index", list);
        }
        public ActionResult Default()
        {
            List<ChargeSubjectDTO> list = new List<ChargeSubjectDTO>();
            return PartialView("Index", list);
        }
        public ActionResult House(int commuDeptId, int resType)
        {

            ChargeSubjectAppService chargeSubjectAppService = new ChargeSubjectAppService();
            List<ChargeSubjectDTO> list = chargeSubjectAppService.GetChargeSubjectsByComDeptId(commuDeptId);
            return PartialView("Index", list);
        }
        #endregion

        #region 绑定科目或设置开发商代缴 NEW
        /// <summary>
        /// 绑定科目或设置开发商代缴(解绑后原来绑定的项目作废IsDel=true)
        /// </summary>
        /// <param name="ids">绑定资源的ID</param>
        /// <param name="subjectId">科目</param>
        /// <param name="resType">资源类型</param>
        /// <param name="bindBatchOrNot">批量绑定的资源（如:某栋楼）1：全选、2：没选、3半选</param>
        /// <param name="unBindAllHouseByBuild">绑定的或解除绑定的资源(如:某间房)</param>
        /// <param name="isPostBillSave">解绑时是否有账单</param>
        /// <returns></returns>
        public ActionResult SaveDataNew(string ids, int subjectId, int resType, string bindBatchOrNot, string unBindAllHouseByBuild, bool isPostBillSave)
        {

            DeptAppService deptService = new DeptAppService();
            DateTime time = DateTime.Now;
            SubjectHouseRefAppService seubjectHouseService = new SubjectHouseRefAppService();
            List<BindSubjectHouseRefBuild> listBindSubjectHouseRefBuild = JsonConvert.DeserializeObject<List<BindSubjectHouseRefBuild>>(bindBatchOrNot).ToList();/*批量处理某栋*/

            List<SubjectHouseRefDTO> bindList = new List<SubjectHouseRefDTO>();/*要绑定的资源*/
            List<SubjectHouseRefDTO> notBindSubjectBindHouseRefList = new List<SubjectHouseRefDTO>();/*要解除绑定的资源*/

            var jResult = new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new ResultModel() { IsSuccess = false, Msg = "没有资源用于绑定设置" }
            };

            #region 车库绑定
            if (resType == (int)EDeptType.CheKu)
            {
                /*需要绑定的车位*/
                Dictionary<int, int> dic = new Dictionary<int, int>();
                if (!string.IsNullOrEmpty(ids))
                {
                    ids.TrimEnd(',').Split(',').ToList().ForEach(o =>
                    {
                        var resArr = o.Split('_');
                        dic.Add(Convert.ToInt32(resArr[0]), Convert.ToInt32(resArr[1]));
                    });
                }
                /*需要解绑的车位*/
                List<int?> notBindCarPortList = new List<int?>();
                if (!string.IsNullOrEmpty(unBindAllHouseByBuild))
                {
                    unBindAllHouseByBuild.TrimEnd(',').Split(',').ToList().ForEach(o =>
                    {
                        var resArr = o.Split('_');
                        notBindCarPortList.Add(Convert.ToInt32(resArr[0]));
                    });
                }
                SubjectHouseRefAppService service = new SubjectHouseRefAppService();
                List<SubjectHouseRefDTO> subjectBindHouseRefList = service.GetSubjectHouseRefList(subjectId, dic.ToList().ConvertAll<int?>(o => o.Key).ToList());
                subjectBindHouseRefList.ForEach(o =>
                {
                    /*移除已经绑定过的*/
                    if (dic.Any(m => m.Key == o.ResourcesId))
                    {
                        dic.Remove(o.ResourcesId.Value);
                    }
                });
                /*新增绑定*/
                foreach (var o in dic.Keys)
                {
                    bindList.Add(new SubjectHouseRefDTO()
                    {
                        ChargeSubjecId = subjectId,
                        CreateTime = time,
                        UpdateTime = time,
                        HouseDeptId = dic[o],
                        Operator = CurrentAdminUser.Id,
                        ResourcesId = o,
                        SubjectType = (int)SubjectTypeEnum.ParkingSpace,
                        IsDel = false,
                        IsDevPay = false,
                    });
                }
                notBindSubjectBindHouseRefList = service.GetSubjectHouseRefList(subjectId, notBindCarPortList);
                notBindSubjectBindHouseRefList.ForEach(o => { o.IsDel = true; });
            }

            #endregion

            #region 三表绑定
            else if (resType == (int)MeterTypeEnum.GasMeter || resType == (int)MeterTypeEnum.WattHourMeter || resType == (int)MeterTypeEnum.WaterMeter)
            {
                MeterAppService merterAppService = new MeterAppService();
                /*1.绑定资源*/
                Dictionary<int, int> dic = new Dictionary<int, int>();
                List<int?> houseIds = new List<int?>();
                if (!string.IsNullOrEmpty(ids))
                {
                    ids.TrimEnd(',').Split(',').ToList().ForEach(o =>
                    {
                        dic.Add(Convert.ToInt32(o.Split('_')[0]), Convert.ToInt32(o.Split('_')[1]));
                        houseIds.Add(Convert.ToInt32(o.Split('_')[1]));
                    });

                }
                /*2.整栋资源*/
                var buildList = listBindSubjectHouseRefBuild.Where(o => o.State == 1);/*绑定的楼宇*/
                var bindHoustList = deptService.GetDeptHouseList(buildList.ToList().ConvertAll<int?>(o => Convert.ToInt32(o.ResId.Split('_')[0])).ToList());
                var bindDeptHouseIds = bindHoustList.ConvertAll<int?>(o => o.Id).ToList();
                bindDeptHouseIds.AddRange(houseIds);
                var meterList = merterAppService.GetMeterDTOS(bindDeptHouseIds, resType);/*有三表的房屋*/
                var haveBindMeters = seubjectHouseService.GetSubjectHouseRefList(subjectId, meterList.ConvertAll<int?>(o => Convert.ToInt32(o.Id)).ToList());/*已经绑定的三表房屋*/
                haveBindMeters.ForEach(o =>
                {
                    meterList.Remove(meterList.SingleOrDefault(m => m.Id == o.ResourcesId));/*移除已经绑定过的三表*/
                });
                meterList.ForEach(o =>
                {
                    bindList.Add(new SubjectHouseRefDTO()
                    {
                        ChargeSubjecId = subjectId,
                        CreateTime = time,
                        UpdateTime = time,
                        HouseDeptId = o.HouseDeptID,
                        Operator = CurrentAdminUser.Id,
                        ResourcesId = o.Id,
                        SubjectType = (int)SubjectTypeEnum.House,
                        IsDel = false,
                        IsDevPay = false,
                    });
                });


                /*3.解除绑定*/
                var buildBindAllNot = listBindSubjectHouseRefBuild.Where(o => o.State == 2);
                var notBindHoustList = deptService.GetDeptHouseList(buildBindAllNot.ToList().ConvertAll<int?>(o => Convert.ToInt32(o.ResId.Split('_')[0])).ToList());
                var notBindDeptHouseIds = notBindHoustList.ConvertAll<int?>(o => o.Id);
                List<int> resIdNotBind = new List<int>();/*要解除绑定的资源*/
                if (!string.IsNullOrEmpty(unBindAllHouseByBuild))
                {
                    /*单个解除绑定的房屋*/
                    string[] notBindRes = unBindAllHouseByBuild.TrimEnd(',').Split(',');
                    notBindRes.ToList().ForEach(o =>
                    {
                        string[] items = o.Split('_');
                        if (items.Length == 3)
                        {
                            resIdNotBind.Add(Convert.ToInt32(o.Split('_')[2]));
                            notBindDeptHouseIds.Add(Convert.ToInt32(o.Split('_')[0]));
                        }

                    });
                }
                var houseMeters = merterAppService.GetMeterDTOS(notBindDeptHouseIds, resType);/*解除绑定有三表的房屋*/
                notBindSubjectBindHouseRefList = seubjectHouseService.GetSubjectHouseRefList(subjectId, houseMeters.ConvertAll<int?>(o => o.Id));
                notBindSubjectBindHouseRefList.ForEach(o => { o.IsDel = true; });

            }
            #endregion

            #region 楼宇绑定
            else if (resType == (int)EDeptType.LouYu)
            {
                /*全部绑定的楼宇*/
                var buildList = listBindSubjectHouseRefBuild.Where(o => o.State == 1);/*绑定的楼宇*/
                var bindHoustList = deptService.GetDeptHouseList(buildList.ToList().ConvertAll<int?>(o => Convert.ToInt32(o.ResId.Split('_')[0])).ToList());
                var bindDeptHouseIds = bindHoustList.ConvertAll<int?>(o => o.Id).ToList();
                if (!string.IsNullOrEmpty(ids))
                {
                    var resChecks = ids.TrimEnd(',').Split(',').ToList().ConvertAll<int?>(o => Convert.ToInt32(o)).ToList();
                    bindDeptHouseIds.AddRange(resChecks);
                }

                /*已经绑定的房屋要排除*/
                var haveBindList = seubjectHouseService.GetSubjectHouseRefList(subjectId, bindDeptHouseIds);
                var tempListIds = haveBindList.ConvertAll(o => o.HouseDeptId).ToList();
                bindDeptHouseIds.RemoveAll(o => tempListIds.Any(m => m == o));
                bindDeptHouseIds.ForEach(o =>
                {
                    bindList.Add(new SubjectHouseRefDTO()
                    {
                        ChargeSubjecId = subjectId,
                        CreateTime = time,
                        UpdateTime = time,
                        HouseDeptId = o,
                        Operator = CurrentAdminUser.Id,
                        ResourcesId = o,
                        SubjectType = (int)SubjectTypeEnum.House,
                        IsDel = false,
                        IsDevPay = false,
                    });
                });
                /*获取解除绑定的房屋ID*/
                var buildBindAllNot = listBindSubjectHouseRefBuild.Where(o => o.State == 2);
                var notBindHoustList = deptService.GetDeptHouseList(buildBindAllNot.ToList().ConvertAll<int?>(o => Convert.ToInt32(o.ResId.Split('_')[0])).ToList());
                var notBindDeptHouseIds = notBindHoustList.ConvertAll<int?>(o => o.Id);
                if (!string.IsNullOrEmpty(unBindAllHouseByBuild))
                {
                    /*单个解除绑定的房屋*/
                    string[] notBindRes = unBindAllHouseByBuild.TrimEnd(',').Split(',');
                    notBindRes.ToList().ForEach(o =>
                    {
                        notBindDeptHouseIds.Add(Convert.ToInt32(o.Split('_')[0]));
                    });
                }
                /*已绑定的房屋解除绑定*/
                notBindSubjectBindHouseRefList = seubjectHouseService.GetSubjectHouseRefList(subjectId, notBindDeptHouseIds);
                notBindSubjectBindHouseRefList.ForEach(o =>
                {
                    o.IsDel = true;
                });
            }
            #endregion

            if (bindList.Count > 0 || notBindSubjectBindHouseRefList.Count > 0)
            {
                ChargeSubjectAppService subjectService = new ChargeSubjectAppService();
                var subjectModel = subjectService.GetChargeSubjectByKey(subjectId);
                jResult.Data = seubjectHouseService.InsertSubjectHouseRefList(resType, bindList, subjectModel.ChargeFormula, notBindSubjectBindHouseRefList, subjectId, isPostBillSave, CurrentAdminUser.Id.Value, CurrentAdminUser.UserName);
            }
            return jResult;
        }
        #endregion

        #region 绑定科目或设置开发商代缴
        /// <summary>
        /// 绑定科目或设置开发商代缴(解绑后原来绑定的项目作废IsDel=true)
        /// </summary>
        /// <param name="ids">绑定资源的ID</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="isDevPay">是否开发商代缴</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="subjectId">科目</param>
        /// <param name="houseDeptIds">房屋ID</param>
        /// <param name="resType">资源类型</param>
        /// <param name="isSetTime">设置开发商代缴（如果为false那么就是绑定科目）</param>
        /// <param name="isBind">设置开发商代缴（如果为false那么就是绑定科目）</param>
        /// <returns></returns>
        [HttpPost]

        public ActionResult SaveData(string ids, int subjectId, int resType, string bindBatchOrNot, string unBindAllHouseByBuild, bool isPostBillSave)
        {
            try
            {
                /*
                 * 房屋:14499_20 ，房屋ID+类型
                 * 车位:3_4d2ebfab-d458-4eb4-b3aa-4025f4215c93_14467，车位ID+车库ID+房屋ID
                 * 三表:14467_1000_6,房屋ID+类型+三表ID
                 */
                string SingleSet = "";
                int resTypeSource = resType;
                DateTime time = DateTime.Now;
                SubjectHouseRefDTO model;
                DeptAppService deptService = new DeptAppService();
                ChargeSubjectAppService subjectService = new ChargeSubjectAppService();
                ChargeSubjectDTO subjectDTO = subjectService.GetChargeSubjectByKey(subjectId);
                List<SubjectHouseRefDTO> list = new List<SubjectHouseRefDTO>();
                List<SubjectHouseRefDTO> listNotBindSubjectHouseRef = new List<SubjectHouseRefDTO>();

                List<string> resIds = new List<string>();
                string strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";" + (int)EDeptType.LouYu + ";" + (int)EDeptType.FangWu;

                if (!(subjectDTO.Id > 0))
                {
                    return new JsonResult() { Data = new ResultModel() { IsSuccess = false, Msg = "没有科目信息" } };
                }
                int operatorPer = Convert.ToInt32(CurrentAdminUser.Id);

                DateTime devBeginDate = DateTime.MinValue;
                DateTime devEndDate = DateTime.MinValue;
                List<BindSubjectHouseRefBuild> listBindSubjectHouseRefBuild = JsonConvert.DeserializeObject<List<BindSubjectHouseRefBuild>>(bindBatchOrNot).ToList();/*批量处理某栋*/
                listBindSubjectHouseRefBuild = listBindSubjectHouseRefBuild.Where(o => o.State != 3).ToList();/*半选状态的不做处理*/
                listBindSubjectHouseRefBuild = listBindSubjectHouseRefBuild.Where((x, i) => listBindSubjectHouseRefBuild.FindIndex(z => z.ResId == x.ResId) == i).ToList();
                StringBuilder sb = new StringBuilder();

                List<AsynTreeNodeModel> listBindAsynTreeNodeModel = new List<AsynTreeNodeModel>();
                List<AsynTreeNodeModel> listNotBindAsynTreeNodeModel = new List<AsynTreeNodeModel>();
                List<int?> listAllNotBindBuild = new List<int?>();
                List<int?> listAllBindBuild = new List<int?>();

                List<AsynTreeNodeModel> listMeterBindT = new List<AsynTreeNodeModel>();
                List<AsynTreeNodeModel> listMeterUnBindT = new List<AsynTreeNodeModel>();

                SubjectHouseRefAppService subjectHouseAppService = new SubjectHouseRefAppService();
                MeterAppService merterAppService = new MeterAppService();

                var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();

                /*绑定的三表*/
                List<MeterDTO> listBindMeters = new List<MeterDTO>();
                /*解绑的三表 */
                List<MeterDTO> listBindUnMeters = new List<MeterDTO>();


                /*绑定的楼栋ID*/
                List<int?> listBuilds = new List<int?>();
                /*解绑的楼栋ID*/
                List<int?> listUnBuilds = new List<int?>();
                /*绑定的房屋ID*/
                List<int?> listBindHouseIds = new List<int?>();
                /*解绑的房屋ID*/
                List<int?> listUnBindHouseIds = new List<int?>();


                /*绑定的楼宇信息ID*/
                listBindSubjectHouseRefBuild.Where(o => o.IsBind == true).ToList().ForEach(o =>
                {
                    listBuilds.Add(Convert.ToInt32(o.ResId.ToString().Split('_')[0]));
                });
                /*绑定的房屋信息ID*/
                foreach (var o in deptService.GetDeptHouseList(listBuilds))
                {
                    listBindAsynTreeNodeModel.Add(new AsynTreeNodeModel()
                    {
                        id = o.Id + "_" + 20,
                        text = o.Name
                    });
                    listBindHouseIds.Add(o.Id);
                }
                /*解绑的楼宇ID*/
                listBindSubjectHouseRefBuild.Where(o => o.IsBind == false).ToList().ForEach(o =>
                {
                    listUnBuilds.Add(Convert.ToInt32(o.ResId.ToString().Split('_')[0]));
                });
                /*解绑的房屋信息ID*/
                foreach (var o in deptService.GetDeptHouseList(listUnBuilds))
                {
                    listNotBindAsynTreeNodeModel.Add(new AsynTreeNodeModel()
                    {
                        id = o.Id + "_" + 20,
                        text = o.Name
                    });
                    listUnBindHouseIds.Add(o.Id);
                }

                if (resType == (int)MeterTypeEnum.GasMeter || resType == (int)MeterTypeEnum.WattHourMeter || resType == (int)MeterTypeEnum.WaterMeter)
                {
                    /*解绑的三表*/
                    listBindUnMeters = merterAppService.GetMeterDTOS(listUnBindHouseIds, resType);/*房屋下的三表*/
                    listBindUnMeters.ForEach(o =>
                    {
                        if (!listMeterUnBindT.Any(m => m.id == (o.Id + "_" + o.HouseDeptID)))
                        {
                            listMeterUnBindT.Add(new AsynTreeNodeModel() { id = o.Id + "_" + o.HouseDeptID });
                        }
                    });
                    /*绑定的三表*/
                    listBindMeters = merterAppService.GetMeterDTOS(listBindHouseIds, resType);/*房屋下的三表*/
                    listBindMeters.ForEach(o =>
                    {
                        if (!listMeterBindT.Any(m => m.id == (o.Id + "_" + o.HouseDeptID)))
                        {
                            listMeterBindT.Add(new AsynTreeNodeModel() { id = o.Id + "_" + o.HouseDeptID });
                        }
                    });
                    listNotBindAsynTreeNodeModel = listMeterUnBindT.Distinct().ToList();
                    listBindAsynTreeNodeModel = listMeterBindT.Distinct().ToList();
                }

                listNotBindAsynTreeNodeModel.ForEach(o =>
                {
                    listAllNotBindBuild.Add(Convert.ToInt32(o.id.Split('_')[0]));
                });

                /*选择取消绑定的具体资源*/
                string[] arr = new string[3];
                if (!string.IsNullOrEmpty(unBindAllHouseByBuild))
                {
                    foreach (string item in unBindAllHouseByBuild.TrimEnd(',').Split(','))
                    {
                        arr = item.Split('_');
                        /*三表*/
                        if ((resType == (int)MeterTypeEnum.GasMeter || resType == (int)MeterTypeEnum.WattHourMeter || resType == (int)MeterTypeEnum.WaterMeter) && arr.Length == 3)
                        {
                            listAllNotBindBuild.Add(Convert.ToInt32(arr[2]));
                        }
                        else
                        {
                            /*车库和房屋*/
                            listAllNotBindBuild.Add(Convert.ToInt32(arr[0]));
                        }
                    }
                }
                /*1.需要消除绑定的资源*/
                listNotBindSubjectHouseRef = subjectHouseAppService.GetSubjectHouseRefList(subjectId, listAllNotBindBuild);
                listNotBindSubjectHouseRef.ForEach(o => { o.IsDel = true; });


                /*构建需要绑定元素的ID*/
                List<int?> resIdNoType = new List<int?>();
                if (resType == (int)EDeptType.LouYu)
                {
                    resType = (int)ReourceTypeEnum.House;
                    listBindHouseIds.ForEach(o =>
                    {
                        sb.Append(o.Value + ",");
                    });
                    sb.Append(ids);
                    resIds = (ids + sb.ToString()).Split(',').Distinct().ToList();
                    resIdNoType = Array.ConvertAll<string, int?>(resIds.Where(s => !string.IsNullOrEmpty(s)).ToArray(), s => int.Parse(s)).ToList();
                }
                else
                {
                    resType = (int)ReourceTypeEnum.ThreeMeter;
                    foreach (var item in listBindAsynTreeNodeModel)
                    {
                        sb.Append(item.id);
                        sb.Append(",");
                    }
                    sb.Append(ids);
                    resIds = (ids + sb.ToString()).Split(',').Distinct().ToList();
                    resIdNoType = Array.ConvertAll<string, int?>(resIds.Where(s => !string.IsNullOrEmpty(s)).ToArray(), s => int.Parse(s.Split('_')[0])).ToList();
                }

                int notHasHouseDept = 0;
                /*获已经绑定的元素*/
                SubjectHouseRefAppService service = new SubjectHouseRefAppService();
                List<SubjectHouseRefDTO> listSubjectRef = service.GetSubjectHouseRefList(subjectId, resIdNoType);
                SubjectHouseRefDTO isExists = new SubjectHouseRefDTO();
                /*2.需要设置开发商代缴的资源*/
                List<DevTime> listDevSetTime = null;
                if (!string.IsNullOrEmpty(SingleSet))
                {
                    listDevSetTime = JsonConvert.DeserializeObject<List<DevTime>>(SingleSet).ToList();/*批量处理某栋*/
                }
                if (listDevSetTime != null && listDevSetTime.Count > 0)
                {
                    List<int?> setTimeResIdList = new List<int?>();
                    listDevSetTime.ForEach(o => { setTimeResIdList.Add(o.ResId); });
                    List<SubjectHouseRefDTO> setTimeSubjectHouseRefList = service.GetSubjectHouseRefList(subjectId, setTimeResIdList);

                    foreach (var m in listDevSetTime)
                    {
                        devBeginDate = Convert.ToDateTime(m.SetTime.Split('_')[0]);
                        devEndDate = Convert.ToDateTime(m.SetTime.Split('_')[1]);

                        /*移除选中的元素*/
                        if (resType == (int)ReourceTypeEnum.House)
                        {
                            resIds.Remove(m.ResId.ToString());/*移除需要单个设置时间的元素*/
                        }
                        else
                        {
                            resIds.Remove(m.ResId.ToString() + "_" + m.HouseId);/*移除需要单个设置时间的元素*/
                        }

                        SubjectHouseRefDTO isBindSetTimemModel = setTimeSubjectHouseRefList.Where(o => o.ResourcesId == m.ResId).FirstOrDefault();
                        if (m.HouseId > 0)
                        {
                            bool? setIsDevPay;
                            DateTime? setDevBeginDate = devBeginDate;
                            DateTime? setDevEndDate = devEndDate;
                            if (!string.IsNullOrEmpty(devBeginDate.ToString()) && !string.IsNullOrEmpty(devEndDate.ToString()))
                            {
                                setIsDevPay = true;
                                setDevBeginDate = devBeginDate;
                                setDevEndDate = devEndDate;
                            }
                            else
                            {
                                setIsDevPay = false;
                                setDevBeginDate = null;
                                setDevEndDate = null;
                            }
                            if (isBindSetTimemModel != null)
                            {

                                isBindSetTimemModel.DevBeginDate = setDevBeginDate;
                                isBindSetTimemModel.DevEndDate = setDevEndDate;
                                isBindSetTimemModel.IsDevPay = setIsDevPay;
                            }
                            else
                            {
                                isBindSetTimemModel = new SubjectHouseRefDTO()
                                {
                                    ChargeSubjecId = subjectId,
                                    ResourcesId = m.ResId,
                                    CreateTime = time,
                                    UpdateTime = time,
                                    IsDel = false,
                                    Operator = operatorPer,
                                    SubjectType = subjectDTO.SubjectType,
                                    HouseDeptId = m.HouseId,
                                    DevBeginDate = setDevBeginDate,
                                    DevEndDate = setDevEndDate,
                                    IsDevPay = setIsDevPay
                                };
                                if (isBindSetTimemModel.HouseDeptId == 0)
                                {
                                    notHasHouseDept = notHasHouseDept + 1;
                                }

                            }
                            if (!list.Any(o => o.SubjectType == isBindSetTimemModel.SubjectType && o.ResourcesId == isBindSetTimemModel.ResourcesId && o.ChargeSubjecId == isBindSetTimemModel.ChargeSubjecId) && isBindSetTimemModel.HouseDeptId != 0)
                            {
                                list.Add(isBindSetTimemModel);
                            }
                        }
                    }
                }
                if (resType == (int)ReourceTypeEnum.House)
                {

                    listSubjectRef.ForEach(o =>
                    {
                        if (resIds.Any(m => m.Contains(o.ResourcesId.ToString())))
                        {
                            o.IsDevPay = false;
                            o.DevBeginDate = null;
                            o.DevEndDate = null;
                            resIds.Remove(o.ResourcesId.ToString());
                        }
                    });

                }
                else
                {
                    listSubjectRef.ForEach(n =>
                    {
                        if (resIds.Any(m => m.Contains(n.ResourcesId.ToString())))
                        {
                            n.IsDevPay = false;
                            n.DevBeginDate = null;
                            n.DevEndDate = null;
                            resIds.Remove(string.Format("{0}_{1}", n.ResourcesId, n.HouseDeptId));
                        }
                    });
                }

                list.AddRange(listSubjectRef);


                /*3.需要绑定的资源*/
                for (int i = 0; i <= resIds.Count - 2; i++)
                {
                    int? resourcesId;
                    int? houseDeptId;
                    /*房屋的ID*/
                    houseDeptId = resType == (int)ReourceTypeEnum.House ? Convert.ToInt32(resIds[i]) : Convert.ToInt32(resIds[i].ToString().TrimEnd(',').Split('_')[1]);
                    //if (houseDeptId == 0)
                    //{
                    //    continue;
                    //}
                    /*具体资源的ID*/
                    resourcesId = resType == (int)ReourceTypeEnum.House ? Convert.ToInt32(resIds[i]) : Convert.ToInt32(resIds[i].ToString().Split('_')[0]);
                    model = new SubjectHouseRefDTO()
                    {
                        BeginDateBill = DateTime.Now,
                        ChargeSubjecId = subjectId,
                        ResourcesId = resourcesId,
                        CreateTime = time,
                        UpdateTime = time,
                        IsDel = false,
                        Operator = operatorPer,
                        SubjectType = subjectDTO.SubjectType,
                        IsDevPay = false,
                        DevBeginDate = null,
                        DevEndDate = null,
                        HouseDeptId = houseDeptId ?? 0//修改bug4551 2017-7-4 zzh
                    };
                    if (model.HouseDeptId == 0)
                    {
                        notHasHouseDept = notHasHouseDept + 1;
                    }
                    list.Add(model);
                }
                SubjectHouseRefAppService sbujectBindAppService = new SubjectHouseRefAppService();
                ResultModel res = new ResultModel();
                if (list.Count > 0 || listNotBindSubjectHouseRef.Count > 0)
                {
                    res = sbujectBindAppService.InsertSubjectHouseRefList(resTypeSource, list, subjectDTO.ChargeFormula, listNotBindSubjectHouseRef, subjectId, isPostBillSave, CurrentAdminUser.Id.Value, CurrentAdminUser.UserName);
                    if (notHasHouseDept > 0 && res.IsSuccess)
                    {
                        res.Msg = string.Format("{0}", res.Msg);
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.Msg = "没有资源用于绑定设置";
                }

                var jResult = new JsonResult();
                jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                jResult.Data = res;
                return jResult;
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile("SaveData 异常：" + ex.Message, "YK.PropertyMgr.MVCWeb/ResTypeController", FileLogType.Exception);
                ResultModel res = new ResultModel() { IsSuccess = false, Msg = "数据异常" };
                return new JsonResult() { Data = res };
            }
        }

        #endregion

        #region 获取资源树
        /// <summary>
        /// 获取资源树
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="typeEle"></param>
        /// <param name="subjectId">0-不返回任何信息，>0返回正常的信息,-1: </param>
        /// <returns></returns>
        public List<CustomTreeNodeModel> GetTree(int id, int type, int typeEle, int subjectId, int? comDeptId)
        {
            string strFilter = string.Empty;
            ChargeSubjectDTO subjetcDTO = new ChargeSubjectDTO();
            ChargeSubjectAppService subjectAppService = new ChargeSubjectAppService();
            SubjectHouseRefAppService subjectHousRefAppService = new SubjectHouseRefAppService();
            DeptAppService deptService = new DeptAppService();
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            List<CustomTreeNodeModel> list = new List<CustomTreeNodeModel>();
            List<SubjectHouseRefDTO> listHouses = new List<SubjectHouseRefDTO>();
            StringBuilder sb = new StringBuilder();
            List<int?> listIds = new List<int?>();
            try
            {
                if (subjectId > 0)
                {
                    subjetcDTO = subjectAppService.GetChargeSubjectByKey(subjectId);
                }

                string[] itemFormulas = subjetcDTO.ChargeFormula.Split(',');

                switch (type)
                {
                    case (int)EDeptType.LouYu:/*房屋*/
                        strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";" + (int)EDeptType.LouYu + ";" + (int)EDeptType.FangWu;
                        if (subjetcDTO.SubjectType == (int)SubjectTypeEnum.House || subjetcDTO.SubjectType == (int)SubjectTypeEnum.Other && subjetcDTO.Id > 0)
                        {
                            List<CustomTreeNodeModel> listAsynTree = new List<CustomTreeNodeModel>();
                            list = deptService.GetDeptTree(CurrentAdminUser, id, strFilter, ref dic).Select(s => new CustomTreeNodeModel()
                            {
                                id = s.id,
                                icon = s.icon,
                                text = s.text,
                                children = s.children
                            }).ToList();
                            list = GetTreeSetChecked(list, dic, subjectId, type);
                            list = SetTime(list, subjectId);
                        }

                        break;
                    case (int)EDeptType.CheKu:/*车库*/
                        if (subjetcDTO.SubjectType == (int)SubjectTypeEnum.ParkingSpace && subjetcDTO.Id > 0)
                        {
                            var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();

                            list = propertyService.GetAsynParkingTree(id, ref dic).Where(o => o.children == true).Select(s => new CustomTreeNodeModel()
                            {
                                id = s.id,
                                icon = s.icon,
                                text = s.text,
                                children = s.children,
                                state = new { opened = true }
                            }).ToList();
                        }
                        break;
                    case (int)MeterTypeEnum.WaterMeter:/*水表*/
                        strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";" + (int)EDeptType.LouYu + ";" + (int)EDeptType.FangWu;
                        foreach (var item in itemFormulas)
                        {
                            if (item == ((int)ChargeFormulaEnum.WaterUnit).ToString())
                            {
                                if (subjetcDTO.SubjectType == (int)SubjectTypeEnum.Meter && subjetcDTO.Id > 0)
                                {
                                    list = GetListByResType(id, strFilter, (int)MeterTypeEnum.WaterMeter, subjectId, ref dic, comDeptId).Select(s => new CustomTreeNodeModel()
                                    {
                                        id = s.id,
                                        icon = s.icon,
                                        text = s.text,
                                        children = s.children,
                                        state = s.state
                                    }).ToList();
                                }
                            }
                        }
                        if (type == (int)MeterTypeEnum.WaterMeter && typeEle == 0)
                        {
                            list = GetTreeSetChecked(list, dic, subjectId, type);
                        }
                        break;
                    case (int)MeterTypeEnum.GasMeter:/*气表*/
                        strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";" + (int)EDeptType.LouYu + ";" + (int)EDeptType.FangWu;
                        foreach (var item in itemFormulas)
                        {
                            if (item == ((int)ChargeFormulaEnum.GasUnit).ToString())
                            {
                                if (subjetcDTO.SubjectType == (int)SubjectTypeEnum.Meter && subjetcDTO.Id > 0)
                                {
                                    list = GetListByResType(id, strFilter, (int)MeterTypeEnum.GasMeter, subjectId, ref dic, comDeptId).Select(s => new CustomTreeNodeModel()
                                    {
                                        id = s.id,
                                        icon = s.icon,
                                        text = s.text,
                                        children = s.children,
                                        state = s.state
                                    }).ToList();
                                }
                            }
                        }
                        if (type == (int)MeterTypeEnum.GasMeter && typeEle == 0)
                        {
                            list = GetTreeSetChecked(list, dic, subjectId, type);
                        }
                        break;
                    case (int)MeterTypeEnum.WattHourMeter:/*电表*/

                        foreach (var item in itemFormulas)
                        {
                            if (item == ((int)ChargeFormulaEnum.ElectricUnit).ToString())
                            {
                                strFilter = (int)EDeptType.RootNode + ";" + (int)EDeptType.WuYE + ";" + (int)EDeptType.XiaoQu + ";" + (int)EDeptType.LouYu + ";" + (int)EDeptType.FangWu;
                                if (subjetcDTO.SubjectType == (int)SubjectTypeEnum.Meter && subjetcDTO.Id > 0)
                                {
                                    list = GetListByResType(id, strFilter, (int)MeterTypeEnum.WattHourMeter, subjectId, ref dic, comDeptId).Select(s => new CustomTreeNodeModel()
                                    {
                                        id = s.id,
                                        icon = s.icon,
                                        text = s.text,
                                        children = s.children,
                                        state = s.state
                                    }).ToList();
                                }
                            }
                        }
                        if (type == (int)MeterTypeEnum.WattHourMeter && typeEle == 0)
                        {
                            list = GetTreeSetChecked(list, dic, subjectId, type);
                        }
                        break;

                }
                if (typeEle == 0)
                {
                    return list.Where(o => (bool)o.children).ToList();

                }
                else
                {
                    return list;
                }

            }
            catch (Exception ex)
            {
                return list;
            }
        }

        #endregion

        #region 结构树
        /// <summary>
        /// 结构树
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"> tab切换卡的类型  [楼宇12、车库13、水表1、电表2、气表3]</param>
        /// <param name="typeEle">资源类型</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSubjectSelectChildTree(int id, int type, int typeEle, int subjectId, int? comDeptId, string DecorationState_PM, string HouseState_PM)
        {
            var jResult = new JsonResult();
            int DecorationState; int HouseState;
            DeptAppService deptService = new DeptAppService();
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jResult.MaxJsonLength = int.MaxValue;
            if (string.IsNullOrEmpty(DecorationState_PM) || DecorationState_PM == "null")
            {
                DecorationState = 0;
            }
            else
            {
                DecorationState = Convert.ToInt32(DecorationState_PM);
            }
            if (string.IsNullOrEmpty(HouseState_PM) || HouseState_PM == "null")
            {
                HouseState = 0;
            }
            else
            {
                HouseState = Convert.ToInt32(HouseState_PM);
            }
            if (type == 13)
            {
                HouseState = 0;
                DecorationState = 0;
            }
            if ((DecorationState == 0 && HouseState == 0))
            {
                jResult.Data = GetTree(id, type, typeEle, subjectId, comDeptId);
            }
            else
            {
                jResult.Data = GetSubjectSelectTreeByHome(id, type, typeEle, subjectId, comDeptId, DecorationState, HouseState);
            }
            return jResult;
        }
        #endregion
        #region 按房屋条件显示结构树
        public List<CustomTreeNodeModel> GetSubjectSelectTreeByHome(int id, int type, int typeEle, int subjectId, int? comDeptId, int DecorationState_PM, int HouseState_PM)
        {
            var Data = GetTree(id, type, typeEle, subjectId, comDeptId);
            List<CustomTreeNodeModel> resultlist = new List<CustomTreeNodeModel>();
            foreach (var item in Data)
            {
                var tree = GetTree(Convert.ToInt32(item.id.Split('_')[0]), type, Convert.ToInt32(12), subjectId, comDeptId);
                var houseList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouseByComDeptId(Convert.ToInt32(item.id.Split('_')[0]), DecorationState_PM, HouseState_PM);
                var treelist = from t in tree
                               join h in houseList on Convert.ToInt32(t.id.Split('_')[0]) equals h.HouseId
                               select new CustomTreeNodeModel
                               {
                                   id = t.id,
                                   icon = t.icon,
                                   text = t.text,
                                   state = t.state,
                                   children = t.children
                               };
                var testlist = treelist.ToList();
                if (treelist.Count() > 0)
                {
                    var resultRoot = item;
                    resultRoot.children = testlist;
                    resultlist.Add(resultRoot);
                }
            }
            return resultlist;
        }


        #endregion

        #region 获取小区下的科目信息
        /// <summary>
        /// 获取小区下的科目信息
        /// </summary>
        /// <param name="CommId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSubjects(int commId)
        {
            ChargeSubjectAppService service = new ChargeSubjectAppService();
            var json = new JsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            json.Data = service.GetChargeSubjectsByComDeptId(commId).Where(o => o.SubjectType != (int)SubjectTypeEnum.SystemPreset && o.SubjectType != (int)SubjectTypeEnum.Other && o.BillPeriod != (int)BillPeriodEnum.Once).OrderByDescending(o => o.CreateTime).ToList(); ; ;
            return json;
        }
        #endregion

        #region 设置选中半选中(第一级树的选中状态)
        /// <summary>
        /// 设置选中半选中
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dic"></param>
        /// <param name="subjectId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<CustomTreeNodeModel> GetTreeSetChecked(List<CustomTreeNodeModel> list, Dictionary<string, List<string>> dic, int subjectId, int type)
        {
            SubjectHouseRefAppService subjectHousRefAppService = new SubjectHouseRefAppService();

            List<int?> listIds = new List<int?>();

            foreach (var m in dic)
            {
                foreach (var n in m.Value)
                {
                    listIds.Add(Convert.ToInt32(n.Split('_')[0]));
                }
            }
            List<SubjectHouseRefDTO> listHaveBind = subjectHousRefAppService.GetSubjectHouseRefList(subjectId, listIds);
            if (type == (int)MeterTypeEnum.WattHourMeter || type == (int)MeterTypeEnum.WaterMeter || type == (int)MeterTypeEnum.GasMeter)
            {
                MeterAppService meterAppService = new MeterAppService();
                List<MeterDTO> listMeter = meterAppService.GetMeterDTOS(listIds, type);
                listIds.Clear();
                foreach (var item in listMeter)
                {
                    listIds.Add(Convert.ToInt32(item.Id));
                }
                listHaveBind = subjectHousRefAppService.GetSubjectHouseRefList(subjectId, listIds);

            }

            foreach (var m in dic)
            {
                int isExists = 0;
                foreach (var n in m.Value)
                {
                    if (type == (int)MeterTypeEnum.WattHourMeter || type == (int)MeterTypeEnum.WaterMeter || type == (int)MeterTypeEnum.GasMeter)
                    {
                        if (listHaveBind.Any(o => o.HouseDeptId == Convert.ToInt32(n.Split('_')[0])))
                        {
                            isExists += 1;
                        }
                    }
                    else if (type == (int)EDeptType.CheKu)
                    {
                        if (listHaveBind.Any(o => o.ResourcesId == Convert.ToInt32(n.Split('_')[0])))
                        {
                            isExists += 1;
                        }
                    }
                    else
                    {
                        if (listHaveBind.Any(o => o.ResourcesId == Convert.ToInt32(n)))
                        {
                            isExists += 1;
                        }
                    }
                }
                /*设置全选状态*/
                if (m.Value.Count == isExists && isExists != 0)/*子节点和已经设置的子节点总数*/
                {
                    switch (type)
                    {
                        case (int)EDeptType.LouYu:
                            list.Single(o => o.id == m.Key.ToString()).state = new { selected = true, opened = false };
                            break;
                        case (int)EDeptType.CheKu:
                            list.Single(o => o.id.Split('_')[0] == m.Key.ToString()).state = new { selected = true, opened = false };
                            break;
                        case (int)MeterTypeEnum.WaterMeter:
                            list.Single(o => o.id == m.Key.ToString()).state = new { selected = true, opened = false };
                            break;
                        case (int)MeterTypeEnum.WattHourMeter:
                            list.Single(o => o.id == m.Key.ToString()).state = new { selected = true, opened = false };
                            break;
                        case (int)MeterTypeEnum.GasMeter:
                            list.Single(o => o.id == m.Key.ToString()).state = new { selected = true, opened = false };
                            break;
                    }
                }
                /*设置半选状态*/
                else if (m.Value.Count > isExists && isExists != 0)
                {
                    switch (type)
                    {
                        case (int)EDeptType.LouYu:
                            list.Single(o => o.id == m.Key.ToString()).state = new { undetermined = true, opened = false };
                            break;
                        case (int)EDeptType.CheKu:
                            list.Single(o => o.id.Split('_')[0] == m.Key.ToString()).state = new { undetermined = true, opened = false };
                            break;
                        case (int)MeterTypeEnum.WaterMeter:
                            list.Single(o => o.id == m.Key.ToString()).state = new { undetermined = true, opened = false };
                            break;
                        case (int)MeterTypeEnum.WattHourMeter:
                            list.Single(o => o.id == m.Key.ToString()).state = new { undetermined = true, opened = false };
                            break;
                        case (int)MeterTypeEnum.GasMeter:
                            list.Single(o => o.id == m.Key.ToString()).state = new { undetermined = true, opened = false };
                            break;
                    }
                }
                else
                {
                    list.Single(o => o.id == m.Key.ToString()).state = new { selected = false };
                }
            }

            return list;
        }
        #endregion


        #region 设置房屋资源时间(设置选中 )
        /// <summary>
        /// 设置时间
        /// </summary>
        /// <param name="list">资源列表集合（第二级树）</param>
        /// <param name="subjectId">科目</param>
        /// <returns></returns>
        public List<CustomTreeNodeModel> SetTime(List<CustomTreeNodeModel> list, int subjectId)
        {
            SubjectHouseRefAppService subjectHousRefAppService = new SubjectHouseRefAppService();
            List<SubjectHouseRefDTO> listHouses = new List<SubjectHouseRefDTO>();
            StringBuilder sb = new StringBuilder();
            List<int?> listIds = new List<int?>();
            /*设置时间*/
            foreach (var t in list)
            {
                listIds.Add(Convert.ToInt32(t.id.Split('_')[0]));
            }
            listHouses = subjectHousRefAppService.GetSubjectHouseRefList(subjectId, listIds);
            foreach (var n in list)
            {
                SubjectHouseRefDTO model = listHouses.FirstOrDefault(o => o.ResourcesId == Convert.ToUInt32(n.id.Split('_')[0]));
                if (model != null)
                {
                    n.state = new { selected = true };
                }
                if (model != null && model.DevBeginDate.HasValue)
                {
                    sb.Append("<span class='treeLabel' onclick='fnTreeLabel(event,this)'>" + n.text + "</span>");
                    sb.Append(string.Format("{0}_{1}", string.Format("{0:yyyy-MM-dd}", model.DevBeginDate), string.Format("{0:yyyy-MM-dd}", model.DevEndDate)));
                    n.text = sb.ToString();
                    sb.Clear();
                }
            }
            return list;
        }
        #endregion

        #region 加载三表（单一加载水、电、气,设置时间  ）

        private object GetPublicMeterState(int pmcount, int mcount)
        {
            if (pmcount == 0)
            {
                return new { selected = false, opened = false };
            }
            //全选
            if (pmcount == mcount)
            {
                return new { selected = true, opened = false };
            }
            //半选
            else
            {
                return new { undetermined = true, opened = false };
            }
        }


        /// <summary>
        /// 加载三表（单一加载水、电、气）
        /// </summary>
        /// <param name="id">资源ID</param>
        /// <param name="typeEle">类型为房屋</param>
        /// <param name="strFilter">过滤</param>
        /// <param name="threeType">三表类型（水、电、气）</param>
        /// <returns></returns>
        public List<CustomTreeNodeModel> GetListByResType(int id, string strFilter, int threeType, int subjectId, ref Dictionary<string, List<string>> dics, int? comDeptId)
        {
            DeptAppService deptService = new DeptAppService();
            MeterAppService meterAppservice = new MeterAppService();

            if (id == comDeptId)
            {
                List<CustomTreeNodeModel> dataList = deptService.GetDeptTree(CurrentAdminUser, id, strFilter, ref dics).Select(o => new CustomTreeNodeModel()
                {
                    id = o.id,
                    icon = o.icon,
                    text = o.text,
                    children = o.children
                }).ToList();
                var mList = meterAppservice.GetPublicMeterDTOS(comDeptId, (MeterTypeEnum)threeType);
                if (mList.Count() > 0)
                {
                    string strPublicMeter = string.Empty;
            
                    //获取公共表
                    switch (threeType)
                    {
                        case (int)MeterTypeEnum.WaterMeter://水
                            {
                                strPublicMeter = "公区水表";
                            }; break;
                        case (int)MeterTypeEnum.WattHourMeter://电
                            {
                                strPublicMeter = "公区电表";
                            }; break;
                        case (int)MeterTypeEnum.GasMeter://气
                            {
                                strPublicMeter = "公区气表";
                            }; break;
                        default:
                            break;
                    }
                    SubjectHouseRefDomainService subjecthouse = new SubjectHouseRefDomainService();
                    //选中的公区表列表
                    var pmList = subjecthouse.GetSelectPublicMeter(subjectId);
                  
                    CustomTreeNodeModel tree = new CustomTreeNodeModel()
                    {
                        state = GetPublicMeterState(pmList.Count(), mList.Count()),
                        id = subjectId.ToString(),
                        icon = "fa fa-dashboard",
                        text = strPublicMeter,
                        children = mList.Select(m => new CustomTreeNodeModel()
                        {
                            id = "0_1000_" + m.Id.ToString(),
                            icon = "fa fa-dashboard",
                            children = false,
                            text = pmList.Any(p => p.Id == m.Id) ? pmList.First(p => p.Id == m.Id).MeterText : m.MeterNum, //strMetertime.ToString(),
                            //SetMeterTime(pmList.Where(o =>o.Id==m.Id).FirstOrDefault().DevBeginDate.Value,m.MeterNum,pmList.Where(o => o.Id == m.Id).FirstOrDefault().DevEndDate.Value),
                            state = new { selected = pmList.Any(p => p.Id == m.Id), opened = false }

                        }).ToList()
                    };
                    dataList.Add(tree);
                }
                return dataList;
            }



            //string[] arr = new string[2];
            ///*获取房屋的ID信息*/
            //foreach (var item in list)
            //{
            //    arr = item.id.Split('_');
            //    if (Convert.ToInt32(arr[1]) == (int)EDeptType.FangWu)
            //    {
            //        if (!listIds.Contains(Convert.ToInt32(arr[0])))
            //        {
            //            listIds.Add(Convert.ToInt32(Convert.ToInt32(arr[0])));
            //        }
            //    }
            //} 
            List<MeterDTO> meterDTOList = meterAppservice.GetMeterDTOS(comDeptId, threeType);/*获取该栋楼下的三表*/
            List<CustomTreeNodeModel> list = deptService.GetDeptTree(CurrentAdminUser, id, strFilter, ref dics).Select(o => new CustomTreeNodeModel()
            {
                id = o.id,
                icon = o.icon,
                text = o.text,
                children = o.children
            }).Where(d => meterDTOList.Any(m => m.HouseDeptID.ToString() == d.id.Split('_')[0])).ToList();
            List<CustomTreeNodeModel> listHousHaveThreeMeter = new List<CustomTreeNodeModel>();
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            List<int?> listIds = new List<int?>();
            List<string> listStr = new List<string>();
            /*一个房屋可能会有多只表*/
            foreach (var item in meterDTOList)
            {
                listHousHaveThreeMeter.AddRange(list.Where(o => o.id.Split('_')[0] == item.HouseDeptID.ToString()));
                if (dic.ContainsKey(item.HouseDeptID.ToString()))
                {
                    dic[item.HouseDeptID.ToString()].Add(item.MeterNum + "_" + item.Id);/*表号和表ID*/

                }
                else
                {
                    dic.Add(item.HouseDeptID.ToString(), new List<string>() { item.MeterNum + "_" + item.Id });
                }
            }
            string[] meterInfo = new string[2];
            List<string> isExitsKey = new List<string>();
            //listIds.Clear();
            /*哪些房屋有表*/
            foreach (var item in listHousHaveThreeMeter.Distinct())
            {
                if (!isExitsKey.Contains(item.id.Split('_')[0]))
                {
                    isExitsKey.Add(item.id.Split('_')[0]);
                    list.Remove(item);/*移除原来的元素（原来只有一个元素、如果这个房屋下有两只表那么会新增两个元素、以此类推）*/
                    if (dic.Keys.Contains(item.id.Split('_')[0]))
                    {
                        foreach (string value in dic[item.id.Split('_')[0]])
                        {
                            meterInfo = value.Split('_');
                            list.Add(new CustomTreeNodeModel()
                            {
                                children = false,
                                id = item.id.Split('_')[0] + "_1000_" + meterInfo[1],
                                icon = item.icon,
                                text = meterInfo[0] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "(" + item.text + ")"
                            });
                            /*获取表的ID、方便后面获取表的绑定的时间*/
                            listIds.Add(Convert.ToInt32(meterInfo[1]));
                        }
                    }
                }
                else
                {
                    list.Add(new CustomTreeNodeModel()
                    {
                        children = false,
                        id = item.id,
                        icon = item.icon,
                        text = item.text,

                    });
                }
            }
            /*时间设置*/
            List<SubjectHouseRefDTO> listSubjectHouseRef = new SubjectHouseRefAppService().GetSubjectHouseRefList(subjectId, listIds);
            StringBuilder sb = new StringBuilder();
            foreach (var n in list)
            {
                if (n.id.Split('_').Length == 3)
                {
                    SubjectHouseRefDTO subjectHouseHref = listSubjectHouseRef.FirstOrDefault(o => o.ResourcesId == Convert.ToUInt32(n.id.Split('_')[2]));
                    if (subjectHouseHref != null)
                    {
                        n.state = new { selected = true };
                    }
                    if (subjectHouseHref != null && subjectHouseHref.DevBeginDate.HasValue)
                    {
                        sb.Append("<span class='treeLabel' onclick='fnTreeLabel(event,this)'>" + n.text + "</span>");
                        sb.Append(string.Format("{0}_{1}", string.Format("{0:yyyy-MM-dd}", subjectHouseHref.DevBeginDate), string.Format("{0:yyyy-MM-dd}", subjectHouseHref.DevEndDate)));
                        n.text = sb.ToString();
                        sb.Clear();
                    }
                }
            }
            //dics = dic;
            list.OrderBy(o => o.id);
            return list;
        }
        #endregion

        #region 获取车位(设置时间)
        /// <summary>
        /// 获取车位
        /// </summary>
        /// <param name="parkIngId">车库ID</param>
        /// <returns></returns>
        public ActionResult GetCarParkSpace(string parkIngId, int subjectId, string carState, string carType)
        {
            var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
            List<CustomTreeNodeModel> listParkSpace = new List<CustomTreeNodeModel>();
            List<CustomTreeNodeModel> list;
            if (string.IsNullOrEmpty(carState) || carState == "null")
            {
                carState = ((int)ParkingSpaceStatusEnum.Bought).ToString();
            }
            string carStateAndcarType = string.Format("{0}|{1}", carState, carType);
            if (!string.IsNullOrEmpty(carStateAndcarType))
            {

                list = propertyService.GetParkingSpaceCarportStateAndType(parkIngId, carStateAndcarType);
            }
            else
            {
                list = propertyService.GetAsynParkingSpaceTree(parkIngId);
            }
            CustomTreeNodeModel model;
            List<int?> listIds = new List<int?>();
            foreach (var item in list)
            {
                listIds.Add(Convert.ToInt32(item.id.Split('_')[0]));
                var deptInfo = propertyService.GetSecDeptInfo(Convert.ToInt32(item.id.Split('_')[2]));
                model = new CustomTreeNodeModel();
                model.id = item.id;
                model.icon = item.icon;
                model.children = item.children;
                if (deptInfo != null)
                {
                    model.text = item.text + " (" + deptInfo.Name + ") ";
                }
                else
                {
                    model.text = item.text;
                    //     model.state = new { disabled = false };//房间等于空
                }
                listParkSpace.Add(model);
            }
            List<SubjectHouseRefDTO> listSubjectHouseRef = new SubjectHouseRefAppService().GetSubjectHouseRefList(subjectId, listIds);
            StringBuilder sb = new StringBuilder();
            foreach (var n in listParkSpace)
            {
                SubjectHouseRefDTO subjectHouseHref = listSubjectHouseRef.FirstOrDefault(o => o.ResourcesId == Convert.ToUInt32(n.id.Split('_')[0]));

                if (subjectHouseHref != null)
                {
                    n.state = new { selected = true };
                }
                if (subjectHouseHref != null && subjectHouseHref.DevBeginDate.HasValue)
                {
                    sb.Append("<span class='treeLabel' onclick='fnTreeLabel(event,this)'>" + n.text + "</span>");
                    sb.Append(string.Format("{0}_{1}", string.Format("{0:yyyy-MM-dd}", subjectHouseHref.DevBeginDate), string.Format("{0:yyyy-MM-dd}", subjectHouseHref.DevEndDate)));
                    n.text = sb.ToString();
                    sb.Clear();
                }
            }

            var jResult = new JsonResult();
            DeptAppService deptService = new DeptAppService();
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jResult.Data = listParkSpace;
            return jResult;
        }

        #endregion

        #region 获取车位状态、车位类别
        public ActionResult GetCarStateDictionary()
        {
            List<DictionaryModel> list = PresentationServiceHelper.LookUp<IPropertyService>().GetDictionaryModels("CarportState");
            var jResult = new JsonResult();
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jResult.Data = list;
            return jResult;
        }

        public ActionResult GeCarortTypeDictionary()
        {
            List<DictionaryModel> list = PresentationServiceHelper.LookUp<IPropertyService>().GetDictionaryModels("CarportType");
            var jResult = new JsonResult();
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jResult.Data = list;
            return jResult;
        }
        #endregion

        #region 获取房屋状态、房屋装修状态
        public ActionResult GetHomeStateDictionary()
        {
            List<DictionaryModel> list = PresentationServiceHelper.LookUp<IPropertyService>().GetDictionaryModels("HouseState").Where(o => o.IsUsed == true).ToList();
            var jResult = new JsonResult();
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jResult.Data = list;
            return jResult;
        }

        public ActionResult GeHouseDecorationStateDictionary()
        {
            List<DictionaryModel> list = PresentationServiceHelper.LookUp<IPropertyService>().GetDictionaryModels("HouseDecorationState").Where(o => o.IsUsed == true).ToList();
            var jResult = new JsonResult();
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jResult.Data = list;
            return jResult;
        }
        #endregion

        #region 获取对象绑定科目资源对应的时间信息(单个用于选中时时间呈现)
        /// <summary>
        /// 获取对象绑定科目资源对应的时间信息
        /// </summary>
        /// <param name="resId">资源ID</param>
        /// <param name="subjectId">科目ID</param>
        /// <returns></returns>
        public ActionResult GetBindSubjectTime(int resId, int subjectId)
        {
            SubjectHouseRefAppService subjectHouseRefService = new SubjectHouseRefAppService();
            ChargeSubjectDTO subject = new ChargeSubjectAppService().GetChargeSubjectByKey(subjectId);
            SubjectHouseRefDTO model = subjectHouseRefService.GetSubjectHouseRefByResId(resId, subjectId);
            var jResult = new JsonResult();
            if (subject.BillPeriod == (int)BillPeriodEnum.DailyCharge || subject.BillPeriod == (int)BillPeriodEnum.Once)
            {
                if (model != null)
                {
                    jResult.Data = new
                    {
                        IsDevPay = model.IsDevPay,
                        DevBeginDate = model != null ? (string.Format("{0:yyyy-MM-dd}", model.DevBeginDate)) : "",
                        DevEndDate = model != null ? (string.Format("{0:yyyy-MM-dd}", model.DevEndDate)) : ""
                    };
                }
            }
            else
            {
                if (model != null)
                {
                    jResult.Data = new
                    {
                        IsDevPay = model.IsDevPay,
                        DevBeginDate = model != null ? (!string.IsNullOrEmpty(model.DevBeginDate.ToString()) ? string.Format("{0:yyyy-MM}", model.DevBeginDate) : "") : "",
                        DevEndDate = model != null ? (!string.IsNullOrEmpty(model.DevEndDate.ToString()) ? string.Format("{0:yyyy-MM}", model.DevEndDate) : "") : ""
                    };
                }
            }

            DeptAppService deptService = new DeptAppService();

            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jResult;
        }

        #endregion

        public class DevTime
        {
            public int? ResId { get; set; }
            public int? HouseId { get; set; }
            public string SetTime { get; set; }
        }

        public class BindSubjectHouseRefBuild
        {
            public bool IsBind { get; set; }
            public string ResId { get; set; }/*带有类型*/
            public int State { get; set; }/*全选1,半选2,未选3-*/
        }

    }
}











