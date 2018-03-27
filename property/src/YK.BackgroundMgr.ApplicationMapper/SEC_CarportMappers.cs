using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class SEC_CarportMappers
	{
		public static SEC_Carport ChangeDTOToSEC_CarportNew(SEC_CarportDTO dtoSEC_Carport)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_CarportDTO, SEC_Carport>();
            });
            var domainSEC_Carport = config.CreateMapper().Map<SEC_CarportDTO, SEC_Carport>(dtoSEC_Carport);

            return domainSEC_Carport;
        }

		public static void ChangeDTOToSEC_CarportUpdate(SEC_CarportDTO dtoSEC_Carport, SEC_Carport domainSEC_Carport)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_CarportDTO, SEC_Carport>();
            });
            config.CreateMapper().Map<SEC_CarportDTO, SEC_Carport>(dtoSEC_Carport, domainSEC_Carport);
        }

		public static void ChangeSEC_CarportToDTO(SEC_CarportDTO dtoSEC_Carport, SEC_Carport domainSEC_Carport)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Carport, SEC_CarportDTO>();
            });
            config.CreateMapper().Map<SEC_Carport, SEC_CarportDTO>(domainSEC_Carport, dtoSEC_Carport);
        }

		public static SEC_CarportDTO ChangeSEC_CarportToDTO(SEC_Carport domainSEC_Carport)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Carport, SEC_CarportDTO>();
            });
            return config.CreateMapper().Map<SEC_Carport, SEC_CarportDTO>(domainSEC_Carport);
        }

		public static List<SEC_CarportDTO> ChangeSEC_CarportToDTOs(List<SEC_Carport> domainSEC_Carport)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Carport, SEC_CarportDTO>();
            });
            var dtoSEC_Carport = config.CreateMapper().Map<List<SEC_Carport>, List<SEC_CarportDTO>>(domainSEC_Carport);

            return dtoSEC_Carport;
        }

		public static IEnumerable<SEC_CarportDTO> ChangeSEC_CarportToDTOs(IEnumerable<SEC_Carport> domainSEC_Carports)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Carport, SEC_CarportDTO>();
            });
            var dtoSEC_Carport = config.CreateMapper().Map<IEnumerable<SEC_Carport>, IEnumerable<SEC_CarportDTO>>(domainSEC_Carports);

            return dtoSEC_Carport;
        }
	}
}
