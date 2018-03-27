using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class SEC_AreaMappers
	{
		public static SEC_Area ChangeDTOToSEC_AreaNew(SEC_AreaDTO dtoSEC_Area)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_AreaDTO, SEC_Area>();
            });
            var domainSEC_Area = config.CreateMapper().Map<SEC_AreaDTO, SEC_Area>(dtoSEC_Area);

            return domainSEC_Area;
        }

		public static void ChangeDTOToSEC_AreaUpdate(SEC_AreaDTO dtoSEC_Area, SEC_Area domainSEC_Area)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_AreaDTO, SEC_Area>();
            });
            config.CreateMapper().Map<SEC_AreaDTO, SEC_Area>(dtoSEC_Area, domainSEC_Area);
        }

		public static void ChangeSEC_AreaToDTO(SEC_AreaDTO dtoSEC_Area, SEC_Area domainSEC_Area)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Area, SEC_AreaDTO>();
            });
            config.CreateMapper().Map<SEC_Area, SEC_AreaDTO>(domainSEC_Area, dtoSEC_Area);
        }

		public static SEC_AreaDTO ChangeSEC_AreaToDTO(SEC_Area domainSEC_Area)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Area, SEC_AreaDTO>();
            });
            return config.CreateMapper().Map<SEC_Area, SEC_AreaDTO>(domainSEC_Area);
        }

		public static List<SEC_AreaDTO> ChangeSEC_AreaToDTOs(List<SEC_Area> domainSEC_Area)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Area, SEC_AreaDTO>();
            });
            var dtoSEC_Area = config.CreateMapper().Map<List<SEC_Area>, List<SEC_AreaDTO>>(domainSEC_Area);

            return dtoSEC_Area;
        }

		public static IEnumerable<SEC_AreaDTO> ChangeSEC_AreaToDTOs(IEnumerable<SEC_Area> domainSEC_Areas)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Area, SEC_AreaDTO>();
            });
            var dtoSEC_Area = config.CreateMapper().Map<IEnumerable<SEC_Area>, IEnumerable<SEC_AreaDTO>>(domainSEC_Areas);

            return dtoSEC_Area;
        }
	}
}
