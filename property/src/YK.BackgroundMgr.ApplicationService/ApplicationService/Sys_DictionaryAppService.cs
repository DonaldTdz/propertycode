using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.ApplicationMapper;
using YK.BackgroundMgr.DomainService;
using YK.BackgroundMgr.PresentationService;

namespace YK.BackgroundMgr.ApplicationService
{
	public partial class Sys_DictionaryAppService
	{
        /// <summary>
        /// 根据字典Id获取字典项
        /// </summary>
        /// <param name="dictionaryId">字典Id</param>
        /// <returns>字典项</returns>
        public List<DictionaryModel> GetDictionaryModels(int dictionaryId)
        {
            var sys_DictionaryItems = Sys_DictionaryService.GetSys_DictionaryItems(dictionaryId);

            var dtoDictionaryModels = Sys_DictionaryItemMappers.ChangeSys_DictionaryItemToTemplateModels(sys_DictionaryItems);

            return dtoDictionaryModels;
        }

        /// <summary>
        /// 根据字典Code获取字典项
        /// </summary>
        /// <param name="code">字典EnName</param>
        /// <returns>字典项</returns>
        public List<DictionaryModel> GetDictionaryModels(string code)
        {
            var sys_DictionaryItems = Sys_DictionaryService.GetSys_DictionaryItems(code);

            var dtoDictionaryModels = Sys_DictionaryItemMappers.ChangeSys_DictionaryItemToTemplateModels(sys_DictionaryItems);

            return dtoDictionaryModels;
        }
    }
}
