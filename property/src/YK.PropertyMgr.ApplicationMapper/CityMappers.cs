using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class CityMappers
	{
		public static City ChangeDTOToCityNew(CityDTO dtoCity)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CityDTO, City>();
            });
            var domainCity = config.CreateMapper().Map<CityDTO, City>(dtoCity);

            return domainCity;
        }

		public static void ChangeDTOToCityUpdate(CityDTO dtoCity, City domainCity)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CityDTO, City>();
            });
            config.CreateMapper().Map<CityDTO, City>(dtoCity, domainCity);
        }

		public static void ChangeCityToDTO(CityDTO dtoCity, City domainCity)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<City, CityDTO>();
            });
            config.CreateMapper().Map<City, CityDTO>(domainCity, dtoCity);
        }

		public static CityDTO ChangeCityToDTO(City domainCity)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<City, CityDTO>();
            });
            return config.CreateMapper().Map<City, CityDTO>(domainCity);
        }

		public static List<CityDTO> ChangeCityToDTOs(List<City> domainCity)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<City, CityDTO>();
            });
            var dtoCity = config.CreateMapper().Map<List<City>, List<CityDTO>>(domainCity);

            return dtoCity;
        }

		public static IEnumerable<CityDTO> ChangeCityToDTOs(IEnumerable<City> domainCitys)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<City, CityDTO>();
            });
            var dtoCity = config.CreateMapper().Map<IEnumerable<City>, IEnumerable<CityDTO>>(domainCitys);

            return dtoCity;
        }
	}
}
