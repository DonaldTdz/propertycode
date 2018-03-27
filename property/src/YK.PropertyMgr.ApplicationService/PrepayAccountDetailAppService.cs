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
	public partial class PrepayAccountDetailAppService
	{
		private PrepayAccountDetailDomainService _PrepayAccountDetailDomainService;
        protected PrepayAccountDetailDomainService PrepayAccountDetailService
        {
            get
            {
                if (_PrepayAccountDetailDomainService == null)
                {
                    _PrepayAccountDetailDomainService = new PrepayAccountDetailDomainService();
                }

                return _PrepayAccountDetailDomainService;
            }
        }   

        public bool InsertPrepayAccountDetail(PrepayAccountDetailDTO dtoPrepayAccountDetail)
        {
            var domainPrepayAccountDetail = PrepayAccountDetailMappers.ChangeDTOToPrepayAccountDetailNew(dtoPrepayAccountDetail);

            return PrepayAccountDetailService.InsertPrepayAccountDetail(domainPrepayAccountDetail);
        }

        public bool UpdatePrepayAccountDetail(PrepayAccountDetailDTO dtoPrepayAccountDetail)
        {
            var domainPrepayAccountDetail = PrepayAccountDetailMappers.ChangeDTOToPrepayAccountDetailNew(dtoPrepayAccountDetail);

            return PrepayAccountDetailService.UpdatePrepayAccountDetail(domainPrepayAccountDetail);
        }

        public bool DeletePrepayAccountDetail(object id)
        {
            return PrepayAccountDetailService.DeletePrepayAccountDetail(id);
        }

        public List<PrepayAccountDetailDTO> GetPrepayAccountDetails()
        {
            var domainPrepayAccountDetails = PrepayAccountDetailService.GetPrepayAccountDetails();

            return PrepayAccountDetailMappers.ChangePrepayAccountDetailToDTOs(domainPrepayAccountDetails);
        }

		public PrepayAccountDetailDTO GetPrepayAccountDetailByKey(object id)
        {
            var domainPrepayAccountDetail = PrepayAccountDetailService.GetPrepayAccountDetailByKey(id);

            return PrepayAccountDetailMappers.ChangePrepayAccountDetailToDTO(domainPrepayAccountDetail);
        }
	}
}
