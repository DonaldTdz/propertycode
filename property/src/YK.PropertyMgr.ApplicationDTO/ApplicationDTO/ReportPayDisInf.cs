using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.PresentationService;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class ReportPayDisInf
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 优惠类型
        /// </summary>
        public int? DiscountType { get; set; }
        /// <summary>
        /// 优惠类型
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 优惠名称
        /// </summary>
        public string DiscountDesc { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal? DiscountAmount { get; set; }

        /// <summary>
        /// 引用ID
        /// </summary>
        public string RefId { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        public bool? IsDel { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        public string CreateTimeFormat
        {
            get
            {
                return this.CreateTime.HasValue ? this.CreateTime.Value.ToString("yyyy-MM-dd") : "";
            }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public int? Operator { get; set; }

        /// <summary>
        /// 操作人名
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 票据号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourcesNames { get; set; }
    }
}
