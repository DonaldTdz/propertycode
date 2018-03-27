using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class AlipayAPPAuthTokenMappers
	{
		public static AlipayAPPAuthToken ChangeDTOToAlipayAPPAuthTokenNew(AlipayAPPAuthTokenDTO dtoAlipayAPPAuthToken)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayAPPAuthTokenDTO, AlipayAPPAuthToken>();
            });
            var domainAlipayAPPAuthToken = config.CreateMapper().Map<AlipayAPPAuthTokenDTO, AlipayAPPAuthToken>(dtoAlipayAPPAuthToken);

            return domainAlipayAPPAuthToken;
        }

		public static void ChangeDTOToAlipayAPPAuthTokenUpdate(AlipayAPPAuthTokenDTO dtoAlipayAPPAuthToken, AlipayAPPAuthToken domainAlipayAPPAuthToken)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayAPPAuthTokenDTO, AlipayAPPAuthToken>();
            });
            config.CreateMapper().Map<AlipayAPPAuthTokenDTO, AlipayAPPAuthToken>(dtoAlipayAPPAuthToken, domainAlipayAPPAuthToken);
        }

		public static void ChangeAlipayAPPAuthTokenToDTO(AlipayAPPAuthTokenDTO dtoAlipayAPPAuthToken, AlipayAPPAuthToken domainAlipayAPPAuthToken)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayAPPAuthToken, AlipayAPPAuthTokenDTO>();
            });
            config.CreateMapper().Map<AlipayAPPAuthToken, AlipayAPPAuthTokenDTO>(domainAlipayAPPAuthToken, dtoAlipayAPPAuthToken);
        }

		public static AlipayAPPAuthTokenDTO ChangeAlipayAPPAuthTokenToDTO(AlipayAPPAuthToken domainAlipayAPPAuthToken)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayAPPAuthToken, AlipayAPPAuthTokenDTO>();
            });
            return config.CreateMapper().Map<AlipayAPPAuthToken, AlipayAPPAuthTokenDTO>(domainAlipayAPPAuthToken);
        }

		public static List<AlipayAPPAuthTokenDTO> ChangeAlipayAPPAuthTokenToDTOs(List<AlipayAPPAuthToken> domainAlipayAPPAuthToken)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayAPPAuthToken, AlipayAPPAuthTokenDTO>();
            });
            var dtoAlipayAPPAuthToken = config.CreateMapper().Map<List<AlipayAPPAuthToken>, List<AlipayAPPAuthTokenDTO>>(domainAlipayAPPAuthToken);

            return dtoAlipayAPPAuthToken;
        }

		public static IEnumerable<AlipayAPPAuthTokenDTO> ChangeAlipayAPPAuthTokenToDTOs(IEnumerable<AlipayAPPAuthToken> domainAlipayAPPAuthTokens)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayAPPAuthToken, AlipayAPPAuthTokenDTO>();
            });
            var dtoAlipayAPPAuthToken = config.CreateMapper().Map<IEnumerable<AlipayAPPAuthToken>, IEnumerable<AlipayAPPAuthTokenDTO>>(domainAlipayAPPAuthTokens);

            return dtoAlipayAPPAuthToken;
        }
	}
}
