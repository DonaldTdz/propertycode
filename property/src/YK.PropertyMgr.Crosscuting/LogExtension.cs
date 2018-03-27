using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using YK.BackgroundMgr.PresentationService;
using YK.FrameworkLog.LogEntity;
using YK.FrameworkLog.PluginHelper;

namespace YK.PropertyMgr.Crosscuting
{
    public static class LogProperty
    {
        public static bool WriteLoginToFile(string msg, string modelName, FileLogType logType) 
        {
            bool result;
            try
            {
                //保存路径 默认更目录log
                //string serverPath = HttpContext.Current.Server.MapPath("~");
                string logPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"].ToString();
                if (string.IsNullOrEmpty(logPath))
                {
                    logPath = @"D:\Property";
                }
                logPath = logPath + @"\Logs\" + logType.ToString() + @"\" + modelName;
                if (!Directory.Exists(logPath)) 
                {
                    Directory.CreateDirectory(logPath);
                }
                string logFile = logPath + @"\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                string content = "\r\n*********************************************************\r\n记录时间:" 
                    + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") 
                    + "\r\n记录明细:" + msg
                    + "\r\n*********************************************************\r\n";
                File.AppendAllText(logFile, content, Encoding.UTF8);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 操作日志方法
        /// </summary>
        /// <param name="UserId">操作人Id</param>
        /// <param name="UserName">操作人时间</param>
        /// <param name="ParentType">页面/controller</param>
        /// <param name="DetailType">操作/Action</param>
        /// <param name="OperateName">OperateName </param>
        /// <param name="Extension">内容</param>
        public static  void WirteFrameworkLog(string UserId,string UserName,string ParentType,string DetailType,string OperateName,string Extension,string Extension2 ="", string Extension3 = "")
        {
            OperateLog operatelog = new OperateLog()
            {
                EntityKey = Guid.NewGuid().ToString(),
                CreateTime = DateTime.Now,
                LogFrom = ELogFrom.物业后台,
                UserId = UserId,
                UserName = UserName,
                ParentType = ParentType,
                DetailType = DetailType,
                OperateName = OperateName,
                Extension1 = Extension,
                Extension2 = Extension2,
                Extension3 = Extension3,
                IP = IPHelper.GetClientIp()
            };
            operatelog.EntityKey = Guid.NewGuid().ToString();
            try
            {
                PluginHelper.WriteOperateToCache(operatelog);
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("写日志 WriteOperateToCache异常：{0}", ex.Message), "WirteFrameworkLog", FileLogType.Exception);
            }     
        }





    }

    public enum FileLogType 
    {
        Info = 1,
        Exception = 2
    }
}
