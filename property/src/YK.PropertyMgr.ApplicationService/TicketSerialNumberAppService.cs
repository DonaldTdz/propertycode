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
	public partial class TicketSerialNumberAppService
	{
		private TicketSerialNumberDomainService _TicketSerialNumberDomainService;
        protected TicketSerialNumberDomainService TicketSerialNumberService
        {
            get
            {
                if (_TicketSerialNumberDomainService == null)
                {
                    _TicketSerialNumberDomainService = new TicketSerialNumberDomainService();
                }

                return _TicketSerialNumberDomainService;
            }
        }   

        public bool InsertTicketSerialNumber(TicketSerialNumberDTO dtoTicketSerialNumber)
        {
            var domainTicketSerialNumber = TicketSerialNumberMappers.ChangeDTOToTicketSerialNumberNew(dtoTicketSerialNumber);

            return TicketSerialNumberService.InsertTicketSerialNumber(domainTicketSerialNumber);
        }

        public bool UpdateTicketSerialNumber(TicketSerialNumberDTO dtoTicketSerialNumber)
        {
            var domainTicketSerialNumber = TicketSerialNumberMappers.ChangeDTOToTicketSerialNumberNew(dtoTicketSerialNumber);

            return TicketSerialNumberService.UpdateTicketSerialNumber(domainTicketSerialNumber);
        }

        public bool DeleteTicketSerialNumber(object id)
        {
            return TicketSerialNumberService.DeleteTicketSerialNumber(id);
        }

        public List<TicketSerialNumberDTO> GetTicketSerialNumbers()
        {
            var domainTicketSerialNumbers = TicketSerialNumberService.GetTicketSerialNumbers();

            return TicketSerialNumberMappers.ChangeTicketSerialNumberToDTOs(domainTicketSerialNumbers);
        }

		public TicketSerialNumberDTO GetTicketSerialNumberByKey(object id)
        {
            var domainTicketSerialNumber = TicketSerialNumberService.GetTicketSerialNumberByKey(id);

            return TicketSerialNumberMappers.ChangeTicketSerialNumberToDTO(domainTicketSerialNumber);
        }
	}
}
