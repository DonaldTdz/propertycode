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
	public partial class ChargeRecordAppService
	{
		private ChargeRecordDomainService _ChargeRecordDomainService;
        protected ChargeRecordDomainService ChargeRecordService
        {
            get
            {
                if (_ChargeRecordDomainService == null)
                {
                    _ChargeRecordDomainService = new ChargeRecordDomainService();
                }

                return _ChargeRecordDomainService;
            }
        }   

        public bool InsertChargeRecord(ChargeRecordDTO dtoChargeRecord)
        {
            var domainChargeRecord = ChargeRecordMappers.ChangeDTOToChargeRecordNew(dtoChargeRecord);

            return ChargeRecordService.InsertChargeRecord(domainChargeRecord);
        }

        public bool UpdateChargeRecord(ChargeRecordDTO dtoChargeRecord)
        {
            var domainChargeRecord = ChargeRecordMappers.ChangeDTOToChargeRecordNew(dtoChargeRecord);

            return ChargeRecordService.UpdateChargeRecord(domainChargeRecord);
        }

        public bool DeleteChargeRecord(object id)
        {
            return ChargeRecordService.DeleteChargeRecord(id);
        }

        public List<ChargeRecordDTO> GetChargeRecords()
        {
            var domainChargeRecords = ChargeRecordService.GetChargeRecords();

            return ChargeRecordMappers.ChangeChargeRecordToDTOs(domainChargeRecords);
        }

		public ChargeRecordDTO GetChargeRecordByKey(object id)
        {
            var domainChargeRecord = ChargeRecordService.GetChargeRecordByKey(id);

            return ChargeRecordMappers.ChangeChargeRecordToDTO(domainChargeRecord);
        }
	}
}
