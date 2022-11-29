using System.Security.Claims;
using DeliveryAppAPI.Models.Response;
using DeliveryAppAPI.Services.BasketService;
using DeliveryAppAPI.Services.JwtService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAppAPI.Controllers;

public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;
    private readonly IJwtClaimService _jwtClaimService;

    public BasketController(IBasketService basketService, IJwtClaimService jwtClaimService)
    {
        _basketService = basketService;
        _jwtClaimService = jwtClaimService;
    }

    [HttpGet]
    [Authorize]
    [Route("/api/basket")]
    public async Task<IActionResult> GetCart()
    {
        var userId = Guid.Parse(_jwtClaimService.GetClaimValue(ClaimTypes.NameIdentifier, Request));
        var cart = await _basketService.GetCart(userId);

        if (cart == null) return Unauthorized();
        return Ok(cart);
    }

    [HttpPost]
    [Authorize]
    [Route("/api/basket/dish/{dishId:guid}")]
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

    [HttpDelete]
    [Authorize]
    [Route("/api/basket/dish/{dishId:guid}")]
    public async Task<IActionResult> DeleteBasket(Guid dishId, bool increase = false) //todo add increase param
    {
        var userId = Guid.Parse(_jwtClaimService.GetClaimValue(ClaimTypes.NameIdentifier, Request));
        var isDeleted = await _basketService.DeleteBasket(dishId, userId, increase);

        if (isDeleted == false)
            return StatusCode(500, new Response("Internal server error", "This error isn't implemented"));
        return Ok();
    }
}