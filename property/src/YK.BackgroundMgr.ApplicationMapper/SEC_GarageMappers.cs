using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class SEC_GarageMappers
	{
		public static SEC_Garage ChangeDTOToSEC_GarageNew(SEC_GarageDTO dtoSEC_Garage)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_GarageDTO, SEC_Garage>();
            });
            var domainSEC_Garage = config.CreateMapper().Map<SEC_GarageDTO, SEC_Garage>(dtoSEC_Garage);

            return domainSEC_Garage;
        }

		public static void ChangeDTOToSEC_GarageUpdate(SEC_GarageDTO dtoSEC_Garage, SEC_Garage domainSEC_Garage)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_GarageDTO, SEC_Garage>();
            });
            config.CreateMapper().Map<SEC_GarageDTO, SEC_Garage>(dtoSEC_Garage, domainSEC_Garage);
        }

		public static void ChangeSEC_GarageToDTO(SEC_GarageDTO dtoSEC_Garage, SEC_Garage domainSEC_Garage)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Garage, SEC_GarageDTO>();
            });
            config.CreateMapper().Map<SEC_Garage, SEC_GarageDTO>(domainSEC_Garage, dtoSEC_Garage);
        }

		public static SEC_GarageDTO ChangeSEC_GarageToDTO(SEC_Garage domainSEC_Garage)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Garage, SEC_GarageDTO>();
            });
            return config.CreateMapper().Map<SEC_Garage, SEC_GarageDTO>(domainSEC_Garage);
        }

		public static List<SEC_GarageDTO> ChangeSEC_GarageToDTOs(List<SEC_Garage> domainSEC_Garage)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Garage, SEC_GarageDTO>();
            });
            var dtoSEC_Garage = config.CreateMapper().Map<List<SEC_Garage>, List<SEC_GarageDTO>>(domainSEC_Garage);

            return dtoSEC_Garage;
        }

		public static IEnumerable<SEC_GarageDTO> ChangeSEC_GarageToDTOs(IEnumerable<SEC_Garage> domainSEC_Garages)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Garage, SEC_GarageDTO>();
            });
            var dtoSEC_Garage = config.CreateMapper().Map<IEnumerable<SEC_Garage>, IEnumerable<SEC_GarageDTO>>(domainSEC_Garages);

            return dtoSEC_Garage;
        }
	}
}
