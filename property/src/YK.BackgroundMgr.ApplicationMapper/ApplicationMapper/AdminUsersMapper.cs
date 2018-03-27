﻿using AutoMapper;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using System;
using System.Collections.Generic;
using YK.BackgroundMgr.Crosscuting;

namespace YK.BackgroundMgr.ApplicationMapper
{
    public class AdminUsersMapper
    {
        public static SEC_AdminUser ChangeDTOToSEC_AdminUserNew(SEC_AdminUserDTO dtoSEC_AdminUser)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_AdminUserDTO, SEC_AdminUser>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => DataEncrypt.EncryptToDB((src.Password)))); 
            });
            var domainSEC_AdminUser = config.CreateMapper().Map<SEC_AdminUserDTO, SEC_AdminUser>(dtoSEC_AdminUser);

            return domainSEC_AdminUser;
        }

        public static void ChangeDTOToSEC_AdminUserUpdate(SEC_AdminUserDTO dtoSEC_AdminUser, SEC_AdminUser domainSEC_AdminUser)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_AdminUserDTO, SEC_AdminUser>()
                 .ForMember(dest => dest.Password, opt => opt.MapFrom(src => DataEncrypt.EncryptToDB((src.Password))));
            });
            config.CreateMapper().Map<SEC_AdminUserDTO, SEC_AdminUser>(dtoSEC_AdminUser, domainSEC_AdminUser);
        }

        public static void ChangeSEC_AdminUserToDTO(SEC_AdminUserDTO dtoSEC_AdminUser, SEC_AdminUser domainSEC_AdminUser)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_AdminUser, SEC_AdminUserDTO>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => DataEncrypt.DecryptFromDB((src.Password))));
            });
            config.CreateMapper().Map<SEC_AdminUser, SEC_AdminUserDTO>(domainSEC_AdminUser, dtoSEC_AdminUser);
        }

        public static SEC_AdminUserDTO ChangeSEC_AdminUserToDTO(SEC_AdminUser domainSEC_AdminUser)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_AdminUser, SEC_AdminUserDTO>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => DataEncrypt.DecryptFromDB((src.Password))));
            });
            return config.CreateMapper().Map<SEC_AdminUser, SEC_AdminUserDTO>(domainSEC_AdminUser);
        }

        public static List<SEC_AdminUserDTO> ChangeSEC_AdminUserToDTOs(List<SEC_AdminUser> domainSEC_AdminUser)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_AdminUser, SEC_AdminUserDTO>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => DataEncrypt.DecryptFromDB((src.Password))));
            });
            var dtoSEC_AdminUser = config.CreateMapper().Map<List<SEC_AdminUser>, List<SEC_AdminUserDTO>>(domainSEC_AdminUser);

            return dtoSEC_AdminUser;
        }

        public static IEnumerable<SEC_AdminUserDTO> ChangeSEC_AdminUserToDTOs(IEnumerable<SEC_AdminUser> domainSEC_AdminUsers)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SEC_AdminUser, SEC_AdminUserDTO>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => DataEncrypt.DecryptFromDB((src.Password))));
            });
            var dtoSEC_AdminUser = config.CreateMapper().Map<IEnumerable<SEC_AdminUser>, IEnumerable<SEC_AdminUserDTO>>(domainSEC_AdminUsers);

            return dtoSEC_AdminUser;
        }
    }
}