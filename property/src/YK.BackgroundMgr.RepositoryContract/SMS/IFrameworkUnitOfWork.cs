using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.RepositoryContract
{
    /// <summary>
    /// 通过UnitOfWork对数据访问进行统一管理
    /// </summary>
    public partial interface IBackgroundMgrUnitOfWork : IUnitOfWork, IDisposable
    {
        #region Repository
        ISms_LogRepository Sms_LogRepository { get; }
        ISms_IdentifyingCodeRepository Sms_IdentifyingCodeRepository { get; }
        #endregion
    }
}
