using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;

namespace YK.PropertyMgr.CompositeDomainService
{
    public interface IReportService
    {

        /// <summary>
        /// 应收报表-科目
        /// </summary>
        /// <returns></returns>
        ResultModel ArrearsChargeSubjectReport(int PageIndex, int PageSize, out int totalCount, string queryStr);

        /// <summary>
        /// 应缴报表-小区
        /// </summary>
        /// <returns></returns>
        ResultModel ArrearsCommunityReport(int PageIndex, int PageSize, out int totalCount, string queryStr);

        /// <summary>
        /// 应收明细报表
        /// </summary>
        /// <returns></returns>
        ResultModel ArrearsDetailReport(int PageIndex, int PageSize, out int totalCount, string queryStr);



        /// <summary>
        /// 应收报表-科目
        /// </summary>
        /// <returns></returns>
        ResultModel CollectionsChargeSubjectReport(int PageIndex, int PageSize, out int totalCount, string queryStr);


        /// <summary>
        /// 应收报表-小区
        /// </summary>
        /// <returns></returns>
        ResultModel CollectionsCommunityReport(int PageIndex, int PageSize, out int totalCount, string queryStr);


        /// <summary>
        /// 应收明细报表
        /// </summary>
        /// <returns></returns>
        ResultModel CollectionsDetailReport(int PageIndex, int PageSize, out int totalCount, string queryStr);

        /// <summary>
        /// 实收报表统计金额
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        decimal GetCollectionsMoney(Expression<Func<ChargeRecord, bool>> predicate);

        /// <summary>
        /// 未收/实收对比表
        /// </summary>
        /// <returns></returns>
        ResultModel GetArrearsCollComparisoCharts(int ComDeptId);


        List<ReportArrearsDetailDTO> GetArrearsReportDetailList(int PageIndex, int PageSize, ReportArrearsSearchDTO search, out int totalCount, bool IsExport );


    //    void GetArrearsReportDataList(int startRowIndex, int PageSize, ReportArrearsSearchDTO search, string OwnerName, out int totalCount);


    }
}
