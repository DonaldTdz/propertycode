using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using YK.BackgroundMgr.ApplicationDTO;
using System.Text;

namespace YK.BackgroundMgr.PresentationService
{
    public class BaseController : Controller
    {
        protected const string AdminUserInfoKey = "AdminUserInfo";

        // 获取语言
        protected string Language
        {
            get
            {
                return CultureInfo.CurrentCulture.Name;
            }
        }

        protected SEC_AdminUserDTO CurrentAdminUser
        {
            get
            {
                var sessionService = PresentationServiceHelper.LookUp<ISessionService>();
                return sessionService.GetSession<SEC_AdminUserDTO>(AdminUserInfoKey);
            }
        }

        public FileStreamResult DownloadFile(string fileName)
        {
            string absoluFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileDownload", fileName);
            return File(new FileStream(absoluFilePath, FileMode.Open), "application/octet-stream", Server.UrlEncode(fileName));
        }

        protected void ExportExcel(string fileName, byte[] exportContent)
        {

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.BinaryWrite(exportContent);
            //Response.End();
        }

        protected void ExportExcel_NewFirefox(string fileName, byte[] exportContent)
        {

            Encoding encoding;
            string outputFileName = null;
            string browser = Request.UserAgent.ToUpper();
            if (browser.Contains("MS") == true && browser.Contains("IE") == true)
            {
                outputFileName = HttpUtility.UrlEncode(fileName);
                encoding = System.Text.Encoding.Default;
            }
            else if (browser.Contains("FIREFOX") == true)
            {
                outputFileName = fileName;
                encoding = System.Text.Encoding.GetEncoding("GB2312");
            }
            else
            {
                outputFileName = HttpUtility.UrlEncode(fileName);
                encoding = System.Text.Encoding.Default;
            }
            Response.Clear();
            Response.Buffer = true;
            Response.ContentEncoding = encoding;
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", string.IsNullOrEmpty(outputFileName) ? DateTime.Now.ToString("yyyyMMddHHmmssfff") : outputFileName));
            Response.BinaryWrite(exportContent.ToArray());

            //Response.End();
        }



    }
}