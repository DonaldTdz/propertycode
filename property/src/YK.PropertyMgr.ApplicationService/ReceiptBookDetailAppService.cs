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
	public partial class ReceiptBookDetailAppService
	{
		private ReceiptBookDetailDomainService _ReceiptBookDetailDomainService;
        protected ReceiptBookDetailDomainService ReceiptBookDetailService
        {
            get
            {
                if (_ReceiptBookDetailDomainService == null)
                {
                    _ReceiptBookDetailDomainService = new ReceiptBookDetailDomainService();
                }

                return _ReceiptBookDetailDomainService;
            }
        }   

        public bool InsertReceiptBookDetail(ReceiptBookDetailDTO dtoReceiptBookDetail)
        {
            var domainReceiptBookDetail = ReceiptBookDetailMappers.ChangeDTOToReceiptBookDetailNew(dtoReceiptBookDetail);

            return ReceiptBookDetailService.InsertReceiptBookDetail(domainReceiptBookDetail);
        }

        public bool UpdateReceiptBookDetail(ReceiptBookDetailDTO dtoReceiptBookDetail)
        {
            var domainReceiptBookDetail = ReceiptBookDetailMappers.ChangeDTOToReceiptBookDetailNew(dtoReceiptBookDetail);

            return ReceiptBookDetailService.UpdateReceiptBookDetail(domainReceiptBookDetail);
        }

        public bool DeleteReceiptBookDetail(object id)
        {
            return ReceiptBookDetailService.DeleteReceiptBookDetail(id);
        }

        public List<ReceiptBookDetailDTO> GetReceiptBookDetails()
        {
            var domainReceiptBookDetails = ReceiptBookDetailService.GetReceiptBookDetails();

            return ReceiptBookDetailMappers.ChangeReceiptBookDetailToDTOs(domainReceiptBookDetails);
        }

		public ReceiptBookDetailDTO GetReceiptBookDetailByKey(object id)
        {
            var domainReceiptBookDetail = ReceiptBookDetailService.GetReceiptBookDetailByKey(id);

            return ReceiptBookDetailMappers.ChangeReceiptBookDetailToDTO(domainReceiptBookDetail);
        }
	}
}
