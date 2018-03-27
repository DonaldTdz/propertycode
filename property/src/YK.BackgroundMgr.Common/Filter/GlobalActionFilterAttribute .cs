using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using YK.BackgroundMgr.PresentationService;

namespace YK.BackgroundMgr.Common
{
    /// <summary>
    /// 自定义全局的ActionFilter，用于处理Session过期
    /// </summary>
    public class GlobalActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllserName = ((filterContext.ActionDescriptor).ControllerDescriptor).ControllerName;
            string actionName = (filterContext.ActionDescriptor).ActionName;
            if (controllserName != "Login") // 登录界面不做Session过期判断
            {
                if (PresentationServiceHelper.LookUp<ISessionService>().IsSessionExpire) // 判断Session是否已经过期
                {
                    if (controllserName == "Home" && actionName == "Index") // 如果直接刷新首页，则不需要弹出框
                    {
                        RedirectResult redirectResult = new RedirectResult(ConfigurationManager.AppSettings["BasicUrl"]);
                        filterContext.Result = redirectResult;
                    }
                    else
                    {
                        filterContext.Result = new HttpUnauthorizedResult();
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
