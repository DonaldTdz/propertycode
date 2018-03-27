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
        private Lazy<ISms_LogRepository> _ISms_LogRepository;
        public ISms_LogRepository Sms_LogRepository
        {
            get
            {
                if (_ISms_LogRepository == null)
                {
                    _ISms_LogRepository = new Lazy<ISms_LogRepository>(() => new Sms_LogRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISms_LogRepository.Value;
            }
        }

        private Lazy<ISms_IdentifyingCodeRepository> _ISms_IdentifyingCodeRepository;
        public ISms_IdentifyingCodeRepository Sms_IdentifyingCodeRepository
        {
            get
            {
                if (_ISms_IdentifyingCodeRepository == null)
                {
                    _ISms_IdentifyingCodeRepository = new Lazy<ISms_IdentifyingCodeRepository>(() => new Sms_IdentifyingCodeRepository(m_DbContext), LazyThreadSafetyMode.ExecutionAndPublication);
                }
                return _ISms_IdentifyingCodeRepository.Value;
            }
        }
    }
}
