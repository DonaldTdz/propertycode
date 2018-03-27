using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class TemplatePrintRecordDetailMappers
	{
		public static TemplatePrintRecordDetail ChangeDTOToTemplatePrintRecordDetailNew(TemplatePrintRecordDetailDTO dtoTemplatePrintRecordDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TemplatePrintRecordDetailDTO, TemplatePrintRecordDetail>();
            });
            var domainTemplatePrintRecordDetail = config.CreateMapper().Map<TemplatePrintRecordDetailDTO, TemplatePrintRecordDetail>(dtoTemplatePrintRecordDetail);

            return domainTemplatePrintRecordDetail;
        }

		public static void ChangeDTOToTemplatePrintRecordDetailUpdate(TemplatePrintRecordDetailDTO dtoTemplatePrintRecordDetail, TemplatePrintRecordDetail domainTemplatePrintRecordDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TemplatePrintRecordDetailDTO, TemplatePrintRecordDetail>();
            });
            config.CreateMapper().Map<TemplatePrintRecordDetailDTO, TemplatePrintRecordDetail>(dtoTemplatePrintRecordDetail, domainTemplatePrintRecordDetail);
        }

		public static void ChangeTemplatePrintRecordDetailToDTO(TemplatePrintRecordDetailDTO dtoTemplatePrintRecordDetail, TemplatePrintRecordDetail domainTemplatePrintRecordDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TemplatePrintRecordDetail, TemplatePrintRecordDetailDTO>();
            });
            config.CreateMapper().Map<TemplatePrintRecordDetail, TemplatePrintRecordDetailDTO>(domainTemplatePrintRecordDetail, dtoTemplatePrintRecordDetail);
        }

		public static TemplatePrintRecordDetailDTO ChangeTemplatePrintRecordDetailToDTO(TemplatePrintRecordDetail domainTemplatePrintRecordDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TemplatePrintRecordDetail, TemplatePrintRecordDetailDTO>();
            });
            return config.CreateMapper().Map<TemplatePrintRecordDetail, TemplatePrintRecordDetailDTO>(domainTemplatePrintRecordDetail);
        }

		public static List<TemplatePrintRecordDetailDTO> ChangeTemplatePrintRecordDetailToDTOs(List<TemplatePrintRecordDetail> domainTemplatePrintRecordDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TemplatePrintRecordDetail, TemplatePrintRecordDetailDTO>();
            });
            var dtoTemplatePrintRecordDetail = config.CreateMapper().Map<List<TemplatePrintRecordDetail>, List<TemplatePrintRecordDetailDTO>>(domainTemplatePrintRecordDetail);

            return dtoTemplatePrintRecordDetail;
        }

		public static IEnumerable<TemplatePrintRecordDetailDTO> ChangeTemplatePrintRecordDetailToDTOs(IEnumerable<TemplatePrintRecordDetail> domainTemplatePrintRecordDetails)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TemplatePrintRecordDetail, TemplatePrintRecordDetailDTO>();
            });
            var dtoTemplatePrintRecordDetail = config.CreateMapper().Map<IEnumerable<TemplatePrintRecordDetail>, IEnumerable<TemplatePrintRecordDetailDTO>>(domainTemplatePrintRecordDetails);

            return dtoTemplatePrintRecordDetail;
        }
	}
}
