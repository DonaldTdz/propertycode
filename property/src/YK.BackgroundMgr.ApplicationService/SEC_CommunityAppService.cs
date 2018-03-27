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
	public partial class SEC_CommunityAppService
	{
		private SEC_CommunityDomainService _SEC_CommunityDomainService;
        protected SEC_CommunityDomainService SEC_CommunityService
        {
            get
            {
                if (_SEC_CommunityDomainService == null)
                {
                    _SEC_CommunityDomainService = new SEC_CommunityDomainService();
                }

                return _SEC_CommunityDomainService;
            }
        }   

        public bool InsertSEC_Community(SEC_CommunityDTO dtoSEC_Community)
        {
            var domainSEC_Community = SEC_CommunityMappers.ChangeDTOToSEC_CommunityNew(dtoSEC_Community);

            return SEC_CommunityService.InsertSEC_Community(domainSEC_Community);
        }

        public bool UpdateSEC_Community(SEC_CommunityDTO dtoSEC_Community)
        {
            var domainSEC_Community = SEC_CommunityMappers.ChangeDTOToSEC_CommunityNew(dtoSEC_Community);

            return SEC_CommunityService.UpdateSEC_Community(domainSEC_Community);
        }

        public bool DeleteSEC_Community(object id)
        {
            return SEC_CommunityService.DeleteSEC_Community(id);
        }

        public List<SEC_CommunityDTO> GetSEC_Communitys()
        {
            var domainSEC_Communitys = SEC_CommunityService.GetSEC_Communitys();

            return SEC_CommunityMappers.ChangeSEC_CommunityToDTOs(domainSEC_Communitys);
        }

		public SEC_CommunityDTO GetSEC_CommunityByKey(object id)
        {
            var domainSEC_Community = SEC_CommunityService.GetSEC_CommunityByKey(id);

            return SEC_CommunityMappers.ChangeSEC_CommunityToDTO(domainSEC_Community);
        }
	}
}
