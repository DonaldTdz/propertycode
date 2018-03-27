using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public class ChargBillSearchDTO : BaseSearchDTO
    {
        /// <summary>
        /// 房屋Id
        /// </summary>
       public int? HouseDeptId { get; set; }

        /// <summary>
        /// 是否开发商代缴
        /// </summary>
        public bool IsDevPay { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public int SubjectType { get; set; }

        /// <summary>
        /// 房间编号
        /// </summary>
        public string HouseDeptName { get; set; }


        /// <summary>
        /// 资源编号
        /// </summary>
        public string ResourcesName { get; set; }
    
        /// <summary>
        /// 科目Id
        /// </summary>
        public string ChargeSubjectId { get; set; }


        public bool IsCarPark { get; set; }

    }
}
