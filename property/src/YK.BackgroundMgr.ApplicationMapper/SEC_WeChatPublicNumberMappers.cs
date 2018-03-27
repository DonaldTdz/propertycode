using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class SEC_WeChatPublicNumberMappers
	{
		public static SEC_WeChatPublicNumber ChangeDTOToSEC_WeChatPublicNumberNew(SEC_WeChatPublicNumberDTO dtoSEC_WeChatPublicNumber)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_WeChatPublicNumberDTO, SEC_WeChatPublicNumber>();
            });
            var domainSEC_WeChatPublicNumber = config.CreateMapper().Map<SEC_WeChatPublicNumberDTO, SEC_WeChatPublicNumber>(dtoSEC_WeChatPublicNumber);

            return domainSEC_WeChatPublicNumber;
        }

		public static void ChangeDTOToSEC_WeChatPublicNumberUpdate(SEC_WeChatPublicNumberDTO dtoSEC_WeChatPublicNumber, SEC_WeChatPublicNumber domainSEC_WeChatPublicNumber)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_WeChatPublicNumberDTO, SEC_WeChatPublicNumber>();
            });
            config.CreateMapper().Map<SEC_WeChatPublicNumberDTO, SEC_WeChatPublicNumber>(dtoSEC_WeChatPublicNumber, domainSEC_WeChatPublicNumber);
        }

		public static void ChangeSEC_WeChatPublicNumberToDTO(SEC_WeChatPublicNumberDTO dtoSEC_WeChatPublicNumber, SEC_WeChatPublicNumber domainSEC_WeChatPublicNumber)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_WeChatPublicNumber, SEC_WeChatPublicNumberDTO>();
            });
            config.CreateMapper().Map<SEC_WeChatPublicNumber, SEC_WeChatPublicNumberDTO>(domainSEC_WeChatPublicNumber, dtoSEC_WeChatPublicNumber);
        }

		public static SEC_WeChatPublicNumberDTO ChangeSEC_WeChatPublicNumberToDTO(SEC_WeChatPublicNumber domainSEC_WeChatPublicNumber)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_WeChatPublicNumber, SEC_WeChatPublicNumberDTO>();
            });
            return config.CreateMapper().Map<SEC_WeChatPublicNumber, SEC_WeChatPublicNumberDTO>(domainSEC_WeChatPublicNumber);
        }

		public static List<SEC_WeChatPublicNumberDTO> ChangeSEC_WeChatPublicNumberToDTOs(List<SEC_WeChatPublicNumber> domainSEC_WeChatPublicNumber)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_WeChatPublicNumber, SEC_WeChatPublicNumberDTO>();
            });
            var dtoSEC_WeChatPublicNumber = config.CreateMapper().Map<List<SEC_WeChatPublicNumber>, List<SEC_WeChatPublicNumberDTO>>(domainSEC_WeChatPublicNumber);

            return dtoSEC_WeChatPublicNumber;
        }

		public static IEnumerable<SEC_WeChatPublicNumberDTO> ChangeSEC_WeChatPublicNumberToDTOs(IEnumerable<SEC_WeChatPublicNumber> domainSEC_WeChatPublicNumbers)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_WeChatPublicNumber, SEC_WeChatPublicNumberDTO>();
            });
            var dtoSEC_WeChatPublicNumber = config.CreateMapper().Map<IEnumerable<SEC_WeChatPublicNumber>, IEnumerable<SEC_WeChatPublicNumberDTO>>(domainSEC_WeChatPublicNumbers);

            return dtoSEC_WeChatPublicNumber;
        }
	}
}
