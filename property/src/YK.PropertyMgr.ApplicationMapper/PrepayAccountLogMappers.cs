using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class PrepayAccountLogMappers
	{
		public static PrepayAccountLog ChangeDTOToPrepayAccountLogNew(PrepayAccountLogDTO dtoPrepayAccountLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrepayAccountLogDTO, PrepayAccountLog>();
            });
            var domainPrepayAccountLog = config.CreateMapper().Map<PrepayAccountLogDTO, PrepayAccountLog>(dtoPrepayAccountLog);

            return domainPrepayAccountLog;
        }

		public static void ChangeDTOToPrepayAccountLogUpdate(PrepayAccountLogDTO dtoPrepayAccountLog, PrepayAccountLog domainPrepayAccountLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrepayAccountLogDTO, PrepayAccountLog>();
            });
            config.CreateMapper().Map<PrepayAccountLogDTO, PrepayAccountLog>(dtoPrepayAccountLog, domainPrepayAccountLog);
        }

		public static void ChangePrepayAccountLogToDTO(PrepayAccountLogDTO dtoPrepayAccountLog, PrepayAccountLog domainPrepayAccountLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrepayAccountLog, PrepayAccountLogDTO>();
            });
            config.CreateMapper().Map<PrepayAccountLog, PrepayAccountLogDTO>(domainPrepayAccountLog, dtoPrepayAccountLog);
        }

		public static PrepayAccountLogDTO ChangePrepayAccountLogToDTO(PrepayAccountLog domainPrepayAccountLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrepayAccountLog, PrepayAccountLogDTO>();
            });
            return config.CreateMapper().Map<PrepayAccountLog, PrepayAccountLogDTO>(domainPrepayAccountLog);
        }

		public static List<PrepayAccountLogDTO> ChangePrepayAccountLogToDTOs(List<PrepayAccountLog> domainPrepayAccountLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrepayAccountLog, PrepayAccountLogDTO>();
            });
            var dtoPrepayAccountLog = config.CreateMapper().Map<List<PrepayAccountLog>, List<PrepayAccountLogDTO>>(domainPrepayAccountLog);

            return dtoPrepayAccountLog;
        }

		public static IEnumerable<PrepayAccountLogDTO> ChangePrepayAccountLogToDTOs(IEnumerable<PrepayAccountLog> domainPrepayAccountLogs)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrepayAccountLog, PrepayAccountLogDTO>();
            });
            var dtoPrepayAccountLog = config.CreateMapper().Map<IEnumerable<PrepayAccountLog>, IEnumerable<PrepayAccountLogDTO>>(domainPrepayAccountLogs);

            return dtoPrepayAccountLog;
        }
	}
}
