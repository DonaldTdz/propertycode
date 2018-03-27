using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    /// <summary>
    /// 获取的小区信息
    /// </summary>
    public class APIVillage
    {
        public int DeptId { get; set; }
        public string Name { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string County { get; set; }
    }
    /// <summary>
    /// 获取的设备信息
    /// </summary>
    public class APIEnrance
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? KeyID { get; set; }
        public string Address { get; set; }
        public int? CommunityDeptId { get; set; }
        public int? IncrementId { get; set; }
        public int? PwdGroup { get; set; }
        public string DeviceId { get; set; }

    }

    public class APIEntrancParameter
    {
        public int EntranceId { get; set; }
        public int NewKeyId { get; set; }
        public string DeviceId { get; set; }



    }
    public class APIEntrancs
    {
        public string APIEntrancList { get; set; }

    }

    /// <summary>
    /// 接口返回
    /// </summary>
    public class APIResult
    {
        public bool Result { get; set; }
        public string Msg { get; set; }
        public object Data { get; set; }

    }
}
