using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class SEC_User_Owner: IAggregateRoot
	{
		/// <summary>
        /// 主键
        /// </summary>
		public Guid? Id { get; set; }
		/// <summary>
        /// 门牌号
        /// </summary>
		public string DoorNo { get; set; }
		/// <summary>
        /// 昵称
        /// </summary>
		public string UserName { get; set; }
		/// <summary>
        /// 是否户主
        /// </summary>
		public int? IsMaster { get; set; }
		/// <summary>
        /// 客户类型
        /// </summary>
		public int? CustomerType { get; set; }
		/// <summary>
        /// 联系地址
        /// </summary>
		public string Address { get; set; }
		/// <summary>
        /// 民族
        /// </summary>
		public string Nation { get; set; }
		/// <summary>
        /// 邮编
        /// </summary>
		public string PostCode { get; set; }
		/// <summary>
        /// Email
        /// </summary>
		public string Email { get; set; }
		/// <summary>
        /// 绑定手机户名
        /// </summary>
		public string BindingUser { get; set; }
		/// <summary>
        /// 绑定手机号码
        /// </summary>
		public string BindingPhonerNumber { get; set; }
		/// <summary>
        /// 证件类型
        /// </summary>
		public string CertificateType { get; set; }
		/// <summary>
        /// 证件号码
        /// </summary>
		public string CertificateNum { get; set; }
		/// <summary>
        /// 出入证号(门卡号)
        /// </summary>
		public string OutInNumber { get; set; }
		/// <summary>
        /// 门卡持有人姓名
        /// </summary>
		public string CertificateName { get; set; }
		/// <summary>
        /// 发卡时间
        /// </summary>
		public DateTime? SendCardTime { get; set; }
		/// <summary>
        /// 个人
        /// </summary>
		public int? IsPersonage { get; set; }
		/// <summary>
        /// 性别
        /// </summary>
		public int? Gender { get; set; }
		/// <summary>
        /// 生日
        /// </summary>
		public DateTime? Birthday { get; set; }
		/// <summary>
        /// 职业
        /// </summary>
		public string Profession { get; set; }
		/// <summary>
        /// 工作单位
        /// </summary>
		public string WorkUnit { get; set; }
		/// <summary>
        /// 爱好
        /// </summary>
		public string Hobby { get; set; }
		/// <summary>
        /// 等级
        /// </summary>
		public string Level { get; set; }
		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }
		/// <summary>
        /// 微信OpenId
        /// </summary>
		public string OpenID { get; set; }
		/// <summary>
        /// 手机App密码
        /// </summary>
		public string AppPass { get; set; }
		/// <summary>
        /// 是否删除
        /// </summary>
		public int? IsDelete { get; set; }
		/// <summary>
        /// 是否认证通过
        /// </summary>
		public int? IsAuth { get; set; }
		/// <summary>
        /// user_img
        /// </summary>
		public string user_img { get; set; }
		/// <summary>
        /// 小区编码
        /// </summary>
		public string CommunityCode { get; set; }
		/// <summary>
        /// 其他系统主键Id
        /// </summary>
		public string OtherSysId { get; set; }
		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
        /// 微信认证时间
        /// </summary>
		public DateTime? WechatAuthTime { get; set; }
		/// <summary>
        /// App认证时间
        /// </summary>
		public DateTime? AppAuthTime { get; set; }
		/// <summary>
        /// 是否授权
        /// </summary>
		public int? IsPermission { get; set; }
		/// <summary>
        /// 真实姓名
        /// </summary>
		public string RealName { get; set; }
		/// <summary>
        /// 身份证号
        /// </summary>
		public string IdentityCardNo { get; set; }
		/// <summary>
        /// 是否通过跨境电商验证
        /// </summary>
		public int? IsAuthedCEC { get; set; }
		
      public virtual ICollection<SEC_User_OwnerSEC_Dept> SEC_User_OwnerSEC_Dept { get; set; }
      public virtual ICollection<SEC_User_OwnerSEC_Carport> SEC_User_OwnerSEC_Carport { get; set; }
    
	 }
	public partial class SEC_User_OwnerMapper : EntityMapper<SEC_User_Owner>
    {
        public SEC_User_OwnerMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.DoorNo).HasMaxLength(200).IsRequired();
			Property(s => s.UserName).HasMaxLength(50).IsRequired();
			Property(s => s.IsMaster).IsRequired();
			Property(s => s.CustomerType).IsOptional();
			Property(s => s.Address).HasMaxLength(300).IsOptional();
			Property(s => s.Nation).HasMaxLength(50).IsOptional();
			Property(s => s.PostCode).HasMaxLength(16).IsOptional();
			Property(s => s.Email).HasMaxLength(300).IsOptional();
			Property(s => s.BindingUser).HasMaxLength(50).IsOptional();
			Property(s => s.BindingPhonerNumber).HasMaxLength(20).IsRequired();
			Property(s => s.CertificateType).HasMaxLength(50).IsOptional();
			Property(s => s.CertificateNum).HasMaxLength(50).IsOptional();
			Property(s => s.OutInNumber).HasMaxLength(50).IsOptional();
			Property(s => s.CertificateName).HasMaxLength(20).IsOptional();
			Property(s => s.SendCardTime).IsOptional();
			Property(s => s.IsPersonage).IsOptional();
			Property(s => s.Gender).IsOptional();
			Property(s => s.Birthday).IsOptional();
			Property(s => s.Profession).HasMaxLength(200).IsOptional();
			Property(s => s.WorkUnit).HasMaxLength(200).IsOptional();
			Property(s => s.Hobby).HasMaxLength(200).IsOptional();
			Property(s => s.Level).HasMaxLength(100).IsOptional();
			Property(s => s.Remark).HasMaxLength(2000).IsOptional();
			Property(s => s.OpenID).HasMaxLength(100).IsOptional();
			Property(s => s.AppPass).HasMaxLength(50).IsOptional();
			Property(s => s.IsDelete).IsOptional();
			Property(s => s.IsAuth).IsOptional();
			Property(s => s.user_img).IsMaxLength().IsOptional();
			Property(s => s.CommunityCode).HasMaxLength(50).IsOptional();
			Property(s => s.OtherSysId).HasMaxLength(100).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.WechatAuthTime).IsOptional();
			Property(s => s.AppAuthTime).IsOptional();
			Property(s => s.IsPermission).IsOptional();
			Property(s => s.RealName).HasMaxLength(10).IsOptional();
			Property(s => s.IdentityCardNo).HasMaxLength(18).IsOptional();
			Property(s => s.IsAuthedCEC).IsOptional();
			
      HasMany(s => s.SEC_User_OwnerSEC_Dept).WithRequired(s => s.SEC_User_Owner).HasForeignKey(s => s.SEC_User_Owner_Id).WillCascadeOnDelete(true);
      HasMany(s => s.SEC_User_OwnerSEC_Carport).WithRequired(s => s.SEC_User_Owner).HasForeignKey(s => s.SEC_User_Owner_Id).WillCascadeOnDelete(true);
    
        }
    }
}
