using DeliveryAppAPI.Configurations;
using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Models.Response;
using DeliveryAppAPI.Services.OrderService;
using DeliveryAppAPI.Services.RepositoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAppAPI.Controllers;

[ApiController]
[Produces(AppConfigurations.ResponseContentType)]
[Route("api/order")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IRepositoryService _repositoryService;

    public OrderController(IOrderService orderService, IRepositoryService repositoryService)
    {
        _orderService = orderService;
        _repositoryService = repositoryService;
    }

    /// <summary>
    /// Get information about concrete order
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">InternalServerError</response>
    [HttpGet, Authorize, Authorize(AppConfigurations.ActiveTokenPolicy), Route("{id:guid}")]
    [Produces(AppConfigurations.ResponseContentType)]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOrderDto(Guid id)
    {
        var order = await _repositoryService.GetOrder(id);
        return Ok(await _orderService.GetOrderDto(order));
    }

    /// <summary>
    /// Get a list of orders
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">InternalServerError</response>
    [HttpGet, Authorize(AppConfigurations.ActiveTokenPolicy)]
    [Produces(AppConfigurations.ResponseContentType)]
    [ProducesResponseType(typeof(IEnumerable<OrderInfoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllOrders()
    {
        var user = await _repositoryService.GetUser(User);
        return Ok(await _orderService.GetAllOrders(user.Id));
    }
    
    /// <summary>
    /// Creating the order from dishes in basket
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">InternalServerError</response>
    [HttpPost, Authorize, Authorize(AppConfigurations.ActiveTokenPolicy)]
    [Produces(AppConfigurations.ResponseContentType)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderCreateDto)
    {
        var user = await _repositoryService.GetUser(User);
        if (await _orderService.CreateOrder(orderCreateDto, user)) return Ok();
        
        return StatusCode(StatusCodes.Status403Forbidden, ErrorMessage.EmptyCart);
    }

    /// <summary>
    /// Confirm order delivery
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">InternalServerError</response>
    [HttpPost, Authorize, Authorize(AppConfigurations.ActiveTokenPolicy), Route("{id:guid}/status")]
    [Produces(AppConfigurations.ResponseContentType)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConfirmOrderDelivery(Guid id)
    {
        _orderService.ConfirmOrderDelivery(await _repositoryService.GetOrder(id));
        return Ok();
    }
}