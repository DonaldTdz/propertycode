using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Xml;
using YK.BackgroundMgr.ApplicationService;
using YK.BackgroundMgr.PresentationService;

namespace YK.BackgroundMgr.Common
{
    public class TemplateService : ITemplateService
    {
        private static object ObjTemplateLock = new object();

        /// <summary>
        /// 读取TemplateModel信息
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="templateName">Template名称</param>
        /// <param name="isSaveToCache">是否存储缓存</param>
        /// <returns>TemplateModel信息</returns>
        public IEnumerable<TemplateModel> GetTemplateModels(string fileName, string templateName, bool isSaveToCache = true)
        {
            Sys_DictionaryAppService dictionaryItemService = new Sys_DictionaryAppService();
            var templateModels = ReadTemplate(fileName, templateName, isSaveToCache);
            templateModels.ForEach(r =>
            {
                if (r.DictId != "-1")
                {
                    r.DictionaryModels = dictionaryItemService.GetDictionaryModels(r.DictId);
                }
            });

            return templateModels.OrderBy(r=>r.Seq);
        }

        /// <summary>
        /// 读取TemplateModel信息
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="templateName">Template名称</param>
        /// <param name="isSaveToCache">是否存储缓存</param>
        /// <returns>TemplateModel信息</returns>
        private List<TemplateModel> ReadTemplate(string fileName, string templateName, bool isSaveToCache)
        {
            if (!isSaveToCache)
            {
                List<TemplateModel> lstTemplateModels = new List<TemplateModel>();

                //创建并初始化一个xml对象
                XmlDocument xml = new XmlDocument();
                //加载xml文件
                xml.Load(HttpContext.Current.Server.MapPath("~/Models/" + fileName));
                XmlNode templateNode = xml.SelectSingleNode("/FrameworkTemplates/FrameworkTemplate[@Name='" + templateName + "']");

                foreach (XmlNode itemNode in templateNode.ChildNodes)
                {
                    if (itemNode.LocalName != "TemplateItem")
                    {
                        continue;
                    }
                    TemplateModel tempTemplateModel = new TemplateModel();
                    var templateType = tempTemplateModel.GetType();
                    foreach (XmlNode itemDetailNode in itemNode.ChildNodes)
                    {
                        var itemProperty = templateType.GetProperty(itemDetailNode.LocalName);
                        if (itemProperty == null)
                        {
                            continue;
                        }
                        var itemPropertyType = itemProperty.PropertyType;
                        var propertyVal = Convert.ChangeType(itemDetailNode.InnerText, itemPropertyType);
                        templateType.GetProperty(itemDetailNode.LocalName).SetValue(tempTemplateModel, propertyVal);
                    }

                    lstTemplateModels.Add(tempTemplateModel);
                }

                return lstTemplateModels;
            }
            else
            {
                var cacheService = PresentationServiceHelper.LookUp<ICacheService>();
                if (cacheService.IsContainsKey(fileName)) // 如果缓存有数据，直接从缓存获取数据
                {
                    return cacheService.Get<Dictionary<string, List<TemplateModel>>>(fileName)[templateName];
                }
                lock (ObjTemplateLock)
                {
                    if (cacheService.IsContainsKey(fileName)) // 再次判断缓存是否有数据
                    {
                        return cacheService.Get<Dictionary<string, List<TemplateModel>>>(fileName)[templateName];
                    }
                    var templates = GetAllTemplate(fileName);

                    return templates[templateName];
                }
            }
        }

        private Dictionary<string, List<TemplateModel>> GetAllTemplate(string fileName)
        {
            string fileFullName = HttpContext.Current.Server.MapPath("~/Models/" + fileName);
            Dictionary<string, List<TemplateModel>> dictTemplateModels = new Dictionary<string, List<TemplateModel>>();

            XmlDocument xml = new XmlDocument();
            xml.Load(fileFullName);
            var templateNodes = xml.SelectNodes("/FrameworkTemplates/FrameworkTemplate");
            foreach(XmlNode templateNode in templateNodes)
            {
                var templateName = templateNode.Attributes["Name"].Value;
                List<TemplateModel> lstTemplateModels = new List<TemplateModel>();
                foreach (XmlNode itemNode in templateNode.ChildNodes)
                {
                    if(itemNode.LocalName != "TemplateItem")
                    {
                        continue;
                    }
                    TemplateModel tempTemplateModel = new TemplateModel();
                    var templateType = tempTemplateModel.GetType();
                    foreach (XmlNode itemDetailNode in itemNode.ChildNodes)
                    {
                        var itemProperty = templateType.GetProperty(itemDetailNode.LocalName);
                        if (itemProperty == null)
                        {
                            continue;
                        }
                        var itemPropertyType = itemProperty.PropertyType;
                        var propertyVal = Convert.ChangeType(itemDetailNode.InnerText, itemPropertyType);
                        templateType.GetProperty(itemDetailNode.LocalName).SetValue(tempTemplateModel, propertyVal);
                    }
                    lstTemplateModels.Add(tempTemplateModel);
                }

                dictTemplateModels.Add(templateName, lstTemplateModels);
            }

            CacheDependency templateCacheDependency = new CacheDependency(fileFullName);
            PresentationServiceHelper.LookUp<ICacheService>().Set(fileName, dictTemplateModels, templateCacheDependency);

            return dictTemplateModels;
        }
    }
}
