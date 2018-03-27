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
	public partial class SEC_GatewayAuthAppService
	{
		private SEC_GatewayAuthDomainService _SEC_GatewayAuthDomainService;
        protected SEC_GatewayAuthDomainService SEC_GatewayAuthService
        {
            get
            {
                if (_SEC_GatewayAuthDomainService == null)
                {
                    _SEC_GatewayAuthDomainService = new SEC_GatewayAuthDomainService();
                }

                return _SEC_GatewayAuthDomainService;
            }
        }   

        public bool InsertSEC_GatewayAuth(SEC_GatewayAuthDTO dtoSEC_GatewayAuth)
        {
            var domainSEC_GatewayAuth = SEC_GatewayAuthMappers.ChangeDTOToSEC_GatewayAuthNew(dtoSEC_GatewayAuth);

            return SEC_GatewayAuthService.InsertSEC_GatewayAuth(domainSEC_GatewayAuth);
        }

        public bool UpdateSEC_GatewayAuth(SEC_GatewayAuthDTO dtoSEC_GatewayAuth)
        {
            var domainSEC_GatewayAuth = SEC_GatewayAuthMappers.ChangeDTOToSEC_GatewayAuthNew(dtoSEC_GatewayAuth);

            return SEC_GatewayAuthService.UpdateSEC_GatewayAuth(domainSEC_GatewayAuth);
        }

        public bool DeleteSEC_GatewayAuth(object id)
        {
            return SEC_GatewayAuthService.DeleteSEC_GatewayAuth(id);
        }

        public List<SEC_GatewayAuthDTO> GetSEC_GatewayAuths()
        {
            var domainSEC_GatewayAuths = SEC_GatewayAuthService.GetSEC_GatewayAuths();

            return SEC_GatewayAuthMappers.ChangeSEC_GatewayAuthToDTOs(domainSEC_GatewayAuths);
        }

		public SEC_GatewayAuthDTO GetSEC_GatewayAuthByKey(object id)
        {
            var domainSEC_GatewayAuth = SEC_GatewayAuthService.GetSEC_GatewayAuthByKey(id);

            return SEC_GatewayAuthMappers.ChangeSEC_GatewayAuthToDTO(domainSEC_GatewayAuth);
        }
	}
}
