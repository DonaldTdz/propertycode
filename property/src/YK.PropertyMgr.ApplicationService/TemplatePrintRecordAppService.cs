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
	public partial class TemplatePrintRecordAppService
	{
		private TemplatePrintRecordDomainService _TemplatePrintRecordDomainService;
        protected TemplatePrintRecordDomainService TemplatePrintRecordService
        {
            get
            {
                if (_TemplatePrintRecordDomainService == null)
                {
                    _TemplatePrintRecordDomainService = new TemplatePrintRecordDomainService();
                }

                return _TemplatePrintRecordDomainService;
            }
        }   

        public bool InsertTemplatePrintRecord(TemplatePrintRecordDTO dtoTemplatePrintRecord)
        {
            var domainTemplatePrintRecord = TemplatePrintRecordMappers.ChangeDTOToTemplatePrintRecordNew(dtoTemplatePrintRecord);

            return TemplatePrintRecordService.InsertTemplatePrintRecord(domainTemplatePrintRecord);
        }

        public bool UpdateTemplatePrintRecord(TemplatePrintRecordDTO dtoTemplatePrintRecord)
        {
            var domainTemplatePrintRecord = TemplatePrintRecordMappers.ChangeDTOToTemplatePrintRecordNew(dtoTemplatePrintRecord);

            return TemplatePrintRecordService.UpdateTemplatePrintRecord(domainTemplatePrintRecord);
        }

        public bool DeleteTemplatePrintRecord(object id)
        {
            return TemplatePrintRecordService.DeleteTemplatePrintRecord(id);
        }

        public List<TemplatePrintRecordDTO> GetTemplatePrintRecords()
        {
            var domainTemplatePrintRecords = TemplatePrintRecordService.GetTemplatePrintRecords();

            return TemplatePrintRecordMappers.ChangeTemplatePrintRecordToDTOs(domainTemplatePrintRecords);
        }

		public TemplatePrintRecordDTO GetTemplatePrintRecordByKey(object id)
        {
            var domainTemplatePrintRecord = TemplatePrintRecordService.GetTemplatePrintRecordByKey(id);

            return TemplatePrintRecordMappers.ChangeTemplatePrintRecordToDTO(domainTemplatePrintRecord);
        }
	}
}
