using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    /// <summary>
    /// 消息中心模板
    /// </summary>
    [Serializable]
    public class MessageInfo
    {
        public MessageInfo()
        {
            MsgType = "105";
            TemplateType = "Notice";
        }

        /*
         * 
         * 101~199 为 通知消息
            201~299 为 优惠促销
            301~399 为 物业消息(纯文本)
         * 
         */
        /// <summary>
        /// 订单类 = 101
        /// 礼包类 = 102
        /// 优惠券类 = 103
        ///  物业收费类 =105
        /// 活动推荐 = 201
        /// 商家、 商品促销 = 202
        /// 促销推文 = 203
        /// 报事报修 = 301
        /// 物业通知 = 302
        /// 小区动态 = 303
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 模板类型
        /// Notice 通知消息
        ///  Image 图文消息
        ///  Txt 文本消息
        /// </summary>
        public string TemplateType { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 图标路径
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        /// 跳转链接
        /// </summary>
        public string ActionUrl { get; set; }

      

    }

    [Serializable]
    public class TxtMsg
    {
        /*
        * 
        * 101~199 为 通知消息
           201~299 为 优惠促销
           301~399 为 物业消息(纯文本)
        * 
        */
        /// <summary>
        /// 订单类 = 101
        /// 礼包类 = 102
        /// 优惠券类 = 103
        ///  物业收费类 =105
        /// 活动推荐 = 201
        /// 商家、 商品促销 = 202
        /// 促销推文 = 203
        /// 报事报修 = 301
        /// 物业通知 = 302
        /// 小区动态 = 303
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 模板类型
        /// Notice 通知消息
        ///  Image 图文消息
        ///  Txt 文本消息
        /// </summary>
        public string TemplateType { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
    }

}
