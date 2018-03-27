using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class RefundRecordMappers
	{
		public static RefundRecord ChangeDTOToRefundRecordNew(RefundRecordDTO dtoRefundRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RefundRecordDTO, RefundRecord>();
            });
            var domainRefundRecord = config.CreateMapper().Map<RefundRecordDTO, RefundRecord>(dtoRefundRecord);

            return domainRefundRecord;
        }

		public static void ChangeDTOToRefundRecordUpdate(RefundRecordDTO dtoRefundRecord, RefundRecord domainRefundRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RefundRecordDTO, RefundRecord>();
            });
            config.CreateMapper().Map<RefundRecordDTO, RefundRecord>(dtoRefundRecord, domainRefundRecord);
        }

		public static void ChangeRefundRecordToDTO(RefundRecordDTO dtoRefundRecord, RefundRecord domainRefundRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RefundRecord, RefundRecordDTO>();
            });
            config.CreateMapper().Map<RefundRecord, RefundRecordDTO>(domainRefundRecord, dtoRefundRecord);
        }

		public static RefundRecordDTO ChangeRefundRecordToDTO(RefundRecord domainRefundRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RefundRecord, RefundRecordDTO>();
            });
            return config.CreateMapper().Map<RefundRecord, RefundRecordDTO>(domainRefundRecord);
        }

		public static List<RefundRecordDTO> ChangeRefundRecordToDTOs(List<RefundRecord> domainRefundRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RefundRecord, RefundRecordDTO>();
            });
            var dtoRefundRecord = config.CreateMapper().Map<List<RefundRecord>, List<RefundRecordDTO>>(domainRefundRecord);

            return dtoRefundRecord;
        }

		public static IEnumerable<RefundRecordDTO> ChangeRefundRecordToDTOs(IEnumerable<RefundRecord> domainRefundRecords)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RefundRecord, RefundRecordDTO>();
            });
            var dtoRefundRecord = config.CreateMapper().Map<IEnumerable<RefundRecord>, IEnumerable<RefundRecordDTO>>(domainRefundRecords);

            return dtoRefundRecord;
        }
	}
}
