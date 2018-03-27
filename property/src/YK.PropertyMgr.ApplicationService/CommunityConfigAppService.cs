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
	public partial class CommunityConfigAppService
	{
		private CommunityConfigDomainService _CommunityConfigDomainService;
        protected CommunityConfigDomainService CommunityConfigService
        {
            get
            {
                if (_CommunityConfigDomainService == null)
                {
                    _CommunityConfigDomainService = new CommunityConfigDomainService();
                }

                return _CommunityConfigDomainService;
            }
        }   

        public bool InsertCommunityConfig(CommunityConfigDTO dtoCommunityConfig)
        {
            var domainCommunityConfig = CommunityConfigMappers.ChangeDTOToCommunityConfigNew(dtoCommunityConfig);

            return CommunityConfigService.InsertCommunityConfig(domainCommunityConfig);
        }

        public bool UpdateCommunityConfig(CommunityConfigDTO dtoCommunityConfig)
        {
            var domainCommunityConfig = CommunityConfigMappers.ChangeDTOToCommunityConfigNew(dtoCommunityConfig);

            return CommunityConfigService.UpdateCommunityConfig(domainCommunityConfig);
        }

        public bool DeleteCommunityConfig(object id)
        {
            return CommunityConfigService.DeleteCommunityConfig(id);
        }

        public List<CommunityConfigDTO> GetCommunityConfigs()
        {
            var domainCommunityConfigs = CommunityConfigService.GetCommunityConfigs();

            return CommunityConfigMappers.ChangeCommunityConfigToDTOs(domainCommunityConfigs);
        }

		public CommunityConfigDTO GetCommunityConfigByKey(object id)
        {
            var domainCommunityConfig = CommunityConfigService.GetCommunityConfigByKey(id);

            return CommunityConfigMappers.ChangeCommunityConfigToDTO(domainCommunityConfig);
        }
	}
}
