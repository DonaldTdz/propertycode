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
	public partial class SEC_User_OwnerAppService
	{
		private SEC_User_OwnerDomainService _SEC_User_OwnerDomainService;
        protected SEC_User_OwnerDomainService SEC_User_OwnerService
        {
            get
            {
                if (_SEC_User_OwnerDomainService == null)
                {
                    _SEC_User_OwnerDomainService = new SEC_User_OwnerDomainService();
                }

                return _SEC_User_OwnerDomainService;
            }
        }   

        public bool InsertSEC_User_Owner(SEC_User_OwnerDTO dtoSEC_User_Owner)
        {
            var domainSEC_User_Owner = SEC_User_OwnerMappers.ChangeDTOToSEC_User_OwnerNew(dtoSEC_User_Owner);

            return SEC_User_OwnerService.InsertSEC_User_Owner(domainSEC_User_Owner);
        }

        public bool UpdateSEC_User_Owner(SEC_User_OwnerDTO dtoSEC_User_Owner)
        {
            var domainSEC_User_Owner = SEC_User_OwnerMappers.ChangeDTOToSEC_User_OwnerNew(dtoSEC_User_Owner);

            return SEC_User_OwnerService.UpdateSEC_User_Owner(domainSEC_User_Owner);
        }

        public bool DeleteSEC_User_Owner(object id)
        {
            return SEC_User_OwnerService.DeleteSEC_User_Owner(id);
        }

        public List<SEC_User_OwnerDTO> GetSEC_User_Owners()
        {
            var domainSEC_User_Owners = SEC_User_OwnerService.GetSEC_User_Owners();

            return SEC_User_OwnerMappers.ChangeSEC_User_OwnerToDTOs(domainSEC_User_Owners);
        }

		public SEC_User_OwnerDTO GetSEC_User_OwnerByKey(object id)
        {
            var domainSEC_User_Owner = SEC_User_OwnerService.GetSEC_User_OwnerByKey(id);

            return SEC_User_OwnerMappers.ChangeSEC_User_OwnerToDTO(domainSEC_User_Owner);
        }
	}
}
