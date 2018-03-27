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
	public partial class SEC_DeptAppService
	{
		private SEC_DeptDomainService _SEC_DeptDomainService;
        protected SEC_DeptDomainService SEC_DeptService
        {
            get
            {
                if (_SEC_DeptDomainService == null)
                {
                    _SEC_DeptDomainService = new SEC_DeptDomainService();
                }

                return _SEC_DeptDomainService;
            }
        }   

        public bool InsertSEC_Dept(SEC_DeptDTO dtoSEC_Dept)
        {
            var domainSEC_Dept = SEC_DeptMappers.ChangeDTOToSEC_DeptNew(dtoSEC_Dept);

            return SEC_DeptService.InsertSEC_Dept(domainSEC_Dept);
        }

        public bool UpdateSEC_Dept(SEC_DeptDTO dtoSEC_Dept)
        {
            var domainSEC_Dept = SEC_DeptMappers.ChangeDTOToSEC_DeptNew(dtoSEC_Dept);

            return SEC_DeptService.UpdateSEC_Dept(domainSEC_Dept);
        }

        public bool DeleteSEC_Dept(object id)
        {
            return SEC_DeptService.DeleteSEC_Dept(id);
        }

        public List<SEC_DeptDTO> GetSEC_Depts()
        {
            var domainSEC_Depts = SEC_DeptService.GetSEC_Depts();

            return SEC_DeptMappers.ChangeSEC_DeptToDTOs(domainSEC_Depts);
        }

		public SEC_DeptDTO GetSEC_DeptByKey(object id)
        {
            var domainSEC_Dept = SEC_DeptService.GetSEC_DeptByKey(id);

            return SEC_DeptMappers.ChangeSEC_DeptToDTO(domainSEC_Dept);
        }
	}
}
