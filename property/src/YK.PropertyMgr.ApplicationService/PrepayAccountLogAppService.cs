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
	public partial class PrepayAccountLogAppService
	{
		private PrepayAccountLogDomainService _PrepayAccountLogDomainService;
        protected PrepayAccountLogDomainService PrepayAccountLogService
        {
            get
            {
                if (_PrepayAccountLogDomainService == null)
                {
                    _PrepayAccountLogDomainService = new PrepayAccountLogDomainService();
                }

                return _PrepayAccountLogDomainService;
            }
        }   

        public bool InsertPrepayAccountLog(PrepayAccountLogDTO dtoPrepayAccountLog)
        {
            var domainPrepayAccountLog = PrepayAccountLogMappers.ChangeDTOToPrepayAccountLogNew(dtoPrepayAccountLog);

            return PrepayAccountLogService.InsertPrepayAccountLog(domainPrepayAccountLog);
        }

        public bool UpdatePrepayAccountLog(PrepayAccountLogDTO dtoPrepayAccountLog)
        {
            var domainPrepayAccountLog = PrepayAccountLogMappers.ChangeDTOToPrepayAccountLogNew(dtoPrepayAccountLog);

            return PrepayAccountLogService.UpdatePrepayAccountLog(domainPrepayAccountLog);
        }

        public bool DeletePrepayAccountLog(object id)
        {
            return PrepayAccountLogService.DeletePrepayAccountLog(id);
        }

        public List<PrepayAccountLogDTO> GetPrepayAccountLogs()
        {
            var domainPrepayAccountLogs = PrepayAccountLogService.GetPrepayAccountLogs();

            return PrepayAccountLogMappers.ChangePrepayAccountLogToDTOs(domainPrepayAccountLogs);
        }

		public PrepayAccountLogDTO GetPrepayAccountLogByKey(object id)
        {
            var domainPrepayAccountLog = PrepayAccountLogService.GetPrepayAccountLogByKey(id);

            return PrepayAccountLogMappers.ChangePrepayAccountLogToDTO(domainPrepayAccountLog);
        }
	}
}
