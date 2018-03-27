using System.Web;
using System;
using System.Collections.Generic;
using System.Text;
using YK.BackgroundMgr.PresentationService;

namespace YK.BackgroundMgr.Common
{
    public class SessionService : ISessionService
    {
        private const string SessionMustHasName = "AdminUserInfo"; // 登录系统，必须设置用户信息的Session

        /// <summary>
        /// 判断Session是否过期
        /// </summary>
        public bool IsSessionExpire
        {
            get
            {
                return HttpContext.Current.Session[SessionMustHasName] == null;
            }
        }

        /// <summary>
        /// 判断是否存储指定的Session
        /// </summary>
        /// <param name="name">Session名称</param>
        /// <returns>是否存储指定的Session</returns>
        public bool HasSession(string name)
        {
            return HttpContext.Current.Session[name] != null;
        }

        /// <summary>
        /// 设置Session的过期时间
        /// </summary>
        /// <param name="iExpireTime">过期时间，分钟计算</param>
        public void SetSessionExpireTime(int iExpireTime)
        {
            HttpContext.Current.Session.Timeout = iExpireTime;
        }

        /// <summary>
        /// 根据session名获取session对象
        /// </summary>
        /// <typeparam name="T">获取的Session对象类型</typeparam>
        /// <param name="name">Session名称</param>
        /// <returns>session对象</returns>
        public T GetSession<T>(string name)
        {
            return (T)HttpContext.Current.Session[name];
        }

        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="name">session 名</param>
        /// <param name="value">session 值</param>
        public void SetSession(string name, object value)
        {
            HttpContext.Current.Session.Add(name, value);
        }

        /// <summary>
        /// 移除Session
        /// </summary>
        /// <param name="name">Session名称</param>
        public void RemoveSession(string name)
        {
            HttpContext.Current.Session.Remove(name);
        }

        /// <summary>
        /// 清空Session信息
        /// </summary>
        public void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}
