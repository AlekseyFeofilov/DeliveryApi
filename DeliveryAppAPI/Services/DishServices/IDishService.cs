using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Services.DishServices;

public interface IDishService
{
    Task<DishPagedListDto> GetAllDishes(DishCategory? category, DishSorting? sorting, int? page, bool vegetarian);
    Task<DishDto?> GetDish(Guid id);
    Task<bool> CheckReviewAccess(Guid dishId, Guid userId);
    Task<bool> SetReview(Guid dishId, Guid userId, int rating);
}