using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class SEC_GatewayMappers
	{
		public static SEC_Gateway ChangeDTOToSEC_GatewayNew(SEC_GatewayDTO dtoSEC_Gateway)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_GatewayDTO, SEC_Gateway>();
            });
            var domainSEC_Gateway = config.CreateMapper().Map<SEC_GatewayDTO, SEC_Gateway>(dtoSEC_Gateway);

            return domainSEC_Gateway;
        }

		public static void ChangeDTOToSEC_GatewayUpdate(SEC_GatewayDTO dtoSEC_Gateway, SEC_Gateway domainSEC_Gateway)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_GatewayDTO, SEC_Gateway>();
            });
            config.CreateMapper().Map<SEC_GatewayDTO, SEC_Gateway>(dtoSEC_Gateway, domainSEC_Gateway);
        }

		public static void ChangeSEC_GatewayToDTO(SEC_GatewayDTO dtoSEC_Gateway, SEC_Gateway domainSEC_Gateway)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Gateway, SEC_GatewayDTO>();
            });
            config.CreateMapper().Map<SEC_Gateway, SEC_GatewayDTO>(domainSEC_Gateway, dtoSEC_Gateway);
        }

		public static SEC_GatewayDTO ChangeSEC_GatewayToDTO(SEC_Gateway domainSEC_Gateway)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Gateway, SEC_GatewayDTO>();
            });
            return config.CreateMapper().Map<SEC_Gateway, SEC_GatewayDTO>(domainSEC_Gateway);
        }

		public static List<SEC_GatewayDTO> ChangeSEC_GatewayToDTOs(List<SEC_Gateway> domainSEC_Gateway)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Gateway, SEC_GatewayDTO>();
            });
            var dtoSEC_Gateway = config.CreateMapper().Map<List<SEC_Gateway>, List<SEC_GatewayDTO>>(domainSEC_Gateway);

            return dtoSEC_Gateway;
        }

		public static IEnumerable<SEC_GatewayDTO> ChangeSEC_GatewayToDTOs(IEnumerable<SEC_Gateway> domainSEC_Gateways)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Gateway, SEC_GatewayDTO>();
            });
            var dtoSEC_Gateway = config.CreateMapper().Map<IEnumerable<SEC_Gateway>, IEnumerable<SEC_GatewayDTO>>(domainSEC_Gateways);

            return dtoSEC_Gateway;
        }
	}
}
