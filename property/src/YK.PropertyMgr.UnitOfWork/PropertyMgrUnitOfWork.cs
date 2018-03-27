using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using KW.Sprite.Common.Repository;
using YK.PropertyMgr.RepositoryContract;
using YK.PropertyMgr.Repository;

namespace YK.PropertyMgr.UnitOfWork
{
    public partial class PropertyMgrUnitOfWork : UnitOfWorkWithEntityFramework, IPropertyMgrUnitOfWork, IDisposable
    {
        private bool m_Disposed;
        private PropertyMgrDataBaseContext m_DbContext;
        private Lazy<ICommunityConfigRepository> _ICommunityConfigRepository;
        public ICommunityConfigRepository CommunityConfigRepository
        {
            get
            {
                if (_ICommunityConfigRepository == null)
                {
                    _ICommunityConfigRepository = new Lazy<ICommunityConfigRepository>(() => new CommunityConfigRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ICommunityConfigRepository.Value;
            }
        }
        private Lazy<IPrepayAccountLogRepository> _IPrepayAccountLogRepository;
        public IPrepayAccountLogRepository PrepayAccountLogRepository
        {
            get
            {
                if (_IPrepayAccountLogRepository == null)
                {
                    _IPrepayAccountLogRepository = new Lazy<IPrepayAccountLogRepository>(() => new PrepayAccountLogRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IPrepayAccountLogRepository.Value;
            }
        }
        private Lazy<INotificeConfigRepository> _INotificeConfigRepository;
        public INotificeConfigRepository NotificeConfigRepository
        {
            get
            {
                if (_INotificeConfigRepository == null)
                {
                    _INotificeConfigRepository = new Lazy<INotificeConfigRepository>(() => new NotificeConfigRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _INotificeConfigRepository.Value;
            }
        }
        private Lazy<IChargeSubjectRepository> _IChargeSubjectRepository;
        public IChargeSubjectRepository ChargeSubjectRepository
        {
            get
            {
                if (_IChargeSubjectRepository == null)
                {
                    _IChargeSubjectRepository = new Lazy<IChargeSubjectRepository>(() => new ChargeSubjectRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IChargeSubjectRepository.Value;
            }
        }
        private Lazy<ISubjectHouseRefRepository> _ISubjectHouseRefRepository;
        public ISubjectHouseRefRepository SubjectHouseRefRepository
        {
            get
            {
                if (_ISubjectHouseRefRepository == null)
                {
                    _ISubjectHouseRefRepository = new Lazy<ISubjectHouseRefRepository>(() => new SubjectHouseRefRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISubjectHouseRefRepository.Value;
            }
        }
        private Lazy<IChargBillRepository> _IChargBillRepository;
        public IChargBillRepository ChargBillRepository
        {
            get
            {
                if (_IChargBillRepository == null)
                {
                    _IChargBillRepository = new Lazy<IChargBillRepository>(() => new ChargBillRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IChargBillRepository.Value;
            }
        }
        private Lazy<IChargeSubjectSnaRepository> _IChargeSubjectSnaRepository;
        public IChargeSubjectSnaRepository ChargeSubjectSnaRepository
        {
            get
            {
                if (_IChargeSubjectSnaRepository == null)
                {
                    _IChargeSubjectSnaRepository = new Lazy<IChargeSubjectSnaRepository>(() => new ChargeSubjectSnaRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IChargeSubjectSnaRepository.Value;
            }
        }
        private Lazy<IChargeRecordRepository> _IChargeRecordRepository;
        public IChargeRecordRepository ChargeRecordRepository
        {
            get
            {
                if (_IChargeRecordRepository == null)
                {
                    _IChargeRecordRepository = new Lazy<IChargeRecordRepository>(() => new ChargeRecordRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IChargeRecordRepository.Value;
            }
        }
        private Lazy<IPrepayAccountRepository> _IPrepayAccountRepository;
        public IPrepayAccountRepository PrepayAccountRepository
        {
            get
            {
                if (_IPrepayAccountRepository == null)
                {
                    _IPrepayAccountRepository = new Lazy<IPrepayAccountRepository>(() => new PrepayAccountRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IPrepayAccountRepository.Value;
            }
        }
        private Lazy<IPrepayAccountDetailRepository> _IPrepayAccountDetailRepository;
        public IPrepayAccountDetailRepository PrepayAccountDetailRepository
        {
            get
            {
                if (_IPrepayAccountDetailRepository == null)
                {
                    _IPrepayAccountDetailRepository = new Lazy<IPrepayAccountDetailRepository>(() => new PrepayAccountDetailRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IPrepayAccountDetailRepository.Value;
            }
        }
        private Lazy<IPreferentialRecordRepository> _IPreferentialRecordRepository;
        public IPreferentialRecordRepository PreferentialRecordRepository
        {
            get
            {
                if (_IPreferentialRecordRepository == null)
                {
                    _IPreferentialRecordRepository = new Lazy<IPreferentialRecordRepository>(() => new PreferentialRecordRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IPreferentialRecordRepository.Value;
            }
        }
        private Lazy<IPaymentTasksRepository> _IPaymentTasksRepository;
        public IPaymentTasksRepository PaymentTasksRepository
        {
            get
            {
                if (_IPaymentTasksRepository == null)
                {
                    _IPaymentTasksRepository = new Lazy<IPaymentTasksRepository>(() => new PaymentTasksRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IPaymentTasksRepository.Value;
            }
        }
        private Lazy<IPaymentTaskDetailRepository> _IPaymentTaskDetailRepository;
        public IPaymentTaskDetailRepository PaymentTaskDetailRepository
        {
            get
            {
                if (_IPaymentTaskDetailRepository == null)
                {
                    _IPaymentTaskDetailRepository = new Lazy<IPaymentTaskDetailRepository>(() => new PaymentTaskDetailRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IPaymentTaskDetailRepository.Value;
            }
        }
        private Lazy<IMeterRepository> _IMeterRepository;
        public IMeterRepository MeterRepository
        {
            get
            {
                if (_IMeterRepository == null)
                {
                    _IMeterRepository = new Lazy<IMeterRepository>(() => new MeterRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IMeterRepository.Value;
            }
        }
        private Lazy<IMeterReadRecordRepository> _IMeterReadRecordRepository;
        public IMeterReadRecordRepository MeterReadRecordRepository
        {
            get
            {
                if (_IMeterReadRecordRepository == null)
                {
                    _IMeterReadRecordRepository = new Lazy<IMeterReadRecordRepository>(() => new MeterReadRecordRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IMeterReadRecordRepository.Value;
            }
        }
        private Lazy<IReceiptRepository> _IReceiptRepository;
        public IReceiptRepository ReceiptRepository
        {
            get
            {
                if (_IReceiptRepository == null)
                {
                    _IReceiptRepository = new Lazy<IReceiptRepository>(() => new ReceiptRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IReceiptRepository.Value;
            }
        }
        private Lazy<IRefundRecordRepository> _IRefundRecordRepository;
        public IRefundRecordRepository RefundRecordRepository
        {
            get
            {
                if (_IRefundRecordRepository == null)
                {
                    _IRefundRecordRepository = new Lazy<IRefundRecordRepository>(() => new RefundRecordRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IRefundRecordRepository.Value;
            }
        }
        private Lazy<IChargeBillRecordMatchingRepository> _IChargeBillRecordMatchingRepository;
        public IChargeBillRecordMatchingRepository ChargeBillRecordMatchingRepository
        {
            get
            {
                if (_IChargeBillRecordMatchingRepository == null)
                {
                    _IChargeBillRecordMatchingRepository = new Lazy<IChargeBillRecordMatchingRepository>(() => new ChargeBillRecordMatchingRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IChargeBillRecordMatchingRepository.Value;
            }
        }
        private Lazy<IEntranceRepository> _IEntranceRepository;
        public IEntranceRepository EntranceRepository
        {
            get
            {
                if (_IEntranceRepository == null)
                {
                    _IEntranceRepository = new Lazy<IEntranceRepository>(() => new EntranceRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IEntranceRepository.Value;
            }
        }
        private Lazy<IEntranceLogRepository> _IEntranceLogRepository;
        public IEntranceLogRepository EntranceLogRepository
        {
            get
            {
                if (_IEntranceLogRepository == null)
                {
                    _IEntranceLogRepository = new Lazy<IEntranceLogRepository>(() => new EntranceLogRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IEntranceLogRepository.Value;
            }
        }
        private Lazy<IEntranceUserRepository> _IEntranceUserRepository;
        public IEntranceUserRepository EntranceUserRepository
        {
            get
            {
                if (_IEntranceUserRepository == null)
                {
                    _IEntranceUserRepository = new Lazy<IEntranceUserRepository>(() => new EntranceUserRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IEntranceUserRepository.Value;
            }
        }
        private Lazy<IEntranceUserDetailRepository> _IEntranceUserDetailRepository;
        public IEntranceUserDetailRepository EntranceUserDetailRepository
        {
            get
            {
                if (_IEntranceUserDetailRepository == null)
                {
                    _IEntranceUserDetailRepository = new Lazy<IEntranceUserDetailRepository>(() => new EntranceUserDetailRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IEntranceUserDetailRepository.Value;
            }
        }
        private Lazy<ICityRepository> _ICityRepository;
        public ICityRepository CityRepository
        {
            get
            {
                if (_ICityRepository == null)
                {
                    _ICityRepository = new Lazy<ICityRepository>(() => new CityRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ICityRepository.Value;
            }
        }
        private Lazy<ICountyRepository> _ICountyRepository;
        public ICountyRepository CountyRepository
        {
            get
            {
                if (_ICountyRepository == null)
                {
                    _ICountyRepository = new Lazy<ICountyRepository>(() => new CountyRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ICountyRepository.Value;
            }
        }
        private Lazy<IProvinceRepository> _IProvinceRepository;
        public IProvinceRepository ProvinceRepository
        {
            get
            {
                if (_IProvinceRepository == null)
                {
                    _IProvinceRepository = new Lazy<IProvinceRepository>(() => new ProvinceRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IProvinceRepository.Value;
            }
        }
        private Lazy<IShareKeyRepository> _IShareKeyRepository;
        public IShareKeyRepository ShareKeyRepository
        {
            get
            {
                if (_IShareKeyRepository == null)
                {
                    _IShareKeyRepository = new Lazy<IShareKeyRepository>(() => new ShareKeyRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IShareKeyRepository.Value;
            }
        }
        private Lazy<ITicketSerialNumberRepository> _ITicketSerialNumberRepository;
        public ITicketSerialNumberRepository TicketSerialNumberRepository
        {
            get
            {
                if (_ITicketSerialNumberRepository == null)
                {
                    _ITicketSerialNumberRepository = new Lazy<ITicketSerialNumberRepository>(() => new TicketSerialNumberRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ITicketSerialNumberRepository.Value;
            }
        }
        private Lazy<IPaymentDiscountInfoRepository> _IPaymentDiscountInfoRepository;
        public IPaymentDiscountInfoRepository PaymentDiscountInfoRepository
        {
            get
            {
                if (_IPaymentDiscountInfoRepository == null)
                {
                    _IPaymentDiscountInfoRepository = new Lazy<IPaymentDiscountInfoRepository>(() => new PaymentDiscountInfoRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IPaymentDiscountInfoRepository.Value;
            }
        }
        private Lazy<ITemplatePrintRecordRepository> _ITemplatePrintRecordRepository;
        public ITemplatePrintRecordRepository TemplatePrintRecordRepository
        {
            get
            {
                if (_ITemplatePrintRecordRepository == null)
                {
                    _ITemplatePrintRecordRepository = new Lazy<ITemplatePrintRecordRepository>(() => new TemplatePrintRecordRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ITemplatePrintRecordRepository.Value;
            }
        }
        private Lazy<ITemplatePrintRecordDetailRepository> _ITemplatePrintRecordDetailRepository;
        public ITemplatePrintRecordDetailRepository TemplatePrintRecordDetailRepository
        {
            get
            {
                if (_ITemplatePrintRecordDetailRepository == null)
                {
                    _ITemplatePrintRecordDetailRepository = new Lazy<ITemplatePrintRecordDetailRepository>(() => new TemplatePrintRecordDetailRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ITemplatePrintRecordDetailRepository.Value;
            }
        }
        private Lazy<IReceiptBookRepository> _IReceiptBookRepository;
        public IReceiptBookRepository ReceiptBookRepository
        {
            get
            {
                if (_IReceiptBookRepository == null)
                {
                    _IReceiptBookRepository = new Lazy<IReceiptBookRepository>(() => new ReceiptBookRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IReceiptBookRepository.Value;
            }
        }
        private Lazy<IReceiptBookDetailRepository> _IReceiptBookDetailRepository;
        public IReceiptBookDetailRepository ReceiptBookDetailRepository
        {
            get
            {
                if (_IReceiptBookDetailRepository == null)
                {
                    _IReceiptBookDetailRepository = new Lazy<IReceiptBookDetailRepository>(() => new ReceiptBookDetailRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IReceiptBookDetailRepository.Value;
            }
        }
        private Lazy<IReceiptBookHistoryRepository> _IReceiptBookHistoryRepository;
        public IReceiptBookHistoryRepository ReceiptBookHistoryRepository
        {
            get
            {
                if (_IReceiptBookHistoryRepository == null)
                {
                    _IReceiptBookHistoryRepository = new Lazy<IReceiptBookHistoryRepository>(() => new ReceiptBookHistoryRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IReceiptBookHistoryRepository.Value;
            }
        }
        private Lazy<IClientPaymentLogRepository> _IClientPaymentLogRepository;
        public IClientPaymentLogRepository ClientPaymentLogRepository
        {
            get
            {
                if (_IClientPaymentLogRepository == null)
                {
                    _IClientPaymentLogRepository = new Lazy<IClientPaymentLogRepository>(() => new ClientPaymentLogRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IClientPaymentLogRepository.Value;
            }
        }
        private Lazy<IAlipayApiRequestLogRepository> _IAlipayApiRequestLogRepository;
        public IAlipayApiRequestLogRepository AlipayApiRequestLogRepository
        {
            get
            {
                if (_IAlipayApiRequestLogRepository == null)
                {
                    _IAlipayApiRequestLogRepository = new Lazy<IAlipayApiRequestLogRepository>(() => new AlipayApiRequestLogRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IAlipayApiRequestLogRepository.Value;
            }
        }
        private Lazy<IAlipayCommunityRepository> _IAlipayCommunityRepository;
        public IAlipayCommunityRepository AlipayCommunityRepository
        {
            get
            {
                if (_IAlipayCommunityRepository == null)
                {
                    _IAlipayCommunityRepository = new Lazy<IAlipayCommunityRepository>(() => new AlipayCommunityRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IAlipayCommunityRepository.Value;
            }
        }
        private Lazy<IAlipayAPPAuthTokenRepository> _IAlipayAPPAuthTokenRepository;
        public IAlipayAPPAuthTokenRepository AlipayAPPAuthTokenRepository
        {
            get
            {
                if (_IAlipayAPPAuthTokenRepository == null)
                {
                    _IAlipayAPPAuthTokenRepository = new Lazy<IAlipayAPPAuthTokenRepository>(() => new AlipayAPPAuthTokenRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IAlipayAPPAuthTokenRepository.Value;
            }
        }
        private Lazy<IAlipayRoomRepository> _IAlipayRoomRepository;
        public IAlipayRoomRepository AlipayRoomRepository
        {
            get
            {
                if (_IAlipayRoomRepository == null)
                {
                    _IAlipayRoomRepository = new Lazy<IAlipayRoomRepository>(() => new AlipayRoomRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IAlipayRoomRepository.Value;
            }
        }
        private Lazy<IAlipayChargeBillRepository> _IAlipayChargeBillRepository;
        public IAlipayChargeBillRepository AlipayChargeBillRepository
        {
            get
            {
                if (_IAlipayChargeBillRepository == null)
                {
                    _IAlipayChargeBillRepository = new Lazy<IAlipayChargeBillRepository>(() => new AlipayChargeBillRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IAlipayChargeBillRepository.Value;
            }
        }
        private Lazy<IAlipayChargeBillSynchronizerRepository> _IAlipayChargeBillSynchronizerRepository;
        public IAlipayChargeBillSynchronizerRepository AlipayChargeBillSynchronizerRepository
        {
            get
            {
                if (_IAlipayChargeBillSynchronizerRepository == null)
                {
                    _IAlipayChargeBillSynchronizerRepository = new Lazy<IAlipayChargeBillSynchronizerRepository>(() => new AlipayChargeBillSynchronizerRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IAlipayChargeBillSynchronizerRepository.Value;
            }
        }
        private Lazy<IAlipayChargeBillSynchronizerDetailRepository> _IAlipayChargeBillSynchronizerDetailRepository;
        public IAlipayChargeBillSynchronizerDetailRepository AlipayChargeBillSynchronizerDetailRepository
        {
            get
            {
                if (_IAlipayChargeBillSynchronizerDetailRepository == null)
                {
                    _IAlipayChargeBillSynchronizerDetailRepository = new Lazy<IAlipayChargeBillSynchronizerDetailRepository>(() => new AlipayChargeBillSynchronizerDetailRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IAlipayChargeBillSynchronizerDetailRepository.Value;
            }
        }

        public PropertyMgrUnitOfWork()
        {
            m_DbContext = new PropertyMgrDataBaseContext();
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return m_DbContext.Database.ExecuteSqlCommand(sql, parameters);
        }

        public int ExecuteSqlCommand(TransactionalBehavior transactionalBehavior, string sql, params object[] parameters)
        {
            return m_DbContext.Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);
        }

        protected override DbContext InnerDbContext
        {
            get
            {
                return m_DbContext;
            }
        }       

        protected override void AbortCommit(DbUpdateConcurrencyException ex)
        {
        }

        protected override void ResolveIfStoreDeleted(DbEntityEntry failedEntry)
        {
        }

        protected override void ResolveIfStoreModified(DbEntityEntry failedEntry)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (m_Disposed)
            {
                return;
            }

            if (disposing)
            {
                m_DbContext.Dispose();
            }

            m_Disposed = true;
        }

        #region Private Method

        public void AddRelation(string strTableName, string strKey1Name, string strKey2Name, object strKey1Value, object strKey2Value)
        {
            string sqlTemplate = "INSERT INTO [{0}]({1},{2}) VALUES({3},{4})";
            ExecuteSqlCommand(string.Format(sqlTemplate, strTableName, strKey1Name, strKey2Name, strKey1Value, strKey2Value));
        }

        #endregion
    }
}
