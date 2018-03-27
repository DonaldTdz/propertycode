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
	public partial class SEC_CarportAppService
	{
		private SEC_CarportDomainService _SEC_CarportDomainService;
        protected SEC_CarportDomainService SEC_CarportService
        {
            get
            {
                if (_SEC_CarportDomainService == null)
                {
                    _SEC_CarportDomainService = new SEC_CarportDomainService();
                }

                return _SEC_CarportDomainService;
            }
        }   

        public bool InsertSEC_Carport(SEC_CarportDTO dtoSEC_Carport)
        {
            var domainSEC_Carport = SEC_CarportMappers.ChangeDTOToSEC_CarportNew(dtoSEC_Carport);

            return SEC_CarportService.InsertSEC_Carport(domainSEC_Carport);
        }

        public bool UpdateSEC_Carport(SEC_CarportDTO dtoSEC_Carport)
        {
            var domainSEC_Carport = SEC_CarportMappers.ChangeDTOToSEC_CarportNew(dtoSEC_Carport);

            return SEC_CarportService.UpdateSEC_Carport(domainSEC_Carport);
        }

        public bool DeleteSEC_Carport(object id)
        {
            return SEC_CarportService.DeleteSEC_Carport(id);
        }

        public List<SEC_CarportDTO> GetSEC_Carports()
        {
            var domainSEC_Carports = SEC_CarportService.GetSEC_Carports();

            return SEC_CarportMappers.ChangeSEC_CarportToDTOs(domainSEC_Carports);
        }

		public SEC_CarportDTO GetSEC_CarportByKey(object id)
        {
            var domainSEC_Carport = SEC_CarportService.GetSEC_CarportByKey(id);

            return SEC_CarportMappers.ChangeSEC_CarportToDTO(domainSEC_Carport);
        }
	}
}
