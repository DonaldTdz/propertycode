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
	public partial class ReceiptAppService
	{
		private ReceiptDomainService _ReceiptDomainService;
        protected ReceiptDomainService ReceiptService
        {
            get
            {
                if (_ReceiptDomainService == null)
                {
                    _ReceiptDomainService = new ReceiptDomainService();
                }

                return _ReceiptDomainService;
            }
        }   

        public bool InsertReceipt(ReceiptDTO dtoReceipt)
        {
            var domainReceipt = ReceiptMappers.ChangeDTOToReceiptNew(dtoReceipt);

            return ReceiptService.InsertReceipt(domainReceipt);
        }

        public bool UpdateReceipt(ReceiptDTO dtoReceipt)
        {
            var domainReceipt = ReceiptMappers.ChangeDTOToReceiptNew(dtoReceipt);

            return ReceiptService.UpdateReceipt(domainReceipt);
        }

        public bool DeleteReceipt(object id)
        {
            return ReceiptService.DeleteReceipt(id);
        }

        public List<ReceiptDTO> GetReceipts()
        {
            var domainReceipts = ReceiptService.GetReceipts();

            return ReceiptMappers.ChangeReceiptToDTOs(domainReceipts);
        }

		public ReceiptDTO GetReceiptByKey(object id)
        {
            var domainReceipt = ReceiptService.GetReceiptByKey(id);

            return ReceiptMappers.ChangeReceiptToDTO(domainReceipt);
        }
	}
}
