using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using YK.BackgroundMgr.PresentationService;
using System.Threading.Tasks;
using YK.BackgroundMgr.Crosscuting.Log;
using System.Configuration;
using YK.FrameworkLog.LogEntity;
using YK.FrameworkLog.PluginHelper;

namespace YK.BackgroundMgr.MVCWeb
{
    public class FrameworkExceptionFilterAttribute : System.Web.Mvc.IExceptionFilter
    {
        public void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled && filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            if (new HttpException(null, filterContext.Exception).GetHttpCode() != 500)
            {
                return;
            }
            
            //写入日志到统一日志系统
            ErrorLog errorLog = new ErrorLog()
            {
                CreateTime = DateTime.Now,
                EntityKey = Guid.NewGuid().ToString(),
                ErrorLevel = EErrorLevel.严重异常,
                ExceptionMsg = filterContext.Exception.StackTrace,
                LogFrom = (ELogFrom)(int.Parse(ConfigurationManager.AppSettings["SysFromType"])),
                Message = filterContext.RequestContext.HttpContext.Request.RawUrl + ":" + filterContext.Exception.Message,
                IP = IPHelper.GetClientIp(),
            };
            //Task.Run(() =>
            //{
            //    PresentationServiceHelper.LookUp<ILogService>().WriteErrorToCache(errorLog);
            //    //FrameworkLog.PluginHelper.PluginHelper.WriteErrorToCache(errorLog);
            //    LogHelper.Error(new Crosscuting.Log.LogModel(filterContext.Exception.Message), filterContext.Exception);
            //});
            try {
                PluginHelper.WriteErrorToCache(errorLog);
            }
            catch (Exception) { }
            //FrameworkLog.PluginHelper.PluginHelper.WriteErrorToCache(errorLog);
            Crosscuting.Log.LogHelper.Error(new Crosscuting.Log.LogModel(filterContext.Exception.Message), filterContext.Exception);


            var sessionService = PresentationServiceHelper.LookUp<ISessionService>();
            sessionService.RemoveSession("FrameworkError");
            sessionService.SetSession("FrameworkError", filterContext.Exception.Message);

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;

            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //写入日志到统一日志系统
            ErrorLog errorLog = new ErrorLog()
            {
                CreateTime = DateTime.Now,
                EntityKey = Guid.NewGuid().ToString(),
                ErrorLevel = EErrorLevel.严重异常,
                ExceptionMsg = actionExecutedContext.Exception.StackTrace,
                LogFrom = (ELogFrom)(int.Parse(ConfigurationManager.AppSettings["SysFromType"])),
                Message = actionExecutedContext.Request.RequestUri.AbsolutePath + ":" + actionExecutedContext.Exception.Message,
                IP = IPHelper.GetClientIp(),
            };
            PluginHelper.WriteErrorToCache(errorLog);
            Crosscuting.Log.LogHelper.Error(new Crosscuting.Log.LogModel() { LogCategory = "WebApi", LogTime = DateTime.Now.ToString(), LogType = "Error", Message = actionExecutedContext.Exception.Message, UserId = "System" }, actionExecutedContext.Exception);

            base.OnException(actionExecutedContext);
        }
    }
}