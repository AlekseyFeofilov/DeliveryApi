using DeliveryAppAPI.Models.DbSets;
using DeliveryAppAPI.Models.Dto;

namespace DeliveryAppAPI.Services.BasketService;

public interface IDishBasketService
{
    Task<IEnumerable<DishBasketDto>> GetCart(Guid userId);
    Task AddBasket(Dish dish, User user);
    void DeleteBasket(DishBasket dishBasket, bool increase = false);
}