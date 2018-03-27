using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class EntranceUserMappers
	{
		public static EntranceUser ChangeDTOToEntranceUserNew(EntranceUserDTO dtoEntranceUser)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceUserDTO, EntranceUser>();
            });
            var domainEntranceUser = config.CreateMapper().Map<EntranceUserDTO, EntranceUser>(dtoEntranceUser);

            return domainEntranceUser;
        }

		public static void ChangeDTOToEntranceUserUpdate(EntranceUserDTO dtoEntranceUser, EntranceUser domainEntranceUser)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceUserDTO, EntranceUser>();
            });
            config.CreateMapper().Map<EntranceUserDTO, EntranceUser>(dtoEntranceUser, domainEntranceUser);
        }

		public static void ChangeEntranceUserToDTO(EntranceUserDTO dtoEntranceUser, EntranceUser domainEntranceUser)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceUser, EntranceUserDTO>();
            });
            config.CreateMapper().Map<EntranceUser, EntranceUserDTO>(domainEntranceUser, dtoEntranceUser);
        }

		public static EntranceUserDTO ChangeEntranceUserToDTO(EntranceUser domainEntranceUser)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceUser, EntranceUserDTO>();
            });
            return config.CreateMapper().Map<EntranceUser, EntranceUserDTO>(domainEntranceUser);
        }

		public static List<EntranceUserDTO> ChangeEntranceUserToDTOs(List<EntranceUser> domainEntranceUser)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceUser, EntranceUserDTO>();
            });
            var dtoEntranceUser = config.CreateMapper().Map<List<EntranceUser>, List<EntranceUserDTO>>(domainEntranceUser);

            return dtoEntranceUser;
        }

		public static IEnumerable<EntranceUserDTO> ChangeEntranceUserToDTOs(IEnumerable<EntranceUser> domainEntranceUsers)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EntranceUser, EntranceUserDTO>();
            });
            var dtoEntranceUser = config.CreateMapper().Map<IEnumerable<EntranceUser>, IEnumerable<EntranceUserDTO>>(domainEntranceUsers);

            return dtoEntranceUser;
        }
	}
}
