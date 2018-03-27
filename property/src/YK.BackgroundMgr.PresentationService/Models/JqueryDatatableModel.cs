using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace YK.BackgroundMgr.PresentationService
{
    /// <summary>
    /// 通用查询结果类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SearchResultData<T> where T : class
    {
        /// <summary>
        /// 客户端查询次数
        /// </summary>
        public int draw { get; set; }

        /// <summary>
        /// 结果总数
        /// </summary>
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }

        /// <summary>
        /// 分页查询后的数据结果
        /// </summary>
        public IEnumerable<T> data { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string error { get; set; }
    }

    public class DTParameterModel
    {
        /// <summary>
        /// Draw counter. This is used by DataTables to ensure that the Ajax returns from 
        /// server-side processing requests are drawn in sequence by DataTables 
        /// </summary>
        public int Draw { get; set; }

        /// <summary>
        /// Paging first record indicator. This is the start point in the current data set 
        /// (0 index based - i.e. 0 is the first record)
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Number of records that the table can display in the current draw. It is expected
        /// that the number of records returned will be equal to this number, unless the 
        /// server has fewer records to return. Note that this can be -1 to indicate that 
        /// all records should be returned (although that negates any benefits of 
        /// server-side processing!)
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Global Search for the table
        /// </summary>
        public DTSearch Search { get; set; }

        /// <summary>
        /// Custom Search String
        /// </summary>
        public string CustomSearch { get; set; }

        /// <summary>
        /// Collection of all column indexes and their sort directions
        /// </summary>
        public IEnumerable<DTOrder> Order { get; set; }

        /// <summary>
        /// Collection of all columns in the table
        /// </summary>
        public IEnumerable<DTColumn> Columns { get; set; }
    }

    /// <summary>
    /// Represents search values entered into the table
    /// </summary>
    public sealed class DTSearch
    {
        /// <summary>
        /// Global search value. To be applied to all columns which have searchable as true
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// true if the global filter should be treated as a regular expression for advanced 
        /// searching, false otherwise. Note that normally server-side processing scripts 
        /// will not perform regular expression searching for performance reasons on large 
        /// data sets, but it is technically possible and at the discretion of your script
        /// </summary>
        public bool Regex { get; set; }
    }

    /// <summary>
    /// Represents a column and it's order direction
    /// </summary>
    public sealed class DTOrder
    {
        /// <summary>
        /// Column to which ordering should be applied. This is an index reference to the 
        /// columns array of information that is also submitted to the server
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Ordering direction for this column. It will be asc or desc to indicate ascending
        /// ordering or descending ordering, respectively
        /// </summary>
        public string Dir { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public SortingDirection SortDerection { get; set; }

        /// <summary>
        /// 查询数据列
        /// </summary>
        public string ColumnData { get; set; }
    }

    /// <summary>
    /// Represents an individual column in the table
    /// </summary>
    public sealed class DTColumn
    {
        public int Column { get; set; }

        /// <summary>
        /// Column's data source
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Column's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Flag to indicate if this column is orderable (true) or not (false)
        /// </summary>
        public bool Orderable { get; set; }

        /// <summary>
        /// Search to apply to this specific column.
        /// </summary>
        public DTSearch Search { get; set; }
    }

    /// <summary>
    /// Model Binder for DataTablesParameterModel
    /// </summary>
    public class DTModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            base.BindModel(controllerContext, bindingContext);
            HttpRequestBase request = controllerContext.HttpContext.Request;
            // Retrieve request data
            int draw = Convert.ToInt32(request["draw"]);
            string strCustomSearch = request["CustomSearch"];
            int start = Convert.ToInt32(request["start"]);
            int length = Convert.ToInt32(request["length"]);
            // Search
            DTSearch search = new DTSearch
            {
                Value = request["search[value]"],
                Regex = Convert.ToBoolean(request["search[regex]"])
            };
            
            // Columns
            var c = 0;
            var columns = new List<DTColumn>();
            while (request["columns[" + c + "][name]"] != null)
            {
                columns.Add(new DTColumn
                {
                    Column = c,
                    Data = request["columns[" + c + "][data]"],
                    Name = request["columns[" + c + "][name]"],
                    Orderable = Convert.ToBoolean(request["columns[" + c + "][orderable]"]),
                    Search = new DTSearch
                    {
                        Value = request["columns[" + c + "][search][value]"],
                        Regex = Convert.ToBoolean(request["columns[" + c + "][search][regex]"])
                    }
                });
                c++;
            }

            // Order
            var o = 0;
            var order = new List<DTOrder>();
            while (request["order[" + o + "][column]"] != null)
            {
                order.Add(new DTOrder()
                {
                    Column = Convert.ToInt32(request["order[" + o + "][column]"]),
                    Dir = request["order[" + o + "][dir]"],
                    SortDerection = request["order[" + o + "][dir]"] == "asc" ? SortingDirection.Ascending : SortingDirection.Descending,
                    ColumnData = columns.SingleOrDefault(r => r.Column == Convert.ToInt32(request["order[" + o + "][column]"])).Data
                });
                o++;
            }

            return new DTParameterModel
            {
                CustomSearch = strCustomSearch,
                Draw = draw,
                Start = start,
                Length = length,
                Search = search,
                Order = order,
                Columns = columns
            };
        }
    }

    /// <summary>
    /// Contains sorting helpers for In Memory collections.
    /// </summary>
    public static class CollectionHelper
    {
        public static IOrderedEnumerable<TSource> CustomSort<TSource, TKey>(this IEnumerable<TSource> items, SortingDirection direction, Func<TSource, TKey> keySelector)
        {
            if (direction == SortingDirection.Ascending)
            {
                return items.OrderBy(keySelector);
            }

            return items.OrderByDescending(keySelector);
        }

        public static IOrderedEnumerable<TSource> CustomSort<TSource, TKey>(this IOrderedEnumerable<TSource> items, SortingDirection direction, Func<TSource, TKey> keySelector)
        {
            if (direction == SortingDirection.Ascending)
            {
                return items.ThenBy(keySelector);
            }

            return items.ThenByDescending(keySelector);
        }
    }

    /// <summary>
    /// Represents the direction of sorting for a column.
    /// </summary>
    public enum SortingDirection
    {
        Ascending,
        Descending
    }

    public class SearchData
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class CustomerSearch
    {
        public List<SearchData> SearchDatas { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
    }
}
