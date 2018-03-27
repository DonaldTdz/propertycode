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
	public partial class AlipayChargeBillSynchronizerDetailAppService
	{
		private AlipayChargeBillSynchronizerDetailDomainService _AlipayChargeBillSynchronizerDetailDomainService;
        protected AlipayChargeBillSynchronizerDetailDomainService AlipayChargeBillSynchronizerDetailService
        {
            get
            {
                if (_AlipayChargeBillSynchronizerDetailDomainService == null)
                {
                    _AlipayChargeBillSynchronizerDetailDomainService = new AlipayChargeBillSynchronizerDetailDomainService();
                }

                return _AlipayChargeBillSynchronizerDetailDomainService;
            }
        }   

        public bool InsertAlipayChargeBillSynchronizerDetail(AlipayChargeBillSynchronizerDetailDTO dtoAlipayChargeBillSynchronizerDetail)
        {
            var domainAlipayChargeBillSynchronizerDetail = AlipayChargeBillSynchronizerDetailMappers.ChangeDTOToAlipayChargeBillSynchronizerDetailNew(dtoAlipayChargeBillSynchronizerDetail);

            return AlipayChargeBillSynchronizerDetailService.InsertAlipayChargeBillSynchronizerDetail(domainAlipayChargeBillSynchronizerDetail);
        }

        public bool UpdateAlipayChargeBillSynchronizerDetail(AlipayChargeBillSynchronizerDetailDTO dtoAlipayChargeBillSynchronizerDetail)
        {
            var domainAlipayChargeBillSynchronizerDetail = AlipayChargeBillSynchronizerDetailMappers.ChangeDTOToAlipayChargeBillSynchronizerDetailNew(dtoAlipayChargeBillSynchronizerDetail);

            return AlipayChargeBillSynchronizerDetailService.UpdateAlipayChargeBillSynchronizerDetail(domainAlipayChargeBillSynchronizerDetail);
        }

        public bool DeleteAlipayChargeBillSynchronizerDetail(object id)
        {
            return AlipayChargeBillSynchronizerDetailService.DeleteAlipayChargeBillSynchronizerDetail(id);
        }

        public List<AlipayChargeBillSynchronizerDetailDTO> GetAlipayChargeBillSynchronizerDetails()
        {
            var domainAlipayChargeBillSynchronizerDetails = AlipayChargeBillSynchronizerDetailService.GetAlipayChargeBillSynchronizerDetails();

            return AlipayChargeBillSynchronizerDetailMappers.ChangeAlipayChargeBillSynchronizerDetailToDTOs(domainAlipayChargeBillSynchronizerDetails);
        }

		public AlipayChargeBillSynchronizerDetailDTO GetAlipayChargeBillSynchronizerDetailByKey(object id)
        {
            var domainAlipayChargeBillSynchronizerDetail = AlipayChargeBillSynchronizerDetailService.GetAlipayChargeBillSynchronizerDetailByKey(id);

            return AlipayChargeBillSynchronizerDetailMappers.ChangeAlipayChargeBillSynchronizerDetailToDTO(domainAlipayChargeBillSynchronizerDetail);
        }
	}
}
