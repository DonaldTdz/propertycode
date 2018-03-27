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
	public partial class SEC_HouseAppService
	{
		private SEC_HouseDomainService _SEC_HouseDomainService;
        protected SEC_HouseDomainService SEC_HouseService
        {
            get
            {
                if (_SEC_HouseDomainService == null)
                {
                    _SEC_HouseDomainService = new SEC_HouseDomainService();
                }

                return _SEC_HouseDomainService;
            }
        }   

        public bool InsertSEC_House(SEC_HouseDTO dtoSEC_House)
        {
            var domainSEC_House = SEC_HouseMappers.ChangeDTOToSEC_HouseNew(dtoSEC_House);

            return SEC_HouseService.InsertSEC_House(domainSEC_House);
        }

        public bool UpdateSEC_House(SEC_HouseDTO dtoSEC_House)
        {
            var domainSEC_House = SEC_HouseMappers.ChangeDTOToSEC_HouseNew(dtoSEC_House);

            return SEC_HouseService.UpdateSEC_House(domainSEC_House);
        }

        public bool DeleteSEC_House(object id)
        {
            return SEC_HouseService.DeleteSEC_House(id);
        }

        public List<SEC_HouseDTO> GetSEC_Houses()
        {
            var domainSEC_Houses = SEC_HouseService.GetSEC_Houses();

            return SEC_HouseMappers.ChangeSEC_HouseToDTOs(domainSEC_Houses);
        }

		public SEC_HouseDTO GetSEC_HouseByKey(object id)
        {
            var domainSEC_House = SEC_HouseService.GetSEC_HouseByKey(id);

            return SEC_HouseMappers.ChangeSEC_HouseToDTO(domainSEC_House);
        }
	}
}
