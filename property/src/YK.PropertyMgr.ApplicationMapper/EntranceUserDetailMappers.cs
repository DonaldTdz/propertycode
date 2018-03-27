using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class EntranceUserDetailMappers
	{
		public static EntranceUserDetail ChangeDTOToEntranceUserDetailNew(EntranceUserDetailDTO dtoEntranceUserDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceUserDetailDTO, EntranceUserDetail>();
            });
            var domainEntranceUserDetail = config.CreateMapper().Map<EntranceUserDetailDTO, EntranceUserDetail>(dtoEntranceUserDetail);

            return domainEntranceUserDetail;
        }

		public static void ChangeDTOToEntranceUserDetailUpdate(EntranceUserDetailDTO dtoEntranceUserDetail, EntranceUserDetail domainEntranceUserDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceUserDetailDTO, EntranceUserDetail>();
            });
            config.CreateMapper().Map<EntranceUserDetailDTO, EntranceUserDetail>(dtoEntranceUserDetail, domainEntranceUserDetail);
        }

		public static void ChangeEntranceUserDetailToDTO(EntranceUserDetailDTO dtoEntranceUserDetail, EntranceUserDetail domainEntranceUserDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceUserDetail, EntranceUserDetailDTO>();
            });
            config.CreateMapper().Map<EntranceUserDetail, EntranceUserDetailDTO>(domainEntranceUserDetail, dtoEntranceUserDetail);
        }

		public static EntranceUserDetailDTO ChangeEntranceUserDetailToDTO(EntranceUserDetail domainEntranceUserDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceUserDetail, EntranceUserDetailDTO>();
            });
            return config.CreateMapper().Map<EntranceUserDetail, EntranceUserDetailDTO>(domainEntranceUserDetail);
        }

		public static List<EntranceUserDetailDTO> ChangeEntranceUserDetailToDTOs(List<EntranceUserDetail> domainEntranceUserDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceUserDetail, EntranceUserDetailDTO>();
            });
            var dtoEntranceUserDetail = config.CreateMapper().Map<List<EntranceUserDetail>, List<EntranceUserDetailDTO>>(domainEntranceUserDetail);

            return dtoEntranceUserDetail;
        }

		public static IEnumerable<EntranceUserDetailDTO> ChangeEntranceUserDetailToDTOs(IEnumerable<EntranceUserDetail> domainEntranceUserDetails)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceUserDetail, EntranceUserDetailDTO>();
            });
            var dtoEntranceUserDetail = config.CreateMapper().Map<IEnumerable<EntranceUserDetail>, IEnumerable<EntranceUserDetailDTO>>(domainEntranceUserDetails);

            return dtoEntranceUserDetail;
        }
	}
}
