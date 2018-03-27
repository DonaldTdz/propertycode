using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class SEC_HouseMappers
	{
		public static SEC_House ChangeDTOToSEC_HouseNew(SEC_HouseDTO dtoSEC_House)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_HouseDTO, SEC_House>();
            });
            var domainSEC_House = config.CreateMapper().Map<SEC_HouseDTO, SEC_House>(dtoSEC_House);

            return domainSEC_House;
        }

		public static void ChangeDTOToSEC_HouseUpdate(SEC_HouseDTO dtoSEC_House, SEC_House domainSEC_House)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_HouseDTO, SEC_House>();
            });
            config.CreateMapper().Map<SEC_HouseDTO, SEC_House>(dtoSEC_House, domainSEC_House);
        }

		public static void ChangeSEC_HouseToDTO(SEC_HouseDTO dtoSEC_House, SEC_House domainSEC_House)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_House, SEC_HouseDTO>();
            });
            config.CreateMapper().Map<SEC_House, SEC_HouseDTO>(domainSEC_House, dtoSEC_House);
        }

		public static SEC_HouseDTO ChangeSEC_HouseToDTO(SEC_House domainSEC_House)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_House, SEC_HouseDTO>();
            });
            return config.CreateMapper().Map<SEC_House, SEC_HouseDTO>(domainSEC_House);
        }

		public static List<SEC_HouseDTO> ChangeSEC_HouseToDTOs(List<SEC_House> domainSEC_House)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_House, SEC_HouseDTO>();
            });
            var dtoSEC_House = config.CreateMapper().Map<List<SEC_House>, List<SEC_HouseDTO>>(domainSEC_House);

            return dtoSEC_House;
        }

		public static IEnumerable<SEC_HouseDTO> ChangeSEC_HouseToDTOs(IEnumerable<SEC_House> domainSEC_Houses)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_House, SEC_HouseDTO>();
            });
            var dtoSEC_House = config.CreateMapper().Map<IEnumerable<SEC_House>, IEnumerable<SEC_HouseDTO>>(domainSEC_Houses);

            return dtoSEC_House;
        }
	}
}
