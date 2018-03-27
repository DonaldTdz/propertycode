using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace YK.BackgroundMgr.MVCCore
{
    /// <summary>
    /// 服务提供者
    /// </summary>
    public static class ServiceProvider
    {
        //private static IContainer _container;
        ///// <summary>
        ///// 默认服务DLL
        ///// </summary>
        //private const string SERVICE_ASSEMBLY_NAME = "HW.Smart.Framework.ServiceImplement.dll";

        //internal static void Load()
        //{
        //    //添加IOC容器
        //    var builder = new ContainerBuilder();

        //    //注册控制器
        //    builder.RegisterControllers(AppDomain.CurrentDomain.GetAssemblies());

        //    //注册服务
        //    RegisterSerivces(builder, Assembly.UnsafeLoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin\\") + SERVICE_ASSEMBLY_NAME));

        //    _container = builder.Build();

        //    //注册解析器
        //    DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));
        //}

        //public static T Resolver<T>()
        //{
        //    return _container.Resolve<T>();
        //}

        //private static void RegisterSerivces(ContainerBuilder builder, Assembly serviceAssembly)
        //{
        //    var types = TypeFinder.Find<ServiceBase>(serviceAssembly).Where(t => t.Name.EndsWith("Service"));
        //    foreach (var type in types)
        //    {
        //        if (type.IsGenericType)
        //        {
        //            var interfaceTypes = type.GetInterfaces().Where(t => t.Name.StartsWith("I") && t.Name.EndsWith("Service") && t.IsGenericType).ToArray();
        //            builder.RegisterGeneric(type).As(interfaceTypes);
        //        }
        //        else
        //        {
        //            var interfaceTypes = type.GetInterfaces().Where(t => t.Name.StartsWith("I") && t.Name.EndsWith("Service")).ToArray();
        //            builder.RegisterType(type).As(interfaceTypes);
        //        }
        //    }
        //}
    }
}
