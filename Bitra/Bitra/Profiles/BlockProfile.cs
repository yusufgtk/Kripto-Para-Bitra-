using AutoMapper;
using Entitiy.Entites;
using Entity.Dtos;

namespace Bitra.Profiles
{
    public class BlockProfile : Profile
    {
        public BlockProfile()
        {
            CreateMap<Block, BlockDto>()
                .ForMember(dest => dest.Transactions, src => src.MapFrom(p => p.Transactions))
                .ReverseMap();
        }
    }
}
