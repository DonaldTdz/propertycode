using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class SEC_GatewayAuthMappers
	{
		public static SEC_GatewayAuth ChangeDTOToSEC_GatewayAuthNew(SEC_GatewayAuthDTO dtoSEC_GatewayAuth)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_GatewayAuthDTO, SEC_GatewayAuth>();
            });
            var domainSEC_GatewayAuth = config.CreateMapper().Map<SEC_GatewayAuthDTO, SEC_GatewayAuth>(dtoSEC_GatewayAuth);

            return domainSEC_GatewayAuth;
        }

		public static void ChangeDTOToSEC_GatewayAuthUpdate(SEC_GatewayAuthDTO dtoSEC_GatewayAuth, SEC_GatewayAuth domainSEC_GatewayAuth)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_GatewayAuthDTO, SEC_GatewayAuth>();
            });
            config.CreateMapper().Map<SEC_GatewayAuthDTO, SEC_GatewayAuth>(dtoSEC_GatewayAuth, domainSEC_GatewayAuth);
        }

		public static void ChangeSEC_GatewayAuthToDTO(SEC_GatewayAuthDTO dtoSEC_GatewayAuth, SEC_GatewayAuth domainSEC_GatewayAuth)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_GatewayAuth, SEC_GatewayAuthDTO>();
            });
            config.CreateMapper().Map<SEC_GatewayAuth, SEC_GatewayAuthDTO>(domainSEC_GatewayAuth, dtoSEC_GatewayAuth);
        }

		public static SEC_GatewayAuthDTO ChangeSEC_GatewayAuthToDTO(SEC_GatewayAuth domainSEC_GatewayAuth)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_GatewayAuth, SEC_GatewayAuthDTO>();
            });
            return config.CreateMapper().Map<SEC_GatewayAuth, SEC_GatewayAuthDTO>(domainSEC_GatewayAuth);
        }

		public static List<SEC_GatewayAuthDTO> ChangeSEC_GatewayAuthToDTOs(List<SEC_GatewayAuth> domainSEC_GatewayAuth)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_GatewayAuth, SEC_GatewayAuthDTO>();
            });
            var dtoSEC_GatewayAuth = config.CreateMapper().Map<List<SEC_GatewayAuth>, List<SEC_GatewayAuthDTO>>(domainSEC_GatewayAuth);

            return dtoSEC_GatewayAuth;
        }

		public static IEnumerable<SEC_GatewayAuthDTO> ChangeSEC_GatewayAuthToDTOs(IEnumerable<SEC_GatewayAuth> domainSEC_GatewayAuths)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_GatewayAuth, SEC_GatewayAuthDTO>();
            });
            var dtoSEC_GatewayAuth = config.CreateMapper().Map<IEnumerable<SEC_GatewayAuth>, IEnumerable<SEC_GatewayAuthDTO>>(domainSEC_GatewayAuths);

            return dtoSEC_GatewayAuth;
        }
	}
}
