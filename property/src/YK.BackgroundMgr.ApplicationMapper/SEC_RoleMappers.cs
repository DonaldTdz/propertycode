using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class SEC_RoleMappers
	{
		public static SEC_Role ChangeDTOToSEC_RoleNew(SEC_RoleDTO dtoSEC_Role)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_RoleDTO, SEC_Role>();
            });
            var domainSEC_Role = config.CreateMapper().Map<SEC_RoleDTO, SEC_Role>(dtoSEC_Role);

            return domainSEC_Role;
        }

		public static void ChangeDTOToSEC_RoleUpdate(SEC_RoleDTO dtoSEC_Role, SEC_Role domainSEC_Role)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_RoleDTO, SEC_Role>();
            });
            config.CreateMapper().Map<SEC_RoleDTO, SEC_Role>(dtoSEC_Role, domainSEC_Role);
        }

		public static void ChangeSEC_RoleToDTO(SEC_RoleDTO dtoSEC_Role, SEC_Role domainSEC_Role)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Role, SEC_RoleDTO>();
            });
            config.CreateMapper().Map<SEC_Role, SEC_RoleDTO>(domainSEC_Role, dtoSEC_Role);
        }

		public static SEC_RoleDTO ChangeSEC_RoleToDTO(SEC_Role domainSEC_Role)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Role, SEC_RoleDTO>();
            });
            return config.CreateMapper().Map<SEC_Role, SEC_RoleDTO>(domainSEC_Role);
        }

		public static List<SEC_RoleDTO> ChangeSEC_RoleToDTOs(List<SEC_Role> domainSEC_Role)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Role, SEC_RoleDTO>();
            });
            var dtoSEC_Role = config.CreateMapper().Map<List<SEC_Role>, List<SEC_RoleDTO>>(domainSEC_Role);

            return dtoSEC_Role;
        }

		public static IEnumerable<SEC_RoleDTO> ChangeSEC_RoleToDTOs(IEnumerable<SEC_Role> domainSEC_Roles)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Role, SEC_RoleDTO>();
            });
            var dtoSEC_Role = config.CreateMapper().Map<IEnumerable<SEC_Role>, IEnumerable<SEC_RoleDTO>>(domainSEC_Roles);

            return dtoSEC_Role;
        }
	}
}
