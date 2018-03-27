using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class SEC_WeChatOpenIdMappers
	{
		public static SEC_WeChatOpenId ChangeDTOToSEC_WeChatOpenIdNew(SEC_WeChatOpenIdDTO dtoSEC_WeChatOpenId)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_WeChatOpenIdDTO, SEC_WeChatOpenId>();
            });
            var domainSEC_WeChatOpenId = config.CreateMapper().Map<SEC_WeChatOpenIdDTO, SEC_WeChatOpenId>(dtoSEC_WeChatOpenId);

            return domainSEC_WeChatOpenId;
        }

		public static void ChangeDTOToSEC_WeChatOpenIdUpdate(SEC_WeChatOpenIdDTO dtoSEC_WeChatOpenId, SEC_WeChatOpenId domainSEC_WeChatOpenId)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_WeChatOpenIdDTO, SEC_WeChatOpenId>();
            });
            config.CreateMapper().Map<SEC_WeChatOpenIdDTO, SEC_WeChatOpenId>(dtoSEC_WeChatOpenId, domainSEC_WeChatOpenId);
        }

		public static void ChangeSEC_WeChatOpenIdToDTO(SEC_WeChatOpenIdDTO dtoSEC_WeChatOpenId, SEC_WeChatOpenId domainSEC_WeChatOpenId)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_WeChatOpenId, SEC_WeChatOpenIdDTO>();
            });
            config.CreateMapper().Map<SEC_WeChatOpenId, SEC_WeChatOpenIdDTO>(domainSEC_WeChatOpenId, dtoSEC_WeChatOpenId);
        }

		public static SEC_WeChatOpenIdDTO ChangeSEC_WeChatOpenIdToDTO(SEC_WeChatOpenId domainSEC_WeChatOpenId)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_WeChatOpenId, SEC_WeChatOpenIdDTO>();
            });
            return config.CreateMapper().Map<SEC_WeChatOpenId, SEC_WeChatOpenIdDTO>(domainSEC_WeChatOpenId);
        }

		public static List<SEC_WeChatOpenIdDTO> ChangeSEC_WeChatOpenIdToDTOs(List<SEC_WeChatOpenId> domainSEC_WeChatOpenId)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_WeChatOpenId, SEC_WeChatOpenIdDTO>();
            });
            var dtoSEC_WeChatOpenId = config.CreateMapper().Map<List<SEC_WeChatOpenId>, List<SEC_WeChatOpenIdDTO>>(domainSEC_WeChatOpenId);

            return dtoSEC_WeChatOpenId;
        }

		public static IEnumerable<SEC_WeChatOpenIdDTO> ChangeSEC_WeChatOpenIdToDTOs(IEnumerable<SEC_WeChatOpenId> domainSEC_WeChatOpenIds)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_WeChatOpenId, SEC_WeChatOpenIdDTO>();
            });
            var dtoSEC_WeChatOpenId = config.CreateMapper().Map<IEnumerable<SEC_WeChatOpenId>, IEnumerable<SEC_WeChatOpenIdDTO>>(domainSEC_WeChatOpenIds);

            return dtoSEC_WeChatOpenId;
        }
	}
}
