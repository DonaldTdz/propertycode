using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.Enums
{
    public enum HouseStatusEnum
    {
        /// <summary>
        /// 未交房
        /// </summary>
        [Description("未交房")]
        NotSubmit = 1,
        /// <summary>
        /// 未售房
        /// </summary>
        [Description("未售房")]
        Unsold = 2,
        /// <summary>
        /// 未收房
        /// </summary>
        [Description("未收房")]
        NotReceive = 3,
        /// <summary>
        /// 已收房
        /// </summary>
        [Description("已收房")]
        Received = 4,
    }
}
