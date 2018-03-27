using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.ApplicationService.Service;
using YK.PropertyMgr.Crosscuting;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class EntrancePowerController : BaseController
    {
        public ActionResult EntrancePowerList()
        {
            EntranceUserAppService entranceUserAppService = new EntranceUserAppService();
            EntrancePowerListData entranceListDataListData = new EntrancePowerListData();
            entranceListDataListData.Language = Language;
            entranceListDataListData.TemplateModels = entranceUserAppService.GetEntrancePowerViewTemplate();
            return View(entranceListDataListData);
        }

        #region 获取用户数据
        public ActionResult GetEntrancePowers(UserSearchDTO search)
        {
            EntranceUserAppService entranceAppService = new EntranceUserAppService();
            List<YK.Framework.ApplicationDTO.SQUserOwnerInfo> dataList = new List<Framework.ApplicationDTO.SQUserOwnerInfo>();
            if (search.DeptType == EDeptType.XiaoQu)
            {
                dataList = entranceAppService.GetSQUserOwnerInfoByCommunityDeptId(search.DeptId);
                if (dataList != null)
                {
                    dataList = dataList.Where(o => o.PersonState != 2).ToList();/*不为商城会员*/
                }
                else
                {
                    dataList = new List<Framework.ApplicationDTO.SQUserOwnerInfo>();
                }
            }
            else
            {
                if (search.DeptType == EDeptType.LouYu)
                {
                    dataList = entranceAppService.GetUserOwnerInfoByBuildingDeptId(search.DeptId);
                    if (dataList != null)
                    {
                        dataList = dataList.Where(o => o.PersonState != 2).ToList();/*不为商城会员*/
                    }
                    else
                    {
                        dataList = new List<Framework.ApplicationDTO.SQUserOwnerInfo>();
                    }
                }
            }
            if (!string.IsNullOrEmpty(search.Name))
            {
                dataList = dataList.Where(o => !string.IsNullOrEmpty(o.Name) && o.Name.Contains(search.Name)).ToList();
                if (dataList == null)
                {
                    dataList = new List<Framework.ApplicationDTO.SQUserOwnerInfo>();
                }
            }
            if (!string.IsNullOrEmpty(search.Phone))
            {
                dataList = dataList.Where(o => !string.IsNullOrEmpty(o.Telephone) && o.Telephone.Contains(search.Phone)).ToList();
                if (dataList == null)
                {
                    dataList = new List<Framework.ApplicationDTO.SQUserOwnerInfo>();
                }
            }
            if (!string.IsNullOrEmpty(search.DoorNum))
            {
                dataList = dataList.Where(o => !string.IsNullOrEmpty(o.AllRoomNo) &&o.AllRoomNo.Contains(search.DoorNum)).ToList();
                if (dataList == null)
                {
                    dataList = new List<Framework.ApplicationDTO.SQUserOwnerInfo>();
                }
            }
            SearchResultData<YK.Framework.ApplicationDTO.SQUserOwnerInfo> queryResult = new SearchResultData<YK.Framework.ApplicationDTO.SQUserOwnerInfo>()
            {
                draw = search.Draw,
                recordsFiltered = dataList == null ? 0 : dataList.Count,
                recordsTotal = dataList == null ? 0 : dataList.Count,
                data = dataList.Skip((search.PageIndex - 1) * search.PageSize).Take(search.PageSize).ToList()
            };
            return Json(queryResult);
        }
        #endregion

        #region 授权用户信息
        /// <summary>
        /// 授权用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult EntrancePowerViewAdd()
        {
            EntrancePowerViewData entrancePowerData = new EntrancePowerViewData();
            return CreateEntrancePowerView("Add", entrancePowerData);
        }
        public ActionResult EntrancePowerViewEdit(string userIds)
        {
            EntrancePowerViewData entrancePowerData = new EntrancePowerViewData();
            entrancePowerData.UserIds = userIds;
            return CreateEntrancePowerView("AddPowperPersonal", entrancePowerData);
        }
        private ActionResult CreateEntrancePowerView(string viewType, EntrancePowerViewData entrancePowerViewData)
        {
            EntranceAppService entranceAppService = new EntranceAppService();
            EntrancePowerViewListData entrancePowerViewListData = new EntrancePowerViewListData();
            entrancePowerViewListData.Language = Language;
            entrancePowerViewListData.EntrancePowerViewData = entrancePowerViewData;
            entrancePowerViewListData.TemplateModels = viewType == "Add" ? entranceAppService.GetEntrancePowerViewTemplate() : entranceAppService.GetEntrancePowerPersonalViewTemplate();
            return View("EntrancePowerView", entrancePowerViewListData);
        }
        #endregion

        #region 批量授权处理
        /// <summary>
        /// 批量授权处理
        /// </summary>
        /// <param name="entrancePowerData">批量信息</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(EntrancePowerViewData entrancePowerData)
        {
            EntranceUserAppService entranceUserService = new EntranceUserAppService();
            string[] entrancesIds = entrancePowerData.EntrancesIds.TrimEnd(',').Split(',');
            string[] userIds = entrancePowerData.UserIds.TrimEnd(',').Split(',');
            var jResult = new JsonResult();
            var ResultModelObj= entranceUserService.BatchUserRightPower(entrancesIds, userIds, Convert.ToDateTime(entrancePowerData.KeyExpressTime),entrancePowerData.EntranceSendMsgList);
            jResult.Data = ResultModelObj;
            if (ResultModelObj.IsSuccess)
            {
                    LogProperty.WirteFrameworkLog(CurrentAdminUser.Id.ToString(), CurrentAdminUser.RealName, "EntrancePowerController", "Save", "用户ID:"+ entrancePowerData.UserIds,"选择大门Id"+ entrancePowerData.EntrancesIds, "到期时间"+ entrancePowerData.KeyExpressTime);
            }
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            
                return jResult;
        }
        #endregion

        #region 获取设备
        /// <summary>
        /// 获取设备
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult GetEntrances(EntranceSearchDTO search)
        {
            try
            {
                search.State = 1;
                int outCount = 0;
                EntranceAppService entranceAppService = new EntranceAppService();
                if (!string.IsNullOrEmpty(search.DeptId.ToString()))
                {
                    var secDept = PresentationServiceHelper.LookUp<IPropertyService>().GetSecDeptInfo(Convert.ToInt32(search.DeptId));
                    if (secDept.DeptType == (int)EDeptType.LouYu)
                    {
                        search.DeptId = Convert.ToInt32(secDept.Code.Split('.')[2]);
                    }
                }
                if (string.IsNullOrEmpty(search.Guid))
                {/*小区下的所有设备*/


                    IList<EntranceViewDTO> dataList = entranceAppService.GetEntranceDTOList(search, out outCount);
                    SearchResultData<EntranceViewDTO> queryResult = new SearchResultData<EntranceViewDTO>()
                    {
                        draw = search.Draw,
                        recordsFiltered = outCount,
                        recordsTotal = outCount,
                        data = dataList
                    };
                    return Json(queryResult);
                }
                else
                {/*针对个人的所有授权设备*/

                    IList<EntrancePersonal> dataList = entranceAppService.GetEntrancePersonal(search);
                    SearchResultData<EntranceViewDTO> queryResult = new SearchResultData<EntranceViewDTO>()
                    {
                        draw = search.Draw,
                        recordsFiltered = dataList.Count(),
                        recordsTotal = dataList.Count(),
                        data = dataList.Select(o => new EntranceViewDTO()
                        {
                            Id = o.Id,
                            KeyID = o.KeyID,
                            Name = o.Name,
                            KeyExpireTime = o.KeyExpireTime,
                            DeviceType = o.DeviceType
                        })
                    };
                    return Json(queryResult);
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

        #region 获取用户授权的设备信息（包括自动授权和手动授权）
        /// <summary>
        /// 获取用户授权的设备信息（包括自动授权和手动授权）
        /// </summary>
        /// <returns></returns>
        //public ActionResult EntrancePowerPersonal(EntranceSearchDTO search)
        //{
        //    try
        //    {
        //        search.State = 1;
        //        int outCount = 0;
        //        EntranceAppService entranceAppService = new EntranceAppService();
        //        IList<EntranceViewDTO> dataList = entranceAppService.GetEntranceDTOList(search, out outCount);
        //        SearchResultData<EntranceViewDTO> queryResult = new SearchResultData<EntranceViewDTO>()
        //        {
        //            draw = search.Draw,
        //            recordsFiltered = outCount,
        //            recordsTotal = outCount,
        //            data = dataList
        //        };
        //        return Json(queryResult);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //}

        #endregion

        private IEnumerable<TemplateModel> GetTemplateModels()
        {
            return PresentationServiceHelper.LookUp<ITemplateService>().GetTemplateModels("PropertyMgrTemplate.xml", "Entrance", true);
        }
        public class EntrancePowerViewData
        {
            public string UserIds { get; set; }
            public string EntrancesIds { get; set; }
            public string KeyExpressTime { get; set; }
            //public int HouseDeptId { get; set; }
            //public string DoorNo { get; set; }
            //public string Phone { get; set; }

            public IList<EntranceSendMsgModel> EntranceSendMsgList { get; set; }
        }

       

        public class EntrancePowerViewListData
        {
            public EntrancePowerViewData EntrancePowerViewData { get; set; }
            public string Language { get; set; }
            public IEnumerable<TemplateModel> TemplateModels { get; set; }
        }

        public class EntrancePowerListData
        {
            public string Language { get; set; }
            public IEnumerable<TemplateModel> TemplateModels { get; set; }
        }
    }
}