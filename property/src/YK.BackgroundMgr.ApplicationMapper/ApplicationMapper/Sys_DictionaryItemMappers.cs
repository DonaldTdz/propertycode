using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;
using YK.BackgroundMgr.PresentationService;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class Sys_DictionaryItemMappers
	{
        public static List<DictionaryModel> ChangeSys_DictionaryItemToTemplateModels(List<Sys_DictionaryItem> domainSys_DictionaryItem)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Sys_DictionaryItem, DictionaryModel>();
            });
            var dtoDictionaryModel = config.CreateMapper().Map<List<Sys_DictionaryItem>, List<DictionaryModel>>(domainSys_DictionaryItem);

            return dtoDictionaryModel;
        }
	}
}
