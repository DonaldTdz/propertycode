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
        private Lazy<ISEC_User_OwnerSEC_DeptRepository> _ISEC_User_OwnerSEC_DeptRepository;
        public ISEC_User_OwnerSEC_DeptRepository SEC_User_OwnerSEC_DeptRepository
        {
            get
            {
                if (_ISEC_User_OwnerSEC_DeptRepository == null)
                {
                    _ISEC_User_OwnerSEC_DeptRepository = new Lazy<ISEC_User_OwnerSEC_DeptRepository>(() => new SEC_User_OwnerSEC_DeptRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_User_OwnerSEC_DeptRepository.Value;
            }
        }

        private Lazy<ISEC_User_OwnerSEC_CarportRepository> _ISEC_User_OwnerSEC_CarportRepository;
        public ISEC_User_OwnerSEC_CarportRepository SEC_User_OwnerSEC_CarportRepository
        {
            get
            {
                if (_ISEC_User_OwnerSEC_CarportRepository == null)
                {
                    _ISEC_User_OwnerSEC_CarportRepository = new Lazy<ISEC_User_OwnerSEC_CarportRepository>(() => new SEC_User_OwnerSEC_CarportRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISEC_User_OwnerSEC_CarportRepository.Value;
            }
        }
    }
}
