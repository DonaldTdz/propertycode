using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public class AlipayPropertySearchDTO:BaseSearchDTO
    {
        public int? ComDeptId
        {
            get
            {
                if (!string.IsNullOrEmpty(ComDeptIdStr))
                {
                    return int.Parse(ComDeptIdStr.Replace("number:", ""));
                }
                return DefaultComDeptId;
            }
           
          

        }
        public int? DefaultComDeptId { get; set; }
        public string ComDeptIdStr { get; set; }
        /// <summary>
        /// 房间号
        /// </summary>
        public string HouseNumber { get; set; }
        /// <summary>
        /// 收费项目Id
        /// </summary>
        public int? ChargeSubjectId
        {
            get
            {
                if (!string.IsNullOrEmpty(ChargeSubjectIdStr))
                {
                    return int.Parse(ChargeSubjectIdStr.Replace("number:", ""));
                }
                return 0;
            }
        }
        public string ChargeSubjectIdStr { get; set; }


    }
}
