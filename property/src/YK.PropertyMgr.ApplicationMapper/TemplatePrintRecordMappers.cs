using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class TemplatePrintRecordMappers
	{
		public static TemplatePrintRecord ChangeDTOToTemplatePrintRecordNew(TemplatePrintRecordDTO dtoTemplatePrintRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TemplatePrintRecordDTO, TemplatePrintRecord>();
            });
            var domainTemplatePrintRecord = config.CreateMapper().Map<TemplatePrintRecordDTO, TemplatePrintRecord>(dtoTemplatePrintRecord);

            return domainTemplatePrintRecord;
        }

		public static void ChangeDTOToTemplatePrintRecordUpdate(TemplatePrintRecordDTO dtoTemplatePrintRecord, TemplatePrintRecord domainTemplatePrintRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TemplatePrintRecordDTO, TemplatePrintRecord>();
            });
            config.CreateMapper().Map<TemplatePrintRecordDTO, TemplatePrintRecord>(dtoTemplatePrintRecord, domainTemplatePrintRecord);
        }

		public static void ChangeTemplatePrintRecordToDTO(TemplatePrintRecordDTO dtoTemplatePrintRecord, TemplatePrintRecord domainTemplatePrintRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TemplatePrintRecord, TemplatePrintRecordDTO>();
            });
            config.CreateMapper().Map<TemplatePrintRecord, TemplatePrintRecordDTO>(domainTemplatePrintRecord, dtoTemplatePrintRecord);
        }

		public static TemplatePrintRecordDTO ChangeTemplatePrintRecordToDTO(TemplatePrintRecord domainTemplatePrintRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TemplatePrintRecord, TemplatePrintRecordDTO>();
            });
            return config.CreateMapper().Map<TemplatePrintRecord, TemplatePrintRecordDTO>(domainTemplatePrintRecord);
        }

		public static List<TemplatePrintRecordDTO> ChangeTemplatePrintRecordToDTOs(List<TemplatePrintRecord> domainTemplatePrintRecord)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TemplatePrintRecord, TemplatePrintRecordDTO>();
            });
            var dtoTemplatePrintRecord = config.CreateMapper().Map<List<TemplatePrintRecord>, List<TemplatePrintRecordDTO>>(domainTemplatePrintRecord);

            return dtoTemplatePrintRecord;
        }

		public static IEnumerable<TemplatePrintRecordDTO> ChangeTemplatePrintRecordToDTOs(IEnumerable<TemplatePrintRecord> domainTemplatePrintRecords)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TemplatePrintRecord, TemplatePrintRecordDTO>();
            });
            var dtoTemplatePrintRecord = config.CreateMapper().Map<IEnumerable<TemplatePrintRecord>, IEnumerable<TemplatePrintRecordDTO>>(domainTemplatePrintRecords);

            return dtoTemplatePrintRecord;
        }
	}
}
