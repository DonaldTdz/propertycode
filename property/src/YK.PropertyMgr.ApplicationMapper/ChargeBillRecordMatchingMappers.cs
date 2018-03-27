using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class ChargeBillRecordMatchingMappers
	{
		public static ChargeBillRecordMatching ChangeDTOToChargeBillRecordMatchingNew(ChargeBillRecordMatchingDTO dtoChargeBillRecordMatching)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeBillRecordMatchingDTO, ChargeBillRecordMatching>();
            });
            var domainChargeBillRecordMatching = config.CreateMapper().Map<ChargeBillRecordMatchingDTO, ChargeBillRecordMatching>(dtoChargeBillRecordMatching);

            return domainChargeBillRecordMatching;
        }

		public static void ChangeDTOToChargeBillRecordMatchingUpdate(ChargeBillRecordMatchingDTO dtoChargeBillRecordMatching, ChargeBillRecordMatching domainChargeBillRecordMatching)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeBillRecordMatchingDTO, ChargeBillRecordMatching>();
            });
            config.CreateMapper().Map<ChargeBillRecordMatchingDTO, ChargeBillRecordMatching>(dtoChargeBillRecordMatching, domainChargeBillRecordMatching);
        }

		public static void ChangeChargeBillRecordMatchingToDTO(ChargeBillRecordMatchingDTO dtoChargeBillRecordMatching, ChargeBillRecordMatching domainChargeBillRecordMatching)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeBillRecordMatching, ChargeBillRecordMatchingDTO>();
            });
            config.CreateMapper().Map<ChargeBillRecordMatching, ChargeBillRecordMatchingDTO>(domainChargeBillRecordMatching, dtoChargeBillRecordMatching);
        }

		public static ChargeBillRecordMatchingDTO ChangeChargeBillRecordMatchingToDTO(ChargeBillRecordMatching domainChargeBillRecordMatching)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeBillRecordMatching, ChargeBillRecordMatchingDTO>();
            });
            return config.CreateMapper().Map<ChargeBillRecordMatching, ChargeBillRecordMatchingDTO>(domainChargeBillRecordMatching);
        }

		public static List<ChargeBillRecordMatchingDTO> ChangeChargeBillRecordMatchingToDTOs(List<ChargeBillRecordMatching> domainChargeBillRecordMatching)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeBillRecordMatching, ChargeBillRecordMatchingDTO>();
            });
            var dtoChargeBillRecordMatching = config.CreateMapper().Map<List<ChargeBillRecordMatching>, List<ChargeBillRecordMatchingDTO>>(domainChargeBillRecordMatching);

            return dtoChargeBillRecordMatching;
        }

		public static IEnumerable<ChargeBillRecordMatchingDTO> ChangeChargeBillRecordMatchingToDTOs(IEnumerable<ChargeBillRecordMatching> domainChargeBillRecordMatchings)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChargeBillRecordMatching, ChargeBillRecordMatchingDTO>();
            });
            var dtoChargeBillRecordMatching = config.CreateMapper().Map<IEnumerable<ChargeBillRecordMatching>, IEnumerable<ChargeBillRecordMatchingDTO>>(domainChargeBillRecordMatchings);

            return dtoChargeBillRecordMatching;
        }
	}
}
