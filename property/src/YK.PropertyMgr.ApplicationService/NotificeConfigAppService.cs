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
	public partial class NotificeConfigAppService
	{
		private NotificeConfigDomainService _NotificeConfigDomainService;
        protected NotificeConfigDomainService NotificeConfigService
        {
            get
            {
                if (_NotificeConfigDomainService == null)
                {
                    _NotificeConfigDomainService = new NotificeConfigDomainService();
                }

                return _NotificeConfigDomainService;
            }
        }   

        public bool InsertNotificeConfig(NotificeConfigDTO dtoNotificeConfig)
        {
            var domainNotificeConfig = NotificeConfigMappers.ChangeDTOToNotificeConfigNew(dtoNotificeConfig);

            return NotificeConfigService.InsertNotificeConfig(domainNotificeConfig);
        }

        public bool UpdateNotificeConfig(NotificeConfigDTO dtoNotificeConfig)
        {
            var domainNotificeConfig = NotificeConfigMappers.ChangeDTOToNotificeConfigNew(dtoNotificeConfig);

            return NotificeConfigService.UpdateNotificeConfig(domainNotificeConfig);
        }

        public bool DeleteNotificeConfig(object id)
        {
            return NotificeConfigService.DeleteNotificeConfig(id);
        }

        public List<NotificeConfigDTO> GetNotificeConfigs()
        {
            var domainNotificeConfigs = NotificeConfigService.GetNotificeConfigs();

            return NotificeConfigMappers.ChangeNotificeConfigToDTOs(domainNotificeConfigs);
        }

		public NotificeConfigDTO GetNotificeConfigByKey(object id)
        {
            var domainNotificeConfig = NotificeConfigService.GetNotificeConfigByKey(id);

            return NotificeConfigMappers.ChangeNotificeConfigToDTO(domainNotificeConfig);
        }
	}
}
