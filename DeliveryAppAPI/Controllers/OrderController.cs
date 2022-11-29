using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Models.Response;
using DeliveryAppAPI.Services.JwtService;
using DeliveryAppAPI.Services.OrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAppAPI.Controllers;

[ApiController]
[Produces("application/json")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IJwtClaimService _jwtClaimService;

    public OrderController(IOrderService orderService, IJwtClaimService jwtClaimService)
    {
        _orderService = orderService;
        _jwtClaimService = jwtClaimService;
    }

    /// <summary>
    /// Get information about concrete order
    /// </summary>
    /// <remarks>**Need Authorization**</remarks>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">InternalServerError</response>
    [HttpGet]
    [Authorize]
    [Route("api/order/{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var order = await _orderService.GetOrder(id);

        if (order == null) return Unauthorized();

        return Ok(order);
    }
    
    /// <summary>
    /// Get a list of orders
    /// </summary>
    /// <remarks>**Need Authorization**</remarks>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">InternalServerError</response>
    [HttpGet]
    [Authorize]
    [Route("api/order")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<OrderInfoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllOrders()
    {
        var userId = Guid.Parse(_jwtClaimService.GetClaimValue(ClaimTypes.NameIdentifier, Request));
        var orders = await _orderService.GetAllOrders(userId);
        
        if (orders == null) return Unauthorized();

        return Ok(orders);
    }
    
    /// <summary>
    /// Creating the order from dishes in basket
    /// </summary>
    /// <remarks>**Need Authorization**</remarks>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">InternalServerError</response>
    [HttpPost]
    [Authorize]
    [Route("api/order")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderCreateDto)
    {
        var userId = Guid.Parse(_jwtClaimService.GetClaimValue(ClaimTypes.NameIdentifier, Request));
        //var orderCreateDto = new OrderCreateDto(deliveryTime, address);
        if (!await _orderService.CreateOrder(orderCreateDto, userId)) return Unauthorized();
        return Ok();
    }
    
    /// <summary>
    /// Confirm order delivery
    /// </summary>
    /// <remarks>**Need Authorization**</remarks>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">InternalServerError</response>
    [HttpPost]
    [Authorize]
    [Route("api/order/{id:guid}/status")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConfirmOrderDelivery(Guid id)
    {
        var userId = Guid.Parse(_jwtClaimService.GetClaimValue(ClaimTypes.NameIdentifier, Request));
        if (!await _orderService.ConfirmOrderDelivery(id)) return Unauthorized();
        return Ok();
    }
}