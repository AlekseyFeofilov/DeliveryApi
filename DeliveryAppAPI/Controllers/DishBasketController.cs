using System.Security.Claims;
using DeliveryAppAPI.Exceptions;
using DeliveryAppAPI.Models.DbSets;
using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Models.Response;
using DeliveryAppAPI.Services.BasketService;
using DeliveryAppAPI.Services.DishServices;
using DeliveryAppAPI.Services.JwtService;
using DeliveryAppAPI.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAppAPI.Controllers;

[ApiController]
public class DishBasketController : ControllerBase
{
    private readonly IDishBasketService _dishBasketService;
    private readonly IUserService _userService;
    private readonly IDishService _dishService;
    private readonly IJwtClaimService _jwtClaimService;

    public DishBasketController(IDishBasketService dishBasketService, IJwtClaimService jwtClaimService,
        IUserService userService, IDishService dishService)
    {
        _dishBasketService = dishBasketService;
        _jwtClaimService = jwtClaimService;
        _userService = userService;
        _dishService = dishService;
    }

    /// <summary>
    /// Get user cart
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="500">InternalServerError</response>
    [HttpGet, Authorize, Route("/api/basket")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<DishBasketDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCart()
    {
        var userId = _jwtClaimService.GetIdClaim(Request);
        var cart = await _dishBasketService.GetCart(userId);
        return Ok(cart);
    }

    /// <summary>
    /// Add dish to cart
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">InternalServerError</response>
    [HttpPost, Authorize, Route("/api/basket/dish/{dishId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddBasket(Guid dishId)
    {
        var user = await _userService.GetUser(Request);
        var dish = await GetDish(dishId);

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
    [HttpDelete, Authorize, Route("/api/basket/dish/{dishId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBasket(Guid dishId, bool increase = false) //todo add increase param
    {
        var userId = _jwtClaimService.GetIdClaim(Request);
        var dishBasket = await GetDishBasket(dishId, userId);

        _dishBasketService.DeleteBasket(dishBasket, increase);
        return Ok();
    }

    private async Task<Dish> GetDish(Guid dishId)
    {
        var dish = await _dishService.GetDish(dishId);
        if (dish == null) throw new NotFoundException();
        return dish;
    }

    private async Task<DishBasket> GetDishBasket(Guid dishId, Guid userId)
    {
        var dishBasket = await _dishBasketService.GetDishBasket(dishId, userId);
        if (dishBasket == null) throw new NotFoundException();
        return dishBasket;
    }
}