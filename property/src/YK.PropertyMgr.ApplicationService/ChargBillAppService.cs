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
	public partial class ChargBillAppService
	{
		private ChargBillDomainService _ChargBillDomainService;
        protected ChargBillDomainService ChargBillService
        {
            get
            {
                if (_ChargBillDomainService == null)
                {
                    _ChargBillDomainService = new ChargBillDomainService();
                }

                return _ChargBillDomainService;
            }
        }   

        public bool InsertChargBill(ChargBillDTO dtoChargBill)
        {
            var domainChargBill = ChargBillMappers.ChangeDTOToChargBillNew(dtoChargBill);

            return ChargBillService.InsertChargBill(domainChargBill);
        }

        public bool UpdateChargBill(ChargBillDTO dtoChargBill)
        {
            var domainChargBill = ChargBillMappers.ChangeDTOToChargBillNew(dtoChargBill);

            return ChargBillService.UpdateChargBill(domainChargBill);
        }

        public bool DeleteChargBill(object id)
        {
            return ChargBillService.DeleteChargBill(id);
        }

        public List<ChargBillDTO> GetChargBills()
        {
            var domainChargBills = ChargBillService.GetChargBills();

            return ChargBillMappers.ChangeChargBillToDTOs(domainChargBills);
        }

		public ChargBillDTO GetChargBillByKey(object id)
        {
            var domainChargBill = ChargBillService.GetChargBillByKey(id);

            return ChargBillMappers.ChangeChargBillToDTO(domainChargBill);
        }
	}
}
