using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class PreferentialRecordMappers
	{
		public static PreferentialRecord ChangeDTOToPreferentialRecordNew(PreferentialRecordDTO dtoPreferentialRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PreferentialRecordDTO, PreferentialRecord>();
            });
            var domainPreferentialRecord = config.CreateMapper().Map<PreferentialRecordDTO, PreferentialRecord>(dtoPreferentialRecord);

            return domainPreferentialRecord;
        }

		public static void ChangeDTOToPreferentialRecordUpdate(PreferentialRecordDTO dtoPreferentialRecord, PreferentialRecord domainPreferentialRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PreferentialRecordDTO, PreferentialRecord>();
            });
            config.CreateMapper().Map<PreferentialRecordDTO, PreferentialRecord>(dtoPreferentialRecord, domainPreferentialRecord);
        }

		public static void ChangePreferentialRecordToDTO(PreferentialRecordDTO dtoPreferentialRecord, PreferentialRecord domainPreferentialRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PreferentialRecord, PreferentialRecordDTO>();
            });
            config.CreateMapper().Map<PreferentialRecord, PreferentialRecordDTO>(domainPreferentialRecord, dtoPreferentialRecord);
        }

		public static PreferentialRecordDTO ChangePreferentialRecordToDTO(PreferentialRecord domainPreferentialRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PreferentialRecord, PreferentialRecordDTO>();
            });
            return config.CreateMapper().Map<PreferentialRecord, PreferentialRecordDTO>(domainPreferentialRecord);
        }

		public static List<PreferentialRecordDTO> ChangePreferentialRecordToDTOs(List<PreferentialRecord> domainPreferentialRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PreferentialRecord, PreferentialRecordDTO>();
            });
            var dtoPreferentialRecord = config.CreateMapper().Map<List<PreferentialRecord>, List<PreferentialRecordDTO>>(domainPreferentialRecord);

            return dtoPreferentialRecord;
        }

		public static IEnumerable<PreferentialRecordDTO> ChangePreferentialRecordToDTOs(IEnumerable<PreferentialRecord> domainPreferentialRecords)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PreferentialRecord, PreferentialRecordDTO>();
            });
            var dtoPreferentialRecord = config.CreateMapper().Map<IEnumerable<PreferentialRecord>, IEnumerable<PreferentialRecordDTO>>(domainPreferentialRecords);

            return dtoPreferentialRecord;
        }
	}
}
