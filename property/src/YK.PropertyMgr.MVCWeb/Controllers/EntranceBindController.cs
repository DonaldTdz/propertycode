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
    public class EntranceBindController : BaseController
    {
        public ActionResult Index()
        {
            EntranceAppService entranceAppService = new EntranceAppService();
            EntranceListData entranceListDataListData = new EntranceListData();
            entranceListDataListData.Language = Language;
            var templateModels = GetTemplateModels();
            entranceListDataListData.TemplateModels = templateModels;

            entranceListDataListData.TemplateModels = entranceAppService.GetEntranceViewTemplate();
            return View(entranceListDataListData);
        }
        #region 查询信息处理
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="queryParams">查询参数</param>
        /// <returns>返回查询结果</returns>
        [HttpPost]
        public ActionResult GetEntrances(EntranceSearchDTO search)
        {
            try
            {
                int outCount = 0;
                EntranceAppService entranceAppService = new EntranceAppService();
                IList<EntranceViewDTO> dataList = entranceAppService.GetEntranceDTOList(search, out outCount);
                SearchResultData<EntranceViewDTO> queryResult = new SearchResultData<EntranceViewDTO>()
                {
                    draw = search.Draw,
                    recordsFiltered = outCount,
                    recordsTotal = outCount,
                    data = dataList
                };
                return Json(queryResult);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        private IEnumerable<TemplateModel> GetTemplateModels()
        {
            return PresentationServiceHelper.LookUp<ITemplateService>().GetTemplateModels("PropertyMgrTemplate.xml", "Entrance", true);
        }
        #endregion
    }
}