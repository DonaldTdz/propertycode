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
	public partial class SEC_WeChatOpenIdAppService
	{
		private SEC_WeChatOpenIdDomainService _SEC_WeChatOpenIdDomainService;
        protected SEC_WeChatOpenIdDomainService SEC_WeChatOpenIdService
        {
            get
            {
                if (_SEC_WeChatOpenIdDomainService == null)
                {
                    _SEC_WeChatOpenIdDomainService = new SEC_WeChatOpenIdDomainService();
                }

                return _SEC_WeChatOpenIdDomainService;
            }
        }   

        public bool InsertSEC_WeChatOpenId(SEC_WeChatOpenIdDTO dtoSEC_WeChatOpenId)
        {
            var domainSEC_WeChatOpenId = SEC_WeChatOpenIdMappers.ChangeDTOToSEC_WeChatOpenIdNew(dtoSEC_WeChatOpenId);

            return SEC_WeChatOpenIdService.InsertSEC_WeChatOpenId(domainSEC_WeChatOpenId);
        }

        public bool UpdateSEC_WeChatOpenId(SEC_WeChatOpenIdDTO dtoSEC_WeChatOpenId)
        {
            var domainSEC_WeChatOpenId = SEC_WeChatOpenIdMappers.ChangeDTOToSEC_WeChatOpenIdNew(dtoSEC_WeChatOpenId);

            return SEC_WeChatOpenIdService.UpdateSEC_WeChatOpenId(domainSEC_WeChatOpenId);
        }

        public bool DeleteSEC_WeChatOpenId(object id)
        {
            return SEC_WeChatOpenIdService.DeleteSEC_WeChatOpenId(id);
        }

        public List<SEC_WeChatOpenIdDTO> GetSEC_WeChatOpenIds()
        {
            var domainSEC_WeChatOpenIds = SEC_WeChatOpenIdService.GetSEC_WeChatOpenIds();

            return SEC_WeChatOpenIdMappers.ChangeSEC_WeChatOpenIdToDTOs(domainSEC_WeChatOpenIds);
        }

		public SEC_WeChatOpenIdDTO GetSEC_WeChatOpenIdByKey(object id)
        {
            var domainSEC_WeChatOpenId = SEC_WeChatOpenIdService.GetSEC_WeChatOpenIdByKey(id);

            return SEC_WeChatOpenIdMappers.ChangeSEC_WeChatOpenIdToDTO(domainSEC_WeChatOpenId);
        }
	}
}
