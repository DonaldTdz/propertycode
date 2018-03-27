using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;

namespace YK.PropertyMgr.CompositeDomainService
{
   public interface IAlipayService
    {
        List<ChargBillDTO> GetAlipayUpLoadChargeBillList(Expression<Func<ChargBill, bool>> predicate_ChargeBill, Expression<Func<AlipayRoom, bool>> predicate_AlipayRoom, Expression<Func<AlipayChargeBill, bool>> predicate_AlipayChargeBill,int PageSize,int PageIndex,out int totalcount);
        ResultModel SaveUploadAlipayChargeBill(List<ChargBill> SaveChargBillList, string AlipayCommunityId, string AppAuthToken, int? ComDeptId, string OperatorId, string OperatorName);

        ResultModel DeleteAlipayChargeBill(List<int?> Ids, string AlipayCommunityId, string AppAuthToken);
    }
}
