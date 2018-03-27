using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;

namespace YK.PropertyMgr.ApplicationDTO
{
    public class ReportSearchDTO:BaseSearchDTO
    {


        /// <summary>
        /// 可以看到的小区限制
        /// </summary>
        public IList<DeptInfo> ComDeptList { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public BillStatusEnum Status { get; set; }


        public int? DefaultComDeptId { get; set; }
        /// <summary>
        /// 小区Id
        /// </summary>
        public int? ComDeptId { get {

                if (this.ComDeptIdStr == null)
                    return DefaultComDeptId;             
                return int.Parse(this.ComDeptIdStr.Replace("number:", "")); } set { this.ComDeptIdStr = value.ToString(); } }

        /// <summary>
        /// 小区Idstring
        /// </summary>
        public string ComDeptIdStr { get; set; }

        /// <summary>
        /// 门牌号
        /// </summary>
        public string DoorNumber { get; set; }

        /// <summary>
        /// 科目Id
        /// </summary>
        public int? ChargeSubjectId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 付费截止日期
        /// </summary>
        public DateTime Paydate { get; set; }
        /// <summary>
        /// 是否欠费
        /// </summary>
       public int TuitionStatus { get; set; }

        /// <summary>
        /// 是否是房屋
        /// </summary>
        public bool IsHouse { get; set; }
        /// <summary>
        /// 楼栋资源
        /// </summary>
        public string LouyuIdStr { get; set; }

    }
}
