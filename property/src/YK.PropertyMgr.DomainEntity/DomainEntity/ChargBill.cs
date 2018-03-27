using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.DomainEntity
{
    public partial class ChargBill
    {
        /// <summary>
        /// 预交几月
        /// </summary>
        [NotMapped]
        public double? Months { get; set; }

        /// <summary>
        /// 预存收费项目ID
        /// </summary>
        [NotMapped]
        public int? PreChargeSubjectId { get; set; }
    }
}
