using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class SEC_DeptMappers
	{
		public static SEC_Dept ChangeDTOToSEC_DeptNew(SEC_DeptDTO dtoSEC_Dept)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_DeptDTO, SEC_Dept>();
            });
            var domainSEC_Dept = config.CreateMapper().Map<SEC_DeptDTO, SEC_Dept>(dtoSEC_Dept);

            return domainSEC_Dept;
        }

		public static void ChangeDTOToSEC_DeptUpdate(SEC_DeptDTO dtoSEC_Dept, SEC_Dept domainSEC_Dept)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_DeptDTO, SEC_Dept>();
            });
            config.CreateMapper().Map<SEC_DeptDTO, SEC_Dept>(dtoSEC_Dept, domainSEC_Dept);
        }

		public static void ChangeSEC_DeptToDTO(SEC_DeptDTO dtoSEC_Dept, SEC_Dept domainSEC_Dept)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Dept, SEC_DeptDTO>();
            });
            config.CreateMapper().Map<SEC_Dept, SEC_DeptDTO>(domainSEC_Dept, dtoSEC_Dept);
        }

		public static SEC_DeptDTO ChangeSEC_DeptToDTO(SEC_Dept domainSEC_Dept)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Dept, SEC_DeptDTO>();
            });
            return config.CreateMapper().Map<SEC_Dept, SEC_DeptDTO>(domainSEC_Dept);
        }

		public static List<SEC_DeptDTO> ChangeSEC_DeptToDTOs(List<SEC_Dept> domainSEC_Dept)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Dept, SEC_DeptDTO>();
            });
            var dtoSEC_Dept = config.CreateMapper().Map<List<SEC_Dept>, List<SEC_DeptDTO>>(domainSEC_Dept);

            return dtoSEC_Dept;
        }

		public static IEnumerable<SEC_DeptDTO> ChangeSEC_DeptToDTOs(IEnumerable<SEC_Dept> domainSEC_Depts)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Dept, SEC_DeptDTO>();
            });
            var dtoSEC_Dept = config.CreateMapper().Map<IEnumerable<SEC_Dept>, IEnumerable<SEC_DeptDTO>>(domainSEC_Depts);

            return dtoSEC_Dept;
        }
	}
}
