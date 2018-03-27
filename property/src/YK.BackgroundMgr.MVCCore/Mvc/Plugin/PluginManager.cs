using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace YK.BackgroundMgr.MVCCore
{
    /// <summary>
    /// 插件管理器。
    /// </summary>
    internal static class PluginManager
    {
        /// <summary>
        /// 插件字典。
        /// </summary>
        private readonly static IDictionary<string, PluginDescriptor> _plugins = new Dictionary<string, PluginDescriptor>();

        /// <summary>
        /// 初始化。
        /// </summary>
        internal static void Initialize()
        {
            Initialize(PluginLoader.Load());
        }

        /// <summary>
        ///  初始化。
        /// </summary>
        /// <param name="pluginAppend">追加的插件</param>
        private static void Initialize(IEnumerable<PluginDescriptor> pluginAppend)
        {
            //遍历所有插件描述。
            foreach (var plugin in pluginAppend) //循环插件文件夹中的插件
            {
                //卸载插件。
                Unload(plugin);
                //初始化插件。
                Initialize(plugin);
            }
        }

        /// <summary>
        /// 初始化插件。
        /// </summary>
        /// <param name="pluginDescriptor">插件描述</param>
        private static void Initialize(PluginDescriptor pluginDescriptor)
        {
            //使用插件名称做为字典 KEY。
            string key = pluginDescriptor.Plugin.RouteInfo.Area;

            //不存在时才进行初始化。
            if (!_plugins.ContainsKey(key))
            {
                //初始化。
                pluginDescriptor.Plugin.Initialize();

                //增加到字典。
                _plugins.Add(key, pluginDescriptor);
            }
        }

        /// <summary>
        /// 卸载。
        /// </summary>
        internal static void Unload(PluginDescriptor pluginDescriptor)
        {
            pluginDescriptor.Plugin.Unload();
        }

        /// <summary>
        /// 获得当前系统所有插件描述。
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<PluginDescriptor> GetPlugins()
        {
            return _plugins.Select(m => m.Value).ToList();
        }

        /// <summary>
        /// 根据插件名称获得插件描述。
        /// </summary>
        /// <param name="areaName">区域名称。</param>
        /// <returns>插件描述。</returns>
        internal static PluginDescriptor GetPlugin(string areaName)
        {
            return GetPlugins().SingleOrDefault(plugin => plugin.Plugin.RouteInfo.Area == areaName);
        }
    }
}
