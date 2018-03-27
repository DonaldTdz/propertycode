using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class Sys_DictionaryMappers
	{
		public static Sys_Dictionary ChangeDTOToSys_DictionaryNew(Sys_DictionaryDTO dtoSys_Dictionary)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Sys_DictionaryDTO, Sys_Dictionary>();
            });
            var domainSys_Dictionary = config.CreateMapper().Map<Sys_DictionaryDTO, Sys_Dictionary>(dtoSys_Dictionary);

            return domainSys_Dictionary;
        }

		public static void ChangeDTOToSys_DictionaryUpdate(Sys_DictionaryDTO dtoSys_Dictionary, Sys_Dictionary domainSys_Dictionary)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Sys_DictionaryDTO, Sys_Dictionary>();
            });
            config.CreateMapper().Map<Sys_DictionaryDTO, Sys_Dictionary>(dtoSys_Dictionary, domainSys_Dictionary);
        }

		public static void ChangeSys_DictionaryToDTO(Sys_DictionaryDTO dtoSys_Dictionary, Sys_Dictionary domainSys_Dictionary)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Sys_Dictionary, Sys_DictionaryDTO>();
            });
            config.CreateMapper().Map<Sys_Dictionary, Sys_DictionaryDTO>(domainSys_Dictionary, dtoSys_Dictionary);
        }

		public static Sys_DictionaryDTO ChangeSys_DictionaryToDTO(Sys_Dictionary domainSys_Dictionary)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Sys_Dictionary, Sys_DictionaryDTO>();
            });
            return config.CreateMapper().Map<Sys_Dictionary, Sys_DictionaryDTO>(domainSys_Dictionary);
        }

		public static List<Sys_DictionaryDTO> ChangeSys_DictionaryToDTOs(List<Sys_Dictionary> domainSys_Dictionary)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Sys_Dictionary, Sys_DictionaryDTO>();
            });
            var dtoSys_Dictionary = config.CreateMapper().Map<List<Sys_Dictionary>, List<Sys_DictionaryDTO>>(domainSys_Dictionary);

            return dtoSys_Dictionary;
        }

		public static IEnumerable<Sys_DictionaryDTO> ChangeSys_DictionaryToDTOs(IEnumerable<Sys_Dictionary> domainSys_Dictionarys)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Sys_Dictionary, Sys_DictionaryDTO>();
            });
            var dtoSys_Dictionary = config.CreateMapper().Map<IEnumerable<Sys_Dictionary>, IEnumerable<Sys_DictionaryDTO>>(domainSys_Dictionarys);

            return dtoSys_Dictionary;
        }
	}
}
