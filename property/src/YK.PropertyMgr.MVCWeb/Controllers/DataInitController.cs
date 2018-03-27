using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.Crosscuting;
using YK.BackgroundMgr.PresentationService;
using YK.FrameworkTools.ExcelService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationService.Service;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class DataInitController : BaseController
    {
        // GET: DataInit
        public ActionResult Index()
        {
            return View();
        }

        #region 历史账单导入

        public ActionResult ImportArrearageIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportArrearage()
        {
            string filePath = "";
            var uploadFile = System.Web.HttpContext.Current.Request.Files["arrearageFileToUpload"];
            var comDeptId = System.Web.HttpContext.Current.Request.Form["comDeptId"];
            if (uploadFile == null || uploadFile.ContentLength == 0)
            {
                PresentationServiceHelper.LookUp<ICookieService>().SetCookie("ImportResultMsg", HttpUtility.UrlEncode("请选择文件", System.Text.Encoding.UTF8), DateTime.Now.AddHours(5));
                return View("UploadFileResult");
            }
            if (comDeptId == null || comDeptId.Length == 0)
            {
                PresentationServiceHelper.LookUp<ICookieService>().SetCookie("ImportResultMsg", HttpUtility.UrlEncode("请选择小区", System.Text.Encoding.UTF8), DateTime.Now.AddHours(5));
                return View("UploadFileResult");
            }
            
            try
            {
                var fileName = Path.GetFileName(uploadFile.FileName);
                //获取文件扩展名
                string aLastName = fileName.Substring(fileName.LastIndexOf(".") + 1);
                if (aLastName != "xlsx" && aLastName != "xls")
                {
                    PresentationServiceHelper.LookUp<ICookieService>().SetCookie("ImportResultMsg", HttpUtility.UrlEncode("文件格式不对，正确的文件后缀为.xlsx，请下载模板重试", System.Text.Encoding.UTF8), DateTime.Now.AddHours(5));
                    return View("UploadFileResult");
                }
                var serverName = string.Format("ImportArrearage_{0}{1}", Guid.NewGuid(), fileName);
                var root = System.Web.HttpContext.Current.Server.MapPath("/");
                string fileDir = Path.Combine(root, "UploadFile");
                if (!Directory.Exists(fileDir))
                {
                    Directory.CreateDirectory(fileDir);
                }
                DataInitAppService service = new DataInitAppService();
                IEnumerable<TemplateModel> templateModels = service.GetArrearageListTemplate();

                filePath = Path.Combine(fileDir, serverName);
                uploadFile.SaveAs(filePath);
                ResultModel result = service.ImportArrearage(filePath, templateModels,int.Parse(comDeptId), (int)CurrentAdminUser.Id, CurrentAdminUser.RealName);
                if (result.IsSuccess)
                {
                    PresentationServiceHelper.LookUp<ICookieService>().SetCookie("ImportResultMsg", HttpUtility.UrlEncode("导入数据成功", System.Text.Encoding.UTF8), DateTime.Now.AddHours(5));
                }
                else
                {
                    PresentationServiceHelper.LookUp<ICookieService>().SetCookie("ImportResultMsg", HttpUtility.UrlEncode(result.Msg, System.Text.Encoding.UTF8), DateTime.Now.AddHours(5));
                    if (result.Data != null)
                    {
                        var importErrorResult = ExcelHelper.TableToWorkbook(templateModels, result.Data as DataTable);
                        ExportExcel_NewFirefox("导入数据错误结果.xls", importErrorResult.SaveToStream().ToArray());
                    }
                }

                System.IO.File.Delete(filePath);
                return View("UploadFileResult");

            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                ViewBag.UploadResult = ex.Message;
                PresentationServiceHelper.LookUp<ICookieService>().SetCookie("ImportResultMsg", HttpUtility.UrlEncode("模板不匹配，请下载模板重试", System.Text.Encoding.UTF8), DateTime.Now.AddHours(5));
                return View("UploadFileResult");
            }
        }

        #endregion

        #region 历史缴费导入 20170222

        public ActionResult ImportHistoryCostIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportHistoryCost()
        {
            string filePath = "";
            var uploadFile = System.Web.HttpContext.Current.Request.Files["historyCostFileToUpload"];
            var comDeptId = System.Web.HttpContext.Current.Request.Form["historyCostComDeptId"];
            if (uploadFile == null || uploadFile.ContentLength == 0)
            {
                PresentationServiceHelper.LookUp<ICookieService>().SetCookie("ImportResultMsg", HttpUtility.UrlEncode("请选择文件", System.Text.Encoding.UTF8), DateTime.Now.AddHours(5));
                return View("UploadFileResult");
            }
            if (comDeptId == null || comDeptId.Length == 0)
            {
                PresentationServiceHelper.LookUp<ICookieService>().SetCookie("ImportResultMsg", HttpUtility.UrlEncode("请选择小区", System.Text.Encoding.UTF8), DateTime.Now.AddHours(5));
                return View("UploadFileResult");
            }

            try
            {
                var fileName = Path.GetFileName(uploadFile.FileName);
                //获取文件扩展名
                string aLastName = fileName.Substring(fileName.LastIndexOf(".") + 1);
                if (aLastName != "xlsx" && aLastName != "xls")
                {
                    PresentationServiceHelper.LookUp<ICookieService>().SetCookie("ImportResultMsg", HttpUtility.UrlEncode("文件格式不对，正确的文件后缀为.xlsx，请下载模板重试", System.Text.Encoding.UTF8), DateTime.Now.AddHours(5));
                    return View("UploadFileResult");
                }
                var serverName = string.Format("ImportHistoryCost_{0}{1}", Guid.NewGuid(), fileName);
                var root = System.Web.HttpContext.Current.Server.MapPath("/");
                string fileDir = Path.Combine(root, "UploadFile");
                if (!Directory.Exists(fileDir))
                {
                    Directory.CreateDirectory(fileDir);
                }
                DataInitAppService service = new DataInitAppService();
                IEnumerable<TemplateModel> templateModels = service.GetHistoryCostListTemplate();

                filePath = Path.Combine(fileDir, serverName);
                uploadFile.SaveAs(filePath);
                ResultModel result = service.ImportHistoryCost(filePath, templateModels, int.Parse(comDeptId), (int)CurrentAdminUser.Id, CurrentAdminUser.RealName);
                if (result.IsSuccess)
                {
                    PresentationServiceHelper.LookUp<ICookieService>().SetCookie("ImportResultMsg", HttpUtility.UrlEncode("导入数据成功", System.Text.Encoding.UTF8), DateTime.Now.AddHours(5));
                }
                else
                {
                    PresentationServiceHelper.LookUp<ICookieService>().SetCookie("ImportResultMsg", HttpUtility.UrlEncode(result.Msg, System.Text.Encoding.UTF8), DateTime.Now.AddHours(5));
                    if (result.Data != null)
                    {
                        var importErrorResult = ExcelHelper.TableToWorkbook(templateModels, result.Data as DataTable);
                        ExportExcel_NewFirefox("导入数据错误结果.xls", importErrorResult.SaveToStream().ToArray());
                    }
                }

                System.IO.File.Delete(filePath);
                return View("UploadFileResult");

            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                ViewBag.UploadResult = ex.Message;
                PresentationServiceHelper.LookUp<ICookieService>().SetCookie("ImportResultMsg", HttpUtility.UrlEncode("模板不匹配，请下载模板重试", System.Text.Encoding.UTF8), DateTime.Now.AddHours(5));
                return View("UploadFileResult");
            }
        }

        #endregion
    }
}