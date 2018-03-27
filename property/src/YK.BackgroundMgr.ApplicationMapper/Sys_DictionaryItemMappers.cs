using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class Sys_DictionaryItemMappers
	{
		public static Sys_DictionaryItem ChangeDTOToSys_DictionaryItemNew(Sys_DictionaryItemDTO dtoSys_DictionaryItem)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Sys_DictionaryItemDTO, Sys_DictionaryItem>();
            });
            var domainSys_DictionaryItem = config.CreateMapper().Map<Sys_DictionaryItemDTO, Sys_DictionaryItem>(dtoSys_DictionaryItem);

            return domainSys_DictionaryItem;
        }

		public static void ChangeDTOToSys_DictionaryItemUpdate(Sys_DictionaryItemDTO dtoSys_DictionaryItem, Sys_DictionaryItem domainSys_DictionaryItem)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Sys_DictionaryItemDTO, Sys_DictionaryItem>();
            });
            config.CreateMapper().Map<Sys_DictionaryItemDTO, Sys_DictionaryItem>(dtoSys_DictionaryItem, domainSys_DictionaryItem);
        }

		public static void ChangeSys_DictionaryItemToDTO(Sys_DictionaryItemDTO dtoSys_DictionaryItem, Sys_DictionaryItem domainSys_DictionaryItem)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Sys_DictionaryItem, Sys_DictionaryItemDTO>();
            });
            config.CreateMapper().Map<Sys_DictionaryItem, Sys_DictionaryItemDTO>(domainSys_DictionaryItem, dtoSys_DictionaryItem);
        }

		public static Sys_DictionaryItemDTO ChangeSys_DictionaryItemToDTO(Sys_DictionaryItem domainSys_DictionaryItem)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Sys_DictionaryItem, Sys_DictionaryItemDTO>();
            });
            return config.CreateMapper().Map<Sys_DictionaryItem, Sys_DictionaryItemDTO>(domainSys_DictionaryItem);
        }

		public static List<Sys_DictionaryItemDTO> ChangeSys_DictionaryItemToDTOs(List<Sys_DictionaryItem> domainSys_DictionaryItem)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Sys_DictionaryItem, Sys_DictionaryItemDTO>();
            });
            var dtoSys_DictionaryItem = config.CreateMapper().Map<List<Sys_DictionaryItem>, List<Sys_DictionaryItemDTO>>(domainSys_DictionaryItem);

            return dtoSys_DictionaryItem;
        }

		public static IEnumerable<Sys_DictionaryItemDTO> ChangeSys_DictionaryItemToDTOs(IEnumerable<Sys_DictionaryItem> domainSys_DictionaryItems)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Sys_DictionaryItem, Sys_DictionaryItemDTO>();
            });
            var dtoSys_DictionaryItem = config.CreateMapper().Map<IEnumerable<Sys_DictionaryItem>, IEnumerable<Sys_DictionaryItemDTO>>(domainSys_DictionaryItems);

            return dtoSys_DictionaryItem;
        }
	}
}
