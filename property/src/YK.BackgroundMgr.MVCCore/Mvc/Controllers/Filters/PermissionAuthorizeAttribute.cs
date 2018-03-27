using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace YK.BackgroundMgr.MVCCore
{
    /// <summary>
    /// 用于权限检查的Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class PermissionAuthorizeAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public PermissionAuthorizeAttribute(string permissionName)
        {
            PermissionName = permissionName;
        }

        protected string PermissionName { get; set; }

        //public void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    if (filterContext == null)
        //        throw new ArgumentNullException("filterContext");

        //    if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
        //        throw new InvalidOperationException("You cannot use [PermissionAuthorizeAttribute] attribute when a child action cache is active");

        //    SmartSession session = new SmartSession(filterContext.HttpContext);

        //    //没有登录和没有权限区别处理
        //    if (!filterContext.HttpContext.User.Identity.IsAuthenticated || session.UserInfo==null)
        //    {
        //        var dict = new RouteValueDictionary();
        //        dict["controller"] = "Home";
        //        dict["action"] = "ReLogin";
        //        dict["pageUrl"] = filterContext.HttpContext.Request.UrlReferrer;
        //        filterContext.Result = new RedirectToRouteResult(dict);
        //        //返回到登陆页面
        //    }
        //    else if (!HasPermissionAccess(filterContext))
        //    {
        //        //重定向到拒绝访问页面
        //        var dict = new RouteValueDictionary();
        //        dict["controller"] = "Common";
        //        dict["action"] = "Denied";
        //        dict["pageUrl"] = filterContext.HttpContext.Request.UrlReferrer;//便于返回上个页面
        //        filterContext.Result = new RedirectToRouteResult(dict);
        //    }
        //}

        //public virtual bool HasPermissionAccess(AuthorizationContext filterContext)
        //{
        //    var tokens = filterContext.RouteData.DataTokens;
        //    string namespaceName = string.Empty;
        //    if (tokens.ContainsKey("namespaces"))
        //    {
        //        var namespaces = tokens["namespaces"] as string[];
        //        if (namespaces != null && namespaces.Any())
        //        {
        //            namespaceName = namespaces[0];
        //        }
        //    }

        //    var sessionUser = new SmartSession(filterContext.HttpContext).UserInfo;
        //    if (sessionUser != null && sessionUser.UserRoleList != null)
        //    {
        //        bool bAdmin = sessionUser.CommRoleList.Select(t => t.RoleName).Contains(CommonDefine.AdminRoleName);
        //        //Admin管理员跳过权限认证
        //        if (sessionUser.CommRoleList != null && bAdmin)
        //        {
        //            return true;
        //        }

        //        foreach (var role in sessionUser.UserRoleList)
        //        {
        //            if (role.RolePermissionList == null) continue;
        //            if (role.RolePermissionList.Any(permission => permission.Namespace + "." + permission.PermissionName == namespaceName+"."+PermissionName))
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            throw new NotImplementedException();
        }
    }
}