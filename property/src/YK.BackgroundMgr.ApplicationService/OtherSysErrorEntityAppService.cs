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
	public partial class OtherSysErrorEntityAppService
	{
		private OtherSysErrorEntityDomainService _OtherSysErrorEntityDomainService;
        protected OtherSysErrorEntityDomainService OtherSysErrorEntityService
        {
            get
            {
                if (_OtherSysErrorEntityDomainService == null)
                {
                    _OtherSysErrorEntityDomainService = new OtherSysErrorEntityDomainService();
                }

                return _OtherSysErrorEntityDomainService;
            }
        }   

        public bool InsertOtherSysErrorEntity(OtherSysErrorEntityDTO dtoOtherSysErrorEntity)
        {
            var domainOtherSysErrorEntity = OtherSysErrorEntityMappers.ChangeDTOToOtherSysErrorEntityNew(dtoOtherSysErrorEntity);

            return OtherSysErrorEntityService.InsertOtherSysErrorEntity(domainOtherSysErrorEntity);
        }

        public bool UpdateOtherSysErrorEntity(OtherSysErrorEntityDTO dtoOtherSysErrorEntity)
        {
            var domainOtherSysErrorEntity = OtherSysErrorEntityMappers.ChangeDTOToOtherSysErrorEntityNew(dtoOtherSysErrorEntity);

            return OtherSysErrorEntityService.UpdateOtherSysErrorEntity(domainOtherSysErrorEntity);
        }

        public bool DeleteOtherSysErrorEntity(object id)
        {
            return OtherSysErrorEntityService.DeleteOtherSysErrorEntity(id);
        }

        public List<OtherSysErrorEntityDTO> GetOtherSysErrorEntitys()
        {
            var domainOtherSysErrorEntitys = OtherSysErrorEntityService.GetOtherSysErrorEntitys();

            return OtherSysErrorEntityMappers.ChangeOtherSysErrorEntityToDTOs(domainOtherSysErrorEntitys);
        }

		public OtherSysErrorEntityDTO GetOtherSysErrorEntityByKey(object id)
        {
            var domainOtherSysErrorEntity = OtherSysErrorEntityService.GetOtherSysErrorEntityByKey(id);

            return OtherSysErrorEntityMappers.ChangeOtherSysErrorEntityToDTO(domainOtherSysErrorEntity);
        }
	}
}
