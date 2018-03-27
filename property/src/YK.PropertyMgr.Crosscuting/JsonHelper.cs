using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

/// <summary>
/// JSON序列化和反序列化辅助类
/// </summary>
public class JsonHelper
{
    /// <summary>
    /// JSON序列化
    /// </summary>
    public static string JsonSerializer<T>(T t)
    {
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        MemoryStream ms = new MemoryStream();
        ser.WriteObject(ms, t);
        string jsonString = Encoding.UTF8.GetString(ms.ToArray());
        ms.Close();
        return jsonString;
    }

    /// <summary>
    /// JSON反序列化
    /// </summary>
    public static T JsonDeserialize<T>(string jsonString)
    {
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        T obj = (T)ser.ReadObject(ms);
        return obj;
    }

    /// <summary>
    /// 通过Newtonsoft JSON序列化
    /// </summary>
    public static string JsonSerializerByNewtonsoft<T>(T t,bool IsDateFormat=false)
    {
        if(IsDateFormat)
        {
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy'-'MM'-'dd HH':'mm':'ss" };
            return Newtonsoft.Json.JsonConvert.SerializeObject(t,Formatting.Indented, timeConverter);
        }
        else
          return Newtonsoft.Json.JsonConvert.SerializeObject(t);
    }

    public static string JsonSerializerByNewtonsoft(object obj)
    {
    

        return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
    }


    /// <summary>
    /// 通过Newtonsoft JSON反序列化
    /// </summary>
    public static T JsonDeserializeByNewtonsoft<T>(string jsonString)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
    }

    /// <summary>
    /// 通过Newtonsoft JSON反序列化
    /// </summary>
    public static object JsonDeserializeByNewtonsoft(string jsonString)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);
    }



    public static T DeserializeAnonymousTypeByNewtonsoft<T>(string jsonString,T t)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeAnonymousType<T>(jsonString,t);
    }

}