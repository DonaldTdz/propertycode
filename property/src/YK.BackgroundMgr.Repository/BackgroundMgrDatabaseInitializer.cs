using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;

using System.Configuration;

namespace YK.BackgroundMgr.Repository
{
    public class BackgroundMgrDatabaseInitializer : CreateDatabaseIfNotExists<BackgroundMgrDataBaseContext>
    {
        protected override void Seed(BackgroundMgrDataBaseContext context)
        {
            base.Seed(context);
            //InitFrameWorkVersion(context);
        }

        //private void InitFrameWorkVersion(FrameworkDataBaseContext context)
        //{
        //    string fileBasePath = "";
        //    if (System.Environment.CurrentDirectory.TrimEnd(new char[] { '\\' }) == AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[] { '\\' }))
        //    {
        //        fileBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
        //    }
        //    else
        //    {
        //        fileBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
        //    }
        //    var databaseName = context.Database.Connection.Database.ToString();
        //    var connection = (SqlConnection)context.Database.Connection;

        //    try
        //    {
        //        if (connection.State != ConnectionState.Open)
        //        {
        //            connection.Open();
        //        }

        //        var strNormalsql = string.Format(File.ReadAllText(Path.Combine(fileBasePath, "DatabaseExecute.txt")),databaseName);
        //        var serverconnection = new ServerConnection(connection);
        //        serverconnection.ExecuteNonQuery(strNormalsql);
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //}
    }
}
