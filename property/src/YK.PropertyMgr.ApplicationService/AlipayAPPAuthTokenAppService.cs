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
	public partial class AlipayAPPAuthTokenAppService
	{
		private AlipayAPPAuthTokenDomainService _AlipayAPPAuthTokenDomainService;
        protected AlipayAPPAuthTokenDomainService AlipayAPPAuthTokenService
        {
            get
            {
                if (_AlipayAPPAuthTokenDomainService == null)
                {
                    _AlipayAPPAuthTokenDomainService = new AlipayAPPAuthTokenDomainService();
                }

                return _AlipayAPPAuthTokenDomainService;
            }
        }   

        public bool InsertAlipayAPPAuthToken(AlipayAPPAuthTokenDTO dtoAlipayAPPAuthToken)
        {
            var domainAlipayAPPAuthToken = AlipayAPPAuthTokenMappers.ChangeDTOToAlipayAPPAuthTokenNew(dtoAlipayAPPAuthToken);

            return AlipayAPPAuthTokenService.InsertAlipayAPPAuthToken(domainAlipayAPPAuthToken);
        }

        public bool UpdateAlipayAPPAuthToken(AlipayAPPAuthTokenDTO dtoAlipayAPPAuthToken)
        {
            var domainAlipayAPPAuthToken = AlipayAPPAuthTokenMappers.ChangeDTOToAlipayAPPAuthTokenNew(dtoAlipayAPPAuthToken);

            return AlipayAPPAuthTokenService.UpdateAlipayAPPAuthToken(domainAlipayAPPAuthToken);
        }

        public bool DeleteAlipayAPPAuthToken(object id)
        {
            return AlipayAPPAuthTokenService.DeleteAlipayAPPAuthToken(id);
        }

        public List<AlipayAPPAuthTokenDTO> GetAlipayAPPAuthTokens()
        {
            var domainAlipayAPPAuthTokens = AlipayAPPAuthTokenService.GetAlipayAPPAuthTokens();

            return AlipayAPPAuthTokenMappers.ChangeAlipayAPPAuthTokenToDTOs(domainAlipayAPPAuthTokens);
        }

		public AlipayAPPAuthTokenDTO GetAlipayAPPAuthTokenByKey(object id)
        {
            var domainAlipayAPPAuthToken = AlipayAPPAuthTokenService.GetAlipayAPPAuthTokenByKey(id);

            return AlipayAPPAuthTokenMappers.ChangeAlipayAPPAuthTokenToDTO(domainAlipayAPPAuthToken);
        }
	}
}
