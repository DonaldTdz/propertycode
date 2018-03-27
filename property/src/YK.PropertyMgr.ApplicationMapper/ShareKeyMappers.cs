using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class ShareKeyMappers
	{
		public static ShareKey ChangeDTOToShareKeyNew(ShareKeyDTO dtoShareKey)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ShareKeyDTO, ShareKey>();
            });
            var domainShareKey = config.CreateMapper().Map<ShareKeyDTO, ShareKey>(dtoShareKey);

            return domainShareKey;
        }

		public static void ChangeDTOToShareKeyUpdate(ShareKeyDTO dtoShareKey, ShareKey domainShareKey)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ShareKeyDTO, ShareKey>();
            });
            config.CreateMapper().Map<ShareKeyDTO, ShareKey>(dtoShareKey, domainShareKey);
        }

		public static void ChangeShareKeyToDTO(ShareKeyDTO dtoShareKey, ShareKey domainShareKey)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ShareKey, ShareKeyDTO>();
            });
            config.CreateMapper().Map<ShareKey, ShareKeyDTO>(domainShareKey, dtoShareKey);
        }

		public static ShareKeyDTO ChangeShareKeyToDTO(ShareKey domainShareKey)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ShareKey, ShareKeyDTO>();
            });
            return config.CreateMapper().Map<ShareKey, ShareKeyDTO>(domainShareKey);
        }

		public static List<ShareKeyDTO> ChangeShareKeyToDTOs(List<ShareKey> domainShareKey)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ShareKey, ShareKeyDTO>();
            });
            var dtoShareKey = config.CreateMapper().Map<List<ShareKey>, List<ShareKeyDTO>>(domainShareKey);

            return dtoShareKey;
        }

		public static IEnumerable<ShareKeyDTO> ChangeShareKeyToDTOs(IEnumerable<ShareKey> domainShareKeys)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ShareKey, ShareKeyDTO>();
            });
            var dtoShareKey = config.CreateMapper().Map<IEnumerable<ShareKey>, IEnumerable<ShareKeyDTO>>(domainShareKeys);

            return dtoShareKey;
        }
	}
}
