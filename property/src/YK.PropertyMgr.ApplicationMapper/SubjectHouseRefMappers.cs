using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class SubjectHouseRefMappers
	{
		public static SubjectHouseRef ChangeDTOToSubjectHouseRefNew(SubjectHouseRefDTO dtoSubjectHouseRef)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SubjectHouseRefDTO, SubjectHouseRef>();
            });
            var domainSubjectHouseRef = config.CreateMapper().Map<SubjectHouseRefDTO, SubjectHouseRef>(dtoSubjectHouseRef);

            return domainSubjectHouseRef;
        }

		public static void ChangeDTOToSubjectHouseRefUpdate(SubjectHouseRefDTO dtoSubjectHouseRef, SubjectHouseRef domainSubjectHouseRef)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SubjectHouseRefDTO, SubjectHouseRef>();
            });
            config.CreateMapper().Map<SubjectHouseRefDTO, SubjectHouseRef>(dtoSubjectHouseRef, domainSubjectHouseRef);
        }

		public static void ChangeSubjectHouseRefToDTO(SubjectHouseRefDTO dtoSubjectHouseRef, SubjectHouseRef domainSubjectHouseRef)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SubjectHouseRef, SubjectHouseRefDTO>();
            });
            config.CreateMapper().Map<SubjectHouseRef, SubjectHouseRefDTO>(domainSubjectHouseRef, dtoSubjectHouseRef);
        }

		public static SubjectHouseRefDTO ChangeSubjectHouseRefToDTO(SubjectHouseRef domainSubjectHouseRef)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SubjectHouseRef, SubjectHouseRefDTO>();
            });
            return config.CreateMapper().Map<SubjectHouseRef, SubjectHouseRefDTO>(domainSubjectHouseRef);
        }

		public static List<SubjectHouseRefDTO> ChangeSubjectHouseRefToDTOs(List<SubjectHouseRef> domainSubjectHouseRef)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SubjectHouseRef, SubjectHouseRefDTO>();
            });
            var dtoSubjectHouseRef = config.CreateMapper().Map<List<SubjectHouseRef>, List<SubjectHouseRefDTO>>(domainSubjectHouseRef);

            return dtoSubjectHouseRef;
        }

		public static IEnumerable<SubjectHouseRefDTO> ChangeSubjectHouseRefToDTOs(IEnumerable<SubjectHouseRef> domainSubjectHouseRefs)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SubjectHouseRef, SubjectHouseRefDTO>();
            });
            var dtoSubjectHouseRef = config.CreateMapper().Map<IEnumerable<SubjectHouseRef>, IEnumerable<SubjectHouseRefDTO>>(domainSubjectHouseRefs);

            return dtoSubjectHouseRef;
        }
	}
}
