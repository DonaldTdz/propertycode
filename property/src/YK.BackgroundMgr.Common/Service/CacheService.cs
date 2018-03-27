using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using YK.BackgroundMgr.PresentationService;

namespace YK.BackgroundMgr.Common
{   
    /// <summary>
    /// 缓存帮助类
    /// </summary>
    public class CacheService : ICacheService
    {
        /// <summary>
        /// .net缓存对象
        /// </summary>
        protected Cache Cache
        {
            get
            {
                return HttpContext.Current.Cache;
            }
        }

        /// <summary>
        /// 判断缓存是否存在
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存是否存在</returns>
        public bool IsContainsKey(string key)
        {
            return Cache.Get(key) != null;
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">获取的缓存对象类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存对象</returns>
        public T Get<T>(string key)
        {
            return (T)Cache.Get(key);
        }

        /// <summary>
        /// 设置缓存，滑动过期时间24h
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="data">缓存数据</param>
        public void Set(string key, object data)
        {
            Cache.Insert(key, data, null, Cache.NoAbsoluteExpiration, TimeSpan.FromDays(30)); // 设置缓存滑动过期时间为30天;
        }

        /// <summary>
        /// 设置缓存，滑动过期时间24h
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="data">缓存数据</param>
        /// <param name="timeSpan">缓存滑动过期时间</param>
        public void Set(string key, object data,TimeSpan timeSpan)
        {
            Cache.Insert(key, data, null, Cache.NoAbsoluteExpiration, timeSpan);
        }

        /// <summary>
        /// 设置缓存，滑动过期时间24h
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="data">缓存数据</param>
        /// <param name="dependency">缓存依赖项</param>
        public void Set(string key, object data, CacheDependency dependency)
        {
            Cache.Insert(key, data, dependency, Cache.NoAbsoluteExpiration, TimeSpan.FromDays(30));
        }

        /// <summary>
        /// 设置缓存，滑动过期时间24h
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="data">缓存数据</param>
        /// <param name="dependency">缓存依赖项</param>
        /// <param name="timeSpan">缓存滑动过期时间</param>
        public void Set(string key, object data, CacheDependency dependency, TimeSpan timeSpan)
        {
            Cache.Insert(key, data, dependency, Cache.NoAbsoluteExpiration, timeSpan);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        public void Remove(string key)
        {
            Cache.Remove(key);
        }
    }
}
