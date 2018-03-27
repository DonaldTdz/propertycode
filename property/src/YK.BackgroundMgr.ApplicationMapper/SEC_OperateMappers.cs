using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class SEC_OperateMappers
	{
		public static SEC_Operate ChangeDTOToSEC_OperateNew(SEC_OperateDTO dtoSEC_Operate)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_OperateDTO, SEC_Operate>();
            });
            var domainSEC_Operate = config.CreateMapper().Map<SEC_OperateDTO, SEC_Operate>(dtoSEC_Operate);

            return domainSEC_Operate;
        }

		public static void ChangeDTOToSEC_OperateUpdate(SEC_OperateDTO dtoSEC_Operate, SEC_Operate domainSEC_Operate)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_OperateDTO, SEC_Operate>();
            });
            config.CreateMapper().Map<SEC_OperateDTO, SEC_Operate>(dtoSEC_Operate, domainSEC_Operate);
        }

		public static void ChangeSEC_OperateToDTO(SEC_OperateDTO dtoSEC_Operate, SEC_Operate domainSEC_Operate)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Operate, SEC_OperateDTO>();
            });
            config.CreateMapper().Map<SEC_Operate, SEC_OperateDTO>(domainSEC_Operate, dtoSEC_Operate);
        }

		public static SEC_OperateDTO ChangeSEC_OperateToDTO(SEC_Operate domainSEC_Operate)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Operate, SEC_OperateDTO>();
            });
            return config.CreateMapper().Map<SEC_Operate, SEC_OperateDTO>(domainSEC_Operate);
        }

		public static List<SEC_OperateDTO> ChangeSEC_OperateToDTOs(List<SEC_Operate> domainSEC_Operate)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Operate, SEC_OperateDTO>();
            });
            var dtoSEC_Operate = config.CreateMapper().Map<List<SEC_Operate>, List<SEC_OperateDTO>>(domainSEC_Operate);

            return dtoSEC_Operate;
        }

		public static IEnumerable<SEC_OperateDTO> ChangeSEC_OperateToDTOs(IEnumerable<SEC_Operate> domainSEC_Operates)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_Operate, SEC_OperateDTO>();
            });
            var dtoSEC_Operate = config.CreateMapper().Map<IEnumerable<SEC_Operate>, IEnumerable<SEC_OperateDTO>>(domainSEC_Operates);

            return dtoSEC_Operate;
        }
	}
}
