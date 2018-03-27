using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertySysAPI.Accessor
{
	/// <summary>
	/// 数据库帮助类
	/// </summary>
	public class DBHelper : IDisposable
	{
		#region 字段
		/// <summary>
		/// 数据库提供者
		/// </summary>
		private static string dbProviderName = ConfigurationManager.AppSettings["DbHelperProvider"];
        /// <summary>
        /// 连接字符串connstring
        /// </summary>
        public static string connectionString = ConfigurationManager.ConnectionStrings["PropertyMgrConnection"].ToString();
		//public static string connectionString = ConfigurationManager.AppSettings["DBConectionStr"];

        public static string PublicSysdbo = "[YKFrameworkTest]";

		/// <summary>
		/// 数据库连接
		/// </summary>
		private DbConnection connection;
		#endregion

		#region 构造函数
		/// <summary>
		/// 无参构造函数
		/// </summary>
		public DBHelper()
		{
			connection = this.CreateConnection();
		}
		#endregion

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="conString">连接字符串</param>
		public DBHelper(string conString)
		{
			connectionString = conString;
		}

		/// <summary>
		/// 创建数据库连接
		/// </summary>
		/// <returns>数据库连接</returns>
		private DbConnection CreateConnection()
		{
			DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DBHelper.dbProviderName);
			DbConnection dbconn = dbfactory.CreateConnection();
			dbconn.ConnectionString = connectionString;
			return dbconn;
		}

		/// <summary>
		/// 创建数据库命令
		/// </summary>
		/// <param name="storedProcedure">存储过程名</param>
		/// <returns>数据库命令</returns>
		public DbCommand GetStoredProcCommond(string storedProcedure)
		{
			DbCommand dbCommand = connection.CreateCommand();
			dbCommand.CommandText = storedProcedure;
			dbCommand.CommandType = CommandType.StoredProcedure;
			dbCommand.CommandTimeout = 30;
			if (dbCommand.Connection.State == ConnectionState.Closed)
			{
				dbCommand.Connection.Open();
			}
			return dbCommand;
		}

		/// <summary>
		/// 创建数据库命令
		/// </summary>
		/// <param name="sqlQuery">SQL语句</param>
		/// <returns>数据库命令</returns>
		public DbCommand GetSqlStringCommond(string sqlQuery)
		{
			DbCommand dbCommand = connection.CreateCommand();
			dbCommand.CommandText = sqlQuery;
			dbCommand.CommandType = CommandType.Text;
			dbCommand.CommandTimeout = 30;
			if (dbCommand.Connection.State == ConnectionState.Closed)
			{
				dbCommand.Connection.Open();
			}
			return dbCommand;
		}

		/// <summary>
		/// 创建数据库命令
		/// </summary>
		/// <param name="sqlQuery">SQL语句</param>
		/// <returns>数据库命令</returns>
		public DbCommand GetSqlCommond()
		{
			DbCommand dbCommand = connection.CreateCommand();
			dbCommand.CommandType = CommandType.Text;
			dbCommand.CommandTimeout = 30;
			if (dbCommand.Connection.State == ConnectionState.Closed)
			{
				dbCommand.Connection.Open();
			}
			return dbCommand;
		}

		/// <summary>
		/// 添加输出参数数
		/// </summary>
		/// <param name="cmd">数据库命令</param>
		/// <param name="parameterName">参数名(一般指输出参数名)</param>
		/// <param name="dbType">数据类型</param>
		/// <param name="size">数据大小</param>
		public void AddOutParameter(DbCommand cmd, string parameterName, DbType dbType, int size)
		{
			DbParameter dbParameter = cmd.CreateParameter();
			dbParameter.DbType = dbType;
			dbParameter.ParameterName = parameterName;
			dbParameter.Size = size;
			dbParameter.Direction = ParameterDirection.Output;
			cmd.Parameters.Add(dbParameter);
		}

		/// <summary>
		/// 添加数据库命令参数(存储过程或SQL语句的参数)
		/// </summary>
		/// <param name="cmd">数据库命令</param>
		/// <param name="parameterName">参数名</param>
		/// <param name="dbType">数据类型</param>
		/// <param name="value">数据值</param>
		public void AddInParameter(DbCommand cmd, string parameterName, DbType dbType, object value)
		{
			DbParameter dbParameter = cmd.CreateParameter();
			dbParameter.DbType = dbType;
			dbParameter.ParameterName = parameterName;
			dbParameter.Value = value;
			dbParameter.Direction = ParameterDirection.Input;
			cmd.Parameters.Add(dbParameter);
		}

		/// <summary>
		/// 添加返回参数(指执行了数据操作后返回的参数)
		/// </summary>
		/// <param name="cmd">数据库命令</param>
		/// <param name="parameterName">参数名</param>
		/// <param name="dbType">数据类型</param>
		public void AddReturnParameter(DbCommand cmd, string parameterName, DbType dbType)
		{
			DbParameter dbParameter = cmd.CreateParameter();
			dbParameter.DbType = dbType;
			dbParameter.ParameterName = parameterName;
			dbParameter.Direction = ParameterDirection.ReturnValue;
			cmd.Parameters.Add(dbParameter);
		}

		/// <summary>
		/// 获取数据库操作返回的参数
		/// </summary>
		/// <param name="cmd">数据库命令</param>
		/// <param name="parameterName">参数名</param>
		/// <returns>数据库返回参数</returns>
		public DbParameter GetParameter(DbCommand cmd, string parameterName)
		{
			return cmd.Parameters[parameterName];
		}

		/// <summary>
		/// 执行数据库命令
		/// </summary>
		/// <param name="cmd">数据库命令</param>
		/// <returns>返回受影响的行数</returns>
		public int ExecuteNonQuery(DbCommand cmd)
		{
			try
			{
				if (cmd.Connection.State == ConnectionState.Closed)
				{
					cmd.Connection.Open();
				}
				int ret = cmd.ExecuteNonQuery();
				cmd.Connection.Close();
				return ret;
			}
			catch (SqlException ex)
			{
				cmd.Connection.Close();
				throw ex;
			}
		}

		public int ExecuteNonQueryTrans(DbCommand cmd)
		{
			try
			{
				if (cmd.Connection.State == ConnectionState.Closed)
				{
					cmd.Connection.Open();
				}
				int ret = cmd.ExecuteNonQuery();
				return ret;
			}
			catch (SqlException ex)
			{
				cmd.Connection.Close();
				throw ex;
			}
		}
		/// <summary>
		/// 执行数据库读命令
		/// </summary>
		/// <param name="cmd">数据库命令</param>
		/// <returns>DbDataReader对象</returns>
		public DbDataReader ExecuteReader(DbCommand cmd)
		{
			try
			{
				DbDataReader reader = cmd.ExecuteReader();
				return reader;
			}
			catch (Exception ex)
			{
				cmd.Connection.Close();
				throw ex;
			}
		}

		/// <summary>
		/// 执行数据库命令
		/// 此方法适用于返回一条数据
		/// </summary>
		/// <param name="cmd">数据库命令</param>
		/// <returns>返回第一行第一列的值</returns>
		public object ExecuteScalar(DbCommand cmd)
		{
			try
			{
				if (cmd.Connection.State == ConnectionState.Closed)
				{
					cmd.Connection.Open();
				}
				object o = cmd.ExecuteScalar();
				cmd.Connection.Close();
				return o;
			}
			catch (Exception ex)
			{
				cmd.Connection.Close();
				throw ex;
			}
		}
		public object ExecuteScalarTran(DbCommand cmd)
		{
			try
			{
				if (cmd.Connection.State == ConnectionState.Closed)
				{
					cmd.Connection.Open();
				}
				object o = cmd.ExecuteScalar();
				return o;
			}
			catch (Exception ex)
			{
				cmd.Connection.Close();
				throw ex;
			}
		}

		/// <summary>
		/// 根据指定的数据库命令返回DataSet
		/// </summary>
		/// <param name="cmd">数据库命令</param>
		/// <returns>DataSet结果集</returns>
		public DataSet ExecuteDataSet(DbCommand cmd)
		{
			if (cmd.Connection.State == ConnectionState.Closed)
			{
				cmd.Connection.Open();
			}
			DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DBHelper.dbProviderName);
			DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
			dbDataAdapter.SelectCommand = cmd;
			DataSet ds = new DataSet();
			dbDataAdapter.Fill(ds);
			cmd.Connection.Close();
			///cmd.Connection.Dispose();
			return ds;
		}

		/// <summary>
		/// 根据指定的数据库命令返回DataSet
		/// </summary>
		/// <param name="cmd">数据库偏偏</param>
		/// <param name="useTransaction">是否使用事务,true使用事务,false不使用事务</param>
		/// <returns>返回DataSet集合</returns>
		public DataSet ExecuteDataSet(DbCommand cmd, bool useTransaction)
		{
			DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DBHelper.dbProviderName);
			DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
			dbDataAdapter.SelectCommand = cmd;
			DataSet ds = new DataSet();
			dbDataAdapter.Fill(ds);
			cmd.Connection.Close();
			return ds;
		}

		/// <summary>
		/// 根据指定的数据库命令返回DataTable
		/// </summary>
		/// <param name="cmd">数据库命令</param>
		/// <returns>返回DataTable集合</returns>
		public DataTable ExecuteDataTable(DbCommand cmd)
		{
			try
			{
				DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DBHelper.dbProviderName);
				DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
				dbDataAdapter.SelectCommand = cmd;
				DataTable dataTable = new DataTable();
				dbDataAdapter.Fill(dataTable);
				cmd.Connection.Close();
				return dataTable;
			}
			catch (SqlException ex)
			{
				cmd.Connection.Close();
				throw ex;
			}
		}

	
		/// <summary>
		/// 执行存储过程
		/// </summary>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <param name="tableName">DataSet结果中的表名</param>
		/// <returns>DataSet</returns>
		public DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				DataSet dataSet = new DataSet();
				connection.Open();
				SqlDataAdapter sqlDA = new SqlDataAdapter();
				sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
				sqlDA.Fill(dataSet, tableName);
				connection.Close();
				return dataSet;
			}
		}
		public DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, int Times)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				DataSet dataSet = new DataSet();
				connection.Open();
				SqlDataAdapter sqlDA = new SqlDataAdapter();
				sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
				sqlDA.SelectCommand.CommandTimeout = Times;
				sqlDA.Fill(dataSet, tableName);
				connection.Close();
				return dataSet;
			}
		}


		/// <summary>
		/// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
		/// </summary>
		/// <param name="connection">数据库连接</param>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <returns>SqlCommand</returns>
		private SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
		{
			SqlCommand command = new SqlCommand(storedProcName, connection);
			command.CommandType = CommandType.StoredProcedure;
			foreach (SqlParameter parameter in parameters)
			{
				if (parameter != null)
				{
					// 检查未分配值的输出参数,将其分配以DBNull.Value.
					if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
						(parameter.Value == null))
					{
						parameter.Value = DBNull.Value;
					}
					command.Parameters.Add(parameter);
				}
			}

			return command;
		}


		/// <summary>
		/// 执行数据库命令，带事务版本
		/// </summary>
		/// <param name="cmd">数据库命令</param>
		/// <param name="t">事务类</param>
		/// <returns>受影响的行数</returns>
		public int ExecuteNonQuery(DbCommand cmd, Trans t)
		{
			try
			{
				if (t.DbConnection.State == ConnectionState.Closed)
				{
					t.DbConnection.Open();
				}
				cmd.Connection = t.DbConnection;
				cmd.Transaction = t.DbTrans;
				return cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				t.RollBack();
				t.Colse();
				t.DbConnection.Close();
				throw ex;
			}
		}

		public void Dispose()
		{
			this.connection.Close();
		}
	}

	/// <summary>
	/// 数据库事务类
	/// </summary>
	public class Trans : IDisposable
	{

		/// <summary>
		/// 数据库提供者
		/// </summary>
		private static string dbProviderName = ConfigurationManager.AppSettings["DbHelperProvider"];
		/// <summary>
		/// 连接字符串
		/// </summary>
		//public static string connectionString = ConfigurationManager.AppSettings["DbHelperConnectionString"];
		public static string connectionString = ConfigurationManager.ConnectionStrings["PropertyMgrConnection"].ToString();


		private DbConnection conn;
		private DbTransaction dbTrans;
		/// <summary>
		/// 数据库连接
		/// </summary>
		public DbConnection DbConnection
		{
			get { return this.conn; }
		}

		/// <summary>
		/// 事务属性
		/// </summary>
		public DbTransaction DbTrans
		{
			get { return this.dbTrans; }
			set { this.dbTrans = value; }
		}

		/// <summary>
		/// 创建数据库连接
		/// </summary>
		/// <returns>数据库连接</returns>
		private DbConnection CreateConnection()
		{
			DbProviderFactory dbfactory = DbProviderFactories.GetFactory(dbProviderName);
			DbConnection dbconn = dbfactory.CreateConnection();
			dbconn.ConnectionString = connectionString;
			return dbconn;
		}
		/// <summary>
		/// 创建Command
		/// </summary>
		/// <returns></returns>
		public DbCommand Createcommand()
		{
			DbCommand dbCommand = conn.CreateCommand();
			dbCommand.CommandTimeout = 30;
			if (dbCommand.Connection.State == ConnectionState.Closed)
			{
				dbCommand.Connection.Open();
			}
			return dbCommand;
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="connection"></param>
		public Trans(DbConnection connection)
		{
			this.conn = connection;
			if (conn.State == ConnectionState.Closed)
			{
				conn.Open();
			}
			dbTrans = conn.BeginTransaction();
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		public Trans()
		{
			this.conn = CreateConnection();
			if (conn.State == ConnectionState.Closed)
			{
				conn.Open();
			}
			dbTrans = conn.BeginTransaction();
		}

		/// <summary>
		/// 提交事务
		/// </summary>
		public void Commit()
		{
			dbTrans.Commit();
			this.Colse();
		}

		/// <summary>
		/// 回滚事务
		/// </summary>
		public void RollBack()
		{
			dbTrans.Rollback();
			this.Colse();
		}

		/// <summary>
		/// 释放资源
		/// </summary>
		public void Dispose()
		{
			this.Colse();
		}

		/// <summary>
		/// 关闭连接
		/// </summary>
		public void Colse()
		{
			if (conn.State == ConnectionState.Open)
			{
				conn.Close();
			}
		}
	}
}
