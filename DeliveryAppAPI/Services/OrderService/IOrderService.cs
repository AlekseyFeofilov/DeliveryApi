using DeliveryAppAPI.Models.Dto;

namespace DeliveryAppAPI.Services.OrderService;

public interface IOrderService
{
    Task<OrderDto?> GetOrder(Guid id);
    Task<IEnumerable<OrderInfoDto>?> GetAllOrders(Guid userId);
    Task<bool> CreateOrder(OrderCreateDto orderCreateDto, Guid userId);
    Task<bool> ConfirmOrderDelivery(Guid id);
}