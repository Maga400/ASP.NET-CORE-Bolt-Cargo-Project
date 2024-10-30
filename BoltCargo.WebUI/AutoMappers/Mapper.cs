using AutoMapper;
using BoltCargo.Entities.Entities;
using BoltCargo.WebUI.Dtos;

namespace BoltCargo.WebUI.AutoMappers
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<FeedBack, FeedBackDto>().ReverseMap();
            CreateMap<FeedBack, FeedBackExtensionDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, OrderExtensionDto>().ReverseMap()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ReverseMap()
                .ForPath(dest => dest.User, opt => opt.MapFrom(src => src.User));
            CreateMap<CustomIdentityUser, UserDto>().ReverseMap();
            CreateMap<Chat, ChatDto>().ReverseMap();
            CreateMap<Message,MessageDto>().ReverseMap();
            CreateMap<RelationShip, RelationShipDto>().ReverseMap();
            CreateMap<RelationShipRequest, RelationShipRequestDto>().ReverseMap();
        }
    }
}
