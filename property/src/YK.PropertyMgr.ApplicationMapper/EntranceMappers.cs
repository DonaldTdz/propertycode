using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class EntranceMappers
	{
		public static Entrance ChangeDTOToEntranceNew(EntranceDTO dtoEntrance)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceDTO, Entrance>();
            });
            var domainEntrance = config.CreateMapper().Map<EntranceDTO, Entrance>(dtoEntrance);

            return domainEntrance;
        }

		public static void ChangeDTOToEntranceUpdate(EntranceDTO dtoEntrance, Entrance domainEntrance)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceDTO, Entrance>();
            });
            config.CreateMapper().Map<EntranceDTO, Entrance>(dtoEntrance, domainEntrance);
        }

		public static void ChangeEntranceToDTO(EntranceDTO dtoEntrance, Entrance domainEntrance)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Entrance, EntranceDTO>();
            });
            config.CreateMapper().Map<Entrance, EntranceDTO>(domainEntrance, dtoEntrance);
        }

		public static EntranceDTO ChangeEntranceToDTO(Entrance domainEntrance)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Entrance, EntranceDTO>();
            });
            return config.CreateMapper().Map<Entrance, EntranceDTO>(domainEntrance);
        }

		public static List<EntranceDTO> ChangeEntranceToDTOs(List<Entrance> domainEntrance)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Entrance, EntranceDTO>();
            });
            var dtoEntrance = config.CreateMapper().Map<List<Entrance>, List<EntranceDTO>>(domainEntrance);

            return dtoEntrance;
        }

		public static IEnumerable<EntranceDTO> ChangeEntranceToDTOs(IEnumerable<Entrance> domainEntrances)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Entrance, EntranceDTO>();
            });
            var dtoEntrance = config.CreateMapper().Map<IEnumerable<Entrance>, IEnumerable<EntranceDTO>>(domainEntrances);

            return dtoEntrance;
        }
	}
}
