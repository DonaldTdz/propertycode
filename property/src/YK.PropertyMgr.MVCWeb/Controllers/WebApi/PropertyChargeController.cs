using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.CompositeAppService;
using YK.PropertyMgr.Crosscuting;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    /// <summary>
    /// 物业APP收费 API
    /// </summary>
    public class PropertyChargeController : ApiController
    {

        public delegate int SettleAccountHandler(ResultModel recordId);

        #region 示例

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
            var a = value;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        #endregion

        #region APP、微信缴费接口
        /// <summary>
        /// 获取房屋列表
        /// </summary>
        /// <param name="communityDeptId">房屋所在小区DeptId</param>
        /// <param name="doorNo">房屋编号</param>
        /// <returns>房屋视图列表</returns>
        public JsonResult<List<AppHouseView>> GetHouseListByDoorNo(int communityDeptId, string doorNo)
        {
            var result = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouseListByDoorNo(communityDeptId, doorNo);
            return Json(result);
        }

        /// <summary>
        /// 获取账单列表
        /// </summary>
        /// <param name="houseDeptId">房屋deptId</param>
        /// <param name="beginDate">开始时间 格式为Date</param>
        /// <param name="endDate">结束时间 格式为Date</param>
        /// <returns>账单列表</returns>
        public JsonResult<IEnumerable<AppChargBillView>> GetChargBillListByHouseDeptId(int houseDeptId, DateTime beginDate, DateTime endDate)
        {
            //test
            //IList<AppChargBillView> viewList = new List<AppChargBillView>();
            //viewList.Add(new AppChargBillView() { Id = Guid.NewGuid().ToString(), Amount = 50, Description = "项目1", BeginDate = "1016/9/1", EndDate = "1016/9/30" });
            //viewList.Add(new AppChargBillView() { Id = Guid.NewGuid().ToString(), Amount = 60.7m, Description = "项目2", BeginDate = "1016/9/31", EndDate = "1016/9/30" });
            //return Json(viewList);
            ChargBillAppService service = new ChargBillAppService();
            return Json(service.GetChargBillListByHouseDeptId(houseDeptId, beginDate, endDate));
        }

        /// <summary>
        /// 缴费付款
        /// 旧接口PayChargePlanMoney
        /// </summary>
        /// <param name="jobject">json 参数,billIds：账单ids</param>
        /// <returns>JsonResult<ResultModel></returns>
        public JsonResult<ResultModel> PaymentPost([FromBody]JObject jparameter)
        {
            PaymentParameter parameter = jparameter.ToObject<PaymentParameter>();
            LogProperty.WriteLoginToFile(string.Format("参数：{0}", JsonHelper.JsonSerializerByNewtonsoft(parameter)), "PaymentPost", FileLogType.Info);
            //JObject 方式
            //dynamic json = jobject;//动态变量
            //JObject billIds = json.billIds;
            //JObject amount = json.amount;

            if (parameter.BillIds == null || parameter.BillIds.Length < 1)
            {
                return Json(new ResultModel() { IsSuccess = false, ErrorCode = "610", Msg = "请选择要付款的账单" });
            }
            //if (parameter.Amount <= 0)
            //{
            //    return Json(new ResultModel() { IsSuccess = false, ErrorCode = "620", Msg = "支付金额必须大于零" });
            //}

            //var json = JsonHelper.JsonSerializerByNewtonsoft(new ResultModel() { IsSuccess = false, ErrorCode = "620", Msg = "支付金额必须大于零" });
            //return new HttpResponseMessage { Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json") };
            ResultModel result = PaymentAppService.BillsAppPayment(parameter.BillIds, -1, parameter.PayType, -1, "逸社区APP", parameter.CustomerName, parameter.DiscountInfo);
            return Json(result);
        }

        public JsonResult<ResultModel> AppPaymentPostCheck([FromBody]JObject jparameter)
        {
            PaymentParameter parameter = jparameter.ToObject<PaymentParameter>();
            //验证不需要验证支付方式
            if (parameter.PayType == 0)
            {
                parameter.PayType = PayTypeEnum.Alipay;
            }
            ResultModel result = PaymentAppService.AppBillsPaymentCheck(parameter.BillIds, -1, parameter.PayType, parameter.DiscountInfo);
            return Json(result);
        }

        #region 原版社区访问接口

        #region 获取小区缴费项目

        /// <summary>
        /// 获取小区缴费项目
        /// 旧接口名GetChatChargeSubjectId
        /// </summary>
        public JsonResult<ResultModel> GetChargeSubjectByComDeptId(int ComDeptId)
        {
            ResultModel result = new ResultModel();
            try
            {
                ChargeSubjectAppService service = new ChargeSubjectAppService();
                result.Data = service.GetMobileChargeSubjectsByComDeptId(ComDeptId).Select(c => new ChargeSubjectView() { Subject_Id = c.Id.Value, Subject_Name = c.Name });
                result.IsSuccess = true;
                result.Msg = "获取数据成功";
                return Json(result);
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile("GetChargeSubjectByComDeptId 异常：" + ex.Message, "PropertyChargeAPI", FileLogType.Exception);
                result.IsSuccess = false;
                result.Msg = "获取数据失败";
                return Json(result);
            }

        }

        #endregion

        #region 获取账单列表

        /// <summary>
        /// 根据账单Id集合获取账单列表
        /// 旧接口名GetIdToChargePlan
        /// </summary>
        public JsonResult<ResultModel> GetBillListByIds(string BillIds)
        {
            ResultModel result = new ResultModel();
            try
            {
                string[] bills = BillIds.Split(',');
                ChargBillAppService service = new ChargBillAppService();
                result.Data = service.GetBillListByIds(bills);
                result.IsSuccess = true;
                result.Msg = "获取数据成功";
                return Json(result);
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile("GetChargeSubjectByComDeptId 异常：" + ex.Message, "PropertyChargeAPI", FileLogType.Exception);
                result.IsSuccess = false;
                result.Msg = "获取数据失败";
                return Json(result);
            }
        }

        /// <summary>
        /// 获取账单列表
        /// GetRoomToChargePlan
        /// </summary>
        /// <param name="houseDeptId">房屋deptId</param>
        /// <param name="beginDate">开始时间 格式为Date</param>
        /// <param name="endDate">结束时间 格式为Date</param>
        /// <returns>账单列表</returns>
        public JsonResult<ResultModel> GetChargBillListByHouseDeptId(int houseDeptId, BillStatusEnum state, int subjectId, DateTime? beginDate, DateTime? endDate)
        {
            ResultModel result = new ResultModel();
            try
            {
                endDate = null;//查询所有
                ChargBillAppService service = new ChargBillAppService();
                result.Data = service.GetChargBillListByHouseDeptId(houseDeptId, state, subjectId, beginDate, endDate);
                result.IsSuccess = true;
                result.Msg = "获取数据成功";
                return Json(result);
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile("GetChargeSubjectByComDeptId 异常：" + ex.Message, "PropertyChargeAPI", FileLogType.Exception);
                result.IsSuccess = false;
                result.Msg = "获取数据失败";
                return Json(result);
            }
        }

        #endregion

        #endregion

        #endregion

        #region 预存费接口 20170606

        /// <summary>
        /// 获取预存费列表
        /// </summary>
        /// <param name="houseDeptId"></param>
        /// <returns></returns>
        public JsonResult<APIResultDTO> GetSubjectPreCostList(int houseDeptId)
        {
            try
            {
                var data = PaymentAppService.CalculationMonthPrePaymentList(houseDeptId, false);
                APIResultDTO result = new APIResultDTO();
                result.Code = 0;
                result.Message = "获取数据成功";
                result.Data = data;
                return Json(result);
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("houseDeptId:{0} Exception:{1}", houseDeptId, ex.Message), "PropertyCharge/GetSubjectPreCostList", FileLogType.Exception);
                return Json(new APIResultDTO() { Code = 901, Message = "获取数据异常" });
            }
        }

        /// <summary>
        /// 预存费用
        /// </summary>
        /// <param name="jparameter"></param>
        /// <returns></returns>
        public JsonResult<APIResultDTO> SubjectPreCostPost([FromBody]JObject jparameter)
        {
            try
            {
                AppSubjectPreCost parameter = jparameter.ToObject<AppSubjectPreCost>();
                var rlt = PaymentAppService.SaveSubjectPreCostPost(parameter, "逸社区APP");
                APIResultDTO result = new APIResultDTO();
                result.Code = rlt.IsSuccess ? 0 : int.Parse(rlt.ErrorCode);
                result.Message = result.Code == 0? "预存费成功" : rlt.Msg;
                return Json(result);
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("jparameter:{0} Exception:{1}", jparameter, ex.Message), "PropertyCharge/SubjectPreCostPost", FileLogType.Exception);
                return Json(new APIResultDTO() { Code = 901, Message = "缴费异常" });
            } 
        }

        /// <summary>
        /// 预存费用验证检查
        /// </summary>
        /// <param name="jparameter"></param>
        /// <returns></returns>
        public JsonResult<APIResultDTO> SubjectPreCostPostCheck([FromBody]JObject jparameter)
        {
            try
            {
                AppSubjectPreCost parameter = jparameter.ToObject<AppSubjectPreCost>();
                var rlt = PaymentAppService.SubjectPreCostCheck(parameter);
                APIResultDTO result = new APIResultDTO();
                result.Code = rlt.IsSuccess ? 0 : int.Parse(rlt.ErrorCode);
                result.Message = rlt.Msg;
                return Json(result);
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("jparameter:{0} Exception:{1}", jparameter, ex.Message), "PropertyCharge/SubjectPreCostPostCheck", FileLogType.Exception);
                return Json(new APIResultDTO() { Code = 901, Message = "预存费检查异常" });
            }
        }

        #endregion

        #region 历史缴费接口 20170606

        /// <summary>
        /// 获取缴费历史记录
        /// 页面根据缴费时间倒序展示用户使用app的缴费记录 修改bug #4643 2017-7-10
        /// </summary>
        /// <param name="houseDeptId">房屋deptId</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns></returns>
        public JsonResult<APIResultDTO> GetChargeHistoryRecordList(int? houseDeptId, int? pageIndex, int? pageSize)
        {
            houseDeptId = houseDeptId ?? 0;
            pageIndex = pageIndex ?? 1;
            pageSize = pageSize ?? 10;
            try
            {
                var data = PaymentAppService.GetChargeHistoryRecordList(houseDeptId, pageIndex.Value, pageSize.Value);
                APIResultDTO result = new APIResultDTO();
                if (data.Count() > 0)
                {
                    result.Code = 0;
                    result.Message = "获取数据成功";
                }
                else
                {
                    result.Code = 710;
                    result.Message = "没有数据了";
                }
                result.Data = data;
                return Json(result);
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("houseDeptId:{0} Exception:{1}", houseDeptId, ex.Message), "PropertyCharge/GetChargeHistoryRecordList", FileLogType.Exception);
                return Json(new APIResultDTO() { Code = 901, Message = "获取数据异常" });
            }
        }

        /// <summary>
        /// 获取账单明细
        /// </summary>
        /// <param name="chargeRecordId">收费记录Id</param>
        /// <returns></returns>
        public JsonResult<APIResultDTO> GetBillDetail(string chargeRecordId)
        {
            try
            {
                var data = PaymentAppService.GetChargeRecordDetail(chargeRecordId);
                APIResultDTO result = new APIResultDTO();
                result.Code = 0;
                result.Message = "获取数据成功";
                result.Data = data;
                return Json(result);
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("chargeRecordId:{0} Exception:{1}", chargeRecordId, ex.Message), "PropertyCharge/GetSubjectPreCostList", FileLogType.Exception);
                return Json(new APIResultDTO() { Code = 901, Message = "获取数据异常" });
            }
        }

        #endregion

        #region 获取最后一次交费记录 20170710

        public JsonResult<APIResultDTO> GetLastChargeRecord(int? houseDeptId)
        {
            try
            {
                ChargeRecordAppService service = new ChargeRecordAppService();
                var lastData = service.GetLastChargeRecord(houseDeptId);
                APIResultDTO result = new APIResultDTO();
                if (lastData == null)
                {
                    result.Code = 701;
                    result.Message = "没有数据";
                }
                else
                {
                    result.Code = 0;
                    result.Message = "获取数据成功";
                    result.Data = lastData;
                }
               
                return Json(result);
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("houseDeptId:{0} Exception:{1}", houseDeptId, ex.Message), "PropertyCharge/GetLastChargeRecord", FileLogType.Exception);
                return Json(new APIResultDTO() { Code = 901, Message = "获取数据异常" });
            }
        }

        #endregion
    }

    #region 旧接口返回视图

    /// <summary>
    /// 收费项目视图
    /// </summary>
    public class ChargeSubjectView
    {
        public int Subject_Id { get; set; }
        public string Subject_Name { get; set; }
    }

    #endregion
}