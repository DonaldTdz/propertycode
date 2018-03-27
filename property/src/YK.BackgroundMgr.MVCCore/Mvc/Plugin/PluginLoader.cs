using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;

namespace YK.BackgroundMgr.MVCCore
{
    /// <summary>
    /// 插件加载器。
    /// </summary>
    internal static class PluginLoader
    {
        /// <summary>
        /// 插件目录。
        /// </summary>
        private static readonly DirectoryInfo PluginFolder;

        /// <summary>
        /// 临时程序域，加载临时目录所有程序集
        /// </summary>
        private static AppDomain _pluginDomain;

        /// <summary>
        /// 插件临时目录。
        /// </summary>
        private static readonly DirectoryInfo TempPluginFolder;

        /// <summary>
        /// Bin目录下的DLL
        /// </summary>
        private static readonly List<string> BinDllList = new List<string>();

        /// <summary>
        /// 初始化。
        /// </summary>
        static PluginLoader()
        {

            //PluginFolder = new DirectoryInfo(HostingEnvironment.MapPath("~/Plugins"));
            PluginFolder = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Plugins"));
            //TempPluginFolder = new DirectoryInfo(HostingEnvironment.MapPath("~/App_Data/Dependencies"));
            TempPluginFolder = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,@"App_Data\Dependencies"));
            if (TempPluginFolder.Exists)
            {
                TempPluginFolder.Delete(true);
            }
            TempPluginFolder.Create();

            //var binFolder = new DirectoryInfo(HostingEnvironment.MapPath("~/bin"));
            var binFolder = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"bin"));
            binFolder.GetFiles("*.dll", SearchOption.TopDirectoryOnly).ToList().ForEach(file => BinDllList.Add(file.Name));
        }

        /// <summary>
        /// 加载插件。
        /// </summary>
        internal static IEnumerable<PluginDescriptor> Load()
        {
            return GetPluginAssemblies(CopyAndLoadAssembly(PluginFolder));
        }

        /// <summary>
        /// 获得插件信息。
        /// </summary>
        /// <param name="pluginType"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static PluginDescriptor GetPluginInstance(Type pluginType, Assembly assembly)
        {
            if (pluginType != null)
            {
                var plugin = (PluginBase)Activator.CreateInstance(pluginType);
                return new PluginDescriptor(plugin, assembly, assembly.GetTypes());
            }
            return null;
        }

        /// <summary>
        /// 将指定上传的插件目录程序集复制到临时目录
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private static List<Assembly> CopyAndLoadAssembly(DirectoryInfo directory)
        {
            var assemblies = new List<Assembly>();
            if (directory != null && directory.Exists)
            {
                _pluginDomain = AppDomain.CreateDomain("pluginDomain");
                //复制插件进临时文件夹。
                foreach (var file in directory.GetFiles("*.dll", SearchOption.AllDirectories))
                {
                    int index = file.FullName.IndexOf("\\bin\\");
                    if (index <= -1)
                    {
                        continue;
                    }

                    // 过滤重复的DLL
                    var fileName = file.FullName.Substring(index + 5);
                    if (BinDllList.Contains(file.Name))
                    {
                        continue;
                    }

                    var fullPath = Path.Combine(TempPluginFolder.FullName, fileName);
                    var directionPath = Path.GetDirectoryName(fullPath);
                    if (!Directory.Exists(directionPath))
                    {
                        Directory.CreateDirectory(directionPath);
                    }
                    file.CopyTo(fullPath, true);
                    _pluginDomain.Load(File.ReadAllBytes(fullPath));
                }

                assemblies.AddRange(_pluginDomain.GetAssemblies());
                if (_pluginDomain != null)
                {
                    AppDomain.Unload(_pluginDomain);
                }
            }
            return assemblies;
        }

        /// <summary>
        /// 根据程序集列表获得该列表下的所有插件信息。
        /// </summary>
        /// <param name="assemblies">程序集列表</param>
        /// <returns>插件信息集合。</returns>
        private static IEnumerable<PluginDescriptor> GetPluginAssemblies(IEnumerable<Assembly> assemblies)
        {
            IList<PluginDescriptor> plugins = new List<PluginDescriptor>();
            foreach (var assembly in assemblies)
            {
                var pluginTypes = assembly.GetTypes().Where(type => { return typeof(PluginBase).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract; });

                foreach (var plugin in pluginTypes.Select(pluginType => GetPluginInstance(pluginType, assembly)).Where(plugin => plugin != null))
                {
                    plugins.Add(plugin);
                }
            }
            return plugins;
        }
    }
}
