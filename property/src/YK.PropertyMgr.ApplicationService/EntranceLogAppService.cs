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
	public partial class EntranceLogAppService
	{
		private EntranceLogDomainService _EntranceLogDomainService;
        protected EntranceLogDomainService EntranceLogService
        {
            get
            {
                if (_EntranceLogDomainService == null)
                {
                    _EntranceLogDomainService = new EntranceLogDomainService();
                }

                return _EntranceLogDomainService;
            }
        }   

        public bool InsertEntranceLog(EntranceLogDTO dtoEntranceLog)
        {
            var domainEntranceLog = EntranceLogMappers.ChangeDTOToEntranceLogNew(dtoEntranceLog);

            return EntranceLogService.InsertEntranceLog(domainEntranceLog);
        }

        public bool UpdateEntranceLog(EntranceLogDTO dtoEntranceLog)
        {
            var domainEntranceLog = EntranceLogMappers.ChangeDTOToEntranceLogNew(dtoEntranceLog);

            return EntranceLogService.UpdateEntranceLog(domainEntranceLog);
        }

        public bool DeleteEntranceLog(object id)
        {
            return EntranceLogService.DeleteEntranceLog(id);
        }

        public List<EntranceLogDTO> GetEntranceLogs()
        {
            var domainEntranceLogs = EntranceLogService.GetEntranceLogs();

            return EntranceLogMappers.ChangeEntranceLogToDTOs(domainEntranceLogs);
        }

		public EntranceLogDTO GetEntranceLogByKey(object id)
        {
            var domainEntranceLog = EntranceLogService.GetEntranceLogByKey(id);

            return EntranceLogMappers.ChangeEntranceLogToDTO(domainEntranceLog);
        }
	}
}
