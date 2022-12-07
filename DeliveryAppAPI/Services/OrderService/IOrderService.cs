using DeliveryAppAPI.Models.DbSets;
using DeliveryAppAPI.Models.Dto;

namespace DeliveryAppAPI.Services.OrderService;

public interface IOrderService
{
    Task<OrderDto> GetOrderDto(Order order);
    Task<IEnumerable<OrderInfoDto>> GetAllOrders(Guid userId);
    Task<bool> CreateOrder(OrderCreateDto orderCreateDto, User user);
    void ConfirmOrderDelivery(Order order);
}