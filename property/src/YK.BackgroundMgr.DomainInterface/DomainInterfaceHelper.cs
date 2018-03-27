using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.DomainInterface
{
    /// <summary>
    /// Presentation层提供的服务帮助类
    /// </summary>
    public static class DomainInterfaceHelper
    {
        private static readonly Dictionary<Type, object> _services;

        static DomainInterfaceHelper()
        {
            _services = new Dictionary<Type, object>();
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <typeparam name="TServeice">服务的类型接口约束</typeparam>
        /// <param name="serveice">具体服务实现</param>
        public static void Register<TServeice>(TServeice serveice) where TServeice : IDomainInterface
        {
            if (serveice == null)
            {
                return;
            }
            var serverType = typeof(TServeice);
            if (!_services.ContainsKey(serverType))
            {
                _services.Add(serverType, serveice);
            }
        }

        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <typeparam name="TServeice">服务的类型接口约束</typeparam>
        public static void UnRegister<TServeice>() where TServeice : IDomainInterface
        {
            _services.Remove(typeof(TServeice));
        }

        /// <summary>
        /// 获取框架提供的服务
        /// </summary>
        /// <typeparam name="TServeice">服务的类型接口约束</typeparam>
        /// <returns>获取服务接口</returns>
        public static TServeice LookUp<TServeice>() where TServeice : IDomainInterface
        {
            object serveice;
            return _services.TryGetValue(typeof(TServeice), out serveice)
                       ? (TServeice)serveice
                       : default(TServeice);
        }
    }
}
