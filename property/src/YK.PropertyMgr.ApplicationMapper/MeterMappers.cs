using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class MeterMappers
	{
		public static Meter ChangeDTOToMeterNew(MeterDTO dtoMeter)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MeterDTO, Meter>();
            });
            var domainMeter = config.CreateMapper().Map<MeterDTO, Meter>(dtoMeter);

            return domainMeter;
        }

		public static void ChangeDTOToMeterUpdate(MeterDTO dtoMeter, Meter domainMeter)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MeterDTO, Meter>();
            });
            config.CreateMapper().Map<MeterDTO, Meter>(dtoMeter, domainMeter);
        }

		public static void ChangeMeterToDTO(MeterDTO dtoMeter, Meter domainMeter)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Meter, MeterDTO>();
            });
            config.CreateMapper().Map<Meter, MeterDTO>(domainMeter, dtoMeter);
        }

		public static MeterDTO ChangeMeterToDTO(Meter domainMeter)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Meter, MeterDTO>();
            });
            return config.CreateMapper().Map<Meter, MeterDTO>(domainMeter);
        }

		public static List<MeterDTO> ChangeMeterToDTOs(List<Meter> domainMeter)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Meter, MeterDTO>();
            });
            var dtoMeter = config.CreateMapper().Map<List<Meter>, List<MeterDTO>>(domainMeter);

            return dtoMeter;
        }

		public static IEnumerable<MeterDTO> ChangeMeterToDTOs(IEnumerable<Meter> domainMeters)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Meter, MeterDTO>();
            });
            var dtoMeter = config.CreateMapper().Map<IEnumerable<Meter>, IEnumerable<MeterDTO>>(domainMeters);

            return dtoMeter;
        }
	}
}
