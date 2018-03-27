using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class ReceiptBookHistoryMappers
	{
		public static ReceiptBookHistory ChangeDTOToReceiptBookHistoryNew(ReceiptBookHistoryDTO dtoReceiptBookHistory)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptBookHistoryDTO, ReceiptBookHistory>();
            });
            var domainReceiptBookHistory = config.CreateMapper().Map<ReceiptBookHistoryDTO, ReceiptBookHistory>(dtoReceiptBookHistory);

            return domainReceiptBookHistory;
        }

		public static void ChangeDTOToReceiptBookHistoryUpdate(ReceiptBookHistoryDTO dtoReceiptBookHistory, ReceiptBookHistory domainReceiptBookHistory)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptBookHistoryDTO, ReceiptBookHistory>();
            });
            config.CreateMapper().Map<ReceiptBookHistoryDTO, ReceiptBookHistory>(dtoReceiptBookHistory, domainReceiptBookHistory);
        }

		public static void ChangeReceiptBookHistoryToDTO(ReceiptBookHistoryDTO dtoReceiptBookHistory, ReceiptBookHistory domainReceiptBookHistory)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptBookHistory, ReceiptBookHistoryDTO>();
            });
            config.CreateMapper().Map<ReceiptBookHistory, ReceiptBookHistoryDTO>(domainReceiptBookHistory, dtoReceiptBookHistory);
        }

		public static ReceiptBookHistoryDTO ChangeReceiptBookHistoryToDTO(ReceiptBookHistory domainReceiptBookHistory)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptBookHistory, ReceiptBookHistoryDTO>();
            });
            return config.CreateMapper().Map<ReceiptBookHistory, ReceiptBookHistoryDTO>(domainReceiptBookHistory);
        }

		public static List<ReceiptBookHistoryDTO> ChangeReceiptBookHistoryToDTOs(List<ReceiptBookHistory> domainReceiptBookHistory)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptBookHistory, ReceiptBookHistoryDTO>();
            });
            var dtoReceiptBookHistory = config.CreateMapper().Map<List<ReceiptBookHistory>, List<ReceiptBookHistoryDTO>>(domainReceiptBookHistory);

            return dtoReceiptBookHistory;
        }

		public static IEnumerable<ReceiptBookHistoryDTO> ChangeReceiptBookHistoryToDTOs(IEnumerable<ReceiptBookHistory> domainReceiptBookHistorys)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptBookHistory, ReceiptBookHistoryDTO>();
            });
            var dtoReceiptBookHistory = config.CreateMapper().Map<IEnumerable<ReceiptBookHistory>, IEnumerable<ReceiptBookHistoryDTO>>(domainReceiptBookHistorys);

            return dtoReceiptBookHistory;
        }
	}
}
