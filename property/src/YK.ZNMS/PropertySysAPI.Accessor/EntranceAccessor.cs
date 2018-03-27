using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using PropertySysAPI.Entity;

namespace PropertySysAPI.Accessor
{
    /// <summary>
    /// 数据访问层 EntranceAccessor
    /// </summary>
    public class EntranceAccessor
    {
        private DBHelper db = null;
        private static readonly string time = DateTime.Now.ToString("yyyy-MM-dd");
        private static readonly string strEntranceUser = " SELECT A.Id, A.KeyID,C.KeyExpireTime,A.Name,A.VillageID from Entrances A,( SELECT  A.Id,Max(B.KeyExpireTime)KeyExpireTime  FROM Entrances A,EntranceUsers B WHERE A.Id=B.EntranceID AND  A.State=1 and B.UserOwnerInfoId=@in_UserOwnerInfoId and  KeyExpireTime>='" + time + "' group by B.UserOwnerInfoId,A.Id) C where C.Id=A.Id";
        /// <summary>
        /// 构造函数
        /// </summary>
        public EntranceAccessor()
        {
            db = new DBHelper();
        }

        /// <summary>
        /// 获取用户的门禁权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet GetEntranceUserByUserId(string userIds)
        {
            try
            {
                string sql = " SELECT A.Id, A.KeyID,C.KeyExpireTime,A.Name,A.VillageID from Entrances A,( SELECT  A.Id,Max(B.KeyExpireTime)KeyExpireTime  FROM Entrances A,EntranceUsers B WHERE A.Id=B.EntranceID AND  A.State=1 and B.UserOwnerInfoId in (" + userIds + ") and  KeyExpireTime>='" + time + "' group by B.UserOwnerInfoId,A.Id) C where C.Id=A.Id";
                DbCommand cmd = db.GetSqlStringCommond(sql);
                using (DataSet ds = db.ExecuteDataSet(cmd))
                {
                    return ds;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 公共查询方法
        /// </summary>
        /// <param name="table">表枚举</param>
        /// <param name="cols">字段，完全查询为“*”</param>
        /// <param name="where">where过滤条件</param>
        public DataTable CommonSearch(string tab, string cols, string where)
        {
            DbCommand cmd = db.GetStoredProcCommond("p_CommonSearch");
            db.AddInParameter(cmd, "@tbName", DbType.String, tab);
            db.AddInParameter(cmd, "@rows", DbType.String, cols);
            db.AddInParameter(cmd, "@where", DbType.String, where);
            return db.ExecuteDataTable(cmd);
        }
    }
}