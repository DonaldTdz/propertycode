using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;

namespace YK.PropertyMgr.ApplicationService
{
    public partial class MeterReadRecordAppService
    {
        public IList<MeterReadRecordDTO> GetMeterRecords(int meterId, DateTime beginDate, DateTime endDate, List<string> billIds)
        {
            //找到 账单里所有 对应得三表读数 fixed bug #5415 2017-08-29
            //return MeterReadRecordMappers.ChangeMeterReadRecordToDTOs(MeterReadRecordService.GetMeterRecords(o => o.MeterId == meterId && o.ReadDate >= beginDate && o.ReadDate <= endDate && billIds.Contains(o.BillID)).ToList());
            return MeterReadRecordMappers.ChangeMeterReadRecordToDTOs(MeterReadRecordService.GetMeterRecords(o => o.MeterId == meterId && o.ReadDate >= beginDate && o.ReadDate <= endDate && billIds.Any( b => o.BillID.Contains(b))).ToList());
        }
    }
}
