using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO.Enums;

namespace YK.PropertyMgr.ApplicationDTO
{
    public partial class PaymentDiscountInfoDTO
    {
        public PaymentDiscountInfoDTO()
        {
            this.DiscountType = 0;
        }

        public string ChargeRecordId { get; set; }

        public DiscountTypeEnum EDiscountType
        {
            get
            {
                return (DiscountTypeEnum)this.DiscountType;
            }
        }
    }
}
