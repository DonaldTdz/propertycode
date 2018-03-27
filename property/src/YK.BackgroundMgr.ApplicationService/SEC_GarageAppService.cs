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
	public partial class SEC_GarageAppService
	{
		private SEC_GarageDomainService _SEC_GarageDomainService;
        protected SEC_GarageDomainService SEC_GarageService
        {
            get
            {
                if (_SEC_GarageDomainService == null)
                {
                    _SEC_GarageDomainService = new SEC_GarageDomainService();
                }

                return _SEC_GarageDomainService;
            }
        }   

        public bool InsertSEC_Garage(SEC_GarageDTO dtoSEC_Garage)
        {
            var domainSEC_Garage = SEC_GarageMappers.ChangeDTOToSEC_GarageNew(dtoSEC_Garage);

            return SEC_GarageService.InsertSEC_Garage(domainSEC_Garage);
        }

        public bool UpdateSEC_Garage(SEC_GarageDTO dtoSEC_Garage)
        {
            var domainSEC_Garage = SEC_GarageMappers.ChangeDTOToSEC_GarageNew(dtoSEC_Garage);

            return SEC_GarageService.UpdateSEC_Garage(domainSEC_Garage);
        }

        public bool DeleteSEC_Garage(object id)
        {
            return SEC_GarageService.DeleteSEC_Garage(id);
        }

        public List<SEC_GarageDTO> GetSEC_Garages()
        {
            var domainSEC_Garages = SEC_GarageService.GetSEC_Garages();

            return SEC_GarageMappers.ChangeSEC_GarageToDTOs(domainSEC_Garages);
        }

		public SEC_GarageDTO GetSEC_GarageByKey(object id)
        {
            var domainSEC_Garage = SEC_GarageService.GetSEC_GarageByKey(id);

            return SEC_GarageMappers.ChangeSEC_GarageToDTO(domainSEC_Garage);
        }
	}
}
