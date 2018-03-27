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
	public partial class SEC_WeChatPublicNumberAppService
	{
		private SEC_WeChatPublicNumberDomainService _SEC_WeChatPublicNumberDomainService;
        protected SEC_WeChatPublicNumberDomainService SEC_WeChatPublicNumberService
        {
            get
            {
                if (_SEC_WeChatPublicNumberDomainService == null)
                {
                    _SEC_WeChatPublicNumberDomainService = new SEC_WeChatPublicNumberDomainService();
                }

                return _SEC_WeChatPublicNumberDomainService;
            }
        }   

        public bool InsertSEC_WeChatPublicNumber(SEC_WeChatPublicNumberDTO dtoSEC_WeChatPublicNumber)
        {
            var domainSEC_WeChatPublicNumber = SEC_WeChatPublicNumberMappers.ChangeDTOToSEC_WeChatPublicNumberNew(dtoSEC_WeChatPublicNumber);

            return SEC_WeChatPublicNumberService.InsertSEC_WeChatPublicNumber(domainSEC_WeChatPublicNumber);
        }

        public bool UpdateSEC_WeChatPublicNumber(SEC_WeChatPublicNumberDTO dtoSEC_WeChatPublicNumber)
        {
            var domainSEC_WeChatPublicNumber = SEC_WeChatPublicNumberMappers.ChangeDTOToSEC_WeChatPublicNumberNew(dtoSEC_WeChatPublicNumber);

            return SEC_WeChatPublicNumberService.UpdateSEC_WeChatPublicNumber(domainSEC_WeChatPublicNumber);
        }

        public bool DeleteSEC_WeChatPublicNumber(object id)
        {
            return SEC_WeChatPublicNumberService.DeleteSEC_WeChatPublicNumber(id);
        }

        public List<SEC_WeChatPublicNumberDTO> GetSEC_WeChatPublicNumbers()
        {
            var domainSEC_WeChatPublicNumbers = SEC_WeChatPublicNumberService.GetSEC_WeChatPublicNumbers();

            return SEC_WeChatPublicNumberMappers.ChangeSEC_WeChatPublicNumberToDTOs(domainSEC_WeChatPublicNumbers);
        }

		public SEC_WeChatPublicNumberDTO GetSEC_WeChatPublicNumberByKey(object id)
        {
            var domainSEC_WeChatPublicNumber = SEC_WeChatPublicNumberService.GetSEC_WeChatPublicNumberByKey(id);

            return SEC_WeChatPublicNumberMappers.ChangeSEC_WeChatPublicNumberToDTO(domainSEC_WeChatPublicNumber);
        }
	}
}
