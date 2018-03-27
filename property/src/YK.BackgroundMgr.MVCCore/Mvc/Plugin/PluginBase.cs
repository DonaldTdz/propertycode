using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Mvc;
using System.Web.Routing;

namespace YK.BackgroundMgr.MVCCore
{
    /// <summary>
    /// 路由参数实体
    /// </summary>
    public class RouteEntity
    {
        private string _url;
        private string _nameSpace;
        private string _area;
        private string _controller;
        private string _action;

        /// <summary>
        ///  路由参数实体构造器
        /// </summary>
        /// <param name="area">区域名称</param>
        /// <param name="controller">控制器名称</param>
        /// <param name="action">方法名</param>
        /// <param name="url">路由URL配置加上区域</param>
        /// <param name="nameSpace">控制器的命名空间</param>
        public RouteEntity(string area, string controller, string action, string url, string nameSpace)
        {
            Area = area;
            Controller = controller;
            Action = action;
            Url = url;
            NameSpace = nameSpace;
        }

        /// <summary>
        /// 路由URL配置加上区域
        /// Eg:"Area/{controller}/{action}/{id}"
        /// </summary>
        public string Url
        {
            get { return _url; }
            private set
            {
                ArgumentValidator.Validate(value, "url", string.IsNullOrEmpty);
                _url = value;
            }
        }

        /// <summary>
        /// 控制器的命名空间
        /// </summary>
        public string NameSpace
        {
            get { return _nameSpace; }
            private set
            {
                ArgumentValidator.Validate(value, "namespace", string.IsNullOrEmpty);
                _nameSpace = value;
            }
        }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string Area
        {
            get { return _area; }
            private set
            {
                ArgumentValidator.Validate(value, "area", string.IsNullOrEmpty);
                _area = value;
            }
        }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string Controller
        {
            get { return _controller; }
            private set
            {
                ArgumentValidator.Validate(value, "controller", string.IsNullOrEmpty);
                _controller = value;
            }
        }

        /// <summary>
        /// 方法名
        /// </summary>
        public string Action
        {
            get { return _action; }
            private set
            {
                ArgumentValidator.Validate(value, "action", string.IsNullOrEmpty);
                _action = value;
            }
        }

    }

    /// <summary>
    /// 所有插件类的基类
    /// </summary>
    public abstract class PluginBase
    {
        /// <summary>
        /// 路由信息
        /// </summary>
        public abstract RouteEntity RouteInfo { get; }

        /// <summary>
        /// 插件的区域
        /// </summary>
        public string Area
        {
            get { return RouteInfo.Area; }
        }

        /// <summary>
        /// 插件的命名空间
        /// </summary>
        public string NameSpace
        {
            get { return RouteInfo.NameSpace; }
        }

        /// <summary>
        /// 插件所属的模块编码，统一从Common中ModuleCodeConst取
        /// </summary>
        public abstract string ModuleCode
        {
            get;
        }

        /// <summary>
        /// 插件开发者
        /// </summary>
        public abstract string Author
        {
            get;
        }

        /// <summary>
        /// 插件的描述
        /// </summary>
        public abstract string Description
        {
            get;
        }

        /// <summary>
        /// 插件的版本
        /// </summary>
        public abstract string Version { get; }

        /// <summary>
        /// 功能项列表
        /// </summary>
        public abstract IList<PluginFunction> Functions
        {
            get;
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        internal void Initialize()
        {
            LoadRoutes(RouteInfo);
            LoadApiRoutes(ApiRouteInfo);
        }

        /// <summary>
        /// 卸载。
        /// </summary>
        internal void Unload()
        {
            UnloadRoutes(RouteInfo);
        }

        /// <summary>
        /// 注册路由的帮助类
        /// </summary>
        /// <param name="routeEntity">路由实体</param>
        private void LoadRoutes(RouteEntity routeEntity)
        {
            var route = RouteTable.Routes.MapRoute(
                 name: routeEntity.Area,
                 url: routeEntity.Url,
                 namespaces: new string[] { routeEntity.NameSpace },
                 defaults: new
                 {
                     controller = routeEntity.Controller,
                     action = routeEntity.Action,
                     id = UrlParameter.Optional,
                     pluginName = routeEntity.Area
                 }
                 );
            route.DataTokens["area"] = routeEntity.Area;
            UnloadRoutes(routeEntity);
            RouteTable.Routes.Insert(0, route);
        }

        /// <summary>
        /// 卸载路由
        /// </summary>
        private void UnloadRoutes(RouteEntity routeEntity)
        {
            RouteTable.Routes.Remove(RouteTable.Routes[routeEntity.Area]);
        }


        public virtual ApiRouteEntity ApiRouteInfo { get { return null; } }


        /// <summary>
        /// 注册路由的帮助类
        /// </summary>
        /// <param name="routeEntity">路由实体</param>
        private void LoadApiRoutes(ApiRouteEntity routeEntity)
        {
            if (routeEntity == null) return;
            var dataTokes = new Dictionary<string, object>();
            dataTokes["namespace"] = routeEntity.NameSpace;
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                                  routeEntity.Name,
                                  routeEntity.RouteTemplate,
                                  new { id = RouteParameter.Optional },
                                  dataTokes);
        }
    }

    /// <summary>
    /// Api路由参数实体
    /// </summary>
    public class ApiRouteEntity
    {
        private string _name;
        private string _routeTemplate;
        private string _nameSpace;

        public ApiRouteEntity(string name, string routeTemplate, string nameSpace)
        {
            Name = name;
            RouteTemplate = routeTemplate;
            NameSpace = nameSpace;
        }

        /// <summary>
        /// API路由URL配置模板
        /// Eg:"api/{controller}/{id}"
        /// </summary>
        public string RouteTemplate
        {
            get { return _routeTemplate; }
            private set
            {
                ArgumentValidator.Validate(value, "routeTemplate", string.IsNullOrEmpty);
                _routeTemplate = value;
            }
        }

        /// <summary>
        /// 控制器的命名空间
        /// </summary>
        public string NameSpace
        {
            get { return _nameSpace; }
            private set
            {
                ArgumentValidator.Validate(value, "namespace", string.IsNullOrEmpty);
                _nameSpace = value;
            }
        }

        /// <summary>
        /// Api名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            private set
            {
                ArgumentValidator.Validate(value, "name", string.IsNullOrEmpty);
                _name = value;
            }
        }
    }

}
