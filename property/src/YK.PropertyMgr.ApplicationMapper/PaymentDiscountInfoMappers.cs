using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class PaymentDiscountInfoMappers
	{
		public static PaymentDiscountInfo ChangeDTOToPaymentDiscountInfoNew(PaymentDiscountInfoDTO dtoPaymentDiscountInfo)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentDiscountInfoDTO, PaymentDiscountInfo>();
            });
            var domainPaymentDiscountInfo = config.CreateMapper().Map<PaymentDiscountInfoDTO, PaymentDiscountInfo>(dtoPaymentDiscountInfo);

            return domainPaymentDiscountInfo;
        }

		public static void ChangeDTOToPaymentDiscountInfoUpdate(PaymentDiscountInfoDTO dtoPaymentDiscountInfo, PaymentDiscountInfo domainPaymentDiscountInfo)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentDiscountInfoDTO, PaymentDiscountInfo>();
            });
            config.CreateMapper().Map<PaymentDiscountInfoDTO, PaymentDiscountInfo>(dtoPaymentDiscountInfo, domainPaymentDiscountInfo);
        }

		public static void ChangePaymentDiscountInfoToDTO(PaymentDiscountInfoDTO dtoPaymentDiscountInfo, PaymentDiscountInfo domainPaymentDiscountInfo)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentDiscountInfo, PaymentDiscountInfoDTO>();
            });
            config.CreateMapper().Map<PaymentDiscountInfo, PaymentDiscountInfoDTO>(domainPaymentDiscountInfo, dtoPaymentDiscountInfo);
        }

		public static PaymentDiscountInfoDTO ChangePaymentDiscountInfoToDTO(PaymentDiscountInfo domainPaymentDiscountInfo)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentDiscountInfo, PaymentDiscountInfoDTO>();
            });
            return config.CreateMapper().Map<PaymentDiscountInfo, PaymentDiscountInfoDTO>(domainPaymentDiscountInfo);
        }

		public static List<PaymentDiscountInfoDTO> ChangePaymentDiscountInfoToDTOs(List<PaymentDiscountInfo> domainPaymentDiscountInfo)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentDiscountInfo, PaymentDiscountInfoDTO>();
            });
            var dtoPaymentDiscountInfo = config.CreateMapper().Map<List<PaymentDiscountInfo>, List<PaymentDiscountInfoDTO>>(domainPaymentDiscountInfo);

            return dtoPaymentDiscountInfo;
        }

		public static IEnumerable<PaymentDiscountInfoDTO> ChangePaymentDiscountInfoToDTOs(IEnumerable<PaymentDiscountInfo> domainPaymentDiscountInfos)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentDiscountInfo, PaymentDiscountInfoDTO>();
            });
            var dtoPaymentDiscountInfo = config.CreateMapper().Map<IEnumerable<PaymentDiscountInfo>, IEnumerable<PaymentDiscountInfoDTO>>(domainPaymentDiscountInfos);

            return dtoPaymentDiscountInfo;
        }
	}
}
