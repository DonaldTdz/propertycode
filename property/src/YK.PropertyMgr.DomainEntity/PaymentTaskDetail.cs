using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class PaymentTaskDetail: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 费用明细Id
        /// </summary>
		public string ChargeRecordId { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime? UpdateTime { get; set; }

		/// <summary>
        /// 删除标识
        /// </summary>
		public bool? IsDel { get; set; }
		
      public int PaymentTaskID { get; set; }
      public virtual PaymentTasks PaymentTasks { get; set; }
    
	 }
	public partial class PaymentTaskDetailMapper : EntityMapper<PaymentTaskDetail>
    {
        public PaymentTaskDetailMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.ChargeRecordId).HasMaxLength(36).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.UpdateTime).IsOptional();
			Property(s => s.IsDel).IsOptional();
        }
    }
}
