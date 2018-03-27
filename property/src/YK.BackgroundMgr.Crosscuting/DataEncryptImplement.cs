using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace YK.BackgroundMgr.Crosscuting
{
    /// <summary>
    /// 实现数据解密接口
    /// </summary>
    public class DataEncrypt
    {
        private static RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

        public byte[] Encrypt(string inputStr)
        {
            byte[] data = Encoding.UTF8.GetBytes(inputStr);

            if (data == null)
            {
                throw new ArgumentNullException("Data");
            }
            return rsa.Encrypt(data, false);
        }

        public bool Decrypt(byte[] identity)
        {
            if (identity == null)
            {
                return false;
            }

            byte[] inputDataDec = rsa.Decrypt(identity, false);
            string inputDec = Encoding.UTF8.GetString(inputDataDec, 0, inputDataDec.Length);
            return CheckUserLegal(inputDec);
        }

        public static string EncryptToDB(string datastr)
        {
            String keystr = "12345678";
            using (DESCryptoServiceProvider desc = new DESCryptoServiceProvider())
            {
                byte[] key = Encoding.ASCII.GetBytes(keystr);
                byte[] data = Encoding.Unicode.GetBytes(datastr);
                using (MemoryStream ms = new MemoryStream())
                {
                    CryptoStream cs = new CryptoStream(ms, desc.CreateEncryptor(key, key), CryptoStreamMode.Write);
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string DecryptFromDB(string datastr)
        {
            String keystr = "12345678";
            byte[] data = Convert.FromBase64String(datastr);
            using (DESCryptoServiceProvider desc = new DESCryptoServiceProvider())
            {
                byte[] key = Encoding.ASCII.GetBytes(keystr);
                using (MemoryStream ms = new MemoryStream())
                {
                    CryptoStream cs = new CryptoStream(ms, desc.CreateDecryptor(key, key), CryptoStreamMode.Write);
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return Encoding.Unicode.GetString(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// MD5
        /// </summary>
        public static string MD5(string str)
        {
            MD5 m = new MD5CryptoServiceProvider();
            byte[] s = m.ComputeHash(UnicodeEncoding.UTF8.GetBytes(str));
            return BitConverter.ToString(s).Replace("-", "").ToUpper();
        }


        private static bool CheckUserLegal(string decryptStr)
        {
            var chcekAuth = true;
            try
            {
                if (string.IsNullOrEmpty(decryptStr))
                {
                    return false;
                }
                var idenfity = decryptStr.Split('|')[0];
                var user = decryptStr.Split('|')[1];
            }
            catch (Exception ex)
            {
                chcekAuth = false;
                throw new Exception("Authentication DataEncrypt 'CheckUserLegal' failed ", ex);
            }

            return chcekAuth;
        }
    }
}
