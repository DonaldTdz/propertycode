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
	public partial class MeterReadRecordAppService
	{
		private MeterReadRecordDomainService _MeterReadRecordDomainService;
        protected MeterReadRecordDomainService MeterReadRecordService
        {
            get
            {
                if (_MeterReadRecordDomainService == null)
                {
                    _MeterReadRecordDomainService = new MeterReadRecordDomainService();
                }

                return _MeterReadRecordDomainService;
            }
        }   

        public bool InsertMeterReadRecord(MeterReadRecordDTO dtoMeterReadRecord)
        {
            var domainMeterReadRecord = MeterReadRecordMappers.ChangeDTOToMeterReadRecordNew(dtoMeterReadRecord);

            return MeterReadRecordService.InsertMeterReadRecord(domainMeterReadRecord);
        }

        public bool UpdateMeterReadRecord(MeterReadRecordDTO dtoMeterReadRecord)
        {
            var domainMeterReadRecord = MeterReadRecordMappers.ChangeDTOToMeterReadRecordNew(dtoMeterReadRecord);

            return MeterReadRecordService.UpdateMeterReadRecord(domainMeterReadRecord);
        }

        public bool DeleteMeterReadRecord(object id)
        {
            return MeterReadRecordService.DeleteMeterReadRecord(id);
        }

        public List<MeterReadRecordDTO> GetMeterReadRecords()
        {
            var domainMeterReadRecords = MeterReadRecordService.GetMeterReadRecords();

            return MeterReadRecordMappers.ChangeMeterReadRecordToDTOs(domainMeterReadRecords);
        }

		public MeterReadRecordDTO GetMeterReadRecordByKey(object id)
        {
            var domainMeterReadRecord = MeterReadRecordService.GetMeterReadRecordByKey(id);

            return MeterReadRecordMappers.ChangeMeterReadRecordToDTO(domainMeterReadRecord);
        }
	}
}
