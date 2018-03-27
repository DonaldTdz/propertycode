using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class EntrancePersonal
    {     /// <summary>
          /// 设备ID
          /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// KEYID
        /// </summary>
        public int? KeyID { get; set; }
        /// <summary>
        /// 小区ID
        /// </summary>
        public int? VillageID { get; set; }
        /// <summary>
        /// 楼宇
        /// </summary>
        public int? BuildId { get; set; }
        /// <summary>
        /// 单元
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 小区地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? State { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime KeyExpireTime { get; set; }
        /// <summary>
        /// 设备类型[小区、楼宇、单元]
        /// </summary>
        public string DeviceType { get; set; }

    }
}
