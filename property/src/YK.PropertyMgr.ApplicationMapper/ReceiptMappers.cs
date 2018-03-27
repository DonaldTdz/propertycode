using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class ReceiptMappers
	{
		public static Receipt ChangeDTOToReceiptNew(ReceiptDTO dtoReceipt)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptDTO, Receipt>();
            });
            var domainReceipt = config.CreateMapper().Map<ReceiptDTO, Receipt>(dtoReceipt);

            return domainReceipt;
        }

		public static void ChangeDTOToReceiptUpdate(ReceiptDTO dtoReceipt, Receipt domainReceipt)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptDTO, Receipt>();
            });
            config.CreateMapper().Map<ReceiptDTO, Receipt>(dtoReceipt, domainReceipt);
        }

		public static void ChangeReceiptToDTO(ReceiptDTO dtoReceipt, Receipt domainReceipt)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Receipt, ReceiptDTO>();
            });
            config.CreateMapper().Map<Receipt, ReceiptDTO>(domainReceipt, dtoReceipt);
        }

		public static ReceiptDTO ChangeReceiptToDTO(Receipt domainReceipt)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Receipt, ReceiptDTO>();
            });
            return config.CreateMapper().Map<Receipt, ReceiptDTO>(domainReceipt);
        }

		public static List<ReceiptDTO> ChangeReceiptToDTOs(List<Receipt> domainReceipt)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Receipt, ReceiptDTO>();
            });
            var dtoReceipt = config.CreateMapper().Map<List<Receipt>, List<ReceiptDTO>>(domainReceipt);

            return dtoReceipt;
        }

		public static IEnumerable<ReceiptDTO> ChangeReceiptToDTOs(IEnumerable<Receipt> domainReceipts)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Receipt, ReceiptDTO>();
            });
            var dtoReceipt = config.CreateMapper().Map<IEnumerable<Receipt>, IEnumerable<ReceiptDTO>>(domainReceipts);

            return dtoReceipt;
        }
	}
}
