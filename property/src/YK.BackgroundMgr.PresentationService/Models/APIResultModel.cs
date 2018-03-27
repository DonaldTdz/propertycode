using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.PresentationService
{
    /// <summary>
    /// API接口返回对象
    /// </summary>
    public abstract class APIBaseResultModel
    {
        /// <summary>
        /// 空构造函数
        /// </summary>
        public APIBaseResultModel() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="result">结果</param>
        /// <param name="msg">信息</param>
        public APIBaseResultModel(bool result, string msg)
        {
            IsSuccess = result;
            Msg = msg;
        }

        /// <summary>
        /// 方法调用结果
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Msg { get; set; }
    }

    /// <summary>
    /// API接口返回对象
    /// </summary>
    public class APIResultModel : APIBaseResultModel
    {
        /// <summary>
        /// 空构造函数
        /// </summary>
        public APIResultModel() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="result">结果</param>
        /// <param name="msg">信息</param>
        public APIResultModel(bool result, string msg)
            : base(result, msg)
        {
            Data = null;
        }

        public APIResultModel(bool result, string msg, object data)
            : base(result, msg)
        {
            Data = data;
        }

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }
    }

    /// <summary>
    /// API接口返回统一对象，范型版本
    /// </summary>
    public class APIResultModel<T> : APIBaseResultModel
    {
        /// <summary>
        /// 空构造函数
        /// </summary>
        public APIResultModel() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="result">结果</param>
        /// <param name="msg">信息</param>
        /// <param name="data">数据内容</param>
        public APIResultModel(bool result, string msg, T data)
            : base(result, msg)
        {
            IsSuccess = result;
            Msg = msg;
            Data = data;
        }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    /// 统一支付接口返回结果
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        /// 是否有结果
        /// </summary>
        public bool IsResult { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Msg { get; set; }
    }
    
    /// <summary>
    /// 其实项目返回的树结构内容
    /// </summary>
    public class OtherTreeNode
    {
        public string ID { get; set; }
        public string Name { get; set; }

    }
}
