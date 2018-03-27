using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;

namespace YK.PropertyMgr.CompositeDomainService.GenerateBillService.BillServices
{
    public abstract class AbstractBillService : IBillService , IDisposable
    {
        private IPropertyMgrUnitOfWork PropertyMgrUnitOfWork;
        private ChargeSubject ChargeSubject;

        #region 实现接口方法

        public AbstractBillService(IPropertyMgrUnitOfWork _IPropertyMgrUnitOfWork, ChargeSubject _ChargeSubject)
        {
            this.PropertyMgrUnitOfWork = _IPropertyMgrUnitOfWork;
            this.ChargeSubject = _ChargeSubject;
        }
        /// <summary>
        /// 账单类型
        /// </summary>
        public SubjectTypeEnum SubjectType
        {
            get
            {
                return (SubjectTypeEnum)ChargeSubject.SubjectType;
            }
        }

        /// <summary>
        /// 生成账单
        /// </summary>
        public abstract void GenerateBills();

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            this.PropertyMgrUnitOfWork.Dispose();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取收费对象和资源信息
        /// </summary>
        /// <returns></returns>
        public virtual void GetChargeSubjectResources()
        {
           
        }

        #endregion

        #region 私有方法


        #endregion
    }
}
