using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class SEC_FieldMappers
	{
		public static SEC_Field ChangeDTOToSEC_FieldNew(SEC_FieldDTO dtoSEC_Field)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_FieldDTO, SEC_Field>();
            });
            var domainSEC_Field = config.CreateMapper().Map<SEC_FieldDTO, SEC_Field>(dtoSEC_Field);

            return domainSEC_Field;
        }

		public static void ChangeDTOToSEC_FieldUpdate(SEC_FieldDTO dtoSEC_Field, SEC_Field domainSEC_Field)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_FieldDTO, SEC_Field>();
            });
            config.CreateMapper().Map<SEC_FieldDTO, SEC_Field>(dtoSEC_Field, domainSEC_Field);
        }

		public static void ChangeSEC_FieldToDTO(SEC_FieldDTO dtoSEC_Field, SEC_Field domainSEC_Field)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Field, SEC_FieldDTO>();
            });
            config.CreateMapper().Map<SEC_Field, SEC_FieldDTO>(domainSEC_Field, dtoSEC_Field);
        }

		public static SEC_FieldDTO ChangeSEC_FieldToDTO(SEC_Field domainSEC_Field)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Field, SEC_FieldDTO>();
            });
            return config.CreateMapper().Map<SEC_Field, SEC_FieldDTO>(domainSEC_Field);
        }

		public static List<SEC_FieldDTO> ChangeSEC_FieldToDTOs(List<SEC_Field> domainSEC_Field)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Field, SEC_FieldDTO>();
            });
            var dtoSEC_Field = config.CreateMapper().Map<List<SEC_Field>, List<SEC_FieldDTO>>(domainSEC_Field);

            return dtoSEC_Field;
        }

		public static IEnumerable<SEC_FieldDTO> ChangeSEC_FieldToDTOs(IEnumerable<SEC_Field> domainSEC_Fields)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Field, SEC_FieldDTO>();
            });
            var dtoSEC_Field = config.CreateMapper().Map<IEnumerable<SEC_Field>, IEnumerable<SEC_FieldDTO>>(domainSEC_Fields);

            return dtoSEC_Field;
        }
	}
}
