using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class ProvinceMappers
	{
		public static Province ChangeDTOToProvinceNew(ProvinceDTO dtoProvince)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProvinceDTO, Province>();
            });
            var domainProvince = config.CreateMapper().Map<ProvinceDTO, Province>(dtoProvince);

            return domainProvince;
        }

		public static void ChangeDTOToProvinceUpdate(ProvinceDTO dtoProvince, Province domainProvince)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProvinceDTO, Province>();
            });
            config.CreateMapper().Map<ProvinceDTO, Province>(dtoProvince, domainProvince);
        }

		public static void ChangeProvinceToDTO(ProvinceDTO dtoProvince, Province domainProvince)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Province, ProvinceDTO>();
            });
            config.CreateMapper().Map<Province, ProvinceDTO>(domainProvince, dtoProvince);
        }

		public static ProvinceDTO ChangeProvinceToDTO(Province domainProvince)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Province, ProvinceDTO>();
            });
            return config.CreateMapper().Map<Province, ProvinceDTO>(domainProvince);
        }

		public static List<ProvinceDTO> ChangeProvinceToDTOs(List<Province> domainProvince)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Province, ProvinceDTO>();
            });
            var dtoProvince = config.CreateMapper().Map<List<Province>, List<ProvinceDTO>>(domainProvince);

            return dtoProvince;
        }

		public static IEnumerable<ProvinceDTO> ChangeProvinceToDTOs(IEnumerable<Province> domainProvinces)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Province, ProvinceDTO>();
            });
            var dtoProvince = config.CreateMapper().Map<IEnumerable<Province>, IEnumerable<ProvinceDTO>>(domainProvinces);

            return dtoProvince;
        }
	}
}
