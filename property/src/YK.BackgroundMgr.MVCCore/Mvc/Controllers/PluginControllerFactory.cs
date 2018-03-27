using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace YK.BackgroundMgr.MVCCore
{
    /// <summary>
    /// 插件控制器工厂。
    /// </summary>
    public class PluginControllerFactory : DefaultControllerFactory
    {
        private static readonly object Locker = new object();

        private static Dictionary<string, Type> _controllerTypeCache = new Dictionary<string, Type>();

        protected Dictionary<string, Type> ControllerTypeCache
        {
            get
            {
                return _controllerTypeCache;
            }
        }

        /// <summary>
        /// 根据控制器名称及请求信息获得控制器类型。
        /// </summary>
        /// <param name="requestContext">请求信息</param>
        /// <param name="controllerName">控制器名称。</param>
        /// <returns>控制器类型。</returns>
        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            var tokens = requestContext.RouteData.DataTokens;
            string nameValue = string.Empty;
            if (tokens.ContainsKey("namespaces"))
            {
                var namespaces = tokens["namespaces"] as string[];
                if (namespaces != null && namespaces.Any())
                {
                    nameValue = namespaces[0] + ".";
                }
            }

            var fullName = nameValue + controllerName;

            //为了提高效率,缓存ControllerType.安全性考虑,添加lock.
            lock (Locker)
            {
                if (ControllerTypeCache.ContainsKey(fullName))
                {
                    return ControllerTypeCache[fullName];
                }
                else
                {
                    var controllerType = GetControllerType(fullName) ??
                                     base.GetControllerType(requestContext, controllerName);

                    ControllerTypeCache.Add(fullName, controllerType);
                    return controllerType;
                }
            }
        }

        /// <summary>
        /// 根据控制器名称获得控制器类型。
        /// </summary>
        /// <param name="fullControllerName">全称控制器名称。</param>
        /// <returns>控制器类型。</returns>
        private Type GetControllerType(string fullControllerName)
        {
            return PluginManager.GetPlugins().Select(plugin => plugin.GetControllerType(fullControllerName + "Controller")).FirstOrDefault(type => type != null);
        }
    }
}
