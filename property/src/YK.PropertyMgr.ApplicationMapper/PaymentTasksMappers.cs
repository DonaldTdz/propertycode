using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class PaymentTasksMappers
	{
		public static PaymentTasks ChangeDTOToPaymentTasksNew(PaymentTasksDTO dtoPaymentTasks)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentTasksDTO, PaymentTasks>();
            });
            var domainPaymentTasks = config.CreateMapper().Map<PaymentTasksDTO, PaymentTasks>(dtoPaymentTasks);

            return domainPaymentTasks;
        }

		public static void ChangeDTOToPaymentTasksUpdate(PaymentTasksDTO dtoPaymentTasks, PaymentTasks domainPaymentTasks)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentTasksDTO, PaymentTasks>();
            });
            config.CreateMapper().Map<PaymentTasksDTO, PaymentTasks>(dtoPaymentTasks, domainPaymentTasks);
        }

		public static void ChangePaymentTasksToDTO(PaymentTasksDTO dtoPaymentTasks, PaymentTasks domainPaymentTasks)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentTasks, PaymentTasksDTO>();
            });
            config.CreateMapper().Map<PaymentTasks, PaymentTasksDTO>(domainPaymentTasks, dtoPaymentTasks);
        }

		public static PaymentTasksDTO ChangePaymentTasksToDTO(PaymentTasks domainPaymentTasks)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentTasks, PaymentTasksDTO>();
            });
            return config.CreateMapper().Map<PaymentTasks, PaymentTasksDTO>(domainPaymentTasks);
        }

		public static List<PaymentTasksDTO> ChangePaymentTasksToDTOs(List<PaymentTasks> domainPaymentTasks)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentTasks, PaymentTasksDTO>();
            });
            var dtoPaymentTasks = config.CreateMapper().Map<List<PaymentTasks>, List<PaymentTasksDTO>>(domainPaymentTasks);

            return dtoPaymentTasks;
        }

		public static IEnumerable<PaymentTasksDTO> ChangePaymentTasksToDTOs(IEnumerable<PaymentTasks> domainPaymentTaskss)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentTasks, PaymentTasksDTO>();
            });
            var dtoPaymentTasks = config.CreateMapper().Map<IEnumerable<PaymentTasks>, IEnumerable<PaymentTasksDTO>>(domainPaymentTaskss);

            return dtoPaymentTasks;
        }
	}
}
