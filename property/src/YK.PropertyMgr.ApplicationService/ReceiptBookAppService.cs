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
	public partial class ReceiptBookAppService
	{
		private ReceiptBookDomainService _ReceiptBookDomainService;
        protected ReceiptBookDomainService ReceiptBookService
        {
            get
            {
                if (_ReceiptBookDomainService == null)
                {
                    _ReceiptBookDomainService = new ReceiptBookDomainService();
                }

                return _ReceiptBookDomainService;
            }
        }   

        public bool InsertReceiptBook(ReceiptBookDTO dtoReceiptBook)
        {
            var domainReceiptBook = ReceiptBookMappers.ChangeDTOToReceiptBookNew(dtoReceiptBook);

            return ReceiptBookService.InsertReceiptBook(domainReceiptBook);
        }

        public bool UpdateReceiptBook(ReceiptBookDTO dtoReceiptBook)
        {
            var domainReceiptBook = ReceiptBookMappers.ChangeDTOToReceiptBookNew(dtoReceiptBook);

            return ReceiptBookService.UpdateReceiptBook(domainReceiptBook);
        }

        public bool DeleteReceiptBook(object id)
        {
            return ReceiptBookService.DeleteReceiptBook(id);
        }

        public List<ReceiptBookDTO> GetReceiptBooks()
        {
            var domainReceiptBooks = ReceiptBookService.GetReceiptBooks();

            return ReceiptBookMappers.ChangeReceiptBookToDTOs(domainReceiptBooks);
        }

		public ReceiptBookDTO GetReceiptBookByKey(object id)
        {
            var domainReceiptBook = ReceiptBookService.GetReceiptBookByKey(id);

            return ReceiptBookMappers.ChangeReceiptBookToDTO(domainReceiptBook);
        }
	}
}
