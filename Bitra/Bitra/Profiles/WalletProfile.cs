using AutoMapper;
using Entitiy.Entites;
using Entity.Dtos;

namespace Bitra.Profiles
{
    public class WalletProfile : Profile
    {
        public WalletProfile()
        {
            CreateMap<Wallet, WalletDto>().ReverseMap();
            CreateMap<Wallet, WalletLoginDto>().ReverseMap();
        }
    }
}
