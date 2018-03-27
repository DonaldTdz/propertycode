using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class AlipayRoomMappers
	{
		public static AlipayRoom ChangeDTOToAlipayRoomNew(AlipayRoomDTO dtoAlipayRoom)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayRoomDTO, AlipayRoom>();
            });
            var domainAlipayRoom = config.CreateMapper().Map<AlipayRoomDTO, AlipayRoom>(dtoAlipayRoom);

            return domainAlipayRoom;
        }

		public static void ChangeDTOToAlipayRoomUpdate(AlipayRoomDTO dtoAlipayRoom, AlipayRoom domainAlipayRoom)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayRoomDTO, AlipayRoom>();
            });
            config.CreateMapper().Map<AlipayRoomDTO, AlipayRoom>(dtoAlipayRoom, domainAlipayRoom);
        }

		public static void ChangeAlipayRoomToDTO(AlipayRoomDTO dtoAlipayRoom, AlipayRoom domainAlipayRoom)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayRoom, AlipayRoomDTO>();
            });
            config.CreateMapper().Map<AlipayRoom, AlipayRoomDTO>(domainAlipayRoom, dtoAlipayRoom);
        }

		public static AlipayRoomDTO ChangeAlipayRoomToDTO(AlipayRoom domainAlipayRoom)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayRoom, AlipayRoomDTO>();
            });
            return config.CreateMapper().Map<AlipayRoom, AlipayRoomDTO>(domainAlipayRoom);
        }

		public static List<AlipayRoomDTO> ChangeAlipayRoomToDTOs(List<AlipayRoom> domainAlipayRoom)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayRoom, AlipayRoomDTO>();
            });
            var dtoAlipayRoom = config.CreateMapper().Map<List<AlipayRoom>, List<AlipayRoomDTO>>(domainAlipayRoom);

            return dtoAlipayRoom;
        }

		public static IEnumerable<AlipayRoomDTO> ChangeAlipayRoomToDTOs(IEnumerable<AlipayRoom> domainAlipayRooms)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlipayRoom, AlipayRoomDTO>();
            });
            var dtoAlipayRoom = config.CreateMapper().Map<IEnumerable<AlipayRoom>, IEnumerable<AlipayRoomDTO>>(domainAlipayRooms);

            return dtoAlipayRoom;
        }
	}
}
