using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.Repository
{
    public partial class PaymentDiscountInfoRepository: PropertyMgrRepository<PaymentDiscountInfo>, IPaymentDiscountInfoRepository
    {
        public PaymentDiscountInfoRepository(PropertyMgrDataBaseContext context)
            : base(context)
        {
        }

        public override string TableName
        {
            get
            {
                return "PaymentDiscountInfo";
            }
        }
    }
}

