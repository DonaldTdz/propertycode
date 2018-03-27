using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.ApplicationMapper;
using YK.BackgroundMgr.DomainService;

namespace YK.BackgroundMgr.ApplicationService
{
	public partial class SEC_BuildingAppService
	{
		private SEC_BuildingDomainService _SEC_BuildingDomainService;
        protected SEC_BuildingDomainService SEC_BuildingService
        {
            get
            {
                if (_SEC_BuildingDomainService == null)
                {
                    _SEC_BuildingDomainService = new SEC_BuildingDomainService();
                }

                return _SEC_BuildingDomainService;
            }
        }   

        public bool InsertSEC_Building(SEC_BuildingDTO dtoSEC_Building)
        {
            var domainSEC_Building = SEC_BuildingMappers.ChangeDTOToSEC_BuildingNew(dtoSEC_Building);

            return SEC_BuildingService.InsertSEC_Building(domainSEC_Building);
        }

        public bool UpdateSEC_Building(SEC_BuildingDTO dtoSEC_Building)
        {
            var domainSEC_Building = SEC_BuildingMappers.ChangeDTOToSEC_BuildingNew(dtoSEC_Building);

            return SEC_BuildingService.UpdateSEC_Building(domainSEC_Building);
        }

        public bool DeleteSEC_Building(object id)
        {
            return SEC_BuildingService.DeleteSEC_Building(id);
        }

        public List<SEC_BuildingDTO> GetSEC_Buildings()
        {
            var domainSEC_Buildings = SEC_BuildingService.GetSEC_Buildings();

            return SEC_BuildingMappers.ChangeSEC_BuildingToDTOs(domainSEC_Buildings);
        }

		public SEC_BuildingDTO GetSEC_BuildingByKey(object id)
        {
            var domainSEC_Building = SEC_BuildingService.GetSEC_BuildingByKey(id);

            return SEC_BuildingMappers.ChangeSEC_BuildingToDTO(domainSEC_Building);
        }
	}
}
