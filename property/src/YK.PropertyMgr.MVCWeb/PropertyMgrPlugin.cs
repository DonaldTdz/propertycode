using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using YK.FrameworkTools.PluginService;
using YK.PropertyMgr.CompositeAppService;
using YK.PropertyMgr.CompositeAppService.NoticeService;

namespace YK.PropertyMgr.MVCWeb
{
    public class PropertyMgrPlugin : PluginBase
    {
        public override string DefaultAction
        {
            get
            {
                return "Index";
            }
        }

        public override string DefaultController
        {
            get
            {
                return "Vacation";
            }
        }

        public override string Name
        {
            get
            {
                return "PropertyMgr";
            }
        }

        public override string Namespace
        {
            get
            {
                return "YK.PropertyMgr.MVCWeb.Controllers";
            }
        }

        public override string Url
        {
            get
            {
                return "PropertyMgr/{controller}/{action}/{id}";
            }
        }

        private static bool IsLoaded { get; set; }

        public override void CustomInit()
        {
            if (!IsLoaded)
            {
                // ---------
                string IsStartBill = ConfigurationManager.AppSettings["IsStartBill"];
                //是否开启自动生成账单
                if (IsStartBill == "1")
                {
                    string BillStartDate = ConfigurationManager.AppSettings["BillStartDate"];
                    string BillExecutionTime = ConfigurationManager.AppSettings["BillExecutionTime"];
                    string IsTaskRun = ConfigurationManager.AppSettings["IsTaskRun"];

                    DateTime StartDate = DateTime.Today;
                    if (!string.IsNullOrEmpty(BillStartDate))
                    {
                        StartDate = DateTime.Parse(BillStartDate);
                    }
                    int Hour = 2;
                    int Minute = 0;

                    if (!string.IsNullOrEmpty(BillExecutionTime))
                    {
                        string[] ExecutionTime = BillExecutionTime.Split(':');
                        if (ExecutionTime.Count() > 1)
                        {
                            Hour = int.Parse(ExecutionTime[0]);
                            Minute = int.Parse(ExecutionTime[1]);
                        }
                    }
                    //开启账单循环
                    GenerateBillAppService.AutomaticCycleGenerationBill(StartDate, Hour, Minute, IsTaskRun == "1");
                }
                //GenerateBillAppService.GenerateAllChargBill(0, true);
                IsLoaded = true;

                //开始欠费通知任务
                NoticeTaskAppService.FullPointRun();
            }
        }
    }
}