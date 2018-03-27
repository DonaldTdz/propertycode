using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class NotificeConfigMappers
	{
		public static NotificeConfig ChangeDTOToNotificeConfigNew(NotificeConfigDTO dtoNotificeConfig)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NotificeConfigDTO, NotificeConfig>();
            });
            var domainNotificeConfig = config.CreateMapper().Map<NotificeConfigDTO, NotificeConfig>(dtoNotificeConfig);

            return domainNotificeConfig;
        }

		public static void ChangeDTOToNotificeConfigUpdate(NotificeConfigDTO dtoNotificeConfig, NotificeConfig domainNotificeConfig)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NotificeConfigDTO, NotificeConfig>();
            });
            config.CreateMapper().Map<NotificeConfigDTO, NotificeConfig>(dtoNotificeConfig, domainNotificeConfig);
        }

		public static void ChangeNotificeConfigToDTO(NotificeConfigDTO dtoNotificeConfig, NotificeConfig domainNotificeConfig)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NotificeConfig, NotificeConfigDTO>();
            });
            config.CreateMapper().Map<NotificeConfig, NotificeConfigDTO>(domainNotificeConfig, dtoNotificeConfig);
        }

		public static NotificeConfigDTO ChangeNotificeConfigToDTO(NotificeConfig domainNotificeConfig)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NotificeConfig, NotificeConfigDTO>();
            });
            return config.CreateMapper().Map<NotificeConfig, NotificeConfigDTO>(domainNotificeConfig);
        }

		public static List<NotificeConfigDTO> ChangeNotificeConfigToDTOs(List<NotificeConfig> domainNotificeConfig)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NotificeConfig, NotificeConfigDTO>();
            });
            var dtoNotificeConfig = config.CreateMapper().Map<List<NotificeConfig>, List<NotificeConfigDTO>>(domainNotificeConfig);

            return dtoNotificeConfig;
        }

		public static IEnumerable<NotificeConfigDTO> ChangeNotificeConfigToDTOs(IEnumerable<NotificeConfig> domainNotificeConfigs)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NotificeConfig, NotificeConfigDTO>();
            });
            var dtoNotificeConfig = config.CreateMapper().Map<IEnumerable<NotificeConfig>, IEnumerable<NotificeConfigDTO>>(domainNotificeConfigs);

            return dtoNotificeConfig;
        }
	}
}
