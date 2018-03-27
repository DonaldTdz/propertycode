using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.DomainService;

namespace YK.PropertyMgr.ApplicationService
{
	public partial class AlipayRoomAppService
	{
		private AlipayRoomDomainService _AlipayRoomDomainService;
        protected AlipayRoomDomainService AlipayRoomService
        {
            get
            {
                if (_AlipayRoomDomainService == null)
                {
                    _AlipayRoomDomainService = new AlipayRoomDomainService();
                }

                return _AlipayRoomDomainService;
            }
        }   

        public bool InsertAlipayRoom(AlipayRoomDTO dtoAlipayRoom)
        {
            var domainAlipayRoom = AlipayRoomMappers.ChangeDTOToAlipayRoomNew(dtoAlipayRoom);

            return AlipayRoomService.InsertAlipayRoom(domainAlipayRoom);
        }

        public bool UpdateAlipayRoom(AlipayRoomDTO dtoAlipayRoom)
        {
            var domainAlipayRoom = AlipayRoomMappers.ChangeDTOToAlipayRoomNew(dtoAlipayRoom);

            return AlipayRoomService.UpdateAlipayRoom(domainAlipayRoom);
        }

        public bool DeleteAlipayRoom(object id)
        {
            return AlipayRoomService.DeleteAlipayRoom(id);
        }

        public List<AlipayRoomDTO> GetAlipayRooms()
        {
            var domainAlipayRooms = AlipayRoomService.GetAlipayRooms();

            return AlipayRoomMappers.ChangeAlipayRoomToDTOs(domainAlipayRooms);
        }

		public AlipayRoomDTO GetAlipayRoomByKey(object id)
        {
            var domainAlipayRoom = AlipayRoomService.GetAlipayRoomByKey(id);

            return AlipayRoomMappers.ChangeAlipayRoomToDTO(domainAlipayRoom);
        }
	}
}
