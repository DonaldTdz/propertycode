using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class ReceiptBookDetailMappers
	{
		public static ReceiptBookDetail ChangeDTOToReceiptBookDetailNew(ReceiptBookDetailDTO dtoReceiptBookDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptBookDetailDTO, ReceiptBookDetail>();
            });
            var domainReceiptBookDetail = config.CreateMapper().Map<ReceiptBookDetailDTO, ReceiptBookDetail>(dtoReceiptBookDetail);

            return domainReceiptBookDetail;
        }

		public static void ChangeDTOToReceiptBookDetailUpdate(ReceiptBookDetailDTO dtoReceiptBookDetail, ReceiptBookDetail domainReceiptBookDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptBookDetailDTO, ReceiptBookDetail>();
            });
            config.CreateMapper().Map<ReceiptBookDetailDTO, ReceiptBookDetail>(dtoReceiptBookDetail, domainReceiptBookDetail);
        }

		public static void ChangeReceiptBookDetailToDTO(ReceiptBookDetailDTO dtoReceiptBookDetail, ReceiptBookDetail domainReceiptBookDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptBookDetail, ReceiptBookDetailDTO>();
            });
            config.CreateMapper().Map<ReceiptBookDetail, ReceiptBookDetailDTO>(domainReceiptBookDetail, dtoReceiptBookDetail);
        }

		public static ReceiptBookDetailDTO ChangeReceiptBookDetailToDTO(ReceiptBookDetail domainReceiptBookDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptBookDetail, ReceiptBookDetailDTO>();
            });
            return config.CreateMapper().Map<ReceiptBookDetail, ReceiptBookDetailDTO>(domainReceiptBookDetail);
        }

		public static List<ReceiptBookDetailDTO> ChangeReceiptBookDetailToDTOs(List<ReceiptBookDetail> domainReceiptBookDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptBookDetail, ReceiptBookDetailDTO>();
            });
            var dtoReceiptBookDetail = config.CreateMapper().Map<List<ReceiptBookDetail>, List<ReceiptBookDetailDTO>>(domainReceiptBookDetail);

            return dtoReceiptBookDetail;
        }

		public static IEnumerable<ReceiptBookDetailDTO> ChangeReceiptBookDetailToDTOs(IEnumerable<ReceiptBookDetail> domainReceiptBookDetails)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceiptBookDetail, ReceiptBookDetailDTO>();
            });
            var dtoReceiptBookDetail = config.CreateMapper().Map<IEnumerable<ReceiptBookDetail>, IEnumerable<ReceiptBookDetailDTO>>(domainReceiptBookDetails);

            return dtoReceiptBookDetail;
        }
	}
}
