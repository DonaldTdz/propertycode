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
	public partial class ReceiptBookHistoryAppService
	{
		private ReceiptBookHistoryDomainService _ReceiptBookHistoryDomainService;
        protected ReceiptBookHistoryDomainService ReceiptBookHistoryService
        {
            get
            {
                if (_ReceiptBookHistoryDomainService == null)
                {
                    _ReceiptBookHistoryDomainService = new ReceiptBookHistoryDomainService();
                }

                return _ReceiptBookHistoryDomainService;
            }
        }   

        public bool InsertReceiptBookHistory(ReceiptBookHistoryDTO dtoReceiptBookHistory)
        {
            var domainReceiptBookHistory = ReceiptBookHistoryMappers.ChangeDTOToReceiptBookHistoryNew(dtoReceiptBookHistory);

            return ReceiptBookHistoryService.InsertReceiptBookHistory(domainReceiptBookHistory);
        }

        public bool UpdateReceiptBookHistory(ReceiptBookHistoryDTO dtoReceiptBookHistory)
        {
            var domainReceiptBookHistory = ReceiptBookHistoryMappers.ChangeDTOToReceiptBookHistoryNew(dtoReceiptBookHistory);

            return ReceiptBookHistoryService.UpdateReceiptBookHistory(domainReceiptBookHistory);
        }

        public bool DeleteReceiptBookHistory(object id)
        {
            return ReceiptBookHistoryService.DeleteReceiptBookHistory(id);
        }

        public List<ReceiptBookHistoryDTO> GetReceiptBookHistorys()
        {
            var domainReceiptBookHistorys = ReceiptBookHistoryService.GetReceiptBookHistorys();

            return ReceiptBookHistoryMappers.ChangeReceiptBookHistoryToDTOs(domainReceiptBookHistorys);
        }

		public ReceiptBookHistoryDTO GetReceiptBookHistoryByKey(object id)
        {
            var domainReceiptBookHistory = ReceiptBookHistoryService.GetReceiptBookHistoryByKey(id);

            return ReceiptBookHistoryMappers.ChangeReceiptBookHistoryToDTO(domainReceiptBookHistory);
        }
	}
}
