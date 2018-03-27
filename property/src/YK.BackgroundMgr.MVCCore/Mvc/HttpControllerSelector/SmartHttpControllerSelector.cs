/*----------------------------------------------------------------------------------------------
Copyright (c) Huawei.  All rights reserved.
Author:  zhoulingqiu
CreateDate:  2014-07-22
Description:  WebApi控制器选择器
----------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;

namespace YK.BackgroundMgr.MVCCore
{
    public class SmartHttpControllerSelector : IHttpControllerSelector
    {
        private sealed class HttpControllerTypeCache
        {
            // Fields
            private readonly Lazy<Dictionary<string, ILookup<string, Type>>> _cache;
            private readonly HttpConfiguration _configuration;

            // Methods
            public HttpControllerTypeCache(HttpConfiguration configuration)
            {
                if (configuration == null)
                {
                    throw new ArgumentNullException("configuration");
                }
                this._configuration = configuration;
                this._cache = new Lazy<Dictionary<string, ILookup<string, Type>>>(new Func<Dictionary<string, ILookup<string, Type>>>(this.InitializeCache));
            }

            public ICollection<Type> GetControllerTypes(string controllerName)
            {
                ILookup<string, Type> lookup;
                if (string.IsNullOrEmpty(controllerName))
                {
                    throw new ArgumentNullException("controllerName");
                }
                HashSet<Type> set = new HashSet<Type>();
                if (this._cache.Value.TryGetValue(controllerName, out lookup))
                {
                    foreach (IGrouping<string, Type> grouping in lookup)
                    {
                        set.UnionWith(grouping);
                    }
                }
                return set;
            }

            private Dictionary<string, ILookup<string, Type>> InitializeCache()
            {
                IAssembliesResolver assembliesResolver = this._configuration.Services.GetAssembliesResolver();
                return this._configuration.Services.GetHttpControllerTypeResolver().GetControllerTypes(assembliesResolver).GroupBy<Type, string>(t => t.Name.Substring(0, t.Name.Length - SmartHttpControllerSelector.ControllerSuffix.Length), StringComparer.OrdinalIgnoreCase).ToDictionary<IGrouping<string, Type>, string, ILookup<string, Type>>(g => g.Key, g => g.ToLookup<Type, string>(t => (t.Namespace ?? string.Empty), StringComparer.OrdinalIgnoreCase), StringComparer.OrdinalIgnoreCase);
            }

            // Properties
            public Dictionary<string, ILookup<string, Type>> Cache
            {
                get
                {
                    return this._cache.Value;
                }
            }
        }
        private readonly HttpConfiguration _configuration;
        private readonly Lazy<ConcurrentDictionary<string, HttpControllerDescriptor>> _controllerInfoCache;
        private readonly HttpControllerTypeCache _controllerTypeCache;
        private const string ControllerKey = "controller";
        public static readonly string ControllerSuffix = "Controller";
        public SmartHttpControllerSelector(HttpConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            this._controllerInfoCache = new Lazy<ConcurrentDictionary<string, HttpControllerDescriptor>>(new Func<ConcurrentDictionary<string, HttpControllerDescriptor>>(this.InitializeControllerInfoCache));
            this._configuration = configuration;
            this._controllerTypeCache = new HttpControllerTypeCache(this._configuration);
        }

        public virtual IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            return this._controllerInfoCache.Value.ToDictionary<KeyValuePair<string, HttpControllerDescriptor>, string, HttpControllerDescriptor>(c => c.Key, c => c.Value, StringComparer.OrdinalIgnoreCase);
        }

        private string GetControllerInfo(HttpRequestMessage request, string name)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            IHttpRouteData routeData = request.GetRouteData();
            object o = null;
            if (routeData == null)
            {
                return null;
            }
            routeData.Values.TryGetValue(name, out o);
            return o as string;
        }

        private ConcurrentDictionary<string, HttpControllerDescriptor> InitializeControllerInfoCache()
        {
            ConcurrentDictionary<string, HttpControllerDescriptor> dictionary = new ConcurrentDictionary<string, HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);
            HashSet<string> set = new HashSet<string>();
            foreach (KeyValuePair<string, ILookup<string, Type>> pair in this._controllerTypeCache.Cache)
            {
                string key = pair.Key;
                foreach (IGrouping<string, Type> grouping in pair.Value)
                {
                    foreach (Type type in grouping)
                    {
                        if (dictionary.Keys.Contains(key))
                        {
                            set.Add(key);
                            break;
                        }
                        dictionary.TryAdd(key, new HttpControllerDescriptor(this._configuration, key, type));
                    }
                }
            }
            foreach (string str2 in set)
            {
                HttpControllerDescriptor descriptor;
                dictionary.TryRemove(str2, out descriptor);
            }
            return dictionary;
        }

        public virtual HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            HttpControllerDescriptor descriptor;
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            string controllerName = GetControllerInfo(request, "controller");
            string controllerNameSpace = GetControllerInfo(request, "namespace");
            if (string.IsNullOrEmpty(controllerName))
            {
                throw new HttpResponseException(request.CreateErrorResponse(HttpStatusCode.NotFound, "ControllerNameNotFound"));
            }
            if (this._controllerInfoCache.Value.TryGetValue(controllerName, out descriptor))
            {
                return descriptor;
            }
            ICollection<Type> controllerTypes = this._controllerTypeCache.GetControllerTypes(controllerName);
            foreach (Type controllerType in controllerTypes)
            {
                if (string.Compare(controllerType.Namespace,controllerNameSpace,true)==0)
                {
                    return new HttpControllerDescriptor(_configuration, string.Format("{0}.{1}",controllerNameSpace,controllerName), controllerType);
                }
            }
            if (controllerTypes.Count == 0)
            {
                throw new HttpResponseException(request.CreateErrorResponse(HttpStatusCode.NotFound,"ControllerNotFound"));
            }
            throw new InvalidOperationException("ControllerNameAmbiguous WithRouteTemplate");
        }
    }
}
