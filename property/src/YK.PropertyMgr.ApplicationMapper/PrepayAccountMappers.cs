using AutoMapper;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;

namespace YK.PropertyMgr.ApplicationMapper
{
	public partial class PrepayAccountMappers
	{
		public static PrepayAccount ChangeDTOToPrepayAccountNew(PrepayAccountDTO dtoPrepayAccount)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrepayAccountDTO, PrepayAccount>();
            });
            var domainPrepayAccount = config.CreateMapper().Map<PrepayAccountDTO, PrepayAccount>(dtoPrepayAccount);

            return domainPrepayAccount;
        }

		public static void ChangeDTOToPrepayAccountUpdate(PrepayAccountDTO dtoPrepayAccount, PrepayAccount domainPrepayAccount)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrepayAccountDTO, PrepayAccount>();
            });
            config.CreateMapper().Map<PrepayAccountDTO, PrepayAccount>(dtoPrepayAccount, domainPrepayAccount);
        }

		public static void ChangePrepayAccountToDTO(PrepayAccountDTO dtoPrepayAccount, PrepayAccount domainPrepayAccount)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrepayAccount, PrepayAccountDTO>();
            });
            config.CreateMapper().Map<PrepayAccount, PrepayAccountDTO>(domainPrepayAccount, dtoPrepayAccount);
        }

		public static PrepayAccountDTO ChangePrepayAccountToDTO(PrepayAccount domainPrepayAccount)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrepayAccount, PrepayAccountDTO>();
            });
            return config.CreateMapper().Map<PrepayAccount, PrepayAccountDTO>(domainPrepayAccount);
        }

		public static List<PrepayAccountDTO> ChangePrepayAccountToDTOs(List<PrepayAccount> domainPrepayAccount)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrepayAccount, PrepayAccountDTO>();
            });
            var dtoPrepayAccount = config.CreateMapper().Map<List<PrepayAccount>, List<PrepayAccountDTO>>(domainPrepayAccount);

            return dtoPrepayAccount;
        }

		public static IEnumerable<PrepayAccountDTO> ChangePrepayAccountToDTOs(IEnumerable<PrepayAccount> domainPrepayAccounts)
        {
			var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PrepayAccount, PrepayAccountDTO>();
            });
            var dtoPrepayAccount = config.CreateMapper().Map<IEnumerable<PrepayAccount>, IEnumerable<PrepayAccountDTO>>(domainPrepayAccounts);

            return dtoPrepayAccount;
        }
	}
}
