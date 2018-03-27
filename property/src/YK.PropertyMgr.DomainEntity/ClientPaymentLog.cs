using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class ClientPaymentLog: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 费用记录ID
        /// </summary>
		public string ChargeRecordId { get; set; }

		/// <summary>
        /// 支付流水号
        /// </summary>
		public string NumericalNumber { get; set; }

		/// <summary>
        /// 订单编号
        /// </summary>
		public string OrderNum { get; set; }

		/// <summary>
        /// 订单数据
        /// </summary>
		public string OrderData { get; set; }

		/// <summary>
        /// 支付状态 待支付 = 0, 支付中 = 1,支付成功 = 2,支付失败 = 3,冻结中 = 4, 取消 = 5
        /// </summary>
		public int? PayState { get; set; }

		/// <summary>
        /// 支付宝 = 0,贵金支付 = 1,微信 = 2,一网通 = 3,钱包 = 99
        /// </summary>
		public int? PayType { get; set; }

		/// <summary>
        /// 描述
        /// </summary>
		public string Desc { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 回调更时间
        /// </summary>
		public DateTime? CallBackTime { get; set; }
	 }
	public partial class ClientPaymentLogMapper : EntityMapper<ClientPaymentLog>
    {
        public ClientPaymentLogMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.ChargeRecordId).HasMaxLength(36).IsOptional();
			Property(s => s.NumericalNumber).HasMaxLength(100).IsOptional();
			Property(s => s.OrderNum).HasMaxLength(50).IsOptional();
			Property(s => s.OrderData).HasMaxLength(8000).IsOptional();
			Property(s => s.PayState).IsOptional();
			Property(s => s.PayType).IsOptional();
			Property(s => s.Desc).HasMaxLength(200).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.CallBackTime).IsOptional();
        }
    }
}
