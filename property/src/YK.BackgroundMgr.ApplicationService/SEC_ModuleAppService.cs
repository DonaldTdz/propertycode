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
	public partial class SEC_ModuleAppService
	{
		private SEC_ModuleDomainService _SEC_ModuleDomainService;
        protected SEC_ModuleDomainService SEC_ModuleService
        {
            get
            {
                if (_SEC_ModuleDomainService == null)
                {
                    _SEC_ModuleDomainService = new SEC_ModuleDomainService();
                }

                return _SEC_ModuleDomainService;
            }
        }   

        public bool InsertSEC_Module(SEC_ModuleDTO dtoSEC_Module)
        {
            var domainSEC_Module = SEC_ModuleMappers.ChangeDTOToSEC_ModuleNew(dtoSEC_Module);

            return SEC_ModuleService.InsertSEC_Module(domainSEC_Module);
        }

        public bool UpdateSEC_Module(SEC_ModuleDTO dtoSEC_Module)
        {
            var domainSEC_Module = SEC_ModuleMappers.ChangeDTOToSEC_ModuleNew(dtoSEC_Module);

            return SEC_ModuleService.UpdateSEC_Module(domainSEC_Module);
        }

        public bool DeleteSEC_Module(object id)
        {
            return SEC_ModuleService.DeleteSEC_Module(id);
        }

        public List<SEC_ModuleDTO> GetSEC_Modules()
        {
            var domainSEC_Modules = SEC_ModuleService.GetSEC_Modules();

            return SEC_ModuleMappers.ChangeSEC_ModuleToDTOs(domainSEC_Modules);
        }

		public SEC_ModuleDTO GetSEC_ModuleByKey(object id)
        {
            var domainSEC_Module = SEC_ModuleService.GetSEC_ModuleByKey(id);

            return SEC_ModuleMappers.ChangeSEC_ModuleToDTO(domainSEC_Module);
        }
	}
}
