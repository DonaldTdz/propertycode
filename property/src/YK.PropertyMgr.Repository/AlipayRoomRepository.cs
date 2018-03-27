using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.Repository
{
    public partial class AlipayRoomRepository: PropertyMgrRepository<AlipayRoom>, IAlipayRoomRepository
    {
        public AlipayRoomRepository(PropertyMgrDataBaseContext context)
            : base(context)
        {
        }

        public override string TableName
        {
            get
            {
                return "AlipayRooms";
            }
        }
    }
}

