using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.Enums
{
    public enum EPrintTemplate
    {
        /// <summary>
        /// 收费票据
        /// </summary>
        BillTemplate = 1,
        /// <summary>
        /// 缴费通知单
        /// </summary>
        FeeNotify = 2,
        /// <summary>
        /// 交款票据
        /// </summary>
        PayNote = 3,
        /// <summary>
        /// 退款票据
        /// </summary>
        RefundNote = 4
    }
    public enum PrintTemplateEnum
    {
        /// <summary>
        /// 模板一
        /// </summary>
        TemplateOne = 1,
        /// <summary>
        /// 模板二
        /// </summary>
        TemplateTwo = 2,
        /// <summary>
        /// 模板三
        /// </summary>
        TemplateThree = 3,
        /// <summary>
        /// 套打模板-泾华物业
        /// </summary>
        JINHuaTemplatePrint=101,

       /// <summary>
       /// 套打模板-川港       
       /// </summary>
       ChuanGangTemplatePrint = 102


    }
}
