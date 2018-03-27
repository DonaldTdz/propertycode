using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.DomainEntity
{
    public partial class SEC_User_OwnerSEC_Dept : IAggregateRoot
    {
        /// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

        /// <summary>
        /// 业主状态 (1业主 2会员 3家人 4租客)
        /// </summary>
        public int? PersonState { get; set; }

        /// <summary>
        /// 默认房间 (0默认 1正在使用的业主)
        /// 微信、APP登陆后默认的房间
        /// </summary>
        public int? IsDefault { get; set; }

        /// <summary>
        /// 是否删除 (0未删除 1已删除) 
        /// </summary>
        public int? IsDelete { get; set; }

        public virtual Guid SEC_User_Owner_Id { get; set; }
        public virtual int SEC_Dept_Id { get; set; }
        public virtual SEC_Dept SEC_Dept { get; set; }
        public virtual SEC_User_Owner SEC_User_Owner { get; set; }

    }
    public partial class SEC_User_OwnerSEC_DeptMapper : EntityMapper<SEC_User_OwnerSEC_Dept>
    {
        public SEC_User_OwnerSEC_DeptMapper()
        {
            HasKey(s => s.Id);
            Property(s => s.PersonState).IsRequired();
            Property(s => s.IsDefault).IsRequired();
            Property(s => s.IsDelete).IsRequired();
        }
    }
}
