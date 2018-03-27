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
	public partial class ClientPaymentLogAppService
	{
		private ClientPaymentLogDomainService _ClientPaymentLogDomainService;
        protected ClientPaymentLogDomainService ClientPaymentLogService
        {
            get
            {
                if (_ClientPaymentLogDomainService == null)
                {
                    _ClientPaymentLogDomainService = new ClientPaymentLogDomainService();
                }

                return _ClientPaymentLogDomainService;
            }
        }   

        public bool InsertClientPaymentLog(ClientPaymentLogDTO dtoClientPaymentLog)
        {
            var domainClientPaymentLog = ClientPaymentLogMappers.ChangeDTOToClientPaymentLogNew(dtoClientPaymentLog);

            return ClientPaymentLogService.InsertClientPaymentLog(domainClientPaymentLog);
        }

        public bool UpdateClientPaymentLog(ClientPaymentLogDTO dtoClientPaymentLog)
        {
            var domainClientPaymentLog = ClientPaymentLogMappers.ChangeDTOToClientPaymentLogNew(dtoClientPaymentLog);

            return ClientPaymentLogService.UpdateClientPaymentLog(domainClientPaymentLog);
        }

        public bool DeleteClientPaymentLog(object id)
        {
            return ClientPaymentLogService.DeleteClientPaymentLog(id);
        }

        public List<ClientPaymentLogDTO> GetClientPaymentLogs()
        {
            var domainClientPaymentLogs = ClientPaymentLogService.GetClientPaymentLogs();

            return ClientPaymentLogMappers.ChangeClientPaymentLogToDTOs(domainClientPaymentLogs);
        }

		public ClientPaymentLogDTO GetClientPaymentLogByKey(object id)
        {
            var domainClientPaymentLog = ClientPaymentLogService.GetClientPaymentLogByKey(id);

            return ClientPaymentLogMappers.ChangeClientPaymentLogToDTO(domainClientPaymentLog);
        }
	}
}
