using System;
using System.Web.Mvc;
using System.Web.WebPages.Razor;

namespace YK.BackgroundMgr.MVCCore
{
    public class PluginRazorViewEngine : RazorViewEngine
    {
        public PluginRazorViewEngine()
        {
            AreaViewLocationFormats = _areaViewLocationFormats;
            AreaMasterLocationFormats = _areaViewLocationFormats;
            AreaPartialViewLocationFormats = _areaViewLocationFormats;
        }

        /// <summary>
        /// 定义区域视图页所在地址。
        /// </summary>
        private readonly string[] _areaViewLocationFormats = new[]
        {
            "~/Views/Parts/{0}.cshtml",
            "~/Plugins/{2}/Views/{1}/{0}.cshtml",
            "~/Plugins/{2}/Views/Shared/{0}.cshtml",
            "~/{2}/Views/{1}/{0}.cshtml",
            "~/{2}/Views/Shared/{0}.cshtml",
            "~/Areas/{2}/Views/{1}/{0}.cshtml",
            "~/Areas/{2}/Views/Shared/{0}.cshtml",
        };

        /// <summary>
        /// 搜索部分视图页。
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="partialViewName"></param>
        /// <param name="useCache"></param>
        /// <returns></returns>
        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            SetViewAssemblyReference(controllerContext);
            return base.FindPartialView(controllerContext, partialViewName, useCache);
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            SetViewAssemblyReference(controllerContext);
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

        /// <summary>
        /// 给运行时编译的页面加了引用程序集。
        /// </summary>
        /// <param name="areaName">区域名称</param>
        private void CodeGeneration(string areaName)
        {
            RazorBuildProvider.CodeGenerationStarted += (object sender, EventArgs e) =>
            {
                var provider = (RazorBuildProvider)sender;

                var plugin = PluginManager.GetPlugin(areaName);

                if (plugin != null)
                {
                    provider.AssemblyBuilder.AddAssemblyReference(plugin.Assembly);
                }
            };
        }

        /// <summary>
        /// 设置视图中程序集引用
        /// </summary>
        /// <param name="controllerContext"></param>
        private void SetViewAssemblyReference(ControllerContext controllerContext)
        {
            var tokens = controllerContext.RouteData.DataTokens;
            if (tokens.ContainsKey("namespaces"))
            {
                var area = tokens["area"];
                if (area != null)
                {
                    CodeGeneration(area.ToString());
                }
            }
        }
    }
}
