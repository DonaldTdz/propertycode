using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.CompositeDomainService.Model
{
    public class UserCouponpModel
    {
        public int UserCouponpId { get; set; }

        public string CouponName { get; set; }

        public double CouponsMoney { get; set; }

        public double PriceLimit { get; set; }

        public string UseStartDate { get; set; }

        public string UseEndDate { get; set; }
    }
}
