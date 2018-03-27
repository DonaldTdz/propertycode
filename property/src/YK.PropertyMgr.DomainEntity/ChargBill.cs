using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class ChargBill: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public string Id { get; set; }

		/// <summary>
        /// 描述
        /// </summary>
		public string Description { get; set; }

		/// <summary>
        /// 计算表达式
        /// </summary>
		public string Quantity { get; set; }

		/// <summary>
        /// 账单开始日期
        /// </summary>
		public DateTime? BeginDate { get; set; }

		/// <summary>
        /// 账单结束日期
        /// </summary>
		public DateTime? EndDate { get; set; }

		/// <summary>
        /// 房间Id
        /// </summary>
		public int? HouseDeptId { get; set; }

		/// <summary>
        /// 资源Id
        /// </summary>
		public int? ResourcesId { get; set; }

		/// <summary>
        /// 资源名称
        /// </summary>
		public string ResourcesName { get; set; }

		/// <summary>
        /// 房屋编号
        /// </summary>
		public string HouseDoorNo { get; set; }

		/// <summary>
        /// 关联类型
        /// </summary>
		public int? RefType { get; set; }

		/// <summary>
        /// 账单金额
        /// </summary>
		public decimal? BillAmount { get; set; }

		/// <summary>
        /// 已收金额
        /// </summary>
		public decimal? ReceivedAmount { get; set; }

		/// <summary>
        /// 减免金额
        /// </summary>
		public decimal? ReliefAmount { get; set; }

		/// <summary>
        /// 滞纳金额
        /// </summary>
		public decimal? PenaltyAmount { get; set; }

		/// <summary>
        /// 小区Id
        /// </summary>
		public int? ComDeptId { get; set; }

		/// <summary>
        /// 开发商代缴
        /// </summary>
		public bool? IsDevPay { get; set; }

		/// <summary>
        ///  状态
        /// </summary>
		public int? Status { get; set; }

		/// <summary>
        /// 删除标志
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 生成时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime? UpdateTime { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }
		
      public int? ChargeSubjectId { get; set; }
      public virtual ChargeSubject ChargeSubject { get; set; }
    
	 }
	public partial class ChargBillMapper : EntityMapper<ChargBill>
    {
        public ChargBillMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.Description).HasMaxLength(300).IsOptional();
			Property(s => s.Quantity).HasMaxLength(200).IsOptional();
			Property(s => s.BeginDate).IsOptional();
			Property(s => s.EndDate).IsOptional();
			Property(s => s.HouseDeptId).IsOptional();
			Property(s => s.ResourcesId).IsOptional();
			Property(s => s.ResourcesName).HasMaxLength(100).IsOptional();
			Property(s => s.HouseDoorNo).HasMaxLength(100).IsOptional();
			Property(s => s.RefType).IsOptional();
			Property(s => s.BillAmount).IsOptional();
			Property(s => s.ReceivedAmount).IsOptional();
			Property(s => s.ReliefAmount).IsOptional();
			Property(s => s.PenaltyAmount).IsOptional();
			Property(s => s.ComDeptId).IsOptional();
			Property(s => s.IsDevPay).IsOptional();
			Property(s => s.Status).IsOptional();
			Property(s => s.IsDel).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.UpdateTime).IsOptional();
			Property(s => s.Remark).HasMaxLength(300).IsOptional();
        }
    }
}
