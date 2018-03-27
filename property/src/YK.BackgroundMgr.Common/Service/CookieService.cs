using System;
using System.Web;
using YK.BackgroundMgr.PresentationService;

namespace YK.BackgroundMgr.Common
{
    /// <summary>
    /// Cookie帮助类
    /// </summary>
    public class CookieService : ICookieService
    {
        private HttpRequest CurrentRequest
        {
            get
            {
                return HttpContext.Current.Request;
            }
        }

        private HttpResponse CurrentResponse
        {
            get
            {
                return HttpContext.Current.Response;
            }
        }

        /// <summary>
        /// 判断是否存在指定的Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <returns>是否存在指定的Cookie</returns>
        public bool HasCookie(string cookieName)
        {
            return CurrentRequest.Cookies[cookieName] != null;
        }

        #region 获取Cookie

        /// <summary>
        /// 获得Cookie的值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <returns>Cookie值</returns>
        public string GetCookieValue(string cookieName)
        {
            var cookie = CurrentRequest.Cookies[cookieName];
            if (cookie != null)
            {
                return cookie.Value;
            }

            return string.Empty;
        }

        /// <summary>
        /// 获得Cookie的子键值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="key">子键名称</param>
        /// <returns>Cookie的子键值</returns>
        public string GetCookieValue(string cookieName, string key)
        {
            var cookie = CurrentRequest.Cookies[cookieName];
            if (cookie != null)
            {
                if (!string.IsNullOrEmpty(key) && cookie.HasKeys)
                {
                    return cookie.Values[key];
                }
            }

            return string.Empty;
        }

        #endregion

        #region 删除Cookie

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        public void RemoveCookie(string cookieName)
        {
            CurrentResponse.Cookies.Remove(cookieName);
        }

        /// <summary>
        /// 删除Cookie的子键
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="key">子键名称</param>
        public void RemoveCookie(string cookieName, string key)
        {
            HttpCookie cookie = CurrentResponse.Cookies[cookieName];
            if (cookie != null)
            {
                if (!string.IsNullOrEmpty(key) && cookie.HasKeys)
                {
                    cookie.Values.Remove(key);
                }
            }
        }

        #endregion

        #region 添加或修改Cookie

        /// <summary>
        /// 添加Cookie，默认30天过期
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="value">Cookie值</param>
        public void SetCookie(string cookieName, string value)
        {
            HttpCookie cookie = CurrentResponse.Cookies[cookieName];
            if (cookie == null)
            {
                AddCookie(new HttpCookie(cookieName, value));
                cookie.Expires = DateTime.Now.AddDays(30);
            }
            else
            {
                cookie.Value = value;
                CurrentResponse.SetCookie(cookie);
            }
        }

        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="value">Cookie值</param>
        /// <param name="expires">过期时间</param>
        public void SetCookie(string cookieName, string value, DateTime expires)
        {
            HttpCookie cookie = CurrentResponse.Cookies[cookieName];
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieName, value);
                cookie.Expires = expires;
                AddCookie(cookie);
            }
            else
            {
                cookie.Value = value;
                cookie.Expires = expires;
                CurrentResponse.SetCookie(cookie);
            }
        }

        /// <summary>
        /// 添加为Cookie.Values集合，默认30天过期
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="key">子键名称</param>
        /// <param name="value">子键值</param>
        public void SetCookie(string cookieName, string key, string value)
        {
            HttpCookie cookie = CurrentResponse.Cookies[cookieName];
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieName);
                cookie.Expires = DateTime.Now.AddDays(30);
                cookie.Values.Add(key, value);
                AddCookie(cookie);
            }
            else
            {
                cookie.Values.Set(key, value);
                CurrentResponse.SetCookie(cookie);
            }
        }

        /// <summary>
        /// 添加为Cookie.Values集合
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="key">子键名称</param>
        /// <param name="value">子键值</param>
        /// <param name="expires">过期时间</param>
        public void SetCookie(string cookieName, string key, string value, DateTime expires)
        {
            HttpCookie cookie = CurrentResponse.Cookies[cookieName];
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieName);
                cookie.Expires = expires;
                cookie.Values.Add(key, value);
                AddCookie(cookie);
            }
            else
            {
                cookie.Expires = expires;
                cookie.Values.Set(key, value);
                CurrentResponse.SetCookie(cookie);
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="cookie">Cookie</param>
        private void AddCookie(HttpCookie cookie)
        {
            HttpResponse response = HttpContext.Current.Response;
            if (response != null)
            {
                //指定客户端脚本是否可以访问[默认为false]
                cookie.HttpOnly = true;
                //指定统一的Path，比便能通存通取
                cookie.Path = "/";
                //设置跨域,这样在其它二级域名下就都可以访问到了
                //cookie.Domain = "chinesecoo.com";
                response.AppendCookie(cookie);
            }
        }

        #endregion
    }
}