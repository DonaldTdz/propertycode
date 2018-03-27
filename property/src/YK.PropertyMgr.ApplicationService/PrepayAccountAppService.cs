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
	public partial class PrepayAccountAppService
	{
		private PrepayAccountDomainService _PrepayAccountDomainService;
        protected PrepayAccountDomainService PrepayAccountService
        {
            get
            {
                if (_PrepayAccountDomainService == null)
                {
                    _PrepayAccountDomainService = new PrepayAccountDomainService();
                }

                return _PrepayAccountDomainService;
            }
        }   

        public bool InsertPrepayAccount(PrepayAccountDTO dtoPrepayAccount)
        {
            var domainPrepayAccount = PrepayAccountMappers.ChangeDTOToPrepayAccountNew(dtoPrepayAccount);

            return PrepayAccountService.InsertPrepayAccount(domainPrepayAccount);
        }

        public bool UpdatePrepayAccount(PrepayAccountDTO dtoPrepayAccount)
        {
            var domainPrepayAccount = PrepayAccountMappers.ChangeDTOToPrepayAccountNew(dtoPrepayAccount);

            return PrepayAccountService.UpdatePrepayAccount(domainPrepayAccount);
        }

        public bool DeletePrepayAccount(object id)
        {
            return PrepayAccountService.DeletePrepayAccount(id);
        }

        public List<PrepayAccountDTO> GetPrepayAccounts()
        {
            var domainPrepayAccounts = PrepayAccountService.GetPrepayAccounts();

            return PrepayAccountMappers.ChangePrepayAccountToDTOs(domainPrepayAccounts);
        }

		public PrepayAccountDTO GetPrepayAccountByKey(object id)
        {
            var domainPrepayAccount = PrepayAccountService.GetPrepayAccountByKey(id);

            return PrepayAccountMappers.ChangePrepayAccountToDTO(domainPrepayAccount);
        }
	}
}
