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
	public partial class SEC_UserAppService
	{
		private SEC_UserDomainService _SEC_UserDomainService;
        protected SEC_UserDomainService SEC_UserService
        {
            get
            {
                if (_SEC_UserDomainService == null)
                {
                    _SEC_UserDomainService = new SEC_UserDomainService();
                }

                return _SEC_UserDomainService;
            }
        }   

        public bool InsertSEC_User(SEC_UserDTO dtoSEC_User)
        {
            var domainSEC_User = SEC_UserMappers.ChangeDTOToSEC_UserNew(dtoSEC_User);

            return SEC_UserService.InsertSEC_User(domainSEC_User);
        }

        public bool UpdateSEC_User(SEC_UserDTO dtoSEC_User)
        {
            var domainSEC_User = SEC_UserMappers.ChangeDTOToSEC_UserNew(dtoSEC_User);

            return SEC_UserService.UpdateSEC_User(domainSEC_User);
        }

        public bool DeleteSEC_User(object id)
        {
            return SEC_UserService.DeleteSEC_User(id);
        }

        public List<SEC_UserDTO> GetSEC_Users()
        {
            var domainSEC_Users = SEC_UserService.GetSEC_Users();

            return SEC_UserMappers.ChangeSEC_UserToDTOs(domainSEC_Users);
        }

		public SEC_UserDTO GetSEC_UserByKey(object id)
        {
            var domainSEC_User = SEC_UserService.GetSEC_UserByKey(id);

            return SEC_UserMappers.ChangeSEC_UserToDTO(domainSEC_User);
        }
	}
}
