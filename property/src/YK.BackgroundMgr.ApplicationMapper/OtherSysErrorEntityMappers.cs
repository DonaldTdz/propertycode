using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.BackgroundMgr.ApplicationMapper
{
	public partial class OtherSysErrorEntityMappers
	{
		public static OtherSysErrorEntity ChangeDTOToOtherSysErrorEntityNew(OtherSysErrorEntityDTO dtoOtherSysErrorEntity)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OtherSysErrorEntityDTO, OtherSysErrorEntity>();
            });
            var domainOtherSysErrorEntity = config.CreateMapper().Map<OtherSysErrorEntityDTO, OtherSysErrorEntity>(dtoOtherSysErrorEntity);

            return domainOtherSysErrorEntity;
        }

		public static void ChangeDTOToOtherSysErrorEntityUpdate(OtherSysErrorEntityDTO dtoOtherSysErrorEntity, OtherSysErrorEntity domainOtherSysErrorEntity)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OtherSysErrorEntityDTO, OtherSysErrorEntity>();
            });
            config.CreateMapper().Map<OtherSysErrorEntityDTO, OtherSysErrorEntity>(dtoOtherSysErrorEntity, domainOtherSysErrorEntity);
        }

		public static void ChangeOtherSysErrorEntityToDTO(OtherSysErrorEntityDTO dtoOtherSysErrorEntity, OtherSysErrorEntity domainOtherSysErrorEntity)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OtherSysErrorEntity, OtherSysErrorEntityDTO>();
            });
            config.CreateMapper().Map<OtherSysErrorEntity, OtherSysErrorEntityDTO>(domainOtherSysErrorEntity, dtoOtherSysErrorEntity);
        }

		public static OtherSysErrorEntityDTO ChangeOtherSysErrorEntityToDTO(OtherSysErrorEntity domainOtherSysErrorEntity)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OtherSysErrorEntity, OtherSysErrorEntityDTO>();
            });
            return config.CreateMapper().Map<OtherSysErrorEntity, OtherSysErrorEntityDTO>(domainOtherSysErrorEntity);
        }

		public static List<OtherSysErrorEntityDTO> ChangeOtherSysErrorEntityToDTOs(List<OtherSysErrorEntity> domainOtherSysErrorEntity)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OtherSysErrorEntity, OtherSysErrorEntityDTO>();
            });
            var dtoOtherSysErrorEntity = config.CreateMapper().Map<List<OtherSysErrorEntity>, List<OtherSysErrorEntityDTO>>(domainOtherSysErrorEntity);

            return dtoOtherSysErrorEntity;
        }

		public static IEnumerable<OtherSysErrorEntityDTO> ChangeOtherSysErrorEntityToDTOs(IEnumerable<OtherSysErrorEntity> domainOtherSysErrorEntitys)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OtherSysErrorEntity, OtherSysErrorEntityDTO>();
            });
            var dtoOtherSysErrorEntity = config.CreateMapper().Map<IEnumerable<OtherSysErrorEntity>, IEnumerable<OtherSysErrorEntityDTO>>(domainOtherSysErrorEntitys);

            return dtoOtherSysErrorEntity;
        }
	}
}
