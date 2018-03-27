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
	public partial class TemplatePrintRecordDetailAppService
	{
		private TemplatePrintRecordDetailDomainService _TemplatePrintRecordDetailDomainService;
        protected TemplatePrintRecordDetailDomainService TemplatePrintRecordDetailService
        {
            get
            {
                if (_TemplatePrintRecordDetailDomainService == null)
                {
                    _TemplatePrintRecordDetailDomainService = new TemplatePrintRecordDetailDomainService();
                }

                return _TemplatePrintRecordDetailDomainService;
            }
        }   

        public bool InsertTemplatePrintRecordDetail(TemplatePrintRecordDetailDTO dtoTemplatePrintRecordDetail)
        {
            var domainTemplatePrintRecordDetail = TemplatePrintRecordDetailMappers.ChangeDTOToTemplatePrintRecordDetailNew(dtoTemplatePrintRecordDetail);

            return TemplatePrintRecordDetailService.InsertTemplatePrintRecordDetail(domainTemplatePrintRecordDetail);
        }

        public bool UpdateTemplatePrintRecordDetail(TemplatePrintRecordDetailDTO dtoTemplatePrintRecordDetail)
        {
            var domainTemplatePrintRecordDetail = TemplatePrintRecordDetailMappers.ChangeDTOToTemplatePrintRecordDetailNew(dtoTemplatePrintRecordDetail);

            return TemplatePrintRecordDetailService.UpdateTemplatePrintRecordDetail(domainTemplatePrintRecordDetail);
        }

        public bool DeleteTemplatePrintRecordDetail(object id)
        {
            return TemplatePrintRecordDetailService.DeleteTemplatePrintRecordDetail(id);
        }

        public List<TemplatePrintRecordDetailDTO> GetTemplatePrintRecordDetails()
        {
            var domainTemplatePrintRecordDetails = TemplatePrintRecordDetailService.GetTemplatePrintRecordDetails();

            return TemplatePrintRecordDetailMappers.ChangeTemplatePrintRecordDetailToDTOs(domainTemplatePrintRecordDetails);
        }

		public TemplatePrintRecordDetailDTO GetTemplatePrintRecordDetailByKey(object id)
        {
            var domainTemplatePrintRecordDetail = TemplatePrintRecordDetailService.GetTemplatePrintRecordDetailByKey(id);

            return TemplatePrintRecordDetailMappers.ChangeTemplatePrintRecordDetailToDTO(domainTemplatePrintRecordDetail);
        }
	}
}
