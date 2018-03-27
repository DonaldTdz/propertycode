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
	public partial class AlipayChargeBillAppService
	{
		private AlipayChargeBillDomainService _AlipayChargeBillDomainService;
        protected AlipayChargeBillDomainService AlipayChargeBillService
        {
            get
            {
                if (_AlipayChargeBillDomainService == null)
                {
                    _AlipayChargeBillDomainService = new AlipayChargeBillDomainService();
                }

                return _AlipayChargeBillDomainService;
            }
        }   

        public bool InsertAlipayChargeBill(AlipayChargeBillDTO dtoAlipayChargeBill)
        {
            var domainAlipayChargeBill = AlipayChargeBillMappers.ChangeDTOToAlipayChargeBillNew(dtoAlipayChargeBill);

            return AlipayChargeBillService.InsertAlipayChargeBill(domainAlipayChargeBill);
        }

        public bool UpdateAlipayChargeBill(AlipayChargeBillDTO dtoAlipayChargeBill)
        {
            var domainAlipayChargeBill = AlipayChargeBillMappers.ChangeDTOToAlipayChargeBillNew(dtoAlipayChargeBill);

            return AlipayChargeBillService.UpdateAlipayChargeBill(domainAlipayChargeBill);
        }

        public bool DeleteAlipayChargeBill(object id)
        {
            return AlipayChargeBillService.DeleteAlipayChargeBill(id);
        }

        public List<AlipayChargeBillDTO> GetAlipayChargeBills()
        {
            var domainAlipayChargeBills = AlipayChargeBillService.GetAlipayChargeBills();

            return AlipayChargeBillMappers.ChangeAlipayChargeBillToDTOs(domainAlipayChargeBills);
        }

		public AlipayChargeBillDTO GetAlipayChargeBillByKey(object id)
        {
            var domainAlipayChargeBill = AlipayChargeBillService.GetAlipayChargeBillByKey(id);

            return AlipayChargeBillMappers.ChangeAlipayChargeBillToDTO(domainAlipayChargeBill);
        }
	}
}
