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
            CreateMap<Order, OrderExtensionDto>().ReverseMap();
            CreateMap<CustomIdentityUser, UserDto>().ReverseMap();
            CreateMap<Chat, ChatDto>().ReverseMap();
            CreateMap<Message,MessageDto>().ReverseMap();
            CreateMap<RelationShip, RelationShipDto>().ReverseMap();
            CreateMap<RelationShipRequest, RelationShipRequestDto>().ReverseMap();
            CreateMap<Card, CardDto>().ReverseMap();
            CreateMap<Card,CardAddDto>().ReverseMap();
            CreateMap<Complaint, ComplaintDto>().ReverseMap();
            CreateMap<Complaint, ComplaintAddDto>().ReverseMap();
        }
    }
}
