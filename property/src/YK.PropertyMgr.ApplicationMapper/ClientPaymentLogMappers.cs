using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class ClientPaymentLogMappers
	{
		public static ClientPaymentLog ChangeDTOToClientPaymentLogNew(ClientPaymentLogDTO dtoClientPaymentLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ClientPaymentLogDTO, ClientPaymentLog>();
            });
            var domainClientPaymentLog = config.CreateMapper().Map<ClientPaymentLogDTO, ClientPaymentLog>(dtoClientPaymentLog);

            return domainClientPaymentLog;
        }

		public static void ChangeDTOToClientPaymentLogUpdate(ClientPaymentLogDTO dtoClientPaymentLog, ClientPaymentLog domainClientPaymentLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ClientPaymentLogDTO, ClientPaymentLog>();
            });
            config.CreateMapper().Map<ClientPaymentLogDTO, ClientPaymentLog>(dtoClientPaymentLog, domainClientPaymentLog);
        }

		public static void ChangeClientPaymentLogToDTO(ClientPaymentLogDTO dtoClientPaymentLog, ClientPaymentLog domainClientPaymentLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ClientPaymentLog, ClientPaymentLogDTO>();
            });
            config.CreateMapper().Map<ClientPaymentLog, ClientPaymentLogDTO>(domainClientPaymentLog, dtoClientPaymentLog);
        }

		public static ClientPaymentLogDTO ChangeClientPaymentLogToDTO(ClientPaymentLog domainClientPaymentLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ClientPaymentLog, ClientPaymentLogDTO>();
            });
            return config.CreateMapper().Map<ClientPaymentLog, ClientPaymentLogDTO>(domainClientPaymentLog);
        }

		public static List<ClientPaymentLogDTO> ChangeClientPaymentLogToDTOs(List<ClientPaymentLog> domainClientPaymentLog)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ClientPaymentLog, ClientPaymentLogDTO>();
            });
            var dtoClientPaymentLog = config.CreateMapper().Map<List<ClientPaymentLog>, List<ClientPaymentLogDTO>>(domainClientPaymentLog);

            return dtoClientPaymentLog;
        }

		public static IEnumerable<ClientPaymentLogDTO> ChangeClientPaymentLogToDTOs(IEnumerable<ClientPaymentLog> domainClientPaymentLogs)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ClientPaymentLog, ClientPaymentLogDTO>();
            });
            var dtoClientPaymentLog = config.CreateMapper().Map<IEnumerable<ClientPaymentLog>, IEnumerable<ClientPaymentLogDTO>>(domainClientPaymentLogs);

            return dtoClientPaymentLog;
        }
	}
}
