using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.Enums
{
    /// <summary>
    /// 大门锁状态
    /// </summary>
    public enum BindSockStateEnum
    {

        /// <summary>
        /// 未安装锁
        /// </summary>
        NotBindLock = 0,

        /// <summary>
        /// 锁正常
        /// </summary>
        BindLock = 1,

        /// <summary>
        /// 锁坏了
        /// </summary>
        BadLock = 2,
    }
}
