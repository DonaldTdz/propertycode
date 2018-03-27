using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class PrepayAccountDetailMappers
	{
		public static PrepayAccountDetail ChangeDTOToPrepayAccountDetailNew(PrepayAccountDetailDTO dtoPrepayAccountDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrepayAccountDetailDTO, PrepayAccountDetail>();
            });
            var domainPrepayAccountDetail = config.CreateMapper().Map<PrepayAccountDetailDTO, PrepayAccountDetail>(dtoPrepayAccountDetail);

            return domainPrepayAccountDetail;
        }

		public static void ChangeDTOToPrepayAccountDetailUpdate(PrepayAccountDetailDTO dtoPrepayAccountDetail, PrepayAccountDetail domainPrepayAccountDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrepayAccountDetailDTO, PrepayAccountDetail>();
            });
            config.CreateMapper().Map<PrepayAccountDetailDTO, PrepayAccountDetail>(dtoPrepayAccountDetail, domainPrepayAccountDetail);
        }

		public static void ChangePrepayAccountDetailToDTO(PrepayAccountDetailDTO dtoPrepayAccountDetail, PrepayAccountDetail domainPrepayAccountDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrepayAccountDetail, PrepayAccountDetailDTO>();
            });
            config.CreateMapper().Map<PrepayAccountDetail, PrepayAccountDetailDTO>(domainPrepayAccountDetail, dtoPrepayAccountDetail);
        }

		public static PrepayAccountDetailDTO ChangePrepayAccountDetailToDTO(PrepayAccountDetail domainPrepayAccountDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrepayAccountDetail, PrepayAccountDetailDTO>();
            });
            return config.CreateMapper().Map<PrepayAccountDetail, PrepayAccountDetailDTO>(domainPrepayAccountDetail);
        }

		public static List<PrepayAccountDetailDTO> ChangePrepayAccountDetailToDTOs(List<PrepayAccountDetail> domainPrepayAccountDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrepayAccountDetail, PrepayAccountDetailDTO>();
            });
            var dtoPrepayAccountDetail = config.CreateMapper().Map<List<PrepayAccountDetail>, List<PrepayAccountDetailDTO>>(domainPrepayAccountDetail);

            return dtoPrepayAccountDetail;
        }

		public static IEnumerable<PrepayAccountDetailDTO> ChangePrepayAccountDetailToDTOs(IEnumerable<PrepayAccountDetail> domainPrepayAccountDetails)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrepayAccountDetail, PrepayAccountDetailDTO>();
            });
            var dtoPrepayAccountDetail = config.CreateMapper().Map<IEnumerable<PrepayAccountDetail>, IEnumerable<PrepayAccountDetailDTO>>(domainPrepayAccountDetails);

            return dtoPrepayAccountDetail;
        }
	}
}
