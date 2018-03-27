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
	public partial class SEC_AreaAppService
	{
		private SEC_AreaDomainService _SEC_AreaDomainService;
        protected SEC_AreaDomainService SEC_AreaService
        {
            get
            {
                if (_SEC_AreaDomainService == null)
                {
                    _SEC_AreaDomainService = new SEC_AreaDomainService();
                }

                return _SEC_AreaDomainService;
            }
        }   

        public bool InsertSEC_Area(SEC_AreaDTO dtoSEC_Area)
        {
            var domainSEC_Area = SEC_AreaMappers.ChangeDTOToSEC_AreaNew(dtoSEC_Area);

            return SEC_AreaService.InsertSEC_Area(domainSEC_Area);
        }

        public bool UpdateSEC_Area(SEC_AreaDTO dtoSEC_Area)
        {
            var domainSEC_Area = SEC_AreaMappers.ChangeDTOToSEC_AreaNew(dtoSEC_Area);

            return SEC_AreaService.UpdateSEC_Area(domainSEC_Area);
        }

        public bool DeleteSEC_Area(object id)
        {
            return SEC_AreaService.DeleteSEC_Area(id);
        }

        public List<SEC_AreaDTO> GetSEC_Areas()
        {
            var domainSEC_Areas = SEC_AreaService.GetSEC_Areas();

            return SEC_AreaMappers.ChangeSEC_AreaToDTOs(domainSEC_Areas);
        }

		public SEC_AreaDTO GetSEC_AreaByKey(object id)
        {
            var domainSEC_Area = SEC_AreaService.GetSEC_AreaByKey(id);

            return SEC_AreaMappers.ChangeSEC_AreaToDTO(domainSEC_Area);
        }
	}
}
