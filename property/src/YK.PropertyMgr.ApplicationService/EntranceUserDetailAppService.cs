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
	public partial class EntranceUserDetailAppService
	{
		private EntranceUserDetailDomainService _EntranceUserDetailDomainService;
        protected EntranceUserDetailDomainService EntranceUserDetailService
        {
            get
            {
                if (_EntranceUserDetailDomainService == null)
                {
                    _EntranceUserDetailDomainService = new EntranceUserDetailDomainService();
                }

                return _EntranceUserDetailDomainService;
            }
        }   

        public bool InsertEntranceUserDetail(EntranceUserDetailDTO dtoEntranceUserDetail)
        {
            var domainEntranceUserDetail = EntranceUserDetailMappers.ChangeDTOToEntranceUserDetailNew(dtoEntranceUserDetail);

            return EntranceUserDetailService.InsertEntranceUserDetail(domainEntranceUserDetail);
        }

        public bool UpdateEntranceUserDetail(EntranceUserDetailDTO dtoEntranceUserDetail)
        {
            var domainEntranceUserDetail = EntranceUserDetailMappers.ChangeDTOToEntranceUserDetailNew(dtoEntranceUserDetail);

            return EntranceUserDetailService.UpdateEntranceUserDetail(domainEntranceUserDetail);
        }

        public bool DeleteEntranceUserDetail(object id)
        {
            return EntranceUserDetailService.DeleteEntranceUserDetail(id);
        }

        public List<EntranceUserDetailDTO> GetEntranceUserDetails()
        {
            var domainEntranceUserDetails = EntranceUserDetailService.GetEntranceUserDetails();

            return EntranceUserDetailMappers.ChangeEntranceUserDetailToDTOs(domainEntranceUserDetails);
        }

		public EntranceUserDetailDTO GetEntranceUserDetailByKey(object id)
        {
            var domainEntranceUserDetail = EntranceUserDetailService.GetEntranceUserDetailByKey(id);

            return EntranceUserDetailMappers.ChangeEntranceUserDetailToDTO(domainEntranceUserDetail);
        }
	}
}
