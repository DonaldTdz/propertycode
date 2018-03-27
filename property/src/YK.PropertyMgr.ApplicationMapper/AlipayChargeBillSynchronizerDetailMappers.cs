using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class AlipayChargeBillSynchronizerDetailMappers
	{
		public static AlipayChargeBillSynchronizerDetail ChangeDTOToAlipayChargeBillSynchronizerDetailNew(AlipayChargeBillSynchronizerDetailDTO dtoAlipayChargeBillSynchronizerDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayChargeBillSynchronizerDetailDTO, AlipayChargeBillSynchronizerDetail>();
            });
            var domainAlipayChargeBillSynchronizerDetail = config.CreateMapper().Map<AlipayChargeBillSynchronizerDetailDTO, AlipayChargeBillSynchronizerDetail>(dtoAlipayChargeBillSynchronizerDetail);

            return domainAlipayChargeBillSynchronizerDetail;
        }

		public static void ChangeDTOToAlipayChargeBillSynchronizerDetailUpdate(AlipayChargeBillSynchronizerDetailDTO dtoAlipayChargeBillSynchronizerDetail, AlipayChargeBillSynchronizerDetail domainAlipayChargeBillSynchronizerDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayChargeBillSynchronizerDetailDTO, AlipayChargeBillSynchronizerDetail>();
            });
            config.CreateMapper().Map<AlipayChargeBillSynchronizerDetailDTO, AlipayChargeBillSynchronizerDetail>(dtoAlipayChargeBillSynchronizerDetail, domainAlipayChargeBillSynchronizerDetail);
        }

		public static void ChangeAlipayChargeBillSynchronizerDetailToDTO(AlipayChargeBillSynchronizerDetailDTO dtoAlipayChargeBillSynchronizerDetail, AlipayChargeBillSynchronizerDetail domainAlipayChargeBillSynchronizerDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayChargeBillSynchronizerDetail, AlipayChargeBillSynchronizerDetailDTO>();
            });
            config.CreateMapper().Map<AlipayChargeBillSynchronizerDetail, AlipayChargeBillSynchronizerDetailDTO>(domainAlipayChargeBillSynchronizerDetail, dtoAlipayChargeBillSynchronizerDetail);
        }

		public static AlipayChargeBillSynchronizerDetailDTO ChangeAlipayChargeBillSynchronizerDetailToDTO(AlipayChargeBillSynchronizerDetail domainAlipayChargeBillSynchronizerDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayChargeBillSynchronizerDetail, AlipayChargeBillSynchronizerDetailDTO>();
            });
            return config.CreateMapper().Map<AlipayChargeBillSynchronizerDetail, AlipayChargeBillSynchronizerDetailDTO>(domainAlipayChargeBillSynchronizerDetail);
        }

		public static List<AlipayChargeBillSynchronizerDetailDTO> ChangeAlipayChargeBillSynchronizerDetailToDTOs(List<AlipayChargeBillSynchronizerDetail> domainAlipayChargeBillSynchronizerDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayChargeBillSynchronizerDetail, AlipayChargeBillSynchronizerDetailDTO>();
            });
            var dtoAlipayChargeBillSynchronizerDetail = config.CreateMapper().Map<List<AlipayChargeBillSynchronizerDetail>, List<AlipayChargeBillSynchronizerDetailDTO>>(domainAlipayChargeBillSynchronizerDetail);

            return dtoAlipayChargeBillSynchronizerDetail;
        }

		public static IEnumerable<AlipayChargeBillSynchronizerDetailDTO> ChangeAlipayChargeBillSynchronizerDetailToDTOs(IEnumerable<AlipayChargeBillSynchronizerDetail> domainAlipayChargeBillSynchronizerDetails)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayChargeBillSynchronizerDetail, AlipayChargeBillSynchronizerDetailDTO>();
            });
            var dtoAlipayChargeBillSynchronizerDetail = config.CreateMapper().Map<IEnumerable<AlipayChargeBillSynchronizerDetail>, IEnumerable<AlipayChargeBillSynchronizerDetailDTO>>(domainAlipayChargeBillSynchronizerDetails);

            return dtoAlipayChargeBillSynchronizerDetail;
        }
	}
}
