using AutoMapper;
using Entitiy.Entites;
using Entity.Dtos;

namespace Bitra.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.Block, src => src.MapFrom(p => p.Block))
                .ReverseMap();
        }
    }
}
