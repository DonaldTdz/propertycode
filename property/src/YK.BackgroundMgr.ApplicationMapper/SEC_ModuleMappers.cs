using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class SEC_ModuleMappers
	{
		public static SEC_Module ChangeDTOToSEC_ModuleNew(SEC_ModuleDTO dtoSEC_Module)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_ModuleDTO, SEC_Module>();
            });
            var domainSEC_Module = config.CreateMapper().Map<SEC_ModuleDTO, SEC_Module>(dtoSEC_Module);

            return domainSEC_Module;
        }

		public static void ChangeDTOToSEC_ModuleUpdate(SEC_ModuleDTO dtoSEC_Module, SEC_Module domainSEC_Module)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_ModuleDTO, SEC_Module>();
            });
            config.CreateMapper().Map<SEC_ModuleDTO, SEC_Module>(dtoSEC_Module, domainSEC_Module);
        }

		public static void ChangeSEC_ModuleToDTO(SEC_ModuleDTO dtoSEC_Module, SEC_Module domainSEC_Module)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Module, SEC_ModuleDTO>();
            });
            config.CreateMapper().Map<SEC_Module, SEC_ModuleDTO>(domainSEC_Module, dtoSEC_Module);
        }

		public static SEC_ModuleDTO ChangeSEC_ModuleToDTO(SEC_Module domainSEC_Module)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Module, SEC_ModuleDTO>();
            });
            return config.CreateMapper().Map<SEC_Module, SEC_ModuleDTO>(domainSEC_Module);
        }

		public static List<SEC_ModuleDTO> ChangeSEC_ModuleToDTOs(List<SEC_Module> domainSEC_Module)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Module, SEC_ModuleDTO>();
            });
            var dtoSEC_Module = config.CreateMapper().Map<List<SEC_Module>, List<SEC_ModuleDTO>>(domainSEC_Module);

            return dtoSEC_Module;
        }

		public static IEnumerable<SEC_ModuleDTO> ChangeSEC_ModuleToDTOs(IEnumerable<SEC_Module> domainSEC_Modules)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Module, SEC_ModuleDTO>();
            });
            var dtoSEC_Module = config.CreateMapper().Map<IEnumerable<SEC_Module>, IEnumerable<SEC_ModuleDTO>>(domainSEC_Modules);

            return dtoSEC_Module;
        }
	}
}
