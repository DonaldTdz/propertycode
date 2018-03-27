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
	public partial class CountyAppService
	{
		private CountyDomainService _CountyDomainService;
        protected CountyDomainService CountyService
        {
            get
            {
                if (_CountyDomainService == null)
                {
                    _CountyDomainService = new CountyDomainService();
                }

                return _CountyDomainService;
            }
        }   

        public bool InsertCounty(CountyDTO dtoCounty)
        {
            var domainCounty = CountyMappers.ChangeDTOToCountyNew(dtoCounty);

            return CountyService.InsertCounty(domainCounty);
        }

        public bool UpdateCounty(CountyDTO dtoCounty)
        {
            var domainCounty = CountyMappers.ChangeDTOToCountyNew(dtoCounty);

            return CountyService.UpdateCounty(domainCounty);
        }

        public bool DeleteCounty(object id)
        {
            return CountyService.DeleteCounty(id);
        }

        public List<CountyDTO> GetCountys()
        {
            var domainCountys = CountyService.GetCountys();

            return CountyMappers.ChangeCountyToDTOs(domainCountys);
        }

		public CountyDTO GetCountyByKey(object id)
        {
            var domainCounty = CountyService.GetCountyByKey(id);

            return CountyMappers.ChangeCountyToDTO(domainCounty);
        }
	}
}
