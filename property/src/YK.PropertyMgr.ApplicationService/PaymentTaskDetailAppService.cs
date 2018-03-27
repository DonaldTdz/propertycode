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
	public partial class PaymentTaskDetailAppService
	{
		private PaymentTaskDetailDomainService _PaymentTaskDetailDomainService;
        protected PaymentTaskDetailDomainService PaymentTaskDetailService
        {
            get
            {
                if (_PaymentTaskDetailDomainService == null)
                {
                    _PaymentTaskDetailDomainService = new PaymentTaskDetailDomainService();
                }

                return _PaymentTaskDetailDomainService;
            }
        }   

        public bool InsertPaymentTaskDetail(PaymentTaskDetailDTO dtoPaymentTaskDetail)
        {
            var domainPaymentTaskDetail = PaymentTaskDetailMappers.ChangeDTOToPaymentTaskDetailNew(dtoPaymentTaskDetail);

            return PaymentTaskDetailService.InsertPaymentTaskDetail(domainPaymentTaskDetail);
        }

        public bool UpdatePaymentTaskDetail(PaymentTaskDetailDTO dtoPaymentTaskDetail)
        {
            var domainPaymentTaskDetail = PaymentTaskDetailMappers.ChangeDTOToPaymentTaskDetailNew(dtoPaymentTaskDetail);

            return PaymentTaskDetailService.UpdatePaymentTaskDetail(domainPaymentTaskDetail);
        }

        public bool DeletePaymentTaskDetail(object id)
        {
            return PaymentTaskDetailService.DeletePaymentTaskDetail(id);
        }

        public List<PaymentTaskDetailDTO> GetPaymentTaskDetails()
        {
            var domainPaymentTaskDetails = PaymentTaskDetailService.GetPaymentTaskDetails();

            return PaymentTaskDetailMappers.ChangePaymentTaskDetailToDTOs(domainPaymentTaskDetails);
        }

		public PaymentTaskDetailDTO GetPaymentTaskDetailByKey(object id)
        {
            var domainPaymentTaskDetail = PaymentTaskDetailService.GetPaymentTaskDetailByKey(id);

            return PaymentTaskDetailMappers.ChangePaymentTaskDetailToDTO(domainPaymentTaskDetail);
        }
	}
}
