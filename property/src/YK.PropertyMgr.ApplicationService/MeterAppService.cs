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
	public partial class MeterAppService
	{
		private MeterDomainService _MeterDomainService;
        protected MeterDomainService MeterService
        {
            get
            {
                if (_MeterDomainService == null)
                {
                    _MeterDomainService = new MeterDomainService();
                }

                return _MeterDomainService;
            }
        }   

        public bool InsertMeter(MeterDTO dtoMeter)
        {
            var domainMeter = MeterMappers.ChangeDTOToMeterNew(dtoMeter);

            return MeterService.InsertMeter(domainMeter);
        }

        public bool UpdateMeter(MeterDTO dtoMeter)
        {
            var domainMeter = MeterMappers.ChangeDTOToMeterNew(dtoMeter);

            return MeterService.UpdateMeter(domainMeter);
        }

        public bool DeleteMeter(object id)
        {
            return MeterService.DeleteMeter(id);
        }

        public List<MeterDTO> GetMeters()
        {
            var domainMeters = MeterService.GetMeters();

            return MeterMappers.ChangeMeterToDTOs(domainMeters);
        }

		public MeterDTO GetMeterByKey(object id)
        {
            var domainMeter = MeterService.GetMeterByKey(id);

            return MeterMappers.ChangeMeterToDTO(domainMeter);
        }
	}
}
