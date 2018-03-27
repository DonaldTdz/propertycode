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
	public partial class EntranceUserAppService
	{
		private EntranceUserDomainService _EntranceUserDomainService;
        protected EntranceUserDomainService EntranceUserService
        {
            get
            {
                if (_EntranceUserDomainService == null)
                {
                    _EntranceUserDomainService = new EntranceUserDomainService();
                }

                return _EntranceUserDomainService;
            }
        }   

        public bool InsertEntranceUser(EntranceUserDTO dtoEntranceUser)
        {
            var domainEntranceUser = EntranceUserMappers.ChangeDTOToEntranceUserNew(dtoEntranceUser);

            return EntranceUserService.InsertEntranceUser(domainEntranceUser);
        }

        public bool UpdateEntranceUser(EntranceUserDTO dtoEntranceUser)
        {
            var domainEntranceUser = EntranceUserMappers.ChangeDTOToEntranceUserNew(dtoEntranceUser);

            return EntranceUserService.UpdateEntranceUser(domainEntranceUser);
        }

        public bool DeleteEntranceUser(object id)
        {
            return EntranceUserService.DeleteEntranceUser(id);
        }

        public List<EntranceUserDTO> GetEntranceUsers()
        {
            var domainEntranceUsers = EntranceUserService.GetEntranceUsers();

            return EntranceUserMappers.ChangeEntranceUserToDTOs(domainEntranceUsers);
        }

		public EntranceUserDTO GetEntranceUserByKey(object id)
        {
            var domainEntranceUser = EntranceUserService.GetEntranceUserByKey(id);

            return EntranceUserMappers.ChangeEntranceUserToDTO(domainEntranceUser);
        }
	}
}
