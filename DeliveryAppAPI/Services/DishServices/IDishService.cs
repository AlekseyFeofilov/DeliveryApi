using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Services.DishServices;

public interface IDishService
{
    Task<DishPagedListDto> GetAllDishes(DishCategory? categories, DishSorting? sorting, int? page, bool vegetarian);
}