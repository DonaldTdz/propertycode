using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class MeterReadRecordMappers
	{
		public static MeterReadRecord ChangeDTOToMeterReadRecordNew(MeterReadRecordDTO dtoMeterReadRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MeterReadRecordDTO, MeterReadRecord>();
            });
            var domainMeterReadRecord = config.CreateMapper().Map<MeterReadRecordDTO, MeterReadRecord>(dtoMeterReadRecord);

            return domainMeterReadRecord;
        }

		public static void ChangeDTOToMeterReadRecordUpdate(MeterReadRecordDTO dtoMeterReadRecord, MeterReadRecord domainMeterReadRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MeterReadRecordDTO, MeterReadRecord>();
            });
            config.CreateMapper().Map<MeterReadRecordDTO, MeterReadRecord>(dtoMeterReadRecord, domainMeterReadRecord);
        }

		public static void ChangeMeterReadRecordToDTO(MeterReadRecordDTO dtoMeterReadRecord, MeterReadRecord domainMeterReadRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MeterReadRecord, MeterReadRecordDTO>();
            });
            config.CreateMapper().Map<MeterReadRecord, MeterReadRecordDTO>(domainMeterReadRecord, dtoMeterReadRecord);
        }

		public static MeterReadRecordDTO ChangeMeterReadRecordToDTO(MeterReadRecord domainMeterReadRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MeterReadRecord, MeterReadRecordDTO>();
            });
            return config.CreateMapper().Map<MeterReadRecord, MeterReadRecordDTO>(domainMeterReadRecord);
        }

		public static List<MeterReadRecordDTO> ChangeMeterReadRecordToDTOs(List<MeterReadRecord> domainMeterReadRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MeterReadRecord, MeterReadRecordDTO>();
            });
            var dtoMeterReadRecord = config.CreateMapper().Map<List<MeterReadRecord>, List<MeterReadRecordDTO>>(domainMeterReadRecord);

            return dtoMeterReadRecord;
        }

		public static IEnumerable<MeterReadRecordDTO> ChangeMeterReadRecordToDTOs(IEnumerable<MeterReadRecord> domainMeterReadRecords)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MeterReadRecord, MeterReadRecordDTO>();
            });
            var dtoMeterReadRecord = config.CreateMapper().Map<IEnumerable<MeterReadRecord>, IEnumerable<MeterReadRecordDTO>>(domainMeterReadRecords);

            return dtoMeterReadRecord;
        }
	}
}
