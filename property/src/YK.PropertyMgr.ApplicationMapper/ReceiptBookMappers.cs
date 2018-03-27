using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class ReceiptBookMappers
	{
		public static ReceiptBook ChangeDTOToReceiptBookNew(ReceiptBookDTO dtoReceiptBook)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptBookDTO, ReceiptBook>();
            });
            var domainReceiptBook = config.CreateMapper().Map<ReceiptBookDTO, ReceiptBook>(dtoReceiptBook);

            return domainReceiptBook;
        }

		public static void ChangeDTOToReceiptBookUpdate(ReceiptBookDTO dtoReceiptBook, ReceiptBook domainReceiptBook)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptBookDTO, ReceiptBook>();
            });
            config.CreateMapper().Map<ReceiptBookDTO, ReceiptBook>(dtoReceiptBook, domainReceiptBook);
        }

		public static void ChangeReceiptBookToDTO(ReceiptBookDTO dtoReceiptBook, ReceiptBook domainReceiptBook)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptBook, ReceiptBookDTO>();
            });
            config.CreateMapper().Map<ReceiptBook, ReceiptBookDTO>(domainReceiptBook, dtoReceiptBook);
        }

		public static ReceiptBookDTO ChangeReceiptBookToDTO(ReceiptBook domainReceiptBook)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptBook, ReceiptBookDTO>();
            });
            return config.CreateMapper().Map<ReceiptBook, ReceiptBookDTO>(domainReceiptBook);
        }

		public static List<ReceiptBookDTO> ChangeReceiptBookToDTOs(List<ReceiptBook> domainReceiptBook)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptBook, ReceiptBookDTO>();
            });
            var dtoReceiptBook = config.CreateMapper().Map<List<ReceiptBook>, List<ReceiptBookDTO>>(domainReceiptBook);

            return dtoReceiptBook;
        }

		public static IEnumerable<ReceiptBookDTO> ChangeReceiptBookToDTOs(IEnumerable<ReceiptBook> domainReceiptBooks)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptBook, ReceiptBookDTO>();
            });
            var dtoReceiptBook = config.CreateMapper().Map<IEnumerable<ReceiptBook>, IEnumerable<ReceiptBookDTO>>(domainReceiptBooks);

            return dtoReceiptBook;
        }
	}
}
