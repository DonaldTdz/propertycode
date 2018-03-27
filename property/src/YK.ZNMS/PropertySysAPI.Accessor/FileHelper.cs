using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PropertySysAPI.Accessor
{
    /// <summary>
    /// 文件操作,创建静态页面，创建文件，删除文件等
    /// </summary>
    public class FileHelper
    {
        #region 创建目录
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path">文件夹的虚拟路径</param>
        public static void CreateDir(string path)
        {
            try
            {
                path = HttpContext.Current.Server.MapPath(path);
                CreateDirectory(path);
                //if (!Directory.Exists(HttpContext.Current.Server.MapPath(path)))
                //{
                //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 创建目录（返回服务器文件夹路径）
        /// </summary>
        /// <param name="path">文件夹的虚拟路径</param>
        /// <param name="serverPath">返回服务器物理路径</param>
        public static void CreateDir(string path, out string serverPath)
        {
            try
            {
                serverPath = HttpContext.Current.Server.MapPath(path);
                CreateDirectory(serverPath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 递归创建文件夹,私有方法，未公开
        /// <summary>  
        /// 递归创建文件夹  
        /// </summary>  
        /// <param name="path">文件夹的绝对路径</param>  
        private static bool CreateDirectory(string path)
        {
            string sParentDirectory = Path.GetDirectoryName(path);
            try
            {
                if (!Directory.Exists(sParentDirectory))
                    CreateDirectory(sParentDirectory);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 递归删除文件夹
        /// <summary>
        /// 删除文件夹，不管有没有子目录
        /// </summary>
        /// <param name="dir">文件夹的虚拟路径</param>
        public static void DeleteFolder(string dir)
        {
            if (Directory.Exists(HttpContext.Current.Server.MapPath(dir))) //如果存在这个文件夹删除之    
            {
                Directory.Delete(HttpContext.Current.Server.MapPath(dir), true); //删除已空文件夹                    
            }
        }
        #endregion

        #region 删除文件
        /// <summary>
        /// 删除服务器上的文件
        /// </summary>
        /// <param name="filepath">文件的路径,如: /file/test.html</param>
        public static void DeleteFile(string filepath)
        {
            try
            {
                if (File.Exists(HttpContext.Current.Server.MapPath(filepath)))
                {
                    File.Delete(HttpContext.Current.Server.MapPath(filepath));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 删除服务器硬盘上的文件
        /// </summary>
        /// <param name="filepath">文件的路径,如: D:\file\test.html</param>
        public static void DeleteDiskFile(string filepath)
        {
            try
            {
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 写文件操作
        /// <summary>
        /// 写文件操作
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileContent">文件类容</param>
        /// <returns></returns>
        public static bool WriteFileContent(string filePath, string fileContent)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    StreamWriter Fso = new StreamWriter(filePath, false, Encoding.UTF8);
                    Fso.WriteLine(fileContent);
                    Fso.Close();
                    Fso.Dispose();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 下载文件
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="path">文件路径</param>
        public static void DownFile(string fileName, string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            if (HttpContext.Current.Request.Browser.Browser == "IE")
            {
                fileName = HttpUtility.UrlPathEncode(fileName);
            }
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            HttpContext.Current.Response.BinaryWrite(bytes);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        #endregion

        #region 验证上传文件是不是指定格式的文件
        /// <summary>
        /// 验证上传文件是不是指定格式的文件
        /// </summary>
        /// <param name="fu"></param>
        /// <param name="fileEx"></param>
        /// <returns></returns>
        public static bool IsAllowedExtension(HttpPostedFile fu, FileExtension[] fileEx)
        {
            int fileLen = fu.ContentLength;
            byte[] imgArray = new byte[fileLen];
            fu.InputStream.Read(imgArray, 0, fileLen);
            using (MemoryStream ms = new MemoryStream(imgArray))
            {
                System.IO.BinaryReader br = new System.IO.BinaryReader(ms);
                string fileclass = string.Empty;
                byte buffer;
                try
                {
                    buffer = br.ReadByte();
                    fileclass = buffer.ToString();
                    buffer = br.ReadByte();
                    fileclass += buffer.ToString();
                }
                catch
                {
                }
                br.Close();
                ms.Close();
                foreach (FileExtension fe in fileEx)
                {
                    if (Int32.Parse(fileclass) == (int)fe)
                        return true;
                }
            }
            return false;
        }
        #endregion

      

    }

    /// <summary>
    /// 文件类型
    /// </summary>
    public enum FileExtension
    {
        JPG = 255216,
        GIF = 7173,
        PNG = 13780,
        SWF = 6787,
        RAR = 8297,
        ZIP = 8075,
        _7Z = 55122
        // 255216 jpg;  
        // 7173 gif;  
        // 6677 bmp,  
        // 13780 png;  
        // 6787 swf  
        // 7790 exe dll,  
        // 8297 rar  
        // 8075 zip  
        // 55122 7z  
        // 6063 xml  
        // 6033 html  
        // 239187 aspx  
        // 117115 cs  
        // 119105 js  
        // 102100 txt  
        // 255254 sql   
    }


}