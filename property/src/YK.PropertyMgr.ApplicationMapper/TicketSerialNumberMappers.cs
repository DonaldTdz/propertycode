using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class TicketSerialNumberMappers
	{
		public static TicketSerialNumber ChangeDTOToTicketSerialNumberNew(TicketSerialNumberDTO dtoTicketSerialNumber)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TicketSerialNumberDTO, TicketSerialNumber>();
            });
            var domainTicketSerialNumber = config.CreateMapper().Map<TicketSerialNumberDTO, TicketSerialNumber>(dtoTicketSerialNumber);

            return domainTicketSerialNumber;
        }

		public static void ChangeDTOToTicketSerialNumberUpdate(TicketSerialNumberDTO dtoTicketSerialNumber, TicketSerialNumber domainTicketSerialNumber)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TicketSerialNumberDTO, TicketSerialNumber>();
            });
            config.CreateMapper().Map<TicketSerialNumberDTO, TicketSerialNumber>(dtoTicketSerialNumber, domainTicketSerialNumber);
        }

		public static void ChangeTicketSerialNumberToDTO(TicketSerialNumberDTO dtoTicketSerialNumber, TicketSerialNumber domainTicketSerialNumber)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TicketSerialNumber, TicketSerialNumberDTO>();
            });
            config.CreateMapper().Map<TicketSerialNumber, TicketSerialNumberDTO>(domainTicketSerialNumber, dtoTicketSerialNumber);
        }

		public static TicketSerialNumberDTO ChangeTicketSerialNumberToDTO(TicketSerialNumber domainTicketSerialNumber)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TicketSerialNumber, TicketSerialNumberDTO>();
            });
            return config.CreateMapper().Map<TicketSerialNumber, TicketSerialNumberDTO>(domainTicketSerialNumber);
        }

		public static List<TicketSerialNumberDTO> ChangeTicketSerialNumberToDTOs(List<TicketSerialNumber> domainTicketSerialNumber)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TicketSerialNumber, TicketSerialNumberDTO>();
            });
            var dtoTicketSerialNumber = config.CreateMapper().Map<List<TicketSerialNumber>, List<TicketSerialNumberDTO>>(domainTicketSerialNumber);

            return dtoTicketSerialNumber;
        }

		public static IEnumerable<TicketSerialNumberDTO> ChangeTicketSerialNumberToDTOs(IEnumerable<TicketSerialNumber> domainTicketSerialNumbers)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TicketSerialNumber, TicketSerialNumberDTO>();
            });
            var dtoTicketSerialNumber = config.CreateMapper().Map<IEnumerable<TicketSerialNumber>, IEnumerable<TicketSerialNumberDTO>>(domainTicketSerialNumbers);

            return dtoTicketSerialNumber;
        }
	}
}
