using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.RepositoryContract
{
    /// <summary>
    /// 通过UnitOfWork对数据访问进行统一管理
    /// </summary>
    public partial interface IBackgroundMgrUnitOfWork : IUnitOfWork, IDisposable
    {
        #region Repository
        ISEC_AdminUserRepository SEC_AdminUserRepository { get; }
        ISEC_UserRepository SEC_UserRepository { get; }
        ISEC_DeptRepository SEC_DeptRepository { get; }
        ISys_DictionaryRepository Sys_DictionaryRepository { get; }
        ISys_DictionaryItemRepository Sys_DictionaryItemRepository { get; }
        ISEC_RoleRepository SEC_RoleRepository { get; }
        ISEC_FieldRepository SEC_FieldRepository { get; }
        ISEC_OperateRepository SEC_OperateRepository { get; }
        ISEC_ModuleRepository SEC_ModuleRepository { get; }
        ISEC_User_OwnerRepository SEC_User_OwnerRepository { get; }
        ISEC_CommunityRepository SEC_CommunityRepository { get; }
        ISEC_BuildingRepository SEC_BuildingRepository { get; }
        ISEC_HouseRepository SEC_HouseRepository { get; }
        ISEC_CarportRepository SEC_CarportRepository { get; }
        ISEC_WeChatPublicNumberRepository SEC_WeChatPublicNumberRepository { get; }
        ISEC_WeChatOpenIdRepository SEC_WeChatOpenIdRepository { get; }
        ISEC_GarageRepository SEC_GarageRepository { get; }
        ISEC_DeveloperRepository SEC_DeveloperRepository { get; }
        ISEC_GatewayRepository SEC_GatewayRepository { get; }
        ISEC_GatewayAuthRepository SEC_GatewayAuthRepository { get; }
        IOtherSysErrorEntityRepository OtherSysErrorEntityRepository { get; }
        ISEC_PropertyRepository SEC_PropertyRepository { get; }
        ISEC_AreaRepository SEC_AreaRepository { get; }
        #endregion

        /// <summary>
        /// 直接执行Sql语句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>执行结果</returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);

        /// <summary>
        /// 直接执行Sql语句
        /// </summary>
        /// <param name="transactionalBehavior">是否封装在存储过程中</param>
        /// <param name="sql">Sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>执行结果</returns>
        int ExecuteSqlCommand(TransactionalBehavior transactionalBehavior, string sql, params object[] parameters);

		void AddRelation(string strTableName, string strKey1Name, string strKey2Name, object strKey1Value, object strKey2Value);
    }
}
