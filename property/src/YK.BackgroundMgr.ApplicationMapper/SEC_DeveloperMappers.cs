using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class SEC_DeveloperMappers
	{
		public static SEC_Developer ChangeDTOToSEC_DeveloperNew(SEC_DeveloperDTO dtoSEC_Developer)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_DeveloperDTO, SEC_Developer>();
            });
            var domainSEC_Developer = config.CreateMapper().Map<SEC_DeveloperDTO, SEC_Developer>(dtoSEC_Developer);

            return domainSEC_Developer;
        }

		public static void ChangeDTOToSEC_DeveloperUpdate(SEC_DeveloperDTO dtoSEC_Developer, SEC_Developer domainSEC_Developer)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_DeveloperDTO, SEC_Developer>();
            });
            config.CreateMapper().Map<SEC_DeveloperDTO, SEC_Developer>(dtoSEC_Developer, domainSEC_Developer);
        }

		public static void ChangeSEC_DeveloperToDTO(SEC_DeveloperDTO dtoSEC_Developer, SEC_Developer domainSEC_Developer)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Developer, SEC_DeveloperDTO>();
            });
            config.CreateMapper().Map<SEC_Developer, SEC_DeveloperDTO>(domainSEC_Developer, dtoSEC_Developer);
        }

		public static SEC_DeveloperDTO ChangeSEC_DeveloperToDTO(SEC_Developer domainSEC_Developer)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Developer, SEC_DeveloperDTO>();
            });
            return config.CreateMapper().Map<SEC_Developer, SEC_DeveloperDTO>(domainSEC_Developer);
        }

		public static List<SEC_DeveloperDTO> ChangeSEC_DeveloperToDTOs(List<SEC_Developer> domainSEC_Developer)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Developer, SEC_DeveloperDTO>();
            });
            var dtoSEC_Developer = config.CreateMapper().Map<List<SEC_Developer>, List<SEC_DeveloperDTO>>(domainSEC_Developer);

            return dtoSEC_Developer;
        }

		public static IEnumerable<SEC_DeveloperDTO> ChangeSEC_DeveloperToDTOs(IEnumerable<SEC_Developer> domainSEC_Developers)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Developer, SEC_DeveloperDTO>();
            });
            var dtoSEC_Developer = config.CreateMapper().Map<IEnumerable<SEC_Developer>, IEnumerable<SEC_DeveloperDTO>>(domainSEC_Developers);

            return dtoSEC_Developer;
        }
	}
}
