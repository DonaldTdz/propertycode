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
	public partial class ProvinceAppService
	{
		private ProvinceDomainService _ProvinceDomainService;
        protected ProvinceDomainService ProvinceService
        {
            get
            {
                if (_ProvinceDomainService == null)
                {
                    _ProvinceDomainService = new ProvinceDomainService();
                }

                return _ProvinceDomainService;
            }
        }   

        public bool InsertProvince(ProvinceDTO dtoProvince)
        {
            var domainProvince = ProvinceMappers.ChangeDTOToProvinceNew(dtoProvince);

            return ProvinceService.InsertProvince(domainProvince);
        }

        public bool UpdateProvince(ProvinceDTO dtoProvince)
        {
            var domainProvince = ProvinceMappers.ChangeDTOToProvinceNew(dtoProvince);

            return ProvinceService.UpdateProvince(domainProvince);
        }

        public bool DeleteProvince(object id)
        {
            return ProvinceService.DeleteProvince(id);
        }

        public List<ProvinceDTO> GetProvinces()
        {
            var domainProvinces = ProvinceService.GetProvinces();

            return ProvinceMappers.ChangeProvinceToDTOs(domainProvinces);
        }

		public ProvinceDTO GetProvinceByKey(object id)
        {
            var domainProvince = ProvinceService.GetProvinceByKey(id);

            return ProvinceMappers.ChangeProvinceToDTO(domainProvince);
        }
	}
}
