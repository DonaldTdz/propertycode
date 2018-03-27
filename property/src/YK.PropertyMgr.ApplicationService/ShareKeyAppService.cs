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
	public partial class ShareKeyAppService
	{
		private ShareKeyDomainService _ShareKeyDomainService;
        protected ShareKeyDomainService ShareKeyService
        {
            get
            {
                if (_ShareKeyDomainService == null)
                {
                    _ShareKeyDomainService = new ShareKeyDomainService();
                }

                return _ShareKeyDomainService;
            }
        }   

        public bool InsertShareKey(ShareKeyDTO dtoShareKey)
        {
            var domainShareKey = ShareKeyMappers.ChangeDTOToShareKeyNew(dtoShareKey);

            return ShareKeyService.InsertShareKey(domainShareKey);
        }

        public bool UpdateShareKey(ShareKeyDTO dtoShareKey)
        {
            var domainShareKey = ShareKeyMappers.ChangeDTOToShareKeyNew(dtoShareKey);

            return ShareKeyService.UpdateShareKey(domainShareKey);
        }

        public bool DeleteShareKey(object id)
        {
            return ShareKeyService.DeleteShareKey(id);
        }

        public List<ShareKeyDTO> GetShareKeys()
        {
            var domainShareKeys = ShareKeyService.GetShareKeys();

            return ShareKeyMappers.ChangeShareKeyToDTOs(domainShareKeys);
        }

		public ShareKeyDTO GetShareKeyByKey(object id)
        {
            var domainShareKey = ShareKeyService.GetShareKeyByKey(id);

            return ShareKeyMappers.ChangeShareKeyToDTO(domainShareKey);
        }
	}
}
