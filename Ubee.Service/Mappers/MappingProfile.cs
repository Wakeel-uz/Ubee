using AutoMapper;
using Ubee.Domain.Entities;
using Ubee.Service.DTOs;
using Ubee.Service.DTOs.Infos;
using Ubee.Service.DTOs.Transactions;
using Ubee.Service.DTOs.Users;
using Ubee.Service.DTOs.Wallets;

namespace Ubee.Service.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<UserForCreationDto, User>().ReverseMap();
        CreateMap<UserForResultDto, User>().ReverseMap();

        // Wallet
        CreateMap<WalletForCreationDto, Wallet>().ReverseMap();
        CreateMap<WalletForResultDto, Wallet>().ReverseMap();

        // Info
        CreateMap<InfoForCreationDto, Info>().ReverseMap();
        CreateMap<InfoForResultDto , Info>().ReverseMap();

        // Transaction
        CreateMap<TransactionForCreationDto, Transaction>().ReverseMap();
        CreateMap<TransactionForResultDto, Transaction>().ReverseMap();
    }
}
