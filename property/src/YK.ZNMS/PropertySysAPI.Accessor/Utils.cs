using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PropertySysAPI.Accessor
{

    public sealed class Utils
    {
        /// <summary>
        /// 异常日志记录
        /// </summary>
        /// <param name="Server"></param>
        /// <param name="ex"></param>
        public static void ErrorLog(HttpServerUtility Server, string ex)
        {
            //构造日志记录目录(按年份和月份记录)
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            //创建目录
            string directory = string.Empty;
            FileHelper.CreateDir(string.Format("/ErrorLog/{0}/", year), out directory);
            //创建和写入文件
            string filePath = string.Format(directory + "{0}-{1}.txt", year, month);
            File.AppendAllText(filePath, ex);
        }
        public static void ErrorLog(string ex)
        {
            //构造日志记录目录(按年份和月份记录)
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            //创建目录
            string directory = string.Empty;
            FileHelper.CreateDir(string.Format("/ErrorLog/{0}/", year), out directory);
            //创建和写入文件
            string filePath = string.Format(directory + "{0}-{1}.txt", year, month);
            File.AppendAllText(filePath, ex);
        }


    
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>

        public static string EncryptDES(string pToEncrypt, string sKey)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }


        public static string DecryptDES(string pToDecrypt, string sKey)
        {
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }

    }
}