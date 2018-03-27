using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Dispatcher;

[assembly: System.Web.PreApplicationStartMethod(typeof(YK.BackgroundMgr.MVCCore.Bootstrapper), "Initialize")]
namespace YK.BackgroundMgr.MVCCore
{
    /// <summary>
    /// 引导程序。
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        public static void Initialize()
        {           
            AppDomain.CurrentDomain.SetupInformation.ShadowCopyFiles = "true";
            AppDomain.CurrentDomain.SetShadowCopyPath(HostingEnvironment.MapPath("~/App_Data/Dependencies"));

            //注册插件控制器工厂。
            ControllerBuilder.Current.SetControllerFactory(new PluginControllerFactory());

            //注册插件模板引擎。
            ViewEngines.Engines.Clear();

            ViewEngines.Engines.Add(new PluginRazorViewEngine());

            //初始化插件。
            PluginManager.Initialize();

            //加载服务到IOC容器
            //ServiceProvider.Load();


            //modify by zhoulingqiu
            var config = GlobalConfiguration.Configuration;
            config.Services.Replace(typeof(IAssembliesResolver),new SmartAssembliesReslover());
            config.Services.Replace(typeof(IHttpControllerSelector), new SmartHttpControllerSelector(config));
        }
    }
}
