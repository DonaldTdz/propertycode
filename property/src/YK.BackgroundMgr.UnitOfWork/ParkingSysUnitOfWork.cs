using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using KW.Sprite.Common.Repository;
using YK.ParkingSys.RepositoryContract;
using YK.ParkingSys.Repository;

namespace YK.ParkingSys.UnitOfWork
{
    public partial class ParkingSysUnitOfWork : UnitOfWorkWithEntityFramework, IParkingSysUnitOfWork, IDisposable
    {
        private bool m_Disposed;
        private ParkingSysDataBaseContext m_DbContext;
        private Lazy<IParkingRepository> _IParkingRepository;
        public IParkingRepository ParkingRepository
        {
            get
            {
                if (_IParkingRepository == null)
                {
                    _IParkingRepository = new Lazy<IParkingRepository>(() => new ParkingRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IParkingRepository.Value;
            }
        }
        private Lazy<ICarportRepository> _ICarportRepository;
        public ICarportRepository CarportRepository
        {
            get
            {
                if (_ICarportRepository == null)
                {
                    _ICarportRepository = new Lazy<ICarportRepository>(() => new CarportRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ICarportRepository.Value;
            }
        }

        public ParkingSysUnitOfWork()
        {
            m_DbContext = new ParkingSysDataBaseContext();
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
