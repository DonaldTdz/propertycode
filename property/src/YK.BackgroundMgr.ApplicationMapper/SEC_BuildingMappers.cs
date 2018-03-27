using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class SEC_BuildingMappers
	{
		public static SEC_Building ChangeDTOToSEC_BuildingNew(SEC_BuildingDTO dtoSEC_Building)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_BuildingDTO, SEC_Building>();
            });
            var domainSEC_Building = config.CreateMapper().Map<SEC_BuildingDTO, SEC_Building>(dtoSEC_Building);

            return domainSEC_Building;
        }

		public static void ChangeDTOToSEC_BuildingUpdate(SEC_BuildingDTO dtoSEC_Building, SEC_Building domainSEC_Building)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_BuildingDTO, SEC_Building>();
            });
            config.CreateMapper().Map<SEC_BuildingDTO, SEC_Building>(dtoSEC_Building, domainSEC_Building);
        }

		public static void ChangeSEC_BuildingToDTO(SEC_BuildingDTO dtoSEC_Building, SEC_Building domainSEC_Building)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Building, SEC_BuildingDTO>();
            });
            config.CreateMapper().Map<SEC_Building, SEC_BuildingDTO>(domainSEC_Building, dtoSEC_Building);
        }

		public static SEC_BuildingDTO ChangeSEC_BuildingToDTO(SEC_Building domainSEC_Building)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Building, SEC_BuildingDTO>();
            });
            return config.CreateMapper().Map<SEC_Building, SEC_BuildingDTO>(domainSEC_Building);
        }

		public static List<SEC_BuildingDTO> ChangeSEC_BuildingToDTOs(List<SEC_Building> domainSEC_Building)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Building, SEC_BuildingDTO>();
            });
            var dtoSEC_Building = config.CreateMapper().Map<List<SEC_Building>, List<SEC_BuildingDTO>>(domainSEC_Building);

            return dtoSEC_Building;
        }

		public static IEnumerable<SEC_BuildingDTO> ChangeSEC_BuildingToDTOs(IEnumerable<SEC_Building> domainSEC_Buildings)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Building, SEC_BuildingDTO>();
            });
            var dtoSEC_Building = config.CreateMapper().Map<IEnumerable<SEC_Building>, IEnumerable<SEC_BuildingDTO>>(domainSEC_Buildings);

            return dtoSEC_Building;
        }
	}
}
