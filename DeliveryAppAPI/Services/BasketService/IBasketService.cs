using DeliveryAppAPI.Models.Dto;

namespace DeliveryAppAPI.Services.BasketService;

public interface IBasketService
{
    Task<IEnumerable<DishBasketDto>?> GetCart(Guid userId);
    Task<bool> AddBasket(Guid dishId, Guid userId);
    Task<bool> DeleteBasket(Guid dishId, Guid userId, bool increase = false);
}