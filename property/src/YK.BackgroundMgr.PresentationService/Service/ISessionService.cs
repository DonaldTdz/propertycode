using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.PresentationService
{
    public interface ISessionService : IPresentationService
    {
        /// <summary>
        /// 清空Session信息
        /// </summary>
        void ClearSession();

        /// <summary>
        /// 判断Session是否过期
        /// </summary>
        bool IsSessionExpire { get; }

        /// <summary>
        /// 判断是否存储指定的Session
        /// </summary>
        /// <param name="name">Session名称</param>
        /// <returns>是否存储指定的Session</returns>
        bool HasSession(string name);

        /// <summary>
        /// 设置Session的过期时间
        /// </summary>
        /// <param name="iExpireTime">过期时间，分钟计算</param>
        void SetSessionExpireTime(int iExpireTime);

        /// <summary>
        /// 根据session名获取session对象
        /// </summary>
        /// <typeparam name="T">获取的Session对象类型</typeparam>
        /// <param name="name">Session名称</param>
        /// <returns>session对象</returns>
        T GetSession<T>(string name);

        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="name">session 名</param>
        /// <param name="value">session 值</param>
        void SetSession(string name, object value);

        /// <summary>
        /// 移除Session
        /// </summary>
        /// <param name="name">Session名称</param>
        void RemoveSession(string name);
    }
}
