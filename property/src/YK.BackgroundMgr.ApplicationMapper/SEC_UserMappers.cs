using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class SEC_UserMappers
	{
		public static SEC_User ChangeDTOToSEC_UserNew(SEC_UserDTO dtoSEC_User)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_UserDTO, SEC_User>();
            });
            var domainSEC_User = config.CreateMapper().Map<SEC_UserDTO, SEC_User>(dtoSEC_User);

            return domainSEC_User;
        }

		public static void ChangeDTOToSEC_UserUpdate(SEC_UserDTO dtoSEC_User, SEC_User domainSEC_User)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_UserDTO, SEC_User>();
            });
            config.CreateMapper().Map<SEC_UserDTO, SEC_User>(dtoSEC_User, domainSEC_User);
        }

		public static void ChangeSEC_UserToDTO(SEC_UserDTO dtoSEC_User, SEC_User domainSEC_User)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_User, SEC_UserDTO>();
            });
            config.CreateMapper().Map<SEC_User, SEC_UserDTO>(domainSEC_User, dtoSEC_User);
        }

		public static SEC_UserDTO ChangeSEC_UserToDTO(SEC_User domainSEC_User)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_User, SEC_UserDTO>();
            });
            return config.CreateMapper().Map<SEC_User, SEC_UserDTO>(domainSEC_User);
        }

		public static List<SEC_UserDTO> ChangeSEC_UserToDTOs(List<SEC_User> domainSEC_User)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_User, SEC_UserDTO>();
            });
            var dtoSEC_User = config.CreateMapper().Map<List<SEC_User>, List<SEC_UserDTO>>(domainSEC_User);

            return dtoSEC_User;
        }

		public static IEnumerable<SEC_UserDTO> ChangeSEC_UserToDTOs(IEnumerable<SEC_User> domainSEC_Users)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_User, SEC_UserDTO>();
            });
            var dtoSEC_User = config.CreateMapper().Map<IEnumerable<SEC_User>, IEnumerable<SEC_UserDTO>>(domainSEC_Users);

            return dtoSEC_User;
        }
	}
}
