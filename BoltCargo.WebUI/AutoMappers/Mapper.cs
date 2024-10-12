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
        }
    }
}
