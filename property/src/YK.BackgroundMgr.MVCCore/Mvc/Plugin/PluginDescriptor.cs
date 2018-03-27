﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace YK.BackgroundMgr.MVCCore
{
    /// <summary>
    /// 插件信息。
    /// </summary>
    internal class PluginDescriptor
    {
        /// <summary>
        /// 控制器类型字典。
        /// </summary>
        private readonly IDictionary<string, Type> _controllerTypes = new Dictionary<string, Type>();

        /// <summary>
        /// 构造器
        /// </summary>
        public PluginDescriptor(PluginBase plugin, Assembly assembly, IEnumerable<Type> types)
        {
            Plugin = plugin;
            Assembly = assembly;
            Types = types;
            _controllerTypes = new Dictionary<string, Type>();
            foreach (var type in types)
            {
                AddControllerType(type);
            }
        }

        /// <summary>
        /// 插件信息。
        /// </summary>
        public PluginBase Plugin { get; private set; }

        /// <summary>
        /// 程序集。
        /// </summary>
        public Assembly Assembly { get; private set; }

        /// <summary>
        /// 类型。
        /// </summary>
        public IEnumerable<Type> Types { get; private set; }

        /// <summary>
        /// 根据控制器类型名称获得控制器类型。
        /// </summary>
        /// <param name="fullColtrollerTypeName">控制器类型名称。</param>
        /// <returns>控制器类型。</returns>
        internal Type GetControllerType(string fullColtrollerTypeName)
        {
            if (_controllerTypes.ContainsKey(fullColtrollerTypeName))
            {
                return _controllerTypes[fullColtrollerTypeName];
            }

            return null;
        }

        /// <summary>
        /// 增加控制器类型。
        /// </summary>
        /// <param name="type">类型。</param>
        private void AddControllerType(Type type)
        {
            if (type.GetInterface(typeof(IController).Name) != null && type.Name.Contains("Controller") && type.IsClass && !type.IsAbstract)
            {
                _controllerTypes.Add(type.FullName, type);
            }
        }
    }
}
