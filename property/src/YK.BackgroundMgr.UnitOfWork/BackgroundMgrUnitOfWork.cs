using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using KW.Sprite.Common.Repository;
using YK.BackgroundMgr.RepositoryContract;
using YK.BackgroundMgr.Repository;

namespace YK.BackgroundMgr.UnitOfWork
{
    public partial class BackgroundMgrUnitOfWork : UnitOfWorkWithEntityFramework, IBackgroundMgrUnitOfWork, IDisposable
    {
        private bool m_Disposed;
        private BackgroundMgrDataBaseContext m_DbContext;
        private Lazy<ISEC_AdminUserRepository> _ISEC_AdminUserRepository;
        public ISEC_AdminUserRepository SEC_AdminUserRepository
        {
            get
            {
                if (_ISEC_AdminUserRepository == null)
                {
                    _ISEC_AdminUserRepository = new Lazy<ISEC_AdminUserRepository>(() => new SEC_AdminUserRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_AdminUserRepository.Value;
            }
        }
        private Lazy<ISEC_UserRepository> _ISEC_UserRepository;
        public ISEC_UserRepository SEC_UserRepository
        {
            get
            {
                if (_ISEC_UserRepository == null)
                {
                    _ISEC_UserRepository = new Lazy<ISEC_UserRepository>(() => new SEC_UserRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_UserRepository.Value;
            }
        }
        private Lazy<ISEC_DeptRepository> _ISEC_DeptRepository;
        public ISEC_DeptRepository SEC_DeptRepository
        {
            get
            {
                if (_ISEC_DeptRepository == null)
                {
                    _ISEC_DeptRepository = new Lazy<ISEC_DeptRepository>(() => new SEC_DeptRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_DeptRepository.Value;
            }
        }
        private Lazy<ISys_DictionaryRepository> _ISys_DictionaryRepository;
        public ISys_DictionaryRepository Sys_DictionaryRepository
        {
            get
            {
                if (_ISys_DictionaryRepository == null)
                {
                    _ISys_DictionaryRepository = new Lazy<ISys_DictionaryRepository>(() => new Sys_DictionaryRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISys_DictionaryRepository.Value;
            }
        }
        private Lazy<ISys_DictionaryItemRepository> _ISys_DictionaryItemRepository;
        public ISys_DictionaryItemRepository Sys_DictionaryItemRepository
        {
            get
            {
                if (_ISys_DictionaryItemRepository == null)
                {
                    _ISys_DictionaryItemRepository = new Lazy<ISys_DictionaryItemRepository>(() => new Sys_DictionaryItemRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISys_DictionaryItemRepository.Value;
            }
        }
        private Lazy<ISEC_RoleRepository> _ISEC_RoleRepository;
        public ISEC_RoleRepository SEC_RoleRepository
        {
            get
            {
                if (_ISEC_RoleRepository == null)
                {
                    _ISEC_RoleRepository = new Lazy<ISEC_RoleRepository>(() => new SEC_RoleRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_RoleRepository.Value;
            }
        }
        private Lazy<ISEC_FieldRepository> _ISEC_FieldRepository;
        public ISEC_FieldRepository SEC_FieldRepository
        {
            get
            {
                if (_ISEC_FieldRepository == null)
                {
                    _ISEC_FieldRepository = new Lazy<ISEC_FieldRepository>(() => new SEC_FieldRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_FieldRepository.Value;
            }
        }
        private Lazy<ISEC_OperateRepository> _ISEC_OperateRepository;
        public ISEC_OperateRepository SEC_OperateRepository
        {
            get
            {
                if (_ISEC_OperateRepository == null)
                {
                    _ISEC_OperateRepository = new Lazy<ISEC_OperateRepository>(() => new SEC_OperateRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_OperateRepository.Value;
            }
        }
        private Lazy<ISEC_ModuleRepository> _ISEC_ModuleRepository;
        public ISEC_ModuleRepository SEC_ModuleRepository
        {
            get
            {
                if (_ISEC_ModuleRepository == null)
                {
                    _ISEC_ModuleRepository = new Lazy<ISEC_ModuleRepository>(() => new SEC_ModuleRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_ModuleRepository.Value;
            }
        }
        private Lazy<ISEC_User_OwnerRepository> _ISEC_User_OwnerRepository;
        public ISEC_User_OwnerRepository SEC_User_OwnerRepository
        {
            get
            {
                if (_ISEC_User_OwnerRepository == null)
                {
                    _ISEC_User_OwnerRepository = new Lazy<ISEC_User_OwnerRepository>(() => new SEC_User_OwnerRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_User_OwnerRepository.Value;
            }
        }
        private Lazy<ISEC_CommunityRepository> _ISEC_CommunityRepository;
        public ISEC_CommunityRepository SEC_CommunityRepository
        {
            get
            {
                if (_ISEC_CommunityRepository == null)
                {
                    _ISEC_CommunityRepository = new Lazy<ISEC_CommunityRepository>(() => new SEC_CommunityRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_CommunityRepository.Value;
            }
        }
        private Lazy<ISEC_BuildingRepository> _ISEC_BuildingRepository;
        public ISEC_BuildingRepository SEC_BuildingRepository
        {
            get
            {
                if (_ISEC_BuildingRepository == null)
                {
                    _ISEC_BuildingRepository = new Lazy<ISEC_BuildingRepository>(() => new SEC_BuildingRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_BuildingRepository.Value;
            }
        }
        private Lazy<ISEC_HouseRepository> _ISEC_HouseRepository;
        public ISEC_HouseRepository SEC_HouseRepository
        {
            get
            {
                if (_ISEC_HouseRepository == null)
                {
                    _ISEC_HouseRepository = new Lazy<ISEC_HouseRepository>(() => new SEC_HouseRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_HouseRepository.Value;
            }
        }
        private Lazy<ISEC_CarportRepository> _ISEC_CarportRepository;
        public ISEC_CarportRepository SEC_CarportRepository
        {
            get
            {
                if (_ISEC_CarportRepository == null)
                {
                    _ISEC_CarportRepository = new Lazy<ISEC_CarportRepository>(() => new SEC_CarportRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_CarportRepository.Value;
            }
        }
        private Lazy<ISEC_WeChatPublicNumberRepository> _ISEC_WeChatPublicNumberRepository;
        public ISEC_WeChatPublicNumberRepository SEC_WeChatPublicNumberRepository
        {
            get
            {
                if (_ISEC_WeChatPublicNumberRepository == null)
                {
                    _ISEC_WeChatPublicNumberRepository = new Lazy<ISEC_WeChatPublicNumberRepository>(() => new SEC_WeChatPublicNumberRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_WeChatPublicNumberRepository.Value;
            }
        }
        private Lazy<ISEC_WeChatOpenIdRepository> _ISEC_WeChatOpenIdRepository;
        public ISEC_WeChatOpenIdRepository SEC_WeChatOpenIdRepository
        {
            get
            {
                if (_ISEC_WeChatOpenIdRepository == null)
                {
                    _ISEC_WeChatOpenIdRepository = new Lazy<ISEC_WeChatOpenIdRepository>(() => new SEC_WeChatOpenIdRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_WeChatOpenIdRepository.Value;
            }
        }
        private Lazy<ISEC_GarageRepository> _ISEC_GarageRepository;
        public ISEC_GarageRepository SEC_GarageRepository
        {
            get
            {
                if (_ISEC_GarageRepository == null)
                {
                    _ISEC_GarageRepository = new Lazy<ISEC_GarageRepository>(() => new SEC_GarageRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_GarageRepository.Value;
            }
        }
        private Lazy<ISEC_DeveloperRepository> _ISEC_DeveloperRepository;
        public ISEC_DeveloperRepository SEC_DeveloperRepository
        {
            get
            {
                if (_ISEC_DeveloperRepository == null)
                {
                    _ISEC_DeveloperRepository = new Lazy<ISEC_DeveloperRepository>(() => new SEC_DeveloperRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_DeveloperRepository.Value;
            }
        }
        private Lazy<ISEC_GatewayRepository> _ISEC_GatewayRepository;
        public ISEC_GatewayRepository SEC_GatewayRepository
        {
            get
            {
                if (_ISEC_GatewayRepository == null)
                {
                    _ISEC_GatewayRepository = new Lazy<ISEC_GatewayRepository>(() => new SEC_GatewayRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_GatewayRepository.Value;
            }
        }
        private Lazy<ISEC_GatewayAuthRepository> _ISEC_GatewayAuthRepository;
        public ISEC_GatewayAuthRepository SEC_GatewayAuthRepository
        {
            get
            {
                if (_ISEC_GatewayAuthRepository == null)
                {
                    _ISEC_GatewayAuthRepository = new Lazy<ISEC_GatewayAuthRepository>(() => new SEC_GatewayAuthRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_GatewayAuthRepository.Value;
            }
        }
        private Lazy<IOtherSysErrorEntityRepository> _IOtherSysErrorEntityRepository;
        public IOtherSysErrorEntityRepository OtherSysErrorEntityRepository
        {
            get
            {
                if (_IOtherSysErrorEntityRepository == null)
                {
                    _IOtherSysErrorEntityRepository = new Lazy<IOtherSysErrorEntityRepository>(() => new OtherSysErrorEntityRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _IOtherSysErrorEntityRepository.Value;
            }
        }
        private Lazy<ISEC_PropertyRepository> _ISEC_PropertyRepository;
        public ISEC_PropertyRepository SEC_PropertyRepository
        {
            get
            {
                if (_ISEC_PropertyRepository == null)
                {
                    _ISEC_PropertyRepository = new Lazy<ISEC_PropertyRepository>(() => new SEC_PropertyRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_PropertyRepository.Value;
            }
        }
        private Lazy<ISEC_AreaRepository> _ISEC_AreaRepository;
        public ISEC_AreaRepository SEC_AreaRepository
        {
            get
            {
                if (_ISEC_AreaRepository == null)
                {
                    _ISEC_AreaRepository = new Lazy<ISEC_AreaRepository>(() => new SEC_AreaRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_AreaRepository.Value;
            }
        }

        public BackgroundMgrUnitOfWork()
        {
            m_DbContext = new BackgroundMgrDataBaseContext();
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
