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
	public partial class ChargeSubjectAppService
	{
		private ChargeSubjectDomainService _ChargeSubjectDomainService;
        protected ChargeSubjectDomainService ChargeSubjectService
        {
            get
            {
                if (_ChargeSubjectDomainService == null)
                {
                    _ChargeSubjectDomainService = new ChargeSubjectDomainService();
                }

                return _ChargeSubjectDomainService;
            }
        }   

        public bool InsertChargeSubject(ChargeSubjectDTO dtoChargeSubject)
        {
            var domainChargeSubject = ChargeSubjectMappers.ChangeDTOToChargeSubjectNew(dtoChargeSubject);

            return ChargeSubjectService.InsertChargeSubject(domainChargeSubject);
        }

        public bool UpdateChargeSubject(ChargeSubjectDTO dtoChargeSubject)
        {
            var domainChargeSubject = ChargeSubjectMappers.ChangeDTOToChargeSubjectNew(dtoChargeSubject);

            return ChargeSubjectService.UpdateChargeSubject(domainChargeSubject);
        }

        public bool DeleteChargeSubject(object id)
        {
            return ChargeSubjectService.DeleteChargeSubject(id);
        }

        public List<ChargeSubjectDTO> GetChargeSubjects()
        {
            var domainChargeSubjects = ChargeSubjectService.GetChargeSubjects();

            return ChargeSubjectMappers.ChangeChargeSubjectToDTOs(domainChargeSubjects);
        }

		public ChargeSubjectDTO GetChargeSubjectByKey(object id)
        {
            var domainChargeSubject = ChargeSubjectService.GetChargeSubjectByKey(id);

            return ChargeSubjectMappers.ChangeChargeSubjectToDTO(domainChargeSubject);
        }
	}
}
