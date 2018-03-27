using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.ApplicationDTO
{
    [Serializable]
    public class AdminUserAndRoleInfo
    {
        /// <summary>
        /// 用户主键
        /// </summary>
        public int SEC_AdminUser_Id { get; set; }

        /// <summary>
        /// 角色主键
        /// </summary>
        public int SEC_Role_Id { get; set; }
    }

    [Serializable]
    public class DeptAndRoleInfo
    {
        /// <summary>
        /// 部门主键
        /// </summary>
        public int SEC_Dept_Id { get; set; }

        /// <summary>
        /// 角色主键
        /// </summary>
        public int SEC_Role_Id { get; set; }
    }

    [Serializable]
    public class DeptAndAdminUserInfo
    {
        /// <summary>
        /// 部门主键
        /// </summary>
        public int SEC_Dept_Id { get; set; }

        /// <summary>
        /// 角色主键
        /// </summary>
        public int SEC_AdminUser_Id { get; set; }
    }

    [Serializable]
    public class ModuleAndRoleInfo
    {
        /// <summary>
        /// 模块主键
        /// </summary>
        public int SEC_Module_Id { get; set; }

        /// <summary>
        /// 角色主键
        /// </summary>
        public int SEC_Role_Id { get; set; }
    }

    [Serializable]
    public class OperateAndRoleInfo
    {
        /// <summary>
        /// 操作主键
        /// </summary>
        public int SEC_Operate_Id { get; set; }

        /// <summary>
        /// 角色主键
        /// </summary>
        public int SEC_Role_Id { get; set; }
    }

    [Serializable]
    public class OperateCodeAndRoleInfo
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 角色主键
        /// </summary>
        public int SEC_Role_Id { get; set; }
    }

    [Serializable]
    public class OwnerAndDeptInfo
    {      
        /// <summary>
        /// 用户主键
        /// </summary>
        public Guid SEC_User_Owner_Id { get; set; }

        /// <summary>
        /// DeptId
        /// </summary>
        public int SEC_Dept_Id { get; set; }

        /// <summary>
        /// 主键
        /// 社区、电商组使用
        /// </summary>
        public int Id { get; set; }

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
    }

    /// <summary>
    /// 业主车位关系
    /// </summary>
    [Serializable]
    public class OwnerAndCarportInfo
    {
        /// <summary>
        /// 用户主键
        /// </summary>
        public Guid SEC_User_Owner_Id { get; set; }

        /// <summary>
        /// DeptId
        /// </summary>
        public int SEC_Dept_Id { get; set; }

        /// <summary>
        /// 主键
        /// 社区、电商组使用
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
    }

    [Serializable]
    public class AdminUserDeptInfo
    {
        public string Code { get; set; }
        public string DeptName { get; set; }

        /// <summary>
        /// DeptId
        /// </summary>
        public int SEC_Dept_Id { get; set; }

        /// <summary>
        /// 管理员Id
        /// </summary>
        public int SEC_AdminUser_Id { get; set; }
    }
}
