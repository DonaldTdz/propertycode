using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class ChargBillMappers
	{
		public static ChargBill ChangeDTOToChargBillNew(ChargBillDTO dtoChargBill)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargBillDTO, ChargBill>();
            });
            var domainChargBill = config.CreateMapper().Map<ChargBillDTO, ChargBill>(dtoChargBill);

            return domainChargBill;
        }

		public static void ChangeDTOToChargBillUpdate(ChargBillDTO dtoChargBill, ChargBill domainChargBill)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargBillDTO, ChargBill>();
            });
            config.CreateMapper().Map<ChargBillDTO, ChargBill>(dtoChargBill, domainChargBill);
        }

		public static void ChangeChargBillToDTO(ChargBillDTO dtoChargBill, ChargBill domainChargBill)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargBill, ChargBillDTO>();
            });
            config.CreateMapper().Map<ChargBill, ChargBillDTO>(domainChargBill, dtoChargBill);
        }

		public static ChargBillDTO ChangeChargBillToDTO(ChargBill domainChargBill)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargBill, ChargBillDTO>();
            });
            return config.CreateMapper().Map<ChargBill, ChargBillDTO>(domainChargBill);
        }

		public static List<ChargBillDTO> ChangeChargBillToDTOs(List<ChargBill> domainChargBill)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargBill, ChargBillDTO>();
            });
            var dtoChargBill = config.CreateMapper().Map<List<ChargBill>, List<ChargBillDTO>>(domainChargBill);

            return dtoChargBill;
        }

		public static IEnumerable<ChargBillDTO> ChangeChargBillToDTOs(IEnumerable<ChargBill> domainChargBills)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargBill, ChargBillDTO>();
            });
            var dtoChargBill = config.CreateMapper().Map<IEnumerable<ChargBill>, IEnumerable<ChargBillDTO>>(domainChargBills);

            return dtoChargBill;
        }
	}
}
