using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class ChargeSubjectMappers
	{
		public static ChargeSubject ChangeDTOToChargeSubjectNew(ChargeSubjectDTO dtoChargeSubject)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeSubjectDTO, ChargeSubject>();
            });
            var domainChargeSubject = config.CreateMapper().Map<ChargeSubjectDTO, ChargeSubject>(dtoChargeSubject);

            return domainChargeSubject;
        }

		public static void ChangeDTOToChargeSubjectUpdate(ChargeSubjectDTO dtoChargeSubject, ChargeSubject domainChargeSubject)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeSubjectDTO, ChargeSubject>();
            });
            config.CreateMapper().Map<ChargeSubjectDTO, ChargeSubject>(dtoChargeSubject, domainChargeSubject);
        }

		public static void ChangeChargeSubjectToDTO(ChargeSubjectDTO dtoChargeSubject, ChargeSubject domainChargeSubject)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeSubject, ChargeSubjectDTO>();
            });
            config.CreateMapper().Map<ChargeSubject, ChargeSubjectDTO>(domainChargeSubject, dtoChargeSubject);
        }

		public static ChargeSubjectDTO ChangeChargeSubjectToDTO(ChargeSubject domainChargeSubject)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeSubject, ChargeSubjectDTO>();
            });
            return config.CreateMapper().Map<ChargeSubject, ChargeSubjectDTO>(domainChargeSubject);
        }

		public static List<ChargeSubjectDTO> ChangeChargeSubjectToDTOs(List<ChargeSubject> domainChargeSubject)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeSubject, ChargeSubjectDTO>();
            });
            var dtoChargeSubject = config.CreateMapper().Map<List<ChargeSubject>, List<ChargeSubjectDTO>>(domainChargeSubject);

            return dtoChargeSubject;
        }

		public static IEnumerable<ChargeSubjectDTO> ChangeChargeSubjectToDTOs(IEnumerable<ChargeSubject> domainChargeSubjects)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeSubject, ChargeSubjectDTO>();
            });
            var dtoChargeSubject = config.CreateMapper().Map<IEnumerable<ChargeSubject>, IEnumerable<ChargeSubjectDTO>>(domainChargeSubjects);

            return dtoChargeSubject;
        }
	}
}
