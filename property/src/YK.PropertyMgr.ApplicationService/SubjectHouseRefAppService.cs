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
	public partial class SubjectHouseRefAppService
	{
		private SubjectHouseRefDomainService _SubjectHouseRefDomainService;
        protected SubjectHouseRefDomainService SubjectHouseRefService
        {
            get
            {
                if (_SubjectHouseRefDomainService == null)
                {
                    _SubjectHouseRefDomainService = new SubjectHouseRefDomainService();
                }

                return _SubjectHouseRefDomainService;
            }
        }   

        public bool InsertSubjectHouseRef(SubjectHouseRefDTO dtoSubjectHouseRef)
        {
            var domainSubjectHouseRef = SubjectHouseRefMappers.ChangeDTOToSubjectHouseRefNew(dtoSubjectHouseRef);

            return SubjectHouseRefService.InsertSubjectHouseRef(domainSubjectHouseRef);
        }

        public bool UpdateSubjectHouseRef(SubjectHouseRefDTO dtoSubjectHouseRef)
        {
            var domainSubjectHouseRef = SubjectHouseRefMappers.ChangeDTOToSubjectHouseRefNew(dtoSubjectHouseRef);

            return SubjectHouseRefService.UpdateSubjectHouseRef(domainSubjectHouseRef);
        }

        public bool DeleteSubjectHouseRef(object id)
        {
            return SubjectHouseRefService.DeleteSubjectHouseRef(id);
        }

        public List<SubjectHouseRefDTO> GetSubjectHouseRefs()
        {
            var domainSubjectHouseRefs = SubjectHouseRefService.GetSubjectHouseRefs();

            return SubjectHouseRefMappers.ChangeSubjectHouseRefToDTOs(domainSubjectHouseRefs);
        }

		public SubjectHouseRefDTO GetSubjectHouseRefByKey(object id)
        {
            var domainSubjectHouseRef = SubjectHouseRefService.GetSubjectHouseRefByKey(id);

            return SubjectHouseRefMappers.ChangeSubjectHouseRefToDTO(domainSubjectHouseRef);
        }
	}
}
