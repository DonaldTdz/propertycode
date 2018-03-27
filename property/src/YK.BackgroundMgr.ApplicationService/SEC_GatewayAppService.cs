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
	public partial class SEC_GatewayAppService
	{
		private SEC_GatewayDomainService _SEC_GatewayDomainService;
        protected SEC_GatewayDomainService SEC_GatewayService
        {
            get
            {
                if (_SEC_GatewayDomainService == null)
                {
                    _SEC_GatewayDomainService = new SEC_GatewayDomainService();
                }

                return _SEC_GatewayDomainService;
            }
        }   

        public bool InsertSEC_Gateway(SEC_GatewayDTO dtoSEC_Gateway)
        {
            var domainSEC_Gateway = SEC_GatewayMappers.ChangeDTOToSEC_GatewayNew(dtoSEC_Gateway);

            return SEC_GatewayService.InsertSEC_Gateway(domainSEC_Gateway);
        }

        public bool UpdateSEC_Gateway(SEC_GatewayDTO dtoSEC_Gateway)
        {
            var domainSEC_Gateway = SEC_GatewayMappers.ChangeDTOToSEC_GatewayNew(dtoSEC_Gateway);

            return SEC_GatewayService.UpdateSEC_Gateway(domainSEC_Gateway);
        }

        public bool DeleteSEC_Gateway(object id)
        {
            return SEC_GatewayService.DeleteSEC_Gateway(id);
        }

        public List<SEC_GatewayDTO> GetSEC_Gateways()
        {
            var domainSEC_Gateways = SEC_GatewayService.GetSEC_Gateways();

            return SEC_GatewayMappers.ChangeSEC_GatewayToDTOs(domainSEC_Gateways);
        }

		public SEC_GatewayDTO GetSEC_GatewayByKey(object id)
        {
            var domainSEC_Gateway = SEC_GatewayService.GetSEC_GatewayByKey(id);

            return SEC_GatewayMappers.ChangeSEC_GatewayToDTO(domainSEC_Gateway);
        }
	}
}
