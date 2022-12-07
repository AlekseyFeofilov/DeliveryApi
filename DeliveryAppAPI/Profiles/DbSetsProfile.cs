using AutoMapper;
using DeliveryAppAPI.Models;
using DeliveryAppAPI.Models.DbSets;
using DeliveryAppAPI.Models.Dto;

namespace DeliveryAppAPI.Profiles;

public class DbSetsProfile : Profile
{
    public DbSetsProfile()
    {
        CreateMap<UserRegisterModel, User>();//todo
        CreateMap<User, UserDto>();
        CreateMap<Order, OrderDto>()
            .ForMember(dto => dto.DishBaskets, options => options.MapFrom(_ => new List<DishBasketDto>()));

        //CreateMap<Dish, DishDto>()
            //.ForMember(dto => dto.Rating, options => options.MapFrom(dish => dish.Reviews.Average(r => r.Rating)));
            //.ForCtorParam("reviews", options => options.MapFrom(dish => dish.Reviews));

        // CreateMap<DishBasket, DishBasketDto>()
        //     .ForMember(dto => dto.Name, options => options.MapFrom(dishBasket => dishBasket.Dish.Name))
        //     .ForMember(dto => dto.Price, options => options.MapFrom(dishBasket => dishBasket.Dish.Price))
        //     .ForMember(dto => dto.Image, options => options.MapFrom(dishBasket => dishBasket.Dish.Image));
    }
}