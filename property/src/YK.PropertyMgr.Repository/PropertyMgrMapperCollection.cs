using KW.Sprite.Common.Repository;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.Repository
{
    public class PropertyMgrMapperCollection : IMapperCollection
    {
        public IEnumerable<IEntityMapper> Mappers { get; private set; }

        public PropertyMgrMapperCollection()
        {
            Mappers = new List<IEntityMapper>
            {
                new CommunityConfigMapper(),
                new PrepayAccountLogMapper(),
                new NotificeConfigMapper(),
                new ChargeSubjectMapper(),
                new SubjectHouseRefMapper(),
                new ChargBillMapper(),
                new ChargeSubjectSnaMapper(),
                new ChargeRecordMapper(),
                new PrepayAccountMapper(),
                new PrepayAccountDetailMapper(),
                new PreferentialRecordMapper(),
                new PaymentTasksMapper(),
                new PaymentTaskDetailMapper(),
                new MeterMapper(),
                new MeterReadRecordMapper(),
                new ReceiptMapper(),
                new RefundRecordMapper(),
                new ChargeBillRecordMatchingMapper(),
                new EntranceMapper(),
                new EntranceLogMapper(),
                new EntranceUserMapper(),
                new EntranceUserDetailMapper(),
                new CityMapper(),
                new CountyMapper(),
                new ProvinceMapper(),
                new ShareKeyMapper(),
                new TicketSerialNumberMapper(),
                new PaymentDiscountInfoMapper(),
                new TemplatePrintRecordMapper(),
                new TemplatePrintRecordDetailMapper(),
                new ReceiptBookMapper(),
                new ReceiptBookDetailMapper(),
                new ReceiptBookHistoryMapper(),
                new ClientPaymentLogMapper(),
                new AlipayApiRequestLogMapper(),
                new AlipayCommunityMapper(),
                new AlipayAPPAuthTokenMapper(),
                new AlipayRoomMapper(),
                new AlipayChargeBillMapper(),
                new AlipayChargeBillSynchronizerMapper(),
                new AlipayChargeBillSynchronizerDetailMapper(),
            };
        }
    }
}
