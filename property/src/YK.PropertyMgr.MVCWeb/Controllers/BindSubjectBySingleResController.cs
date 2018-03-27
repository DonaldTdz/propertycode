using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.ApplicationService.Service;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class BindSubjectBySingleResController : BaseController
    {
        #region Index
        public ActionResult Index(int commuDeptId, int resType)
        {
            ChargeSubjectAppService service = new ChargeSubjectAppService();
            BindSubjectBySingleResViewData viewData = new BindSubjectBySingleResViewData()
            {
                ChargesubjectList = service.GetChargeSubjectsByComDeptId(commuDeptId, resType)
            };
            return View(viewData);
        }
        #endregion

        #region 获取小区下的科目信息
        /// <summary>
        /// 获取小区下的科目信息
        /// </summary>
        /// <param name="CommId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSubjects(int commId, int type)
        {
            ChargeSubjectAppService service = new ChargeSubjectAppService();
            var json = new JsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            json.Data = service.GetChargeSubjectsByComDeptId(commId, type); ;
            return json;
        }
        #endregion

        #region 结构树
        /// <summary>
        /// 结构树
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"> tab切换卡的类型  [楼宇、车库、水表、电表、气表]</param>
        /// <param name="typeEle">资源类型</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSubjectSelectChildTree(int id, int type, int? comDeptId)
        {
            MeterAppService service = new MeterAppService();
            var data = service.GetTree(CurrentAdminUser, id, type, comDeptId);

            var jResult = new JsonResult();
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jResult.MaxJsonLength = int.MaxValue;
            jResult.Data = data;
            return jResult;
        }
        #endregion

        #region 获取车位
        /// <summary>
        /// 获取车位
        /// </summary>
        /// <param name="parkIngId">车库ID</param>
        /// <returns></returns>
        public ActionResult GetCarParkSpace(string parkIngId)
        {
            try
            {
                var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
                var list = propertyService.GetAsynParkingSpaceTree(parkIngId);
                List<CustomTreeNodeModel> listParkSpace = new List<CustomTreeNodeModel>();

                List<int?> listIds = new List<int?>();


                list.ForEach(o =>
                {
                    listIds.Add(Convert.ToInt32(o.id.Split('_')[2]));
                });

                var deptInfos = propertyService.GetDeptsByIds(listIds);
                CustomTreeNodeModel model;
                int resId = 0;
                int houseDeptId = 0;
                string[] arr;
                foreach (var item in list)
                {
                    arr = item.id.Split('_');
                    model = new CustomTreeNodeModel();
                    model.id = item.id;
                    model.icon = item.icon;
                    model.children = item.children;
                    resId = Convert.ToInt32(arr[0]);
                    houseDeptId = Convert.ToInt32(arr[2]);
                    if (deptInfos.Any(m => m.Id == houseDeptId))
                    {
                        model.text = item.text + " (" + deptInfos.FirstOrDefault(m => m.Id == houseDeptId).Name + ") ";
                    }
                    else
                    {
                        model.text = item.text;
                        //      model.state = new { disabled = true };
                    }
                    listParkSpace.Add(model);
                }
                var jResult = new JsonResult();
                DeptAppService deptService = new DeptAppService();
                jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                jResult.Data = listParkSpace;
                return jResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region 设置开发商代缴
        /// <summary>
        /// 设置开发商代缴
        /// </summary>
        /// <param name="jsonString">数据</param>
        /// <param name="isPostBillSave">是否生成账单</param>
        /// <param name="sourceType">资源</param>
        /// <returns></returns>
        public ActionResult Save(string jsonString, bool isPostBillSave, int sourceType)
        {
            DeveloperSetTimeListDTO list = JsonConvert.DeserializeObject<DeveloperSetTimeListDTO>(jsonString);
            SubjectHouseRefAppService service = new SubjectHouseRefAppService();
            JsonResult json = new JsonResult();
            json.Data = service.SaveSetDevloperPay(list, (int)this.CurrentAdminUser.Id, isPostBillSave, sourceType, Convert.ToInt32(CurrentAdminUser.Id), CurrentAdminUser.UserName);
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }
        #endregion

        #region 设置科目选中
        /// <summary>
        /// 设置科目选中
        /// </summary>
        /// <param name="resId"></param>
        /// <param name="subjectType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetSubjectChecked(int resId, int subjectType)
         {
            SubjectHouseRefAppService service = new SubjectHouseRefAppService();
            var list = service.GetBindSubjectInfo(resId, subjectType);
            var jsonResult = new JsonResult();
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();//这里使用自定义日期格式，默认是ISO8601格式           
            timeConverter.DateTimeFormat = "yyyy-MM-dd";//设置时间格式   
            string json = JsonConvert.SerializeObject(list, Formatting.Indented, timeConverter);
            jsonResult.Data = json;
            return jsonResult;
        }
        #endregion

        #region 获取房屋信息
        /// <summary>
        ///  获取房屋信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetHouseInfo(int houseDeptId, int communityId)
        {
            DeptAppService service = new DeptAppService();
            SubjectHouseRefAppService subjectserve = new SubjectHouseRefAppService();
            MeterAppService meterserve = new MeterAppService();
            var result = new JsonResult();
            var house = new HouseInfo();
            var subject = new SubjectHouseRefDTO();
            var meter = new MeterDTO();
            string res = "未绑定房屋,未绑";
            if (houseDeptId > 0)
            {
                house = service.GetHouseInfo(houseDeptId, communityId);
                subject = subjectserve.GetSubjectHouseRef(houseDeptId,3);
                meter = meterserve.GetMeterByKey(houseDeptId);
                if (house != null)
                {
                    HouseStatusEnum houseStatusEnum = (HouseStatusEnum)house.HouseStatus;
                    res = house.DoorNo + "," + EnumHelpService.GetEnumDes(houseStatusEnum);
                }
                if (subject != null)
                {
                    res = meter.MeterNum + ",已绑";
                }
                if (house == null && subject == null)
                {
                    res = meter.MeterNum + ",未绑";
                }
            }

            return res;
        }
        #endregion

        #region 获取默认计费开始日期

        public ActionResult GetChargList(int? ResourcesId, int? ChargeSubjecId, int? resType, int? CarHouseDeptId)
        {
            ResultModel resultModel = new ResultModel();
            ChargBillAppService chargeRecordAppService = new ChargBillAppService();
            ChargBillDTO bill = chargeRecordAppService.GetEasyDailyChargList(ResourcesId, ChargeSubjecId, resType, CarHouseDeptId);
            SubjectHouseRefAppService refService = new SubjectHouseRefAppService();
            var houseBindRef = refService.GetSubjectHouseRefByResId(ResourcesId.Value, ChargeSubjecId.Value);
            //如果存在绑定关系 表示编辑 默认值为编辑值
            if (houseBindRef != null)
            {
                return Json(new ResultModel()
                {
                    IsSuccess = true,
                    Data = new
                    {
                        DefalutDate = houseBindRef.BeginDateBill.Value.ToString("yyyy-MM-dd"),
                        IsBill = (bill != null),
                        IsNew = false
                    }
                });
            }
            if (bill == null)
            {
                return Json(new ResultModel()
                {
                    IsSuccess = true,
                    Data = new
                    {
                        DefalutDate = DateTime.Today.ToString("yyyy-MM-dd"),
                        IsBill = false,
                        IsNew = true
                    }
                });
            }
            else
            {
                var defaultDate = bill.EndDate.Value.AddDays(1).ToString("yyyy-MM-dd");//默认为账单最后一天+1
                return Json(new ResultModel()
                {
                    IsSuccess = true,
                    Data = new
                    {
                        DefalutDate = defaultDate,
                        MinDate = defaultDate, //如果有账单，选择的最小开始日期也为账单最晚+1
                        IsBill = true,
                        IsNew = true
                    }
                });
            }
        }

        #endregion
    }
    public class BindSubjectBySingleResViewData
    {
        public List<ChargeSubjectDTO> ChargesubjectList { get; set; }
    }

}