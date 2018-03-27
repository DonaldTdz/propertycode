using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class CountyMappers
	{
		public static County ChangeDTOToCountyNew(CountyDTO dtoCounty)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CountyDTO, County>();
            });
            var domainCounty = config.CreateMapper().Map<CountyDTO, County>(dtoCounty);

            return domainCounty;
        }

		public static void ChangeDTOToCountyUpdate(CountyDTO dtoCounty, County domainCounty)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CountyDTO, County>();
            });
            config.CreateMapper().Map<CountyDTO, County>(dtoCounty, domainCounty);
        }

		public static void ChangeCountyToDTO(CountyDTO dtoCounty, County domainCounty)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<County, CountyDTO>();
            });
            config.CreateMapper().Map<County, CountyDTO>(domainCounty, dtoCounty);
        }

		public static CountyDTO ChangeCountyToDTO(County domainCounty)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<County, CountyDTO>();
            });
            return config.CreateMapper().Map<County, CountyDTO>(domainCounty);
        }

		public static List<CountyDTO> ChangeCountyToDTOs(List<County> domainCounty)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<County, CountyDTO>();
            });
            var dtoCounty = config.CreateMapper().Map<List<County>, List<CountyDTO>>(domainCounty);

            return dtoCounty;
        }

		public static IEnumerable<CountyDTO> ChangeCountyToDTOs(IEnumerable<County> domainCountys)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<County, CountyDTO>();
            });
            var dtoCounty = config.CreateMapper().Map<IEnumerable<County>, IEnumerable<CountyDTO>>(domainCountys);

            return dtoCounty;
        }
	}
}
