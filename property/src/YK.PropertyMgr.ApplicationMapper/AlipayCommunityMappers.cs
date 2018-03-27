using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class AlipayCommunityMappers
	{
		public static AlipayCommunity ChangeDTOToAlipayCommunityNew(AlipayCommunityDTO dtoAlipayCommunity)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayCommunityDTO, AlipayCommunity>();
            });
            var domainAlipayCommunity = config.CreateMapper().Map<AlipayCommunityDTO, AlipayCommunity>(dtoAlipayCommunity);

            return domainAlipayCommunity;
        }

		public static void ChangeDTOToAlipayCommunityUpdate(AlipayCommunityDTO dtoAlipayCommunity, AlipayCommunity domainAlipayCommunity)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayCommunityDTO, AlipayCommunity>();
            });
            config.CreateMapper().Map<AlipayCommunityDTO, AlipayCommunity>(dtoAlipayCommunity, domainAlipayCommunity);
        }

		public static void ChangeAlipayCommunityToDTO(AlipayCommunityDTO dtoAlipayCommunity, AlipayCommunity domainAlipayCommunity)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayCommunity, AlipayCommunityDTO>();
            });
            config.CreateMapper().Map<AlipayCommunity, AlipayCommunityDTO>(domainAlipayCommunity, dtoAlipayCommunity);
        }

		public static AlipayCommunityDTO ChangeAlipayCommunityToDTO(AlipayCommunity domainAlipayCommunity)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayCommunity, AlipayCommunityDTO>();
            });
            return config.CreateMapper().Map<AlipayCommunity, AlipayCommunityDTO>(domainAlipayCommunity);
        }

		public static List<AlipayCommunityDTO> ChangeAlipayCommunityToDTOs(List<AlipayCommunity> domainAlipayCommunity)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayCommunity, AlipayCommunityDTO>();
            });
            var dtoAlipayCommunity = config.CreateMapper().Map<List<AlipayCommunity>, List<AlipayCommunityDTO>>(domainAlipayCommunity);

            return dtoAlipayCommunity;
        }

		public static IEnumerable<AlipayCommunityDTO> ChangeAlipayCommunityToDTOs(IEnumerable<AlipayCommunity> domainAlipayCommunitys)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayCommunity, AlipayCommunityDTO>();
            });
            var dtoAlipayCommunity = config.CreateMapper().Map<IEnumerable<AlipayCommunity>, IEnumerable<AlipayCommunityDTO>>(domainAlipayCommunitys);

            return dtoAlipayCommunity;
        }
	}
}
