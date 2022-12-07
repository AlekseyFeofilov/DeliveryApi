using DeliveryAppAPI.Models.DbSets;
using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Services.DishServices;

public interface IDishService
{
    Task<DishPagedListDto> GetAllDishes(DishCategory[]? category, DishSorting? sorting, int? page, bool vegetarian);
    Task<DishDto> GetDishDto(Dish dish);
    bool CheckReviewAccess(Guid dishId, Guid userId);
    Task SetReview(Dish dish, User user, int rating);
}