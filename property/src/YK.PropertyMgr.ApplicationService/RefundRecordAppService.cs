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
	public partial class RefundRecordAppService
	{
		private RefundRecordDomainService _RefundRecordDomainService;
        protected RefundRecordDomainService RefundRecordService
        {
            get
            {
                if (_RefundRecordDomainService == null)
                {
                    _RefundRecordDomainService = new RefundRecordDomainService();
                }

                return _RefundRecordDomainService;
            }
        }   

        public bool InsertRefundRecord(RefundRecordDTO dtoRefundRecord)
        {
            var domainRefundRecord = RefundRecordMappers.ChangeDTOToRefundRecordNew(dtoRefundRecord);

            return RefundRecordService.InsertRefundRecord(domainRefundRecord);
        }

        public bool UpdateRefundRecord(RefundRecordDTO dtoRefundRecord)
        {
            var domainRefundRecord = RefundRecordMappers.ChangeDTOToRefundRecordNew(dtoRefundRecord);

            return RefundRecordService.UpdateRefundRecord(domainRefundRecord);
        }

        public bool DeleteRefundRecord(object id)
        {
            return RefundRecordService.DeleteRefundRecord(id);
        }

        public List<RefundRecordDTO> GetRefundRecords()
        {
            var domainRefundRecords = RefundRecordService.GetRefundRecords();

            return RefundRecordMappers.ChangeRefundRecordToDTOs(domainRefundRecords);
        }

		public RefundRecordDTO GetRefundRecordByKey(object id)
        {
            var domainRefundRecord = RefundRecordService.GetRefundRecordByKey(id);

            return RefundRecordMappers.ChangeRefundRecordToDTO(domainRefundRecord);
        }
	}
}
