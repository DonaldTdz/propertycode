using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.CompositeDomainService;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class HouseChargeSubjectController : BaseController
    {
        //
        // GET: /HouseChargeSubject/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HouseChargeSubjectList(int? DeptId,int? DeptType)
        {
            DeptId = DeptId ?? 0;
            DeptType = DeptType ?? 0;
            HouseChargeSubjectData houseChargeSubjectData = new HouseChargeSubjectData();
            ChargeSubjectAppService service = new ChargeSubjectAppService();
            houseChargeSubjectData.Language = this.Language;

            var ChargeBillInformationDTO = BillCommonService.Instance.GetChargeBillInformationDTOByResourceId(DeptId.Value, DeptType.Value);
            houseChargeSubjectData.chargeBillInformationDTO = ChargeBillInformationDTO;
            houseChargeSubjectData.ChargeSubjectList = service.GetChargeSubjectListByHouseDeptId(DeptId.Value, DeptType.Value);
            var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
            houseChargeSubjectData.DictionaryModels = propertyService.GetDictionaryModels(PropertyEnumType.BillPeriod.ToString());
            return View(houseChargeSubjectData);
        }

        public ActionResult GetChargeSubjectList(int? DeptId,int? DeptType)
        {
            DeptId = DeptId ?? 0;
            DeptType = DeptType ?? 0;
            ChargeSubjectAppService service = new ChargeSubjectAppService();
            HouseChargeSubjectData houseChargeSubjectData = new HouseChargeSubjectData();
            var ChargeBillInformationDTO = BillCommonService.Instance.GetChargeBillInformationDTOByResourceId(DeptId.Value, DeptType.Value);
            houseChargeSubjectData.ChargeSubjectList = service.GetChargeSubjectListByHouseDeptId(DeptId.Value, DeptType.Value);
            houseChargeSubjectData.chargeBillInformationDTO = ChargeBillInformationDTO;
            return Json(houseChargeSubjectData, JsonRequestBehavior.AllowGet);
        }
	}

    public class HouseChargeSubjectData 
    {
        public string Language { get; set; }

        public IList<ChargeSubjectDTO> ChargeSubjectList { get; set; }

        public IList<DictionaryModel> DictionaryModels { get; set; }

        public ChargeBillInformationDTO chargeBillInformationDTO { get; set; }
    }
}