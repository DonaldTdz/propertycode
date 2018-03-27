using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using YK.BackgroundMgr.Common;
using YK.BackgroundMgr.CompositeAppService;
using YK.BackgroundMgr.DomainCompositeService;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.PresentationService;
using YK.FrameworkTools.PluginService;

namespace YK.BackgroundMgr.MVCWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ZY_DBHelper.BaseHelper.connstring = "PropertyMgrConnection";
            
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Register global filter
            GlobalFilters.Filters.Add(new GlobalActionFilterAttribute());
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(DTParameterModel), new DTModelBinder());

            RegistService();

            //启动自动生成账单
            string BillStartDate = ConfigurationManager.AppSettings["BillStartDate"];
            string BillExecutionTime = ConfigurationManager.AppSettings["BillExecutionTime"];

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
            //GenerateBillAppService.AutomaticCycleGenerationBill(StartDate, Hour, Minute);
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense("Aspose.Cells.lic");
 

            //GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector), new AreaHttpControllerSelector(GlobalConfiguration.Configuration));
        }

        protected void Application_End()
        {
            //GenerateBillAppService.CloseAutomaticCycle();
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