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
	public partial class SEC_PropertyAppService
	{
		private SEC_PropertyDomainService _SEC_PropertyDomainService;
        protected SEC_PropertyDomainService SEC_PropertyService
        {
            get
            {
                if (_SEC_PropertyDomainService == null)
                {
                    _SEC_PropertyDomainService = new SEC_PropertyDomainService();
                }

                return _SEC_PropertyDomainService;
            }
        }   

        public bool InsertSEC_Property(SEC_PropertyDTO dtoSEC_Property)
        {
            var domainSEC_Property = SEC_PropertyMappers.ChangeDTOToSEC_PropertyNew(dtoSEC_Property);

            return SEC_PropertyService.InsertSEC_Property(domainSEC_Property);
        }

        public bool UpdateSEC_Property(SEC_PropertyDTO dtoSEC_Property)
        {
            var domainSEC_Property = SEC_PropertyMappers.ChangeDTOToSEC_PropertyNew(dtoSEC_Property);

            return SEC_PropertyService.UpdateSEC_Property(domainSEC_Property);
        }

        public bool DeleteSEC_Property(object id)
        {
            return SEC_PropertyService.DeleteSEC_Property(id);
        }

        public List<SEC_PropertyDTO> GetSEC_Propertys()
        {
            var domainSEC_Propertys = SEC_PropertyService.GetSEC_Propertys();

            return SEC_PropertyMappers.ChangeSEC_PropertyToDTOs(domainSEC_Propertys);
        }

		public SEC_PropertyDTO GetSEC_PropertyByKey(object id)
        {
            var domainSEC_Property = SEC_PropertyService.GetSEC_PropertyByKey(id);

            return SEC_PropertyMappers.ChangeSEC_PropertyToDTO(domainSEC_Property);
        }
	}
}
