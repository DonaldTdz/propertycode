using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.RepositoryContract
{
    /// <summary>
    /// 通过UnitOfWork对数据访问进行统一管理
    /// </summary>
    public partial interface IPropertyMgrUnitOfWork : IUnitOfWork, IDisposable
    {
        #region Repository
        ICommunityConfigRepository CommunityConfigRepository { get; }
        IPrepayAccountLogRepository PrepayAccountLogRepository { get; }
        INotificeConfigRepository NotificeConfigRepository { get; }
        IChargeSubjectRepository ChargeSubjectRepository { get; }
        ISubjectHouseRefRepository SubjectHouseRefRepository { get; }
        IChargBillRepository ChargBillRepository { get; }
        IChargeSubjectSnaRepository ChargeSubjectSnaRepository { get; }
        IChargeRecordRepository ChargeRecordRepository { get; }
        IPrepayAccountRepository PrepayAccountRepository { get; }
        IPrepayAccountDetailRepository PrepayAccountDetailRepository { get; }
        IPreferentialRecordRepository PreferentialRecordRepository { get; }
        IPaymentTasksRepository PaymentTasksRepository { get; }
        IPaymentTaskDetailRepository PaymentTaskDetailRepository { get; }
        IMeterRepository MeterRepository { get; }
        IMeterReadRecordRepository MeterReadRecordRepository { get; }
        IReceiptRepository ReceiptRepository { get; }
        IRefundRecordRepository RefundRecordRepository { get; }
        IChargeBillRecordMatchingRepository ChargeBillRecordMatchingRepository { get; }
        IEntranceRepository EntranceRepository { get; }
        IEntranceLogRepository EntranceLogRepository { get; }
        IEntranceUserRepository EntranceUserRepository { get; }
        IEntranceUserDetailRepository EntranceUserDetailRepository { get; }
        ICityRepository CityRepository { get; }
        ICountyRepository CountyRepository { get; }
        IProvinceRepository ProvinceRepository { get; }
        IShareKeyRepository ShareKeyRepository { get; }
        ITicketSerialNumberRepository TicketSerialNumberRepository { get; }
        IPaymentDiscountInfoRepository PaymentDiscountInfoRepository { get; }
        ITemplatePrintRecordRepository TemplatePrintRecordRepository { get; }
        ITemplatePrintRecordDetailRepository TemplatePrintRecordDetailRepository { get; }
        IReceiptBookRepository ReceiptBookRepository { get; }
        IReceiptBookDetailRepository ReceiptBookDetailRepository { get; }
        IReceiptBookHistoryRepository ReceiptBookHistoryRepository { get; }
        IClientPaymentLogRepository ClientPaymentLogRepository { get; }
        IAlipayApiRequestLogRepository AlipayApiRequestLogRepository { get; }
        IAlipayCommunityRepository AlipayCommunityRepository { get; }
        IAlipayAPPAuthTokenRepository AlipayAPPAuthTokenRepository { get; }
        IAlipayRoomRepository AlipayRoomRepository { get; }
        IAlipayChargeBillRepository AlipayChargeBillRepository { get; }
        IAlipayChargeBillSynchronizerRepository AlipayChargeBillSynchronizerRepository { get; }
        IAlipayChargeBillSynchronizerDetailRepository AlipayChargeBillSynchronizerDetailRepository { get; }
        #endregion

        /// <summary>
        /// 直接执行Sql语句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>执行结果</returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);

        /// <summary>
        /// 直接执行Sql语句
        /// </summary>
        /// <param name="transactionalBehavior">是否封装在存储过程中</param>
        /// <param name="sql">Sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>执行结果</returns>
        int ExecuteSqlCommand(TransactionalBehavior transactionalBehavior, string sql, params object[] parameters);

		void AddRelation(string strTableName, string strKey1Name, string strKey2Name, object strKey1Value, object strKey2Value);
    }
}
