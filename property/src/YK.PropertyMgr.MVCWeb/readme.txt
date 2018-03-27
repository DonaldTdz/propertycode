1. 新建MVC项目，只保留Controllers，Views以及web.config文件，其余全部删除
2. 新建立插件类，示例：
	public class PluginTestPlugin : PluginBase
    {
        public override string DefaultAction
        {
            get
            {
                return "List";
            }
        }

        public override string DefaultController
        {
            get
            {
                return "Content";
            }
        }

        public override string Name
        {
            get
            {
                return "PluginTest";
            }
        }

        public override string Namespace
        {
            get
            {
                return "YK.FrameworkTools.PluginServiceTestPlugin.Controllers";
            }
        }

        public override string Url
        {
            get
            {
                return "PluginTest/{controller}/{action}/{id}";
            }
        }

        public override void CustomInit()
        {
        }
    }

3. 在框架运行目录下面，建立Plugins文件夹，并建立插件子文件夹，如：PluginTest，将插件的bin,Views文件夹拷贝到插件子文件夹中
4. webpi支持：
	1）在框架的WebApiConfig.cs文件夹中，增加以下代码：
	config.Routes.MapHttpRoute(
                name: "SpriteApi",
                routeTemplate: "api/{area}/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );
	2）在框架Global的Application_Start方法，增加GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector), new AreaHttpControllerSelector(GlobalConfiguration.Configuration));
	3） 在框架Web.config，runtime下增加<probing privatePath="App_Data/Dependencies" />