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
	public partial class SEC_RoleAppService
	{
		private SEC_RoleDomainService _SEC_RoleDomainService;
        protected SEC_RoleDomainService SEC_RoleService
        {
            get
            {
                if (_SEC_RoleDomainService == null)
                {
                    _SEC_RoleDomainService = new SEC_RoleDomainService();
                }

                return _SEC_RoleDomainService;
            }
        }   

        public bool InsertSEC_Role(SEC_RoleDTO dtoSEC_Role)
        {
            var domainSEC_Role = SEC_RoleMappers.ChangeDTOToSEC_RoleNew(dtoSEC_Role);

            return SEC_RoleService.InsertSEC_Role(domainSEC_Role);
        }

        public bool UpdateSEC_Role(SEC_RoleDTO dtoSEC_Role)
        {
            var domainSEC_Role = SEC_RoleMappers.ChangeDTOToSEC_RoleNew(dtoSEC_Role);

            return SEC_RoleService.UpdateSEC_Role(domainSEC_Role);
        }

        public bool DeleteSEC_Role(object id)
        {
            return SEC_RoleService.DeleteSEC_Role(id);
        }

        public List<SEC_RoleDTO> GetSEC_Roles()
        {
            var domainSEC_Roles = SEC_RoleService.GetSEC_Roles();

            return SEC_RoleMappers.ChangeSEC_RoleToDTOs(domainSEC_Roles);
        }

		public SEC_RoleDTO GetSEC_RoleByKey(object id)
        {
            var domainSEC_Role = SEC_RoleService.GetSEC_RoleByKey(id);

            return SEC_RoleMappers.ChangeSEC_RoleToDTO(domainSEC_Role);
        }
	}
}
