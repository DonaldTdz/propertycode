using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using YK.BackgroundMgr.DomainInterface;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.CompositeAppService;

namespace YK.PropertyMgr.MVCWeb.Controllers.WebApi
{
    /// <summary>
    /// 自助终端缴费接口 v2.8
    /// </summary>
    public class PropertyClientController : ApiController
    {
        #region 获取楼栋

        public JsonResult<APIResultDTO> GetBuildListByComDeptId(int? ComDeptId, int? PageSize)
        {
            ComDeptId = ComDeptId ?? 0;
            PageSize = PageSize ?? 25;
            var buildList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetBuildingByComDeptId(ComDeptId);
            int i = 1;
            //楼栋列表
            List<ClientDeptInfo> bList = buildList.Select(b => new ClientDeptInfo() { Index = i++, Id = b.Id, Name = b.Name, PId = ComDeptId, DeptType = ClientDeptType.Build }).ToList();
            //如果大于单页显示 25条 则每页显示20条 需要显示导航栏
            if (bList.Count() > PageSize)
            {
                PageSize = 20;
            }
            //导航列表
            List<PageNavigation> pageNavigationList = GetPageNavigationList(bList.Count(), PageSize, "~", "栋", ClientDeptType.Build);
            APIResultDTO result = new APIResultDTO();
            result.Code = 0;
            result.Message = "获取数据成功";
            result.Data = new { NavigationList = pageNavigationList, Items = bList };
            return Json(result);
        }

        #endregion

        #region 获取单元

        public JsonResult<APIResultDTO> GetUnitListByBuildId(int? BuildId, int? PageSize)
        {
            BuildId = BuildId ?? 0;
            PageSize = PageSize ?? 25;
            var houseList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouseDeptInfobyLouyuDeptId(BuildId.Value);
            //单元列表
            List<ClientDeptInfo> bList = GetUnitListByHouseNo(houseList, BuildId);
            //如果大于单页显示 25条 则每页显示20条 需要显示导航栏
            if (bList.Count() > PageSize)
            {
                PageSize = 20;
            }
            //导航列表
            List<PageNavigation> pageNavigationList = GetPageNavigationList(bList.Count(), PageSize, "~", "单元", ClientDeptType.Unit);
            APIResultDTO result = new APIResultDTO();
            result.Code = 0;
            result.Message = "获取数据成功";
            result.Data = new { NavigationList = pageNavigationList, Items = bList };
            return Json(result);
        }

        private List<ClientDeptInfo> GetUnitListByHouseNo(IList<DeptInfo> houseList, int? buildId)
        {
            List<ClientDeptInfo> uList = new List<ClientDeptInfo>();
            List<HouseNoHierarchy> hList = new List<HouseNoHierarchy>();
            foreach (var item in houseList)
            {
                var harry = item.Name.Split('-');
                HouseNoHierarchy hn = new HouseNoHierarchy();
                hn.Id = item.Id;
                hn.HouseDoorNo = item.Name;
                hn.BuildCode = harry[0];
                hn.UnitCode = harry[1];
                hn.FloorCode = harry[2];
                hn.HouseCode = harry[3];
                hList.Add(hn);
            }

            //获取单元
            var unitList = hList.Select(h => new { h.BuildCode, h.UnitCode}).Distinct().ToList();
            foreach (var item in unitList)
            {
                ClientDeptInfo uinfo = new ClientDeptInfo();
                uinfo.PId = buildId;
                uinfo.Name = item.UnitCode + "单元";
                uinfo.Code = item.BuildCode + "-" + item.UnitCode + "-";
                uinfo.DeptType = ClientDeptType.Unit;
                //获取楼层和房屋
                var fList = hList.Where(h => h.BuildCode == item.BuildCode && h.UnitCode == item.UnitCode)
                    .Select(h => new { h.BuildCode, h.UnitCode, h.FloorCode }).Distinct().ToList();
                //楼层导航 楼层默认显示5层
                List<PageNavigation> pageNavigationList = GetPageNavigationList(fList.Count(), 5, "~", "层", ClientDeptType.Floor);
                //先得到单元下的所有房屋
                var uhList = hList.Where(h => h.UnitCode == item.UnitCode && h.BuildCode == item.BuildCode).ToList();
                //按楼层分组
                List<List<ClientDeptInfo>> ghList = new List<List<ClientDeptInfo>>();
                foreach (var pitem in pageNavigationList)
                {
                    List<ClientDeptInfo> iList = new List<ClientDeptInfo>();
                    for (int i = pitem.StartIndex.Value; i <= pitem.EndIndex.Value; i++)
                    {
                        var ihList = uhList.Where(u => u.FloorCode == fList[i - 1].FloorCode)
                            .Select(u => new ClientDeptInfo() { Id = u.Id, PId = buildId , Name = u.HouseDoorNo, Index = i, DeptType = ClientDeptType.House}).ToList();
                        iList.AddRange(ihList);
                    }
                    ghList.Add(iList);
                }
                //单元数据子项  楼层导航，显示房屋
                uinfo.Children = new { NavigationList = pageNavigationList, Items = ghList };

                uList.Add(uinfo);
            }

            return uList;
        }

        #endregion

        #region 获取导航

        private List<PageNavigation> GetPageNavigationList(int? total, int? pageSize, string separator, string unit, ClientDeptType type)
        {
            List<PageNavigation> pnList = new List<PageNavigation>();
            if (pageSize <= 0)
            {
                return pnList;
            }
            
            int? pageCount = total / pageSize;
            for (int i = 1; i <= pageCount; i++)
            {
                PageNavigation navigation = new PageNavigation();
                navigation.StartIndex = pageSize * (i - 1) + 1;
                navigation.EndIndex = pageSize * i;
                navigation.PageIndex = i;
                navigation.DeptType = type;
                navigation.Title = string.Format("{0}{1}{2}{3}", navigation.StartIndex, separator, navigation.EndIndex, unit);
                pnList.Add(navigation);
            }
            //最后一页
            var pt = pageSize * pageCount;
            if (pt < total)
            {
                PageNavigation navigation = new PageNavigation();
                navigation.StartIndex = pt + 1;
                navigation.EndIndex = total;
                navigation.PageIndex = pageCount + 1;
                navigation.DeptType = type;
                navigation.Title = string.Format("{0}{1}{2}{3}", navigation.StartIndex, separator, navigation.EndIndex, unit);
                pnList.Add(navigation);
            }
            return pnList;
        }

        #endregion

        #region 获取用户信息

        public JsonResult<APIResultDTO> GetUserInfoByHouseDeptId(int? HouseDeptId)
        {
            HouseDeptId = HouseDeptId ?? 0;
            var userInfo = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetUserOwnerMasterByHouseDeptId(HouseDeptId.Value);
            if (userInfo == null)
            {
                return Json(new APIResultDTO() { Code = 701, Message = "该房屋没有录入业主信息，请联系物业管理员" });
            }
            ClientUserInfo cInfo = new ClientUserInfo();
            cInfo.UserId = userInfo.Id.ToString();
            cInfo.UserName = userInfo.UserName;
            cInfo.PhoneNumber = userInfo.BindingPhonerNumber;
            APIResultDTO result = new APIResultDTO();
            result.Code = 0;
            result.Message = "获取数据成功";
            result.Data = cInfo;
            return Json(result);
        }

        public JsonResult<APIResultDTO> GetUserInfoByPhoneNum(int? ComDeptId, string PhoneNum)
        {
            PhoneNum = PhoneNum ?? string.Empty;
            ComDeptId = ComDeptId ?? 0;
            var uInfo = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetUserOwnerByPhoneNum(ComDeptId.Value, PhoneNum);
            if (uInfo == null)
            {
                return Json(new APIResultDTO() { Code = 701, Message = "该小区不存在此手机号的用户，请联系物业"});
            }
            ClientUserInfo cInfo = new ClientUserInfo();
            cInfo.UserId = uInfo.UserId;
            cInfo.UserName = uInfo.UserName;
            cInfo.PhoneNumber = uInfo.BindingPhonerNumber;
            cInfo.HouseData = uInfo.HouseList.Select(h => new ClientDeptInfo() { Id = h.HouseDeptID, Name =  h.DoorNo , DeptType = ClientDeptType.House}).ToList();
            APIResultDTO result = new APIResultDTO();
            result.Code = 0;
            result.Message = "获取数据成功";
            result.Data = cInfo;
            return Json(result);
        }

        #endregion

        #region 账单缴费

        private List<ClientBillGroup> ConvertBill(List<ChargBillDTO> billList)
        {
            List<ClientBillGroup> bgList = new List<ClientBillGroup>();
            if (billList.Count() == 0)
            {
                return bgList;
            }
            var group = billList.GroupBy(b => b.Description).Select(b => b.Key).ToList();
            foreach (var item in group)
            {
                ClientBillGroup ig = new ClientBillGroup();
                ig.Name = item;
                ig.BillData = billList.Where(b => b.Description == item).Select(b => new ClientBillInfo() {
                    BillId = b.Id,
                    BeginDate = b.BeginDate,
                    EndDate = b.EndDate,
                    Amount = Math.Round(b.BillAmount.Value + b.PenaltyAmount.Value - b.ReceivedAmount.Value - b.ReliefAmount.Value, 2)
                }).ToList();
                ig.Count = ig.BillData.Count();
                ig.TotalAmount = ig.BillData.Sum(b => b.Amount);
                bgList.Add(ig);
            }
            return bgList;
        }

        public JsonResult<APIResultDTO> GetBillListByHouseDeptId(int? HouseDeptId)
        {
            HouseDeptId = HouseDeptId ?? 0;
            ChargBillAppService service = new ChargBillAppService();
            var billList = service.GetChargBillListByHouseDeptId(HouseDeptId.Value).ToList();
            APIResultDTO result = new APIResultDTO();
            result.Code = 0;
            result.Message = "获取数据成功";
            result.Data = ConvertBill(billList);
            return Json(result);
        }

        #endregion

        #region 预存费

        public JsonResult<APIResultDTO> GetSubjectListByHouseDeptId(int? HouseDeptId)
        {
            HouseDeptId = HouseDeptId ?? 0;
            var data = PaymentAppService.CalculationMonthPrePaymentList(HouseDeptId.Value, false);
            APIResultDTO result = new APIResultDTO();
            result.Code = 0;
            result.Message = "获取数据成功";
            result.Data = data.OrderBy(d => d.BillPeriod).Select(d => new ClientSubject()
            {
                SubjectId = d.SubjectId,
                MonthAmount = d.PreAmount,
                SubjectName = d.SubjectName
            }).ToList();
            return Json(result);
        }

        #endregion

        #region 支付缴费

        /// <summary>
        /// 获取缴费二维码信息
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult<APIResultDTO> QRCodePost([FromBody]JObject jorder)
        {
            ClientPayOrder order = jorder.ToObject<ClientPayOrder>();
            var qrcode = PaymentAppService.GetPayQRCodeUrl(order);
            APIResultDTO dto = new APIResultDTO();
            dto.Code = 0;
            dto.Message = "获取数据成功";
            //NumericalNumber：刷新序列号 AlipayUrl：支付宝URL WeChatUrl：微信URL
            dto.Data = qrcode;
            return Json(dto);
        }

        /// <summary>
        /// 客户端缴费验证
        /// </summary>
        [HttpPost]
        public JsonResult<APIResultDTO> CheckPaymentPost([FromBody]JObject jorder)
        {
            ClientPayOrder order = jorder.ToObject<ClientPayOrder>();
            APIResultDTO dto = PaymentAppService.CheckClientPaymentPost(order);
            return Json(dto);
        }

        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="CallType">1：异常 2：异步 3：同步</param>
        /// <param name="NumericalNumber">支付序列号</param>
        /// <param name="state"> 支付状态
        /// 待支付 = 0,支付中 = 1,支付成功 = 2,支付失败 = 3,冻结中 = 4,取消 = 5
        /// </param>
        /// <param name="PayType">支付方式
        /// 支付宝 = 0,贵金支付 = 1,微信 = 2,一网通 = 3,钱包 = 99
        /// </param>
        /// <returns></returns>
        public JsonResult<APIResultDTO> PayCallBack(int? CallType, string NumericalNumber, int? state, int? PayType)
        {
            var result = PaymentAppService.ClientPayCallBack(CallType, NumericalNumber, state, PayType);
            return Json(result);
        }

        public JsonResult<APIResultDTO> GetNumericalState(string NumericalNumber)
        {
            var state = PaymentAppService.GetNumericalState(NumericalNumber);
            APIResultDTO result = new APIResultDTO();
            result.Code = 0;
            result.Message = "获取状态成功";
            result.Data = state;
            return Json(result);
        }

        [HttpPost]
        public JsonResult<APIResultDTO> CancelPayPost(string NumericalNumber)
        {
            var result = PaymentAppService.ClientLeavePay(NumericalNumber);
            return Json(result);
        }

        #endregion
    }
}