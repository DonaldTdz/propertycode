using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class EntranceSearchDTO : BaseSearchDTO
    {
        /// <summary>
        /// 门禁名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 用户的ID信息 
        /// </summary>
        public string Guid { get; set; }

    }
}
