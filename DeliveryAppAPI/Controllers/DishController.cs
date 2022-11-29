using System.Diagnostics;
using System.Security.Claims;
using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Models.Enums;
using DeliveryAppAPI.Models.Response;
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

    /// <summary>
    /// Get a list of dishes (menu)
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">InternalServerError</response>
    [HttpGet]
    [Route("/api/dish")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(DishPagedListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(DishCategory? categories = null, bool vegetarian = false,
        DishSorting? sorting = null, int? page = 1) //todo set multiple categories choose
    {
        return Ok(await _dishService.GetAllDishes(categories, sorting, page, vegetarian));
    }

    /// <summary>
    /// Get information about concrete dish
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">InternalServerError</response>
    [HttpGet]
    [Route("/api/dish/{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(DishDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(Guid id)
    {
        var dish = await _dishService.GetDish(id);

        if (dish == null)
        {
            return NotFound();
        }

        return Ok(dish);
    }

    /// <summary>
    /// Checks if user is able to set rating of the dish
    /// </summary>
    /// <remarks>**Need Authorization**</remarks>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">InternalServerError</response>
    [HttpGet]
    [Authorize]
    [Route("/api/dish/{id:guid}/rating/check")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CheckReviewAccess(Guid id)
    {
        var userId = Guid.Parse(_jwtClaimService.GetClaimValue(ClaimTypes.NameIdentifier, Request));

        return Ok(await _dishService.CheckReviewAccess(id, userId));
    }

    /// <summary>
    /// Set a rating for a dish
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
    [Route("/api/dish/{id:guid}/rating")]
    [Produces("application/json")] //todo: in swagger documentation there is media type
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SetReview(Guid id, int ratingScore)
    {
        var userId = Guid.Parse(_jwtClaimService.GetClaimValue(ClaimTypes.NameIdentifier, Request));

        if (!await _dishService.SetReview(id, userId, ratingScore)) return BadRequest();

        return Ok();
    }
}