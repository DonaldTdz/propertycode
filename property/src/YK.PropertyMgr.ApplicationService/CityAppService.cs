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
	public partial class CityAppService
	{
		private CityDomainService _CityDomainService;
        protected CityDomainService CityService
        {
            get
            {
                if (_CityDomainService == null)
                {
                    _CityDomainService = new CityDomainService();
                }

                return _CityDomainService;
            }
        }   

        public bool InsertCity(CityDTO dtoCity)
        {
            var domainCity = CityMappers.ChangeDTOToCityNew(dtoCity);

            return CityService.InsertCity(domainCity);
        }

        public bool UpdateCity(CityDTO dtoCity)
        {
            var domainCity = CityMappers.ChangeDTOToCityNew(dtoCity);

            return CityService.UpdateCity(domainCity);
        }

        public bool DeleteCity(object id)
        {
            return CityService.DeleteCity(id);
        }

        public List<CityDTO> GetCitys()
        {
            var domainCitys = CityService.GetCitys();

            return CityMappers.ChangeCityToDTOs(domainCitys);
        }

		public CityDTO GetCityByKey(object id)
        {
            var domainCity = CityService.GetCityByKey(id);

            return CityMappers.ChangeCityToDTO(domainCity);
        }
	}
}
