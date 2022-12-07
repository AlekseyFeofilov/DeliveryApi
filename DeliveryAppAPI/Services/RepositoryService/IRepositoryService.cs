using System.Security.Claims;
using DeliveryAppAPI.Models.DbSets;

namespace DeliveryAppAPI.Services.RepositoryService;

public interface IRepositoryService
{
    Task<User> GetUser(ClaimsPrincipal claims);
    Task<Dish> GetDish(Guid id);
    Task<Order> GetOrder(Guid id);
    Task<DishBasket?> GetDishBasket(Guid dishId, Guid userId);
}