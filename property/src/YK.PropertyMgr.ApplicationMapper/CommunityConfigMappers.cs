using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class CommunityConfigMappers
	{
		public static CommunityConfig ChangeDTOToCommunityConfigNew(CommunityConfigDTO dtoCommunityConfig)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CommunityConfigDTO, CommunityConfig>();
            });
            var domainCommunityConfig = config.CreateMapper().Map<CommunityConfigDTO, CommunityConfig>(dtoCommunityConfig);

            return domainCommunityConfig;
        }

		public static void ChangeDTOToCommunityConfigUpdate(CommunityConfigDTO dtoCommunityConfig, CommunityConfig domainCommunityConfig)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CommunityConfigDTO, CommunityConfig>();
            });
            config.CreateMapper().Map<CommunityConfigDTO, CommunityConfig>(dtoCommunityConfig, domainCommunityConfig);
        }

		public static void ChangeCommunityConfigToDTO(CommunityConfigDTO dtoCommunityConfig, CommunityConfig domainCommunityConfig)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CommunityConfig, CommunityConfigDTO>();
            });
            config.CreateMapper().Map<CommunityConfig, CommunityConfigDTO>(domainCommunityConfig, dtoCommunityConfig);
        }

		public static CommunityConfigDTO ChangeCommunityConfigToDTO(CommunityConfig domainCommunityConfig)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CommunityConfig, CommunityConfigDTO>();
            });
            return config.CreateMapper().Map<CommunityConfig, CommunityConfigDTO>(domainCommunityConfig);
        }

		public static List<CommunityConfigDTO> ChangeCommunityConfigToDTOs(List<CommunityConfig> domainCommunityConfig)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CommunityConfig, CommunityConfigDTO>();
            });
            var dtoCommunityConfig = config.CreateMapper().Map<List<CommunityConfig>, List<CommunityConfigDTO>>(domainCommunityConfig);

            return dtoCommunityConfig;
        }

		public static IEnumerable<CommunityConfigDTO> ChangeCommunityConfigToDTOs(IEnumerable<CommunityConfig> domainCommunityConfigs)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CommunityConfig, CommunityConfigDTO>();
            });
            var dtoCommunityConfig = config.CreateMapper().Map<IEnumerable<CommunityConfig>, IEnumerable<CommunityConfigDTO>>(domainCommunityConfigs);

            return dtoCommunityConfig;
        }
	}
}
