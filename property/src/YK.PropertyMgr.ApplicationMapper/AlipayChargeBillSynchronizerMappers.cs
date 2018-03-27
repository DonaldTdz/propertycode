using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class AlipayChargeBillSynchronizerMappers
	{
		public static AlipayChargeBillSynchronizer ChangeDTOToAlipayChargeBillSynchronizerNew(AlipayChargeBillSynchronizerDTO dtoAlipayChargeBillSynchronizer)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayChargeBillSynchronizerDTO, AlipayChargeBillSynchronizer>();
            });
            var domainAlipayChargeBillSynchronizer = config.CreateMapper().Map<AlipayChargeBillSynchronizerDTO, AlipayChargeBillSynchronizer>(dtoAlipayChargeBillSynchronizer);

            return domainAlipayChargeBillSynchronizer;
        }

		public static void ChangeDTOToAlipayChargeBillSynchronizerUpdate(AlipayChargeBillSynchronizerDTO dtoAlipayChargeBillSynchronizer, AlipayChargeBillSynchronizer domainAlipayChargeBillSynchronizer)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayChargeBillSynchronizerDTO, AlipayChargeBillSynchronizer>();
            });
            config.CreateMapper().Map<AlipayChargeBillSynchronizerDTO, AlipayChargeBillSynchronizer>(dtoAlipayChargeBillSynchronizer, domainAlipayChargeBillSynchronizer);
        }

		public static void ChangeAlipayChargeBillSynchronizerToDTO(AlipayChargeBillSynchronizerDTO dtoAlipayChargeBillSynchronizer, AlipayChargeBillSynchronizer domainAlipayChargeBillSynchronizer)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayChargeBillSynchronizer, AlipayChargeBillSynchronizerDTO>();
            });
            config.CreateMapper().Map<AlipayChargeBillSynchronizer, AlipayChargeBillSynchronizerDTO>(domainAlipayChargeBillSynchronizer, dtoAlipayChargeBillSynchronizer);
        }

		public static AlipayChargeBillSynchronizerDTO ChangeAlipayChargeBillSynchronizerToDTO(AlipayChargeBillSynchronizer domainAlipayChargeBillSynchronizer)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayChargeBillSynchronizer, AlipayChargeBillSynchronizerDTO>();
            });
            return config.CreateMapper().Map<AlipayChargeBillSynchronizer, AlipayChargeBillSynchronizerDTO>(domainAlipayChargeBillSynchronizer);
        }

		public static List<AlipayChargeBillSynchronizerDTO> ChangeAlipayChargeBillSynchronizerToDTOs(List<AlipayChargeBillSynchronizer> domainAlipayChargeBillSynchronizer)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayChargeBillSynchronizer, AlipayChargeBillSynchronizerDTO>();
            });
            var dtoAlipayChargeBillSynchronizer = config.CreateMapper().Map<List<AlipayChargeBillSynchronizer>, List<AlipayChargeBillSynchronizerDTO>>(domainAlipayChargeBillSynchronizer);

            return dtoAlipayChargeBillSynchronizer;
        }

		public static IEnumerable<AlipayChargeBillSynchronizerDTO> ChangeAlipayChargeBillSynchronizerToDTOs(IEnumerable<AlipayChargeBillSynchronizer> domainAlipayChargeBillSynchronizers)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayChargeBillSynchronizer, AlipayChargeBillSynchronizerDTO>();
            });
            var dtoAlipayChargeBillSynchronizer = config.CreateMapper().Map<IEnumerable<AlipayChargeBillSynchronizer>, IEnumerable<AlipayChargeBillSynchronizerDTO>>(domainAlipayChargeBillSynchronizers);

            return dtoAlipayChargeBillSynchronizer;
        }
	}
}
