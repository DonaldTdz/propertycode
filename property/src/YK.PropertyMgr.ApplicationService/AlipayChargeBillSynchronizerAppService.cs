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
	public partial class AlipayChargeBillSynchronizerAppService
	{
		private AlipayChargeBillSynchronizerDomainService _AlipayChargeBillSynchronizerDomainService;
        protected AlipayChargeBillSynchronizerDomainService AlipayChargeBillSynchronizerService
        {
            get
            {
                if (_AlipayChargeBillSynchronizerDomainService == null)
                {
                    _AlipayChargeBillSynchronizerDomainService = new AlipayChargeBillSynchronizerDomainService();
                }

                return _AlipayChargeBillSynchronizerDomainService;
            }
        }   

        public bool InsertAlipayChargeBillSynchronizer(AlipayChargeBillSynchronizerDTO dtoAlipayChargeBillSynchronizer)
        {
            var domainAlipayChargeBillSynchronizer = AlipayChargeBillSynchronizerMappers.ChangeDTOToAlipayChargeBillSynchronizerNew(dtoAlipayChargeBillSynchronizer);

            return AlipayChargeBillSynchronizerService.InsertAlipayChargeBillSynchronizer(domainAlipayChargeBillSynchronizer);
        }

        public bool UpdateAlipayChargeBillSynchronizer(AlipayChargeBillSynchronizerDTO dtoAlipayChargeBillSynchronizer)
        {
            var domainAlipayChargeBillSynchronizer = AlipayChargeBillSynchronizerMappers.ChangeDTOToAlipayChargeBillSynchronizerNew(dtoAlipayChargeBillSynchronizer);

            return AlipayChargeBillSynchronizerService.UpdateAlipayChargeBillSynchronizer(domainAlipayChargeBillSynchronizer);
        }

        public bool DeleteAlipayChargeBillSynchronizer(object id)
        {
            return AlipayChargeBillSynchronizerService.DeleteAlipayChargeBillSynchronizer(id);
        }

        public List<AlipayChargeBillSynchronizerDTO> GetAlipayChargeBillSynchronizers()
        {
            var domainAlipayChargeBillSynchronizers = AlipayChargeBillSynchronizerService.GetAlipayChargeBillSynchronizers();

            return AlipayChargeBillSynchronizerMappers.ChangeAlipayChargeBillSynchronizerToDTOs(domainAlipayChargeBillSynchronizers);
        }

		public AlipayChargeBillSynchronizerDTO GetAlipayChargeBillSynchronizerByKey(object id)
        {
            var domainAlipayChargeBillSynchronizer = AlipayChargeBillSynchronizerService.GetAlipayChargeBillSynchronizerByKey(id);

            return AlipayChargeBillSynchronizerMappers.ChangeAlipayChargeBillSynchronizerToDTO(domainAlipayChargeBillSynchronizer);
        }
	}
}
