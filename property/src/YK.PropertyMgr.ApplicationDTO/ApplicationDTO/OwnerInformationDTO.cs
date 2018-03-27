using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
   public class OwnerInformationDTO
    {
        /// <summary>
        /// 房屋编号
        /// </summary>
        public string DoorNo { get; set; }
        /// <summary>
        /// 业主姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 业主手机号
        /// </summary>
        public string BindingPhonerNumber { get; set; }
    }
}
