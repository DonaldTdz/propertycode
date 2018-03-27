using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YK.PropertyMgr.MVCWeb.Controllers;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.DomainService;
using YK.BackgroundMgr.PresentationService;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.DomainCompositeService;
using YK.BackgroundMgr.Common;
using YK.BackgroundMgr.CompositeAppService;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.ApplicationDTO.Enums;
using System.Net.Http;
using System.Net.Http.Headers;
using YK.PropertyMgr.CompositeDomainService;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using YK.BackgroundMgr.DomainService;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;
using Newtonsoft.Json.Linq;

namespace YK.PropertyMgr.MVCWeb.Tests
{
    [TestClass]
    public class ControllerTest
    {
        public object TemplatePrintRecordAppService { get; private set; }

        [TestMethod]
        public void ChargBillInsert()
        {
            try
            {
                //ChargBill chargbill = new ChargBill()
                //{
                //    Id = Guid.NewGuid().ToString(),
                //    Description = "测试",
                //    Quantity = 1,
                //    BeginDate = DateTime.Now,
                //    EndDate = DateTime.Now.AddDays(30),
                //    HouseDeptId = 14772,
                //    ResourcesId = 14772,
                //    RefType = (int)ReourceTypeEnum.House,
                //    BillAmount = 150,
                //    ReceivedAmount = 0,
                //    ReliefAmount = 0,
                //    PenaltyAmount = 0,
                //    ComDeptId = 14768,
                //    IsDevPay = false,
                //    Status = (int)BillStatusEnum.NoPayment,
                //    IsDel = false,
                //    CreateTime = DateTime.Now,
                //    UpdateTime = DateTime.Now,
                //    Remark = string.Empty,
                //    ChargeSubjectId = 136

                //};
                //ChargBillDomainService chargbillservice = new ChargBillDomainService();
                //chargbillservice.InsertChargBill(chargbill);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [TestMethod]
        public void apiPaymentTest()
        {
            HttpClient m = new HttpClient();
            m.BaseAddress = new Uri("http://localhost:18618/");//new Uri("http://172.16.20.33/"); 
            m.DefaultRequestHeaders.Accept.Clear();
            m.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            m.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));
            HttpResponseMessage response = m.PostAsJsonAsync("api/PropertyCharge/PaymentPost", new { BillIds = new string[] { Guid.NewGuid().ToString() }, Amount = 100, PayType = 1 }).Result;
            if (response.IsSuccessStatusCode)
            {

            }
        }

        [TestMethod]
        public void apiGetBillListByIdsTest()
        {
            HttpClient m = new HttpClient();
            m.BaseAddress = new Uri("http://localhost:18618/");//new Uri("http://172.16.20.33/"); 
            m.DefaultRequestHeaders.Accept.Clear();
            m.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            m.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));
            HttpResponseMessage response = m.PostAsJsonAsync("api/PropertyCharge/GetBillListByIds", new { BillIds = new string[] { "952f21f7-886e-4f6b-9ca3-d5b480fb60ec", "a07a8c5f-ea22-4d84-a2e9-7f73e68f3458" } }).Result;
            if (response.IsSuccessStatusCode)
            {

            }
        }



        [TestMethod]
        public void ChargeSubjectInsert()
        {
            //ChargeSubjectController testController = new ChargeSubjectController();

            //ChargeSubjectDTO chargeSubjectDTO = new ChargeSubjectDTO()
            //{
            //    BillDay = 1,
            //    BillPeriod = 1,
            //    ChargeFormula = "1*2" ,
            //    ChargeFormulaShow = "单价*房屋面积",
            //    Code = "FW201608171105SAQ1",
            //    ComDeptId = 2,
            //    CreateTime = DateTime.Now,
            //    IsDel = false,
            //    IsOnline = false,
            //    Name = "海桐小区2房屋收费项目",
            //    Operator = 1,
            //    PenaltyRate = 0,
            //    Price = 2.36m,
            //    Remark = "donald Test",
            //};




            Console.WriteLine();
        }

        [TestMethod]
        public void ChargeRecordList()
        {
            //string guid = Guid.NewGuid().ToString();
            RegistService();
            ChargeRecordController controller = new ChargeRecordController();
            controller.ChargeRecordList(true,true);
        }


        [TestMethod]
        public void ZHReportTest()
        {
            RegistService();
        var retableRto=PaymentService.Instance.CalculationAllPrepaymentByComDeptId_New(32317, new DateTime(2016, 10, 1), new DateTime(2016, 11, 30), new DateTime(2016, 11, 30));

          var  retrueDTOList = (from h in retableRto
                                group new {  h.ChargeSubjectId,h.TotalRecAmount,h.ReliefAmountTotal,h.UnPaidAmountTotal } by new { h.ChargeSubjectId } into b
                                select new ReportTableDTO
                                {
                                    ChargeSubjectId = b.Key.ChargeSubjectId,
                                    TotalRecAmount =b.Sum(c=>c.TotalRecAmount),
                                    ReliefAmountTotal= b.Sum(c => c.ReliefAmountTotal),
                                    UnPaidAmountTotal = b.Sum(c => c.UnPaidAmountTotal),
                                }
                            ).ToList();


            var a = retrueDTOList;

            // PaymentService.Instance.CalculationAllPrepaymentByComDeptId(323317, new  DateTime(2016,10,1), new DateTime(2016, 11, 30), new DateTime(2016, 11, 30));
            // int a = 0;
            //  PaymentService.Instance.CalculationAllPrepaymenHousetByComDeptId(14438, new DateTime(2016, 10, 1), new DateTime(2016, 11, 30), new DateTime(2016, 11, 01), "", false, 1, 10, out a);
        }

        [TestMethod]
        public void IDtest()
        {

            //msy
            Snowflake sn1 = new Snowflake(0, -1);
            Snowflake sn2 = new Snowflake(1, -1);
            Snowflake sn3 = new Snowflake(2, -1);
            Snowflake sn4 = new Snowflake(3, -1);
            Snowflake sn5 = new Snowflake(4, -1);


            Thread t1 = new Thread(() => DoTestIdWoker(sn1));
            Thread t2 = new Thread(() => DoTestIdWoker(sn2));
            Thread t3 = new Thread(() => DoTestIdWoker(sn3));
            Thread t4 = new Thread(() => DoTestIdWoker(sn4));
            Thread t5 = new Thread(() => DoTestIdWoker(sn5));

            t1.IsBackground = true;
            t2.IsBackground = true;
            t3.IsBackground = true;
            t4.IsBackground = true;
            t5.IsBackground = true;

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();

            try
            {
              
                t1.Abort();
                t2.Abort();
                t3.Abort();
                t4.Abort();
                t5.Abort();
            }
          
            catch (Exception e)
            {

            }
            Console.WriteLine("done");


        }

        private void DoTestIdWoker(Snowflake sn)
        {
            for (int i = 0; i <= 3; i++)
            {
                string a = "开始执行 " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffffff") + "    " + sn.GetId() + "\n";
                Console.WriteLine(a);
            }
        }

        [TestMethod]
        public void CreateTicketSerialNumberTest()
        {
            RegistService();
            string a ="流水号为"+  BillCommonService.Instance.CreateTicketSerialNumberTest(14438, 1);
            Console.WriteLine(a);
        }

        [TestMethod]
        public void TestCarParkTree()
        {
             RegistService();
             var tree= new CarportDomainService().GetCarParkTree("tangdezhou","11");
        }


        [TestMethod]
        public void TestArrearsReport()
        {
            RegistService();
            RePortService service = new RePortService();
            ReportArrearsSearchDTO search = new ReportArrearsSearchDTO()
            {
                ComDeptIdStr = "number:113344",
                ChargeDate = new DateTime(2017, 1, 30)
            };
            int totalCount = 0;
            var retunrModel = service.GetArrearsReportDataList(1,10, search, out totalCount);

       
        }

        [TestMethod]
        public void TestArrearsDetailReport()
        {
            RegistService();
            RePortService service = new RePortService();
            ReportArrearsSearchDTO search = new ReportArrearsSearchDTO()
            {
                ComDeptIdStr = "number:113344",
                ChargeDate = new DateTime(2017, 1, 30)
            };
            int totalCount = 0;

            var retunrModel = service.GetArrearsReportDetailList(1, 10, search, out totalCount,false);


        }

        
        [TestMethod]
        public void TestPrePaymentDetailReport()
        {
            RegistService();
            RePortService service = new RePortService();
            PrePaymentDetailSearchDTO search = new PrePaymentDetailSearchDTO()
            {
                ComDeptIdStr = "number:113344",
                ChargeBeginDate = new DateTime(2017, 1, 1),
                ChargeEndDate = new DateTime(2017, 4, 30),                
                PageSize=10,
                PageStart =10,

          
            
            };
            int totalCount = 0;
          //   service.PrePaymentDetailReport(search,out totalCount);


        }

        [TestMethod]
        public void TestTemplatePrint()
        {
            RegistService();
            //ChargeRecordAppService _ChargeRecordAppService = new ChargeRecordAppService();
            //var byete=   _ChargeRecordAppService.GetTemplatePrint("7cff64fb-7747-4da9-833d-db935c45dd7f",4);
            //var a = 1;
            TemplatePrintRecordAppService _TemplatePrintRecordAppService = new TemplatePrintRecordAppService();
            _TemplatePrintRecordAppService.CreateJingHuaTemplatePrint();
            _TemplatePrintRecordAppService.CreateChuanGangTemplatePrint();


        }

        [TestMethod]
        public void ReceiptCode()
        {

            string code = "1";
            code= code.PadLeft(4, '0');
            Console.WriteLine("{0}", code);
        }

        [TestMethod]
        public void JsonModelTest()
        {

            var roomsetlist = new List<Room_info_set>();
            roomsetlist.Add(new Room_info_set { out_room_id = "14588", room_id = "AJKYU9129" });
            roomsetlist.Add(new Room_info_set { out_room_id = "14581", room_id = "DHGH11828" });
            RoominfoResponse model = new RoominfoResponse()
            {
                alipay_eco_cplife_roominfo_upload_response = new Alipay_eco_cplife_roominfo_upload_response() {
                    code = "10000",
                    msg = "Success",
                    community_id = "dasjdaldhajkhjk",
                     room_info_set = roomsetlist.ToArray()
                }
            };
          var JSONString=  JsonHelper.JsonSerializerByNewtonsoft(model);
          JObject obj = JsonHelper.JsonDeserializeByNewtonsoft(JSONString) as JObject;
          var a = obj["sign"];
          var b = obj["alipay_eco_cplife_roominfo_upload_response"]["room_info_set"].Count();

            for (int i = 0; i <= b - 1; i++)
            {
                var c = obj["alipay_eco_cplife_roominfo_upload_response"]["room_info_set"][i];
            } 


            var d = "111";

        }

      




        /// <summary>
        /// 注册服务
        /// </summary>
        private void RegistService()
        {
    

            PresentationServiceHelper.Register<ICookieService>(new CookieService());
            PresentationServiceHelper.Register<ICacheService>(new CacheService());
            PresentationServiceHelper.Register<ISessionService>(new SessionService());
            PresentationServiceHelper.Register<ITemplateService>(new TemplateService());
            PresentationServiceHelper.Register<IPropertyService>(new PropertyAppService());

            RegistDomainService();
        }

        private void RegistDomainService()
        {
            DomainInterfaceHelper.Register<IPropertyDomainService>(new PropertyDomainService());
        }
    }

    public class RoominfoResponse
    {
        public Alipay_eco_cplife_roominfo_upload_response alipay_eco_cplife_roominfo_upload_response { get; set; }
        public string sign { get; set; }
    }

    public class Alipay_eco_cplife_roominfo_upload_response
    {
        public string code { get; set; }
        public string msg { get; set; }
        public string community_id { get; set; }
        public Room_info_set[] room_info_set { get; set; }
    }
    public class Room_info_set
    {
        public string out_room_id { get; set; }
        public string room_id { get; set; }
    }

}
