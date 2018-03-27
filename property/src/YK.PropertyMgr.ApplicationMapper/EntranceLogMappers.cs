using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class EntranceLogMappers
	{
		public static EntranceLog ChangeDTOToEntranceLogNew(EntranceLogDTO dtoEntranceLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceLogDTO, EntranceLog>();
            });
            var domainEntranceLog = config.CreateMapper().Map<EntranceLogDTO, EntranceLog>(dtoEntranceLog);

            return domainEntranceLog;
        }

		public static void ChangeDTOToEntranceLogUpdate(EntranceLogDTO dtoEntranceLog, EntranceLog domainEntranceLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceLogDTO, EntranceLog>();
            });
            config.CreateMapper().Map<EntranceLogDTO, EntranceLog>(dtoEntranceLog, domainEntranceLog);
        }

		public static void ChangeEntranceLogToDTO(EntranceLogDTO dtoEntranceLog, EntranceLog domainEntranceLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceLog, EntranceLogDTO>();
            });
            config.CreateMapper().Map<EntranceLog, EntranceLogDTO>(domainEntranceLog, dtoEntranceLog);
        }

		public static EntranceLogDTO ChangeEntranceLogToDTO(EntranceLog domainEntranceLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceLog, EntranceLogDTO>();
            });
            return config.CreateMapper().Map<EntranceLog, EntranceLogDTO>(domainEntranceLog);
        }

		public static List<EntranceLogDTO> ChangeEntranceLogToDTOs(List<EntranceLog> domainEntranceLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceLog, EntranceLogDTO>();
            });
            var dtoEntranceLog = config.CreateMapper().Map<List<EntranceLog>, List<EntranceLogDTO>>(domainEntranceLog);

            return dtoEntranceLog;
        }

		public static IEnumerable<EntranceLogDTO> ChangeEntranceLogToDTOs(IEnumerable<EntranceLog> domainEntranceLogs)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceLog, EntranceLogDTO>();
            });
            var dtoEntranceLog = config.CreateMapper().Map<IEnumerable<EntranceLog>, IEnumerable<EntranceLogDTO>>(domainEntranceLogs);

            return dtoEntranceLog;
        }
	}
}
