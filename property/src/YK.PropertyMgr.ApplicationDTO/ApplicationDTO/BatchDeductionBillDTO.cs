using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    /// <summary>
    /// 批量抵扣账单
    /// </summary>
    [Serializable]
    public class BatchDeductionBillDTO
    {
        /// <summary>
        /// 小区DeptId
        /// </summary>
        public int? ComDeptId { get; set; }

        /// <summary>
        /// 房屋DeptId
        /// </summary>
        public int? HouseDeptId { get; set; }

        /// <summary>
        /// 收费项目Id
        /// </summary>
        public int? SubjectId { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 收费项目名称
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 欠费金额
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 收费项目预存费
        /// </summary>
        public decimal? PreAmount { get; set; }

        /// <summary>
        /// 全部收费项预存费
        /// </summary>
        public decimal? CommonPreAmount { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string HouseDoorNo { get; set; }
    }

    [Serializable]
    public class BatchDeductionBillSumDTO
    {
        /// <summary>
        /// 小区DeptId
        /// </summary>
        public int? ComDeptId { get; set; }

        /// <summary>
        /// 房屋DeptId
        /// </summary>
        public int? HouseDeptId { get; set; }

        /// <summary>
        /// 收费项目Id
        /// </summary>
        public int? SubjectId { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 收费项目名称
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 欠费金额
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 收费项目预存费
        /// </summary>
        public decimal? PreAmount { get; set; }

        /// <summary>
        /// 全部收费项预存费
        /// </summary>
        public decimal? CommonPreAmount { get; set; }
    }
}
