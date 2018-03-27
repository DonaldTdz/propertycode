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
	public partial class SEC_OperateAppService
	{
		private SEC_OperateDomainService _SEC_OperateDomainService;
        protected SEC_OperateDomainService SEC_OperateService
        {
            get
            {
                if (_SEC_OperateDomainService == null)
                {
                    _SEC_OperateDomainService = new SEC_OperateDomainService();
                }

                return _SEC_OperateDomainService;
            }
        }   

        public bool InsertSEC_Operate(SEC_OperateDTO dtoSEC_Operate)
        {
            var domainSEC_Operate = SEC_OperateMappers.ChangeDTOToSEC_OperateNew(dtoSEC_Operate);

            return SEC_OperateService.InsertSEC_Operate(domainSEC_Operate);
        }

        public bool UpdateSEC_Operate(SEC_OperateDTO dtoSEC_Operate)
        {
            var domainSEC_Operate = SEC_OperateMappers.ChangeDTOToSEC_OperateNew(dtoSEC_Operate);

            return SEC_OperateService.UpdateSEC_Operate(domainSEC_Operate);
        }

        public bool DeleteSEC_Operate(object id)
        {
            return SEC_OperateService.DeleteSEC_Operate(id);
        }

        public List<SEC_OperateDTO> GetSEC_Operates()
        {
            var domainSEC_Operates = SEC_OperateService.GetSEC_Operates();

            return SEC_OperateMappers.ChangeSEC_OperateToDTOs(domainSEC_Operates);
        }

		public SEC_OperateDTO GetSEC_OperateByKey(object id)
        {
            var domainSEC_Operate = SEC_OperateService.GetSEC_OperateByKey(id);

            return SEC_OperateMappers.ChangeSEC_OperateToDTO(domainSEC_Operate);
        }
	}
}
