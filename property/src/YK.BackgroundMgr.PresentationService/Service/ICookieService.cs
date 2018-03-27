using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.PresentationService
{
    public interface ICookieService : IPresentationService
    {
        /// <summary>
        /// 判断是否存在指定的Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <returns>是否存在指定的Cookie</returns>
        bool HasCookie(string cookieName);

        #region 获取Cookie

        /// <summary>
        /// 获得Cookie的值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <returns>Cookie值</returns>
        string GetCookieValue(string cookieName);

        /// <summary>
        /// 获得Cookie的子键值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="key">子键名称</param>
        /// <returns>Cookie的子键值</returns>
        string GetCookieValue(string cookieName, string key);

        #endregion

        #region 删除Cookie

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        void RemoveCookie(string cookieName);

        /// <summary>
        /// 删除Cookie的子键
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="key">子键名称</param>
        void RemoveCookie(string cookieName, string key);

        #endregion

        #region 添加或修改Cookie

        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="value">Cookie值</param>
        void SetCookie(string cookieName, string value);

        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="value">Cookie值</param>
        /// <param name="expires">过期时间</param>
        void SetCookie(string cookieName, string value, DateTime expires);

        /// <summary>
        /// 添加为Cookie.Values集合
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="key">子键名称</param>
        /// <param name="value">子键值</param>
        void SetCookie(string cookieName, string key, string value);

        /// <summary>
        /// 添加为Cookie.Values集合
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="key">子键名称</param>
        /// <param name="value">子键值</param>
        /// <param name="expires">过期时间</param>
        void SetCookie(string cookieName, string key, string value, DateTime expires);

        #endregion
    }
}
