using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class PaymentTaskDetailMappers
	{
		public static PaymentTaskDetail ChangeDTOToPaymentTaskDetailNew(PaymentTaskDetailDTO dtoPaymentTaskDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentTaskDetailDTO, PaymentTaskDetail>();
            });
            var domainPaymentTaskDetail = config.CreateMapper().Map<PaymentTaskDetailDTO, PaymentTaskDetail>(dtoPaymentTaskDetail);

            return domainPaymentTaskDetail;
        }

		public static void ChangeDTOToPaymentTaskDetailUpdate(PaymentTaskDetailDTO dtoPaymentTaskDetail, PaymentTaskDetail domainPaymentTaskDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentTaskDetailDTO, PaymentTaskDetail>();
            });
            config.CreateMapper().Map<PaymentTaskDetailDTO, PaymentTaskDetail>(dtoPaymentTaskDetail, domainPaymentTaskDetail);
        }

		public static void ChangePaymentTaskDetailToDTO(PaymentTaskDetailDTO dtoPaymentTaskDetail, PaymentTaskDetail domainPaymentTaskDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentTaskDetail, PaymentTaskDetailDTO>();
            });
            config.CreateMapper().Map<PaymentTaskDetail, PaymentTaskDetailDTO>(domainPaymentTaskDetail, dtoPaymentTaskDetail);
        }

		public static PaymentTaskDetailDTO ChangePaymentTaskDetailToDTO(PaymentTaskDetail domainPaymentTaskDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentTaskDetail, PaymentTaskDetailDTO>();
            });
            return config.CreateMapper().Map<PaymentTaskDetail, PaymentTaskDetailDTO>(domainPaymentTaskDetail);
        }

		public static List<PaymentTaskDetailDTO> ChangePaymentTaskDetailToDTOs(List<PaymentTaskDetail> domainPaymentTaskDetail)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentTaskDetail, PaymentTaskDetailDTO>();
            });
            var dtoPaymentTaskDetail = config.CreateMapper().Map<List<PaymentTaskDetail>, List<PaymentTaskDetailDTO>>(domainPaymentTaskDetail);

            return dtoPaymentTaskDetail;
        }

		public static IEnumerable<PaymentTaskDetailDTO> ChangePaymentTaskDetailToDTOs(IEnumerable<PaymentTaskDetail> domainPaymentTaskDetails)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentTaskDetail, PaymentTaskDetailDTO>();
            });
            var dtoPaymentTaskDetail = config.CreateMapper().Map<IEnumerable<PaymentTaskDetail>, IEnumerable<PaymentTaskDetailDTO>>(domainPaymentTaskDetails);

            return dtoPaymentTaskDetail;
        }
	}
}
