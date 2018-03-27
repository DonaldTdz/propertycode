using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class SubjectBindLogController : BaseController
    {
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="commuDeptId"></param>
        /// <returns></returns>
        public ActionResult Index(int commuDeptId)
        {

            SubjectHouseRefAppService service = new SubjectHouseRefAppService();
            SubjectBindLogList SubjectLogData = new SubjectBindLogList();
            SubjectLogData.Language = Language;
            SubjectLogData.TemplateModels = service.GetSubjectBindLogTemplate();
            SubjectLogData.Subjects = new ChargeSubjectAppService().GetChargeSubjectList(commuDeptId);
            return View(SubjectLogData);
        }
        /// <summary>
        /// 绑定科目日志查询
        /// </summary>
        /// <param name="queryParams">查询参数</param>
        /// <returns>返回查询结果</returns>
        [HttpPost]
        public ActionResult GetSubjectRefs(SubjectBindLogSearchDTO search)
        {

            int outCount = 0;
    
            SubjectHouseRefAppService service = new SubjectHouseRefAppService();
            try
            {
                var dataList = service.GetSubjectHouseRefDTOList(search, out outCount);
                SearchResultData<SubjectBindLogDTO> queryResult = new SearchResultData<SubjectBindLogDTO>()
                {
                    draw = search.Draw,
                    recordsFiltered = outCount,
                    recordsTotal = outCount,
                    data = dataList
                };
                return Json(queryResult);
            }
            catch (Exception ex)
            {

                throw;
            }
        
        
        }
    }
    public class SubjectBindLogList
    {
        public string Language { get; set; }
        public List<ChargeSubjectDTO> Subjects { get; set; }
        public IEnumerable<TemplateModel> TemplateModels { get; set; }
    }
}