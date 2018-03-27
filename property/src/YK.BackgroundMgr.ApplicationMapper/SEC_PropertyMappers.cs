using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class SEC_PropertyMappers
	{
		public static SEC_Property ChangeDTOToSEC_PropertyNew(SEC_PropertyDTO dtoSEC_Property)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_PropertyDTO, SEC_Property>();
            });
            var domainSEC_Property = config.CreateMapper().Map<SEC_PropertyDTO, SEC_Property>(dtoSEC_Property);

            return domainSEC_Property;
        }

		public static void ChangeDTOToSEC_PropertyUpdate(SEC_PropertyDTO dtoSEC_Property, SEC_Property domainSEC_Property)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_PropertyDTO, SEC_Property>();
            });
            config.CreateMapper().Map<SEC_PropertyDTO, SEC_Property>(dtoSEC_Property, domainSEC_Property);
        }

		public static void ChangeSEC_PropertyToDTO(SEC_PropertyDTO dtoSEC_Property, SEC_Property domainSEC_Property)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Property, SEC_PropertyDTO>();
            });
            config.CreateMapper().Map<SEC_Property, SEC_PropertyDTO>(domainSEC_Property, dtoSEC_Property);
        }

		public static SEC_PropertyDTO ChangeSEC_PropertyToDTO(SEC_Property domainSEC_Property)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Property, SEC_PropertyDTO>();
            });
            return config.CreateMapper().Map<SEC_Property, SEC_PropertyDTO>(domainSEC_Property);
        }

		public static List<SEC_PropertyDTO> ChangeSEC_PropertyToDTOs(List<SEC_Property> domainSEC_Property)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Property, SEC_PropertyDTO>();
            });
            var dtoSEC_Property = config.CreateMapper().Map<List<SEC_Property>, List<SEC_PropertyDTO>>(domainSEC_Property);

            return dtoSEC_Property;
        }

		public static IEnumerable<SEC_PropertyDTO> ChangeSEC_PropertyToDTOs(IEnumerable<SEC_Property> domainSEC_Propertys)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Property, SEC_PropertyDTO>();
            });
            var dtoSEC_Property = config.CreateMapper().Map<IEnumerable<SEC_Property>, IEnumerable<SEC_PropertyDTO>>(domainSEC_Propertys);

            return dtoSEC_Property;
        }
	}
}
