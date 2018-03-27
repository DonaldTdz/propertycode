using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class SEC_CommunityMappers
	{
		public static SEC_Community ChangeDTOToSEC_CommunityNew(SEC_CommunityDTO dtoSEC_Community)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_CommunityDTO, SEC_Community>();
            });
            var domainSEC_Community = config.CreateMapper().Map<SEC_CommunityDTO, SEC_Community>(dtoSEC_Community);

            return domainSEC_Community;
        }

		public static void ChangeDTOToSEC_CommunityUpdate(SEC_CommunityDTO dtoSEC_Community, SEC_Community domainSEC_Community)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_CommunityDTO, SEC_Community>();
            });
            config.CreateMapper().Map<SEC_CommunityDTO, SEC_Community>(dtoSEC_Community, domainSEC_Community);
        }

		public static void ChangeSEC_CommunityToDTO(SEC_CommunityDTO dtoSEC_Community, SEC_Community domainSEC_Community)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Community, SEC_CommunityDTO>();
            });
            config.CreateMapper().Map<SEC_Community, SEC_CommunityDTO>(domainSEC_Community, dtoSEC_Community);
        }

		public static SEC_CommunityDTO ChangeSEC_CommunityToDTO(SEC_Community domainSEC_Community)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Community, SEC_CommunityDTO>();
            });
            return config.CreateMapper().Map<SEC_Community, SEC_CommunityDTO>(domainSEC_Community);
        }

		public static List<SEC_CommunityDTO> ChangeSEC_CommunityToDTOs(List<SEC_Community> domainSEC_Community)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Community, SEC_CommunityDTO>();
            });
            var dtoSEC_Community = config.CreateMapper().Map<List<SEC_Community>, List<SEC_CommunityDTO>>(domainSEC_Community);

            return dtoSEC_Community;
        }

		public static IEnumerable<SEC_CommunityDTO> ChangeSEC_CommunityToDTOs(IEnumerable<SEC_Community> domainSEC_Communitys)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Community, SEC_CommunityDTO>();
            });
            var dtoSEC_Community = config.CreateMapper().Map<IEnumerable<SEC_Community>, IEnumerable<SEC_CommunityDTO>>(domainSEC_Communitys);

            return dtoSEC_Community;
        }
	}
}
