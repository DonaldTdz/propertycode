//方法一
//须添加对System.Web的引用
//using System.Web.Security;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

public class EncryptData
{
    //方法四(对称加密)：
    //using System.IO;
    //using System.Security.Cryptography;
    private SymmetricAlgorithm mobjCryptoService;
    private string Key;
    /// <summary>   
    /// 对称加密类的构造函数   
    /// </summary>   
    public EncryptData()
    {
        mobjCryptoService = new RijndaelManaged();
        Key = "Guz(%&hj7x89H$yuBI0456FtmaT5&fvHUFCy76*h%(HilJ$lhj!y6&(*jkP87jH7";
    }
    /// <summary>
    /// SHA1加密字符串
    /// </summary>
    /// <param name="source">源字符串</param>
    /// <returns>加密后的字符串</returns>
    public string SHA1(string source)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(source, "SHA1");
    }
    /// <summary>
    /// MD5加密字符串
    /// </summary>
    /// <param name="source">源字符串</param>
    /// <returns>加密后的字符串</returns>
    public string MD5(string source)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(source, "MD5"); ;
    }


   
  


    //方法三(MD5不可逆)：
    //using System.Security.Cryptography;
    //MD5不可逆加密
    //32位加密
    public string GetMD5_32(string s, string _input_charset)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(s));
        StringBuilder sb = new StringBuilder(32);
        for (int i = 0; i < t.Length; i++)
        {
            sb.Append(t[i].ToString("x").PadLeft(2, '0'));
        }
        return sb.ToString();
    }
    //16位加密
    public static string GetMd5_16(string ConvertString)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
        t2 = t2.Replace("-", "");
        return t2;
    }



    /// <summary>   
    /// 获得密钥   
    /// </summary>   
    /// <returns>密钥</returns>   
    private byte[] GetLegalKey()
    {
        string sTemp = Key;
        mobjCryptoService.GenerateKey();
        byte[] bytTemp = mobjCryptoService.Key;
        int KeyLength = bytTemp.Length;
        if (sTemp.Length > KeyLength)
            sTemp = sTemp.Substring(0, KeyLength);
        else if (sTemp.Length < KeyLength)
            sTemp = sTemp.PadRight(KeyLength, ' ');
        return ASCIIEncoding.ASCII.GetBytes(sTemp);
    }
    /// <summary>   
    /// 获得初始向量IV   
    /// </summary>   
    /// <returns>初试向量IV</returns>   
    private byte[] GetLegalIV()
    {
        string sTemp = "E4ghj*Ghg7!rNIfb&95GUY86GfghUb#er57HBh(u%g6HJ($jhWk7&!hg4ui%$hjk";
        mobjCryptoService.GenerateIV();
        byte[] bytTemp = mobjCryptoService.IV;
        int IVLength = bytTemp.Length;
        if (sTemp.Length > IVLength)
            sTemp = sTemp.Substring(0, IVLength);
        else if (sTemp.Length < IVLength)
            sTemp = sTemp.PadRight(IVLength, ' ');
        return ASCIIEncoding.ASCII.GetBytes(sTemp);
    }
    /// <summary>   
    /// 加密方法   
    /// </summary>   
    /// <param name="Source">待加密的串</param>   
    /// <returns>经过加密的串</returns>   
    public string Encrypto(string Source)
    {
        byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
        MemoryStream ms = new MemoryStream();
        mobjCryptoService.Key = GetLegalKey();
        mobjCryptoService.IV = GetLegalIV();
        ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
        CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
        cs.Write(bytIn, 0, bytIn.Length);
        cs.FlushFinalBlock();
        ms.Close();
        byte[] bytOut = ms.ToArray();
        return Convert.ToBase64String(bytOut);
    }
    /// <summary>   
    /// 解密方法   
    /// </summary>   
    /// <param name="Source">待解密的串</param>   
    /// <returns>经过解密的串</returns>   
    public string Decrypto(string Source)
    {
        byte[] bytIn = Convert.FromBase64String(Source);
        MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
        mobjCryptoService.Key = GetLegalKey();
        mobjCryptoService.IV = GetLegalIV();
        ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
        CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
        StreamReader sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }

    //方法五：
    //using System.IO;
    //using System.Security.Cryptography;
    //using System.Text;
    //默认密钥向量
    private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
    /**//**//**//// <summary>
                /// DES加密字符串 keleyi.com
                /// </summary>
                /// <param name="encryptString">待加密的字符串</param>
                /// <param name="encryptKey">加密密钥,要求为8位</param>
                /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
    public static string EncryptDES(string encryptString, string encryptKey)
    {
        try
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }
        catch
        {
            return encryptString;
        }
    }
    /**//**//**//// <summary>
                /// DES解密字符串 keleyi.com
                /// </summary>
                /// <param name="decryptString">待解密的字符串</param>
                /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
                /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
    public static string DecryptDES(string decryptString, string decryptKey)
    {
        try
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
        catch
        {
            return decryptString;
        }
    }


    //方法六(文件加密)：
    //using System.IO;
    //using System.Security.Cryptography;
    //using System.Text;
    //加密文件
    private static void EncryptData_(String inName, String outName, byte[] desKey, byte[] desIV)
    {
        //Create the file streams to handle the input and output files.
        FileStream fin = new FileStream(inName, FileMode.Open, FileAccess.Read);
        FileStream fout = new FileStream(outName, FileMode.OpenOrCreate, FileAccess.Write);
        fout.SetLength(0);
        //Create variables to help with read and write.
        byte[] bin = new byte[100]; //This is intermediate storage for the encryption.
        long rdlen = 0;              //This is the total number of bytes written.
        long totlen = fin.Length;    //This is the total length of the input file.
        int len;                     //This is the number of bytes to be written at a time.
        DES des = new DESCryptoServiceProvider();
        CryptoStream encStream = new CryptoStream(fout, des.CreateEncryptor(desKey, desIV), CryptoStreamMode.Write);
        //Read from the input file, then encrypt and write to the output file.
        while (rdlen < totlen)
        {
            len = fin.Read(bin, 0, 100);
            encStream.Write(bin, 0, len);
            rdlen = rdlen + len;
        }
        encStream.Close();
        fout.Close();
        fin.Close();
    }
}