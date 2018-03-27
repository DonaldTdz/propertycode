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
	public partial class EntranceAppService
	{
		private EntranceDomainService _EntranceDomainService;
        protected EntranceDomainService EntranceService
        {
            get
            {
                if (_EntranceDomainService == null)
                {
                    _EntranceDomainService = new EntranceDomainService();
                }

                return _EntranceDomainService;
            }
        }   

        public bool InsertEntrance(EntranceDTO dtoEntrance)
        {
            var domainEntrance = EntranceMappers.ChangeDTOToEntranceNew(dtoEntrance);

            return EntranceService.InsertEntrance(domainEntrance);
        }

        public bool UpdateEntrance(EntranceDTO dtoEntrance)
        {
            var domainEntrance = EntranceMappers.ChangeDTOToEntranceNew(dtoEntrance);

            return EntranceService.UpdateEntrance(domainEntrance);
        }

        public bool DeleteEntrance(object id)
        {
            return EntranceService.DeleteEntrance(id);
        }

        public List<EntranceDTO> GetEntrances()
        {
            var domainEntrances = EntranceService.GetEntrances();

            return EntranceMappers.ChangeEntranceToDTOs(domainEntrances);
        }

		public EntranceDTO GetEntranceByKey(object id)
        {
            var domainEntrance = EntranceService.GetEntranceByKey(id);

            return EntranceMappers.ChangeEntranceToDTO(domainEntrance);
        }
	}
}
