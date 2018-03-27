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
	public partial class PreferentialRecordAppService
	{
		private PreferentialRecordDomainService _PreferentialRecordDomainService;
        protected PreferentialRecordDomainService PreferentialRecordService
        {
            get
            {
                if (_PreferentialRecordDomainService == null)
                {
                    _PreferentialRecordDomainService = new PreferentialRecordDomainService();
                }

                return _PreferentialRecordDomainService;
            }
        }   

        public bool InsertPreferentialRecord(PreferentialRecordDTO dtoPreferentialRecord)
        {
            var domainPreferentialRecord = PreferentialRecordMappers.ChangeDTOToPreferentialRecordNew(dtoPreferentialRecord);

            return PreferentialRecordService.InsertPreferentialRecord(domainPreferentialRecord);
        }

        public bool UpdatePreferentialRecord(PreferentialRecordDTO dtoPreferentialRecord)
        {
            var domainPreferentialRecord = PreferentialRecordMappers.ChangeDTOToPreferentialRecordNew(dtoPreferentialRecord);

            return PreferentialRecordService.UpdatePreferentialRecord(domainPreferentialRecord);
        }

        public bool DeletePreferentialRecord(object id)
        {
            return PreferentialRecordService.DeletePreferentialRecord(id);
        }

        public List<PreferentialRecordDTO> GetPreferentialRecords()
        {
            var domainPreferentialRecords = PreferentialRecordService.GetPreferentialRecords();

            return PreferentialRecordMappers.ChangePreferentialRecordToDTOs(domainPreferentialRecords);
        }

		public PreferentialRecordDTO GetPreferentialRecordByKey(object id)
        {
            var domainPreferentialRecord = PreferentialRecordService.GetPreferentialRecordByKey(id);

            return PreferentialRecordMappers.ChangePreferentialRecordToDTO(domainPreferentialRecord);
        }
	}
}
