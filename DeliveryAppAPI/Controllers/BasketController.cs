using System.Net.Mime;
using System.Security.Claims;
using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Models.Response;
using DeliveryAppAPI.Services.BasketService;
using DeliveryAppAPI.Services.JwtService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAppAPI.Controllers;

[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;
    private readonly IJwtClaimService _jwtClaimService;

    public BasketController(IBasketService basketService, IJwtClaimService jwtClaimService)
    {
        _basketService = basketService;
        _jwtClaimService = jwtClaimService;
    }

    /// <summary>
    /// Get user cart
    /// </summary>
    /// <remarks>**Need Authorization**</remarks>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">InternalServerError</response>
    [HttpGet]
    [Authorize]
    [Route("/api/basket")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<DishBasketDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCart()
    {
        var userId = Guid.Parse(_jwtClaimService.GetClaimValue(ClaimTypes.NameIdentifier, Request));
        var cart = await _basketService.GetCart(userId);

        if (cart == null) return Unauthorized();
        return Ok(cart);
    }

    /// <summary>
    /// Add dish to cart
    /// </summary>
    /// <remarks>**Need Authorization**</remarks>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">InternalServerError</response>
    [HttpPost]
    [Authorize]
    [Route("/api/basket/dish/{dishId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddBasket(Guid dishId)
    {
        var userId = Guid.Parse(_jwtClaimService.GetClaimValue(ClaimTypes.NameIdentifier, Request));
        var isAdded = await _basketService.AddBasket(dishId, userId);

        if (isAdded == false)
            return StatusCode(500,
                new Response("Internal server error",
                    "This error isn't implemented")); //todo there are two reason for error (there isn't this user and there isn't this dish)
        return Ok();
    }

    /// <summary>
    /// Decrease the number of dishes in the cart(if increase = true), or remove the dish completely(increase = false)
    /// </summary>
    /// <remarks>**Need Authorization**</remarks>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">InternalServerError</response>
    [HttpDelete]
    [Authorize]
    [Route("/api/basket/dish/{dishId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBasket(Guid dishId, bool increase = false) //todo add increase param
    {
        var userId = Guid.Parse(_jwtClaimService.GetClaimValue(ClaimTypes.NameIdentifier, Request));
        var isDeleted = await _basketService.DeleteBasket(dishId, userId, increase);

        if (isDeleted == false)
            return StatusCode(500, new Response("Internal server error", "This error isn't implemented"));
        return Ok();
    }
}