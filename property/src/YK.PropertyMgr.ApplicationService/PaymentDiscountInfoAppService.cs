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
	public partial class PaymentDiscountInfoAppService
	{
		private PaymentDiscountInfoDomainService _PaymentDiscountInfoDomainService;
        protected PaymentDiscountInfoDomainService PaymentDiscountInfoService
        {
            get
            {
                if (_PaymentDiscountInfoDomainService == null)
                {
                    _PaymentDiscountInfoDomainService = new PaymentDiscountInfoDomainService();
                }

                return _PaymentDiscountInfoDomainService;
            }
        }   

        public bool InsertPaymentDiscountInfo(PaymentDiscountInfoDTO dtoPaymentDiscountInfo)
        {
            var domainPaymentDiscountInfo = PaymentDiscountInfoMappers.ChangeDTOToPaymentDiscountInfoNew(dtoPaymentDiscountInfo);

            return PaymentDiscountInfoService.InsertPaymentDiscountInfo(domainPaymentDiscountInfo);
        }

        public bool UpdatePaymentDiscountInfo(PaymentDiscountInfoDTO dtoPaymentDiscountInfo)
        {
            var domainPaymentDiscountInfo = PaymentDiscountInfoMappers.ChangeDTOToPaymentDiscountInfoNew(dtoPaymentDiscountInfo);

            return PaymentDiscountInfoService.UpdatePaymentDiscountInfo(domainPaymentDiscountInfo);
        }

        public bool DeletePaymentDiscountInfo(object id)
        {
            return PaymentDiscountInfoService.DeletePaymentDiscountInfo(id);
        }

        public List<PaymentDiscountInfoDTO> GetPaymentDiscountInfos()
        {
            var domainPaymentDiscountInfos = PaymentDiscountInfoService.GetPaymentDiscountInfos();

            return PaymentDiscountInfoMappers.ChangePaymentDiscountInfoToDTOs(domainPaymentDiscountInfos);
        }

		public PaymentDiscountInfoDTO GetPaymentDiscountInfoByKey(object id)
        {
            var domainPaymentDiscountInfo = PaymentDiscountInfoService.GetPaymentDiscountInfoByKey(id);

            return PaymentDiscountInfoMappers.ChangePaymentDiscountInfoToDTO(domainPaymentDiscountInfo);
        }
	}
}
