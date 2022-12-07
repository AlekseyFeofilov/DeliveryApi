using System.Security.Claims;
using DeliveryAppAPI.Configurations;
using DeliveryAppAPI.Exceptions;
using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Models.Response;
using DeliveryAppAPI.Services.BasketService;
using DeliveryAppAPI.Services.RepositoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAppAPI.Controllers;

[ApiController]
[Route("api/basket")]
public class BasketController : ControllerBase
{
    private readonly IDishBasketService _dishBasketService;
    private readonly IRepositoryService _repositoryService;

    public BasketController(IDishBasketService dishBasketService, IRepositoryService repositoryService)
    {
        _dishBasketService = dishBasketService;
        _repositoryService = repositoryService;
    }

    /// <summary>
    /// Get user cart
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="500">InternalServerError</response>
    [HttpGet, Authorize, Authorize(AppConfigurations.ActiveTokenPolicy)]
    [Produces(AppConfigurations.ResponseContentType)]
    [ProducesResponseType(typeof(IEnumerable<DishBasketDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCart()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return Ok(await _dishBasketService.GetCart(userId));
    }

    /// <summary>
    /// Add dish to cart
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">InternalServerError</response>
    [HttpPost, Authorize, Authorize(AppConfigurations.ActiveTokenPolicy), Route("dish/{dishId:guid}")]
    [Produces(AppConfigurations.ResponseContentType)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddBasket(Guid dishId)
    {
        var user = await _repositoryService.GetUser(User);
        var dish = await _repositoryService.GetDish(dishId);

        await _dishBasketService.AddBasket(dish, user);
        return Ok();
    }

    /// <summary>
    /// Decrease the number of dishes in the cart(if increase = true), or remove the dish completely(increase = false)
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">InternalServerError</response>
    [HttpDelete, Authorize, Authorize(AppConfigurations.ActiveTokenPolicy), Route("dish/{dishId:guid}")]
    [Produces(AppConfigurations.ResponseContentType)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBasket(Guid dishId, bool increase = false)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var dishBasket = await _repositoryService.GetDishBasket(dishId, userId);
        if (dishBasket == null) throw new NotFoundException();
        
        _dishBasketService.DeleteBasket(dishBasket, increase);
        return Ok();
    }
}