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
	public partial class SEC_DeveloperAppService
	{
		private SEC_DeveloperDomainService _SEC_DeveloperDomainService;
        protected SEC_DeveloperDomainService SEC_DeveloperService
        {
            get
            {
                if (_SEC_DeveloperDomainService == null)
                {
                    _SEC_DeveloperDomainService = new SEC_DeveloperDomainService();
                }

                return _SEC_DeveloperDomainService;
            }
        }   

        public bool InsertSEC_Developer(SEC_DeveloperDTO dtoSEC_Developer)
        {
            var domainSEC_Developer = SEC_DeveloperMappers.ChangeDTOToSEC_DeveloperNew(dtoSEC_Developer);

            return SEC_DeveloperService.InsertSEC_Developer(domainSEC_Developer);
        }

        public bool UpdateSEC_Developer(SEC_DeveloperDTO dtoSEC_Developer)
        {
            var domainSEC_Developer = SEC_DeveloperMappers.ChangeDTOToSEC_DeveloperNew(dtoSEC_Developer);

            return SEC_DeveloperService.UpdateSEC_Developer(domainSEC_Developer);
        }

        public bool DeleteSEC_Developer(object id)
        {
            return SEC_DeveloperService.DeleteSEC_Developer(id);
        }

        public List<SEC_DeveloperDTO> GetSEC_Developers()
        {
            var domainSEC_Developers = SEC_DeveloperService.GetSEC_Developers();

            return SEC_DeveloperMappers.ChangeSEC_DeveloperToDTOs(domainSEC_Developers);
        }

		public SEC_DeveloperDTO GetSEC_DeveloperByKey(object id)
        {
            var domainSEC_Developer = SEC_DeveloperService.GetSEC_DeveloperByKey(id);

            return SEC_DeveloperMappers.ChangeSEC_DeveloperToDTO(domainSEC_Developer);
        }
	}
}
