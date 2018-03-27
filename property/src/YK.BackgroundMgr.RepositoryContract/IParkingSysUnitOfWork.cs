using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace YK.ParkingSys.RepositoryContract
{
    /// <summary>
    /// 通过UnitOfWork对数据访问进行统一管理
    /// </summary>
    public partial interface IParkingSysUnitOfWork : IUnitOfWork, IDisposable
    {
        #region Repository
        IParkingRepository ParkingRepository { get; }
        ICarportRepository CarportRepository { get; }
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
