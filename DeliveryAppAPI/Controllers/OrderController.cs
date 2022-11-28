using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Services.JwtService;
using DeliveryAppAPI.Services.OrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAppAPI.Controllers;

public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IJwtClaimService _jwtClaimService;

    public OrderController(IOrderService orderService, IJwtClaimService jwtClaimService)
    {
        _orderService = orderService;
        _jwtClaimService = jwtClaimService;
    }

    [HttpGet]
    //[Authorize]
    [Route("api/order/{id:guid}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var order = await _orderService.GetOrder(id);

        if (order == null) return Unauthorized();

        return Ok(order);
    }
    
    [HttpGet]
    //[Authorize]
    [Route("api/order")]
    public async Task<IActionResult> GetAllOrders()
    {
        //var userId = Guid.Parse(_jwtClaimService.GetClaimValue(ClaimTypes.NameIdentifier, Request));
        var userId = Guid.Parse("7991d400-efd9-416e-b89f-ff72ba8d32ac");
        var orders = await _orderService.GetAllOrders(userId);
        
        if (orders == null) return Unauthorized();

        return Ok(orders);
    }
    
    [HttpPost]
    //[Authorize]
    [Route("api/order")]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderCreateDto)
    {
        //var userId = Guid.Parse(_jwtClaimService.GetClaimValue(ClaimTypes.NameIdentifier, Request));
        var userId = Guid.Parse("7991d400-efd9-416e-b89f-ff72ba8d32ac");
        //var orderCreateDto = new OrderCreateDto(deliveryTime, address);
        if (!await _orderService.CreateOrder(orderCreateDto, userId)) return Unauthorized();
        return Ok();
    }
    
    [HttpPost]
    //[Authorize]
    [Route("api/order/{id:guid}/status")]
    public async Task<IActionResult> ConfirmOrderDelivery(Guid id)
    {
        //var userId = Guid.Parse(_jwtClaimService.GetClaimValue(ClaimTypes.NameIdentifier, Request));
        var userId = Guid.Parse("7991d400-efd9-416e-b89f-ff72ba8d32ac");
        if (!await _orderService.ConfirmOrderDelivery(id)) return Unauthorized();
        return Ok();
    }
}