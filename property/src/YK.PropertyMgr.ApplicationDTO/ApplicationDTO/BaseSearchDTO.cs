using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.PresentationService;

namespace YK.PropertyMgr.ApplicationDTO
{
    public class BaseSearchDTO
    {
        public BaseSearchDTO() 
        {
            PageSize = 10;
            PageStart = 0;
        }
        /// <summary>
        /// 排序表达式
        /// 例：filedName1 desc,filedName2 asc
        /// </summary>
        public string Sorting { get; set; }
        /// <summary>
        /// 每页显示记录条数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 开始条
        /// </summary>
        public int PageStart { get; set; }
        /// <summary>
        /// 第几页
        /// </summary>
        public int PageIndex 
        { 
            get 
            {
                return ((int)(PageStart / PageSize) + 1);
            } 
        }

        public int Draw { get; set; }

        /// <summary>
        /// 资源树DeptId
        /// </summary>
        public int DeptId { get; set; }

        /// <summary>
        /// 资源树名
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public EDeptType? DeptType { get; set; }
    }
}
