using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class ChargeRecordMappers
	{
		public static ChargeRecord ChangeDTOToChargeRecordNew(ChargeRecordDTO dtoChargeRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeRecordDTO, ChargeRecord>();
            });
            var domainChargeRecord = config.CreateMapper().Map<ChargeRecordDTO, ChargeRecord>(dtoChargeRecord);

            return domainChargeRecord;
        }

		public static void ChangeDTOToChargeRecordUpdate(ChargeRecordDTO dtoChargeRecord, ChargeRecord domainChargeRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeRecordDTO, ChargeRecord>();
            });
            config.CreateMapper().Map<ChargeRecordDTO, ChargeRecord>(dtoChargeRecord, domainChargeRecord);
        }

		public static void ChangeChargeRecordToDTO(ChargeRecordDTO dtoChargeRecord, ChargeRecord domainChargeRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeRecord, ChargeRecordDTO>();
            });
            config.CreateMapper().Map<ChargeRecord, ChargeRecordDTO>(domainChargeRecord, dtoChargeRecord);
        }

		public static ChargeRecordDTO ChangeChargeRecordToDTO(ChargeRecord domainChargeRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeRecord, ChargeRecordDTO>();
            });
            return config.CreateMapper().Map<ChargeRecord, ChargeRecordDTO>(domainChargeRecord);
        }

		public static List<ChargeRecordDTO> ChangeChargeRecordToDTOs(List<ChargeRecord> domainChargeRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeRecord, ChargeRecordDTO>();
            });
            var dtoChargeRecord = config.CreateMapper().Map<List<ChargeRecord>, List<ChargeRecordDTO>>(domainChargeRecord);

            return dtoChargeRecord;
        }

		public static IEnumerable<ChargeRecordDTO> ChangeChargeRecordToDTOs(IEnumerable<ChargeRecord> domainChargeRecords)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeRecord, ChargeRecordDTO>();
            });
            var dtoChargeRecord = config.CreateMapper().Map<IEnumerable<ChargeRecord>, IEnumerable<ChargeRecordDTO>>(domainChargeRecords);

            return dtoChargeRecord;
        }
	}
}
