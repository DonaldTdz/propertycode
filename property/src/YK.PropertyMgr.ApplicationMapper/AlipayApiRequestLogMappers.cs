using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class AlipayApiRequestLogMappers
	{
		public static AlipayApiRequestLog ChangeDTOToAlipayApiRequestLogNew(AlipayApiRequestLogDTO dtoAlipayApiRequestLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayApiRequestLogDTO, AlipayApiRequestLog>();
            });
            var domainAlipayApiRequestLog = config.CreateMapper().Map<AlipayApiRequestLogDTO, AlipayApiRequestLog>(dtoAlipayApiRequestLog);

            return domainAlipayApiRequestLog;
        }

		public static void ChangeDTOToAlipayApiRequestLogUpdate(AlipayApiRequestLogDTO dtoAlipayApiRequestLog, AlipayApiRequestLog domainAlipayApiRequestLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayApiRequestLogDTO, AlipayApiRequestLog>();
            });
            config.CreateMapper().Map<AlipayApiRequestLogDTO, AlipayApiRequestLog>(dtoAlipayApiRequestLog, domainAlipayApiRequestLog);
        }

		public static void ChangeAlipayApiRequestLogToDTO(AlipayApiRequestLogDTO dtoAlipayApiRequestLog, AlipayApiRequestLog domainAlipayApiRequestLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayApiRequestLog, AlipayApiRequestLogDTO>();
            });
            config.CreateMapper().Map<AlipayApiRequestLog, AlipayApiRequestLogDTO>(domainAlipayApiRequestLog, dtoAlipayApiRequestLog);
        }

		public static AlipayApiRequestLogDTO ChangeAlipayApiRequestLogToDTO(AlipayApiRequestLog domainAlipayApiRequestLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayApiRequestLog, AlipayApiRequestLogDTO>();
            });
            return config.CreateMapper().Map<AlipayApiRequestLog, AlipayApiRequestLogDTO>(domainAlipayApiRequestLog);
        }

		public static List<AlipayApiRequestLogDTO> ChangeAlipayApiRequestLogToDTOs(List<AlipayApiRequestLog> domainAlipayApiRequestLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayApiRequestLog, AlipayApiRequestLogDTO>();
            });
            var dtoAlipayApiRequestLog = config.CreateMapper().Map<List<AlipayApiRequestLog>, List<AlipayApiRequestLogDTO>>(domainAlipayApiRequestLog);

            return dtoAlipayApiRequestLog;
        }

		public static IEnumerable<AlipayApiRequestLogDTO> ChangeAlipayApiRequestLogToDTOs(IEnumerable<AlipayApiRequestLog> domainAlipayApiRequestLogs)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayApiRequestLog, AlipayApiRequestLogDTO>();
            });
            var dtoAlipayApiRequestLog = config.CreateMapper().Map<IEnumerable<AlipayApiRequestLog>, IEnumerable<AlipayApiRequestLogDTO>>(domainAlipayApiRequestLogs);

            return dtoAlipayApiRequestLog;
        }
	}
}
