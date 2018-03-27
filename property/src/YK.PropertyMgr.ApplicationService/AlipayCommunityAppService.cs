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
	public partial class AlipayCommunityAppService
	{
		private AlipayCommunityDomainService _AlipayCommunityDomainService;
        protected AlipayCommunityDomainService AlipayCommunityService
        {
            get
            {
                if (_AlipayCommunityDomainService == null)
                {
                    _AlipayCommunityDomainService = new AlipayCommunityDomainService();
                }

                return _AlipayCommunityDomainService;
            }
        }   

        public bool InsertAlipayCommunity(AlipayCommunityDTO dtoAlipayCommunity)
        {
            var domainAlipayCommunity = AlipayCommunityMappers.ChangeDTOToAlipayCommunityNew(dtoAlipayCommunity);

            return AlipayCommunityService.InsertAlipayCommunity(domainAlipayCommunity);
        }

        public bool UpdateAlipayCommunity(AlipayCommunityDTO dtoAlipayCommunity)
        {
            var domainAlipayCommunity = AlipayCommunityMappers.ChangeDTOToAlipayCommunityNew(dtoAlipayCommunity);

            return AlipayCommunityService.UpdateAlipayCommunity(domainAlipayCommunity);
        }

        public bool DeleteAlipayCommunity(object id)
        {
            return AlipayCommunityService.DeleteAlipayCommunity(id);
        }

        public List<AlipayCommunityDTO> GetAlipayCommunitys()
        {
            var domainAlipayCommunitys = AlipayCommunityService.GetAlipayCommunitys();

            return AlipayCommunityMappers.ChangeAlipayCommunityToDTOs(domainAlipayCommunitys);
        }

		public AlipayCommunityDTO GetAlipayCommunityByKey(object id)
        {
            var domainAlipayCommunity = AlipayCommunityService.GetAlipayCommunityByKey(id);

            return AlipayCommunityMappers.ChangeAlipayCommunityToDTO(domainAlipayCommunity);
        }
	}
}
