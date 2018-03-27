using System;
using System.Globalization;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;

namespace YK.BackgroundMgr.MVCCore
{
    /// <summary>
    /// 处理控制器Action异常的Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ExceptionFilterAttribute : ActionFilterAttribute, IExceptionFilter
    {
        ///// <summary>
        ///// 默认的异常消息
        ///// </summary>
        //private string DeafaultMessage = CultureInfo.CurrentCulture.Name == CommonDefine.ZhCn ? "发生了一个严重的错误,请与管理员联系" : "A serious error has occurred, please contact administrator.";

        ////private static readonly object SyncObject = new object();

        //private string _message;

        //public ExceptionFilterAttribute()
        //    : this(typeof(Exception))
        //{
        //}

        ///// <summary>
        ///// 构造器
        ///// </summary>
        ///// <param name="exceptionType">异常类型</param>
        //public ExceptionFilterAttribute(Type exceptionType)
        //    : this(exceptionType, null)
        //{
        //}

        ///// <summary>
        ///// 构造器
        ///// </summary>
        ///// <param name="exceptionType">异常类型</param>
        ///// <param name="message">异常消息</param>
        //public ExceptionFilterAttribute(Type exceptionType, string message)
        //{
        //    ExceptionType = exceptionType;
        //    Message = message;
        //}

        ///// <summary>
        ///// 发生异常时的消息
        ///// </summary>
        //public string Message
        //{
        //    get
        //    {
        //        return string.IsNullOrEmpty(_message) ? DeafaultMessage : _message;
        //    }
        //    private set { _message = value; }
        //}

        ///// <summary>
        ///// 异常类型
        ///// </summary>
        //public Type ExceptionType { get; private set; }

        ///// <summary>
        ///// 处理异常
        ///// </summary>
        ///// <param name="filterContext">发生异常时的上下文</param>
        //public void OnException(ExceptionContext filterContext)
        //{
        //    SmartSession session = new SmartSession(filterContext.HttpContext);
        //    if ((!filterContext.HttpContext.User.Identity.IsAuthenticated || session.UserInfo == null) && !filterContext.ExceptionHandled)
        //    {
        //        WriteErrorLog(filterContext, session);
        //        var dict = new RouteValueDictionary();
        //        dict["controller"] = "Home";
        //        dict["action"] = "ReLogin";
        //        dict["pageUrl"] = filterContext.HttpContext.Request.UrlReferrer;
        //        filterContext.Result = new RedirectToRouteResult(dict);
        //        filterContext.ExceptionHandled = true;
        //        //返回到登陆页面
        //    }
        //    if ((filterContext.Exception.GetType() == ExceptionType ||
        //         ExceptionType.IsAssignableFrom(filterContext.Exception.GetType()) && !filterContext.ExceptionHandled))
        //    {
        //        WriteErrorLog(filterContext, session);
        //        //重定向到错误页面
        //        var dict = new RouteValueDictionary();
        //        dict["controller"] = "Common";
        //        dict["action"] = "Error";
        //        dict["message"] = Message;
        //        dict["pageUrl"] = filterContext.HttpContext.Request.UrlReferrer;//便于返回上个页面
        //        filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
        //        filterContext.Result = new RedirectToRouteResult(dict);
                
        //        filterContext.ExceptionHandled = true;//标记为异常已经处理
        //    }
        //}

        ///// <summary>
        ///// 写系统错误日志
        ///// </summary>
        ///// <param name="filterContext"></param>
        ///// <param name="session"></param>
        //private void WriteErrorLog(ExceptionContext filterContext, SmartSession session)
        //{
        //    LogHelper.Error(new LogModel()
        //        {
        //            UserId = session.UserInfo == null ? "NullUserInfo" : session.UserInfo.UserId,
        //            LogTime = DateTime.Now,
        //            Action = filterContext.RouteData.Values["action"].ToString(),
        //            Controller = filterContext.RouteData.Values["controller"].ToString(),
        //            Message = filterContext.Exception.StackTrace,
        //            LogType = "error"
        //        }, filterContext.Exception);
        //}

        public void OnException(ExceptionContext filterContext)
        {
            throw new NotImplementedException();
        }
    }
}
