using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YK.BackgroundMgr.ApplicationService;
using YK.BackgroundMgr.PresentationService;
using YK.FrameworkLog.LogEntity;

namespace YK.BackgroundMgr.MVCWeb.Controllers
{
    public class LoginController : BaseController
    {
        public ActionResult Logout()
        {
            var sessionService = PresentationServiceHelper.LookUp<ISessionService>();
            sessionService.ClearSession();
            var url = ConfigurationManager.AppSettings["BasicUrl"] + "Login";
            return new RedirectResult(url);
        }

        public ActionResult Login(string UserName, string Password)
        {
            SEC_AdminUserAppService adminUserService = new SEC_AdminUserAppService();
            var adminUserInfo = adminUserService.ValidateSEC_AdminUser(UserName, Password);
            if(adminUserInfo == null)
            {
                return RedirectToAction("PageNotFind", "Error");
            }
            else
            {
                var sessionService = PresentationServiceHelper.LookUp<ISessionService>();
                sessionService.SetSession(AdminUserInfoKey, adminUserInfo);
                var cookieService = PresentationServiceHelper.LookUp<ICookieService>();
                string Domainstr = ConfigurationManager.AppSettings["CookieDomain"];
                string isUseDomain = ConfigurationManager.AppSettings["IsUseDomain"].ToString();
                if (isUseDomain.ToUpper() == "TRUE")
                {
                    DateTime overdueDate;
                    string value = CurrentAdminUser.Id + "|" + CurrentAdminUser.UserName + "|" + CurrentAdminUser.Password;
                    overdueDate = DateTime.Now.AddMinutes(30);
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                1,
                                CurrentAdminUser.UserName,
                                DateTime.Now,
                                overdueDate,
                                false,
                                value
                                );
                    string hashTicket = FormsAuthentication.Encrypt(ticket);




                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashTicket)
                    {
                        Domain = Domainstr

                    };
                    HttpResponse response = System.Web.HttpContext.Current.Response;
                    cookie.HttpOnly = false;
                    response.AppendCookie(cookie);
                }
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
