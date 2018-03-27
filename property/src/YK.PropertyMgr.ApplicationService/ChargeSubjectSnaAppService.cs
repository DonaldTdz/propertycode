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
	public partial class ChargeSubjectSnaAppService
	{
		private ChargeSubjectSnaDomainService _ChargeSubjectSnaDomainService;
        protected ChargeSubjectSnaDomainService ChargeSubjectSnaService
        {
            get
            {
                if (_ChargeSubjectSnaDomainService == null)
                {
                    _ChargeSubjectSnaDomainService = new ChargeSubjectSnaDomainService();
                }

                return _ChargeSubjectSnaDomainService;
            }
        }   

        public bool InsertChargeSubjectSna(ChargeSubjectSnaDTO dtoChargeSubjectSna)
        {
            var domainChargeSubjectSna = ChargeSubjectSnaMappers.ChangeDTOToChargeSubjectSnaNew(dtoChargeSubjectSna);

            return ChargeSubjectSnaService.InsertChargeSubjectSna(domainChargeSubjectSna);
        }

        public bool UpdateChargeSubjectSna(ChargeSubjectSnaDTO dtoChargeSubjectSna)
        {
            var domainChargeSubjectSna = ChargeSubjectSnaMappers.ChangeDTOToChargeSubjectSnaNew(dtoChargeSubjectSna);

            return ChargeSubjectSnaService.UpdateChargeSubjectSna(domainChargeSubjectSna);
        }

        public bool DeleteChargeSubjectSna(object id)
        {
            return ChargeSubjectSnaService.DeleteChargeSubjectSna(id);
        }

        public List<ChargeSubjectSnaDTO> GetChargeSubjectSnas()
        {
            var domainChargeSubjectSnas = ChargeSubjectSnaService.GetChargeSubjectSnas();

            return ChargeSubjectSnaMappers.ChangeChargeSubjectSnaToDTOs(domainChargeSubjectSnas);
        }

		public ChargeSubjectSnaDTO GetChargeSubjectSnaByKey(object id)
        {
            var domainChargeSubjectSna = ChargeSubjectSnaService.GetChargeSubjectSnaByKey(id);

            return ChargeSubjectSnaMappers.ChangeChargeSubjectSnaToDTO(domainChargeSubjectSna);
        }
	}
}
