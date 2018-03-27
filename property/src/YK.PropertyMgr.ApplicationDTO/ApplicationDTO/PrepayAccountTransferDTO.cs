using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class PrepayAccountTransferDTO
    {
        public int? Id { get; set; }

        public int? ChargeSubjectID { get; set; }

        public string ChargeSubjectName { get; set; }

        public decimal? Balance { get; set; }

        public bool IsTransfer
        {
            get
            {
                if (ChargeSubjectID == 0 || Balance == 0)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
