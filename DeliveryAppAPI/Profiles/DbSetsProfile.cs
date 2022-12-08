using AutoMapper;
using DeliveryAppAPI.Models;
using DeliveryAppAPI.Models.DbSets;
using DeliveryAppAPI.Models.Dto;
using Microsoft.IdentityModel.Tokens;

namespace DeliveryAppAPI.Profiles;

public class DbSetsProfile : Profile
{
    public DbSetsProfile()
    {
        CreateMap<UserRegisterModel, User>();
        CreateMap<User, UserDto>();
        CreateMap<Order, OrderDto>();

        CreateMap<Dish, DishDto>()
            .ForMember(dto => dto.Rating, options => options.MapFrom(dish =>
                dish.Reviews.IsNullOrEmpty() ? (double?)null : dish.Reviews.Average(r => r.Rating)));

        CreateMap<DishBasket, DishBasketDto>()
            .ForMember(dto => dto.Name, options => options.MapFrom(dishBasket => dishBasket.Dish.Name))
            .ForMember(dto => dto.Price, options => options.MapFrom(dishBasket => dishBasket.Dish.Price))
            .ForMember(dto => dto.Image, options => options.MapFrom(dishBasket => dishBasket.Dish.Image));
    }
}