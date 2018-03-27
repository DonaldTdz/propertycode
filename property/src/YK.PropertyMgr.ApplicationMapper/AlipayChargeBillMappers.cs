using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class AlipayChargeBillMappers
	{
		public static AlipayChargeBill ChangeDTOToAlipayChargeBillNew(AlipayChargeBillDTO dtoAlipayChargeBill)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayChargeBillDTO, AlipayChargeBill>();
            });
            var domainAlipayChargeBill = config.CreateMapper().Map<AlipayChargeBillDTO, AlipayChargeBill>(dtoAlipayChargeBill);

            return domainAlipayChargeBill;
        }

		public static void ChangeDTOToAlipayChargeBillUpdate(AlipayChargeBillDTO dtoAlipayChargeBill, AlipayChargeBill domainAlipayChargeBill)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayChargeBillDTO, AlipayChargeBill>();
            });
            config.CreateMapper().Map<AlipayChargeBillDTO, AlipayChargeBill>(dtoAlipayChargeBill, domainAlipayChargeBill);
        }

		public static void ChangeAlipayChargeBillToDTO(AlipayChargeBillDTO dtoAlipayChargeBill, AlipayChargeBill domainAlipayChargeBill)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayChargeBill, AlipayChargeBillDTO>();
            });
            config.CreateMapper().Map<AlipayChargeBill, AlipayChargeBillDTO>(domainAlipayChargeBill, dtoAlipayChargeBill);
        }

		public static AlipayChargeBillDTO ChangeAlipayChargeBillToDTO(AlipayChargeBill domainAlipayChargeBill)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayChargeBill, AlipayChargeBillDTO>();
            });
            return config.CreateMapper().Map<AlipayChargeBill, AlipayChargeBillDTO>(domainAlipayChargeBill);
        }

		public static List<AlipayChargeBillDTO> ChangeAlipayChargeBillToDTOs(List<AlipayChargeBill> domainAlipayChargeBill)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayChargeBill, AlipayChargeBillDTO>();
            });
            var dtoAlipayChargeBill = config.CreateMapper().Map<List<AlipayChargeBill>, List<AlipayChargeBillDTO>>(domainAlipayChargeBill);

            return dtoAlipayChargeBill;
        }

		public static IEnumerable<AlipayChargeBillDTO> ChangeAlipayChargeBillToDTOs(IEnumerable<AlipayChargeBill> domainAlipayChargeBills)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayChargeBill, AlipayChargeBillDTO>();
            });
            var dtoAlipayChargeBill = config.CreateMapper().Map<IEnumerable<AlipayChargeBill>, IEnumerable<AlipayChargeBillDTO>>(domainAlipayChargeBills);

            return dtoAlipayChargeBill;
        }
	}
}
