using System.Web;
using System.Web.Mvc;

namespace YK.BackgroundMgr.MVCWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new FrameworkExceptionFilterAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}