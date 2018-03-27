using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace YK.BackgroundMgr.PresentationService
{
    public interface ITemplateService : IPresentationService
    {
        /// <summary>
        /// 读取TemplateModel信息
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="templateName">Template名称</param>
        /// <param name="isSaveToCache">是否存储缓存</param>
        /// <returns>TemplateModel信息</returns>
        IEnumerable<TemplateModel> GetTemplateModels(string fileName, string templateName, bool isSaveToCache = true);
    }
}
