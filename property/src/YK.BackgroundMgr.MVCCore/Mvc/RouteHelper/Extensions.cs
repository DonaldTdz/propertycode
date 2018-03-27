using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace YK.BackgroundMgr.MVCCore
{
    /// <summary>
    /// 扩展类
    /// </summary>
    public static class Extensions
    {
        public static string PluginAction(this UrlHelper urlHelper,string area, string controller,string action,RouteValueDictionary routeValues)
        {
            if (routeValues==null)
            {
                routeValues=new RouteValueDictionary();
            }
            routeValues["area"] = area;
            return urlHelper.Action(action, controller, routeValues);
        }

        public static string PluginAction(this UrlHelper urlHelper, string area, string controller, string action)
        {
            return urlHelper.PluginAction(area, controller, action,null);
        }

        public static string PluginAction(this UrlHelper urlHelper,string controller,string action ,RouteValueDictionary routeValues)
        {
            return urlHelper.PluginAction(string.Empty, controller, action, routeValues);
        }

        public static string PluginAction(this UrlHelper urlHelper, string controller, string action)
        {
            return urlHelper.PluginAction(controller,action,new RouteValueDictionary());
        }
      
    }
}