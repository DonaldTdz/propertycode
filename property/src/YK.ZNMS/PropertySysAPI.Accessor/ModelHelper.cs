using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PropertySysAPI.Accessor
{
    /// <summary>
    /// 通用数据转换为实体类
    /// </summary>
    public class ModelHelper
    {


        public static T ConvertToModel<T>(DataRow dr) where T : new()
        {
            T t = new T();
            Type modelType = t.GetType();
            foreach (PropertyInfo pi in modelType.GetProperties())
            {
                if (pi == null) continue;
                if (pi.CanWrite == false) continue;

                if (dr.Table.Columns.Contains(pi.Name))
                {
                    //p.SetValue(t, GetDefaultValue(dr[p.Name], p.PropertyType), null);
                    try
                    {
                        if (dr[pi.Name] != DBNull.Value)
                            pi.SetValue(t, dr[pi.Name], null);
                        else
                            pi.SetValue(t, default(object), null);
                    }
                    catch
                    {
                        pi.SetValue(t, GetDefaultValue(dr[pi.Name], pi.PropertyType), null);
                    }
                }

            }
            return t;
        }
        private static object GetDefaultValue(object obj, Type type)
        {
            if (obj == DBNull.Value)
            {
                return default(object);
            }
            else
            {
                return Convert.ChangeType(obj, type);
            }
        }


        #region DataSet 、 DataTable 转换为实体类泛型集合

        /// <summary>
        /// DataSet 第一个表转换为实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ds"></param>
        /// <returns>实体类泛型集合</returns>
        public static List<T> ConvertToModel<T>(DataSet ds)
        {
            if (ds.Tables.Count == 0)
                return new List<T>();

            return ConvertToModel<T>(ds.Tables[0]);
        }
        /// <summary>
        /// DataTable 第一个表转换为实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns>实体类泛型集合</returns>
        public static List<T> ConvertToModel<T>(DataTable dt)
        {
            List<T> l = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                T model = Activator.CreateInstance<T>();

                foreach (DataColumn dc in dr.Table.Columns)
                {
                    PropertyInfo pi = model.GetType().GetProperty(dc.ColumnName);
                    if (pi == null) continue;
                    if (pi.CanWrite == false) continue;

                    try
                    {
                        if (dr[dc.ColumnName] != DBNull.Value)
                            pi.SetValue(model, dr[dc.ColumnName], null);
                        else
                            pi.SetValue(model, default(object), null);
                    }
                    catch
                    {
                        pi.SetValue(model, GetDefaultValue(dr[pi.Name], pi.PropertyType), null);
                    }

                }
                l.Add(model);
            }

            return l;
        }
        #endregion

        #region 将实体类转换成DataTable

        /// <summary> 
        /// 将实体类转换成DataTable 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="i_objlist"></param> 
        /// <returns></returns> 
        public static DataTable ConvertDataTable<T>(IList<T> objlist)
        {
            if (objlist == null || objlist.Count <= 0)
            {
                return null;
            }
            DataTable dt = new DataTable(typeof(T).Name);
            DataColumn column;
            DataRow row;

            System.Reflection.PropertyInfo[] myPropertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (T t in objlist)
            {
                if (t == null)
                {
                    continue;
                }
               

                row = dt.NewRow();

                for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                {
                    System.Reflection.PropertyInfo pi = myPropertyInfo[i];

                    string name = pi.Name;

                    if (dt.Columns[name] == null)
                    {
                        Type colType = pi.PropertyType;if ((colType.IsGenericType) && colType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                 
                        column = new DataColumn(name, colType);

                        dt.Columns.Add(column);
                    }


                    row[name] = pi.GetValue(t) == null ? DBNull.Value : pi.GetValue(t);
                }

                dt.Rows.Add(row);
            }
            return dt;
        }
        #endregion



       


    }
}
