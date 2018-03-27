using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.ApplicationMapper;
using YK.BackgroundMgr.DomainService;

namespace YK.BackgroundMgr.ApplicationService
{
	public partial class SEC_FieldAppService
	{
		private SEC_FieldDomainService _SEC_FieldDomainService;
        protected SEC_FieldDomainService SEC_FieldService
        {
            get
            {
                if (_SEC_FieldDomainService == null)
                {
                    _SEC_FieldDomainService = new SEC_FieldDomainService();
                }

                return _SEC_FieldDomainService;
            }
        }   

        public bool InsertSEC_Field(SEC_FieldDTO dtoSEC_Field)
        {
            var domainSEC_Field = SEC_FieldMappers.ChangeDTOToSEC_FieldNew(dtoSEC_Field);

            return SEC_FieldService.InsertSEC_Field(domainSEC_Field);
        }

        public bool UpdateSEC_Field(SEC_FieldDTO dtoSEC_Field)
        {
            var domainSEC_Field = SEC_FieldMappers.ChangeDTOToSEC_FieldNew(dtoSEC_Field);

            return SEC_FieldService.UpdateSEC_Field(domainSEC_Field);
        }

        public bool DeleteSEC_Field(object id)
        {
            return SEC_FieldService.DeleteSEC_Field(id);
        }

        public List<SEC_FieldDTO> GetSEC_Fields()
        {
            var domainSEC_Fields = SEC_FieldService.GetSEC_Fields();

            return SEC_FieldMappers.ChangeSEC_FieldToDTOs(domainSEC_Fields);
        }

		public SEC_FieldDTO GetSEC_FieldByKey(object id)
        {
            var domainSEC_Field = SEC_FieldService.GetSEC_FieldByKey(id);

            return SEC_FieldMappers.ChangeSEC_FieldToDTO(domainSEC_Field);
        }
	}
}
