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
	public partial class SEC_AdminUserAppService
	{
		private SEC_AdminUserDomainService _SEC_AdminUserDomainService;
        protected SEC_AdminUserDomainService SEC_AdminUserService
        {
            get
            {
                if (_SEC_AdminUserDomainService == null)
                {
                    _SEC_AdminUserDomainService = new SEC_AdminUserDomainService();
                }

                return _SEC_AdminUserDomainService;
            }
        }   

        public bool InsertSEC_AdminUser(SEC_AdminUserDTO dtoSEC_AdminUser)
        {
            var domainSEC_AdminUser = SEC_AdminUserMappers.ChangeDTOToSEC_AdminUserNew(dtoSEC_AdminUser);

            return SEC_AdminUserService.InsertSEC_AdminUser(domainSEC_AdminUser);
        }

        public bool UpdateSEC_AdminUser(SEC_AdminUserDTO dtoSEC_AdminUser)
        {
            var domainSEC_AdminUser = SEC_AdminUserMappers.ChangeDTOToSEC_AdminUserNew(dtoSEC_AdminUser);

            return SEC_AdminUserService.UpdateSEC_AdminUser(domainSEC_AdminUser);
        }

        public bool DeleteSEC_AdminUser(object id)
        {
            return SEC_AdminUserService.DeleteSEC_AdminUser(id);
        }

        public List<SEC_AdminUserDTO> GetSEC_AdminUsers()
        {
            var domainSEC_AdminUsers = SEC_AdminUserService.GetSEC_AdminUsers();

            return SEC_AdminUserMappers.ChangeSEC_AdminUserToDTOs(domainSEC_AdminUsers);
        }

		public SEC_AdminUserDTO GetSEC_AdminUserByKey(object id)
        {
            var domainSEC_AdminUser = SEC_AdminUserService.GetSEC_AdminUserByKey(id);

            return SEC_AdminUserMappers.ChangeSEC_AdminUserToDTO(domainSEC_AdminUser);
        }
	}
}
