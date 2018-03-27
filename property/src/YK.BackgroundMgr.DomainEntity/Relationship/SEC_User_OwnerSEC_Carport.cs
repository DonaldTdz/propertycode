using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.DomainEntity
{
    public partial class SEC_User_OwnerSEC_Carport : IAggregateRoot
    {
        /// <summary>
        /// 主键
        /// </summary>
		public int Id { get; set; }

        /// <summary>
        /// 关系 (1业主 2租客)
        /// </summary>
        public int PersonState { get; set; }

        /// <summary>
        /// 是否删除 (0未删除 1已删除) 
        /// </summary>
        public int IsDelete { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public virtual Guid SEC_User_Owner_Id { get; set; }
        public virtual int SEC_Dept_Id { get; set; }
        public virtual SEC_Dept SEC_Dept { get; set; }
        public virtual SEC_User_Owner SEC_User_Owner { get; set; }
    }

    public partial class SEC_CarportSEC_DeptMapper : EntityMapper<SEC_User_OwnerSEC_Carport>
    {
        public SEC_CarportSEC_DeptMapper()
        {
            HasKey(s => s.Id);
            Property(s => s.PersonState).IsRequired();
            Property(s => s.IsDelete).IsRequired();
            Property(s => s.CreateTime).IsRequired();
        }
    }
}
