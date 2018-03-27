using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class SEC_User_OwnerMappers
	{
		public static SEC_User_Owner ChangeDTOToSEC_User_OwnerNew(SEC_User_OwnerDTO dtoSEC_User_Owner)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_User_OwnerDTO, SEC_User_Owner>();
            });
            var domainSEC_User_Owner = config.CreateMapper().Map<SEC_User_OwnerDTO, SEC_User_Owner>(dtoSEC_User_Owner);

            return domainSEC_User_Owner;
        }

		public static void ChangeDTOToSEC_User_OwnerUpdate(SEC_User_OwnerDTO dtoSEC_User_Owner, SEC_User_Owner domainSEC_User_Owner)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_User_OwnerDTO, SEC_User_Owner>();
            });
            config.CreateMapper().Map<SEC_User_OwnerDTO, SEC_User_Owner>(dtoSEC_User_Owner, domainSEC_User_Owner);
        }

		public static void ChangeSEC_User_OwnerToDTO(SEC_User_OwnerDTO dtoSEC_User_Owner, SEC_User_Owner domainSEC_User_Owner)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_User_Owner, SEC_User_OwnerDTO>();
            });
            config.CreateMapper().Map<SEC_User_Owner, SEC_User_OwnerDTO>(domainSEC_User_Owner, dtoSEC_User_Owner);
        }

		public static SEC_User_OwnerDTO ChangeSEC_User_OwnerToDTO(SEC_User_Owner domainSEC_User_Owner)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_User_Owner, SEC_User_OwnerDTO>();
            });
            return config.CreateMapper().Map<SEC_User_Owner, SEC_User_OwnerDTO>(domainSEC_User_Owner);
        }

		public static List<SEC_User_OwnerDTO> ChangeSEC_User_OwnerToDTOs(List<SEC_User_Owner> domainSEC_User_Owner)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_User_Owner, SEC_User_OwnerDTO>();
            });
            var dtoSEC_User_Owner = config.CreateMapper().Map<List<SEC_User_Owner>, List<SEC_User_OwnerDTO>>(domainSEC_User_Owner);

            return dtoSEC_User_Owner;
        }

		public static IEnumerable<SEC_User_OwnerDTO> ChangeSEC_User_OwnerToDTOs(IEnumerable<SEC_User_Owner> domainSEC_User_Owners)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_User_Owner, SEC_User_OwnerDTO>();
            });
            var dtoSEC_User_Owner = config.CreateMapper().Map<IEnumerable<SEC_User_Owner>, IEnumerable<SEC_User_OwnerDTO>>(domainSEC_User_Owners);

            return dtoSEC_User_Owner;
        }
	}
}
