using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class ChargeSubjectSnaMappers
	{
		public static ChargeSubjectSna ChangeDTOToChargeSubjectSnaNew(ChargeSubjectSnaDTO dtoChargeSubjectSna)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeSubjectSnaDTO, ChargeSubjectSna>();
            });
            var domainChargeSubjectSna = config.CreateMapper().Map<ChargeSubjectSnaDTO, ChargeSubjectSna>(dtoChargeSubjectSna);

            return domainChargeSubjectSna;
        }

		public static void ChangeDTOToChargeSubjectSnaUpdate(ChargeSubjectSnaDTO dtoChargeSubjectSna, ChargeSubjectSna domainChargeSubjectSna)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeSubjectSnaDTO, ChargeSubjectSna>();
            });
            config.CreateMapper().Map<ChargeSubjectSnaDTO, ChargeSubjectSna>(dtoChargeSubjectSna, domainChargeSubjectSna);
        }

		public static void ChangeChargeSubjectSnaToDTO(ChargeSubjectSnaDTO dtoChargeSubjectSna, ChargeSubjectSna domainChargeSubjectSna)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeSubjectSna, ChargeSubjectSnaDTO>();
            });
            config.CreateMapper().Map<ChargeSubjectSna, ChargeSubjectSnaDTO>(domainChargeSubjectSna, dtoChargeSubjectSna);
        }

		public static ChargeSubjectSnaDTO ChangeChargeSubjectSnaToDTO(ChargeSubjectSna domainChargeSubjectSna)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeSubjectSna, ChargeSubjectSnaDTO>();
            });
            return config.CreateMapper().Map<ChargeSubjectSna, ChargeSubjectSnaDTO>(domainChargeSubjectSna);
        }

		public static List<ChargeSubjectSnaDTO> ChangeChargeSubjectSnaToDTOs(List<ChargeSubjectSna> domainChargeSubjectSna)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeSubjectSna, ChargeSubjectSnaDTO>();
            });
            var dtoChargeSubjectSna = config.CreateMapper().Map<List<ChargeSubjectSna>, List<ChargeSubjectSnaDTO>>(domainChargeSubjectSna);

            return dtoChargeSubjectSna;
        }

		public static IEnumerable<ChargeSubjectSnaDTO> ChangeChargeSubjectSnaToDTOs(IEnumerable<ChargeSubjectSna> domainChargeSubjectSnas)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeSubjectSna, ChargeSubjectSnaDTO>();
            });
            var dtoChargeSubjectSna = config.CreateMapper().Map<IEnumerable<ChargeSubjectSna>, IEnumerable<ChargeSubjectSnaDTO>>(domainChargeSubjectSnas);

            return dtoChargeSubjectSna;
        }
	}
}
