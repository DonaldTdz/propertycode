using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.PresentationService;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class ChargeBillListData
    {
        public string Language { get; set; }

        public DailyChargBillDTO PageModel { get; set; }

        public List<DictionaryModel> PayTypeList { get; set; }

        public IList<ChargBillDTO> ChargeBillList { get; set; }

        public ChargBillSearchDTO ChargBillSearch { get; set; }

        public bool IsChargeSubject { get; set; }

        public List<ChargeSubjectDTO> ChargeSubjectList { get; set; }

        public IEnumerable<TemplateModel> TemplateModels { get; set; }

        public CommunityConfigDTO ComConfig { get; set; }

        public IList<DailyChargPrepayAccountDTO> PreAccountList { get; set; }

        /// <summary>
        /// 是否 是按车位收费页面
        /// </summary>
        public bool IsCarPark { get; set; }
    }

    public class DailyChargPrepayAccountDTO
    {
        public DailyChargPrepayAccountDTO()
        {
            PreAmount = 0;
            DeductionAmount = 0;
            ActualDeductionAmount = 0;
        }

        /// <summary>
        /// 收费项目Id
        /// </summary>
        public int? SubjectId { get; set; }

        /// <summary>
        /// 预存费
        /// </summary>
        public decimal? PreAmount { get; set; }

        /// <summary>
        /// 需要抵扣的金额
        /// </summary>
        public decimal? DeductionAmount { get; set; }

        /// <summary>
        /// 实际可抵扣
        /// </summary>
        public decimal? ActualDeductionAmount { get; set; }
    }
}
