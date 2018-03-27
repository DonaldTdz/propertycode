using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    /// <summary>
    /// 业主信息
    /// </summary>
    public class OwnerInfo
    {
        /// <summary>
        /// 业主Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 业主姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 房屋deptId
        /// </summary>
        public int HouseDeptId { get; set; }
        /// <summary>
        /// 房屋编号
        /// </summary>
        public string DoorNo { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
    }
}
