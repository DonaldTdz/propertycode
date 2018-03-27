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
	public partial class ChargeBillRecordMatchingAppService
	{
		private ChargeBillRecordMatchingDomainService _ChargeBillRecordMatchingDomainService;
        protected ChargeBillRecordMatchingDomainService ChargeBillRecordMatchingService
        {
            get
            {
                if (_ChargeBillRecordMatchingDomainService == null)
                {
                    _ChargeBillRecordMatchingDomainService = new ChargeBillRecordMatchingDomainService();
                }

                return _ChargeBillRecordMatchingDomainService;
            }
        }   

        public bool InsertChargeBillRecordMatching(ChargeBillRecordMatchingDTO dtoChargeBillRecordMatching)
        {
            var domainChargeBillRecordMatching = ChargeBillRecordMatchingMappers.ChangeDTOToChargeBillRecordMatchingNew(dtoChargeBillRecordMatching);

            return ChargeBillRecordMatchingService.InsertChargeBillRecordMatching(domainChargeBillRecordMatching);
        }

        public bool UpdateChargeBillRecordMatching(ChargeBillRecordMatchingDTO dtoChargeBillRecordMatching)
        {
            var domainChargeBillRecordMatching = ChargeBillRecordMatchingMappers.ChangeDTOToChargeBillRecordMatchingNew(dtoChargeBillRecordMatching);

            return ChargeBillRecordMatchingService.UpdateChargeBillRecordMatching(domainChargeBillRecordMatching);
        }

        public bool DeleteChargeBillRecordMatching(object id)
        {
            return ChargeBillRecordMatchingService.DeleteChargeBillRecordMatching(id);
        }

        public List<ChargeBillRecordMatchingDTO> GetChargeBillRecordMatchings()
        {
            var domainChargeBillRecordMatchings = ChargeBillRecordMatchingService.GetChargeBillRecordMatchings();

            return ChargeBillRecordMatchingMappers.ChangeChargeBillRecordMatchingToDTOs(domainChargeBillRecordMatchings);
        }

		public ChargeBillRecordMatchingDTO GetChargeBillRecordMatchingByKey(object id)
        {
            var domainChargeBillRecordMatching = ChargeBillRecordMatchingService.GetChargeBillRecordMatchingByKey(id);

            return ChargeBillRecordMatchingMappers.ChangeChargeBillRecordMatchingToDTO(domainChargeBillRecordMatching);
        }
	}
}
