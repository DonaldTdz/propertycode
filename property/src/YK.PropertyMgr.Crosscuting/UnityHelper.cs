using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Net;

namespace YK.PropertyMgr.Crosscuting
{
    public class UnityHelper
    {
        private static volatile Lazy<IUnityContainer> m_Container = new Lazy<IUnityContainer>(
            () => LoadConfig(new UnityContainer()), LazyThreadSafetyMode.ExecutionAndPublication);
        private const string ConfigRelativePath = @"PropertyMgrUnityConfig.xml";

        public static IUnityContainer UnityContainerInstance
        {
            get
            {
                return m_Container.Value;
            }
        }

        private static IUnityContainer LoadConfig(IUnityContainer container)
        {
            string filePath = "";
            if (System.Environment.CurrentDirectory.TrimEnd(new char[] { '\\' }) == AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[] { '\\' }))
            {
                filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigRelativePath);
            }
            else
            {
                filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", ConfigRelativePath);
            }

            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = filePath };
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            var unitySection = (UnityConfigurationSection)configuration.GetSection("unity");

            container.LoadConfiguration(unitySection);

            return container;
        }
    }
}
