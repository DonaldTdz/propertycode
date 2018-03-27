using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.ApplicationDTO.ApplicationDTO
{
    public class HouseInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// 房屋的DeptID
        /// </summary>
        public int? HouseDeptID { get; set; }

        /// <summary>
        /// 建筑面积
        /// </summary>
        public double? BuildArea { get; set; }

        /// <summary>
        /// 套内面积
        /// </summary>
        public double? HouseInArea { get; set; }
        /// <summary>
        /// 房屋编号
        /// </summary>
        public string DoorNo { get; set; }
        /// <summary>
        /// 物业名称
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 小区名称
        /// </summary>
        public string CommunityName { get; set; }
        /// <summary>
        /// 房屋状态
        /// </summary>
        public int? HouseStatus { get; set; }
        public string HouseStatusStr { get; set; }
        /// <summary>
        /// 未售房是否绑定开发商
        /// </summary>
        public bool? UnsoldIsBindDeveloper { get; set; }
    }
}
