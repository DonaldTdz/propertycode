using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YK.PropertyMgr.CompositeAppService;
using YK.BackgroundMgr.PresentationService;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.DomainCompositeService;
using YK.BackgroundMgr.Common;
using YK.BackgroundMgr.CompositeAppService;
using System.Collections.Generic;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.Crosscuting;
using System.Data;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.CompositeDomainService;
using YK.PropertyMgr.CompositeDomainService.NoticeService;

namespace YK.PropertyMgr.CompositeAppService.Tests
{
    [TestClass]
    public class AppServiceTest
    {
        [TestMethod]
        public void GenerateCommunityChargBill()
        {
            RegistService();
            GenerateBillAppService.GenerateCommunityChargBill(113344);
        }

        [TestMethod]
        public void GenerateAllChargBill()
        {
            RegistService();
            GenerateBillAppService.GenerateAllChargBill(0,false);
        }

        [TestMethod]
        public void AutomaticCycleGenerationBill()
        {
            RegistService();
            GenerateBillAppService.AutomaticCycleGenerationBill(DateTime.Today,2,0,true);
        }

        [TestMethod]
        public void BalanceInitialization()
        {
            RegistService();
            IList<BalanceInfo> BalanceInfoList = new List<BalanceInfo>();
            BalanceInfoList.Add(new BalanceInfo() 
            {
                ComDeptId = 14439,
                HouseDeptId = 14467,
                ResourcesId = 14467,
                Amount = 400m
            });
            BalanceInfoList.Add(new BalanceInfo()
            {
                ComDeptId = 14439,
                HouseDeptId = 14468,
                ResourcesId = 14468,
                Amount = 7200m
            });
            ResultModel result = BalanceAppService.BalanceInitialization(BalanceInfoList,0,"");
        }

        [TestMethod]
        public void WriteLoginToFile()
        {
            RegistService();
            string mm = "XXXTTT";
            Task.Run(() => 
            {
                mm += " BBBB";
                LogProperty.WriteLoginToFile("测试 Task Run" + mm, "Test", FileLogType.Info);
            });
            //LogProperty.WriteLoginToFile("测试 Task Run 666", "Test", FileLogType.Info);
        }

        [TestMethod]
        public void CalculateAmountTest()
        {
            string resultFormula = "12.77*0.73";
            DataTable dt = new DataTable();
            //公式计算
            decimal result = Math.Round(decimal.Parse(dt.Compute(resultFormula, "false").ToString()), 2);
            resultFormula = result.ToString();
        }


        [TestMethod]
        public void BillsDailyPaymentTest()
        {
            RegistService();
            //ResultModel result = PaymentAppService.BillsDailyPayment(new string[] { "31b4d740-6397-44d8-b44f-d4d38b869db6", "522b9aba-4d21-4196-ab22-c0fbfec86b12" },500,"PAL02-2",PayTypeEnum.Cash,true,1,"测试者");
            //ResultModel result = PaymentAppService.BillsDailyPayment(new string[] { "310d6702-59bb-428b-8a50-a8f1392f0a7f" }, 200, true, PayTypeEnum.Cash, false, 1, "测试者3","");
        }

        [TestMethod]
        public void SendArrearsNoticeMsgTest()
        {
            RegistService();
            NoticeMsg msg = new NoticeMsg();
            msg.ComDeptId = 55905;
            msg.ComDeptName = "五二七";
            msg.UserPhones = new string[]{ "13880954084"};
            SMSNoticeService.Instance.SendArrearsNoticeMsg(msg);
        }


        [TestMethod]
        public void ImportArrearageTest()
        {
            RegistService();
            List<ArrearageInfo> infolist = new List<ArrearageInfo>();
            infolist.Add(new ArrearageInfo() {
                SubjectName = "新收物业费",
                ResourceNo = "1-1-1-10",
                Amount = 251.36m,
                BeginDate = DateTime.Now.AddMonths(-1),
                EndDate = DateTime.Now,
                ResourceTypeName = "房屋",
                Remark = "欠费信息导入"
            });
            infolist.Add(new ArrearageInfo()
            {
                SubjectName = "新收物业费",
                ResourceNo = "1-2-5-10",
                Amount = 251.36m,
                BeginDate = DateTime.Now.AddMonths(-1),
                EndDate = DateTime.Now,
                ResourceTypeName = "房屋",
            });
            ResultModel result = DataInitCompositeAppService.ImportArrearage(14438, infolist,0,"");
            if (result.IsSuccess)
            {

            }
        }

        [TestMethod]
        public void TestPostMessageinfo()
        {

            MessageInfo msgInfo = new MessageInfo()
            {
                Title = "缴费提醒",
                Content = string.Format("【{0}】您好，您的物业费已于本月到期，请及时续交费用！", "1-1-1-1"),
                ActionUrl = "", //注：需要跳转到物业APP缴费页面
                IconUrl = ""//消息的ICON
            };

           
                HttpClientService service = new HttpClientService();
              
                service.SendPushWithJson(new string[] { "13699434893" }, msgInfo,"14438");
                  
      

        }

        [TestMethod]
        public void CallPaymentPost()
        {
            //RegistService();
            //HttpClientService service = new HttpClientService();
            //service.CallPaymentPost();
        }

        [TestMethod]
        public void GetPayQRCodeUrl()
        {
            RegistService();
            ClientPayOrder order = new ClientPayOrder();
            order.UserId = "2CE5FF94-A52C-4316-A440-05C7E247B161";
            order.HouseDeptId = 14491;
            order.HouseNo = "1-2-5-10";
            order.PayAmount = 10;
            var str = PaymentAppService.GetPayQRCodeUrl(order);
            if (string.IsNullOrEmpty(str.NumericalNumber))
            {
                str.NumericalNumber = "xx";
            }
        }

        [TestMethod]
        public void CallNoticeTask()
        {
            NoticeTaskService.Instance.Run();
        }

        [TestMethod]
        public void GetSerialNumber()
        {
            string num = BillCommonService.Instance.GetSerialNumber("cp");
            var len = num.Length;
            if (len > 32)
            {
                num = "bbb";
            }
        }


        [TestMethod]
        public void CallNoticeTaskFullRun()
        {
            RegistService();
            NoticeTaskService.Instance.SendArrearsMsg(false);
        }

        [TestMethod]
        public void GTest()
        {
            var weekDay = DateTime.Today.DayOfWeek.GetHashCode();
            var week = DateTime.Today.DayOfWeek.ToString();
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

        /// <summary>
        /// 领域层服务
        /// </summary>
        private void RegistDomainService()
        {
            DomainInterfaceHelper.Register<IPropertyDomainService>(new PropertyDomainService());
        }
    }
}
