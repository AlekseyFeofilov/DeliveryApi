using System.Diagnostics;
using System.Security.Claims;
using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Models.Enums;
using DeliveryAppAPI.Services.DishServices;
using DeliveryAppAPI.Services.JwtService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAppAPI.Controllers;

[ApiController]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;
    private readonly IJwtClaimService _jwtClaimService;

    public DishController(IDishService dishService, IJwtClaimService jwtClaimService)
    {
        _dishService = dishService;
        _jwtClaimService = jwtClaimService;
    }

    [HttpGet]
    [Route("/api/dish")]
    public async Task<IActionResult> Get(DishCategory? categories, DishSorting? sorting, int? page, bool vegetarian)
    {
        return Ok(await _dishService.GetAllDishes(categories, sorting, page, vegetarian));
    }

    [HttpGet]
    [Route("/api/dish/{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var dish = await _dishService.GetDish(id);

        if (dish == null)
        {
            return NotFound();
        }

        return Ok(dish);
    }

    [HttpGet]
    [Authorize]
    [Route("/api/dish/{id:guid}/rating/check")]
    public async Task<IActionResult> CheckReviewAccess(Guid id)
    {
        var userId = Guid.Parse(_jwtClaimService.GetClaimValue(ClaimTypes.NameIdentifier, Request));

        return Ok(await _dishService.CheckReviewAccess(id, userId));
    }

    [HttpPost]
    [Authorize]
    [Route("/api/dish/{id:guid}/rating")]
    public async Task<IActionResult> SetReview(Guid id, int rating)
    {
        var userId = Guid.Parse(_jwtClaimService.GetClaimValue(ClaimTypes.NameIdentifier, Request));

        if (!await _dishService.SetReview(id, userId, rating)) return BadRequest();

        return Ok();
    }
}