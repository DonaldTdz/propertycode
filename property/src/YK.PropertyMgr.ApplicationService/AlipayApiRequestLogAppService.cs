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
	public partial class AlipayApiRequestLogAppService
	{
		private AlipayApiRequestLogDomainService _AlipayApiRequestLogDomainService;
        protected AlipayApiRequestLogDomainService AlipayApiRequestLogService
        {
            get
            {
                if (_AlipayApiRequestLogDomainService == null)
                {
                    _AlipayApiRequestLogDomainService = new AlipayApiRequestLogDomainService();
                }

                return _AlipayApiRequestLogDomainService;
            }
        }   

        public bool InsertAlipayApiRequestLog(AlipayApiRequestLogDTO dtoAlipayApiRequestLog)
        {
            var domainAlipayApiRequestLog = AlipayApiRequestLogMappers.ChangeDTOToAlipayApiRequestLogNew(dtoAlipayApiRequestLog);

            return AlipayApiRequestLogService.InsertAlipayApiRequestLog(domainAlipayApiRequestLog);
        }

        public bool UpdateAlipayApiRequestLog(AlipayApiRequestLogDTO dtoAlipayApiRequestLog)
        {
            var domainAlipayApiRequestLog = AlipayApiRequestLogMappers.ChangeDTOToAlipayApiRequestLogNew(dtoAlipayApiRequestLog);

            return AlipayApiRequestLogService.UpdateAlipayApiRequestLog(domainAlipayApiRequestLog);
        }

        public bool DeleteAlipayApiRequestLog(object id)
        {
            return AlipayApiRequestLogService.DeleteAlipayApiRequestLog(id);
        }

        public List<AlipayApiRequestLogDTO> GetAlipayApiRequestLogs()
        {
            var domainAlipayApiRequestLogs = AlipayApiRequestLogService.GetAlipayApiRequestLogs();

            return AlipayApiRequestLogMappers.ChangeAlipayApiRequestLogToDTOs(domainAlipayApiRequestLogs);
        }

		public AlipayApiRequestLogDTO GetAlipayApiRequestLogByKey(object id)
        {
            var domainAlipayApiRequestLog = AlipayApiRequestLogService.GetAlipayApiRequestLogByKey(id);

            return AlipayApiRequestLogMappers.ChangeAlipayApiRequestLogToDTO(domainAlipayApiRequestLog);
        }
	}
}
