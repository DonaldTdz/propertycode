using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.DomainService;

namespace YK.PropertyMgr.ApplicationService
{
	public partial class PaymentTasksAppService
	{
		private PaymentTasksDomainService _PaymentTasksDomainService;
        protected PaymentTasksDomainService PaymentTasksService
        {
            get
            {
                if (_PaymentTasksDomainService == null)
                {
                    _PaymentTasksDomainService = new PaymentTasksDomainService();
                }

                return _PaymentTasksDomainService;
            }
        }   

        public bool InsertPaymentTasks(PaymentTasksDTO dtoPaymentTasks)
        {
            var domainPaymentTasks = PaymentTasksMappers.ChangeDTOToPaymentTasksNew(dtoPaymentTasks);

            return PaymentTasksService.InsertPaymentTasks(domainPaymentTasks);
        }

        public bool UpdatePaymentTasks(PaymentTasksDTO dtoPaymentTasks)
        {
            var domainPaymentTasks = PaymentTasksMappers.ChangeDTOToPaymentTasksNew(dtoPaymentTasks);

            return PaymentTasksService.UpdatePaymentTasks(domainPaymentTasks);
        }

        public bool DeletePaymentTasks(object id)
        {
            return PaymentTasksService.DeletePaymentTasks(id);
        }

        public List<PaymentTasksDTO> GetPaymentTaskss()
        {
            var domainPaymentTaskss = PaymentTasksService.GetPaymentTaskss();

            return PaymentTasksMappers.ChangePaymentTasksToDTOs(domainPaymentTaskss);
        }

		public PaymentTasksDTO GetPaymentTasksByKey(object id)
        {
            var domainPaymentTasks = PaymentTasksService.GetPaymentTasksByKey(id);

            return PaymentTasksMappers.ChangePaymentTasksToDTO(domainPaymentTasks);
        }
	}
}
