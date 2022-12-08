using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using DeliveryAppAPI.Configurations;
using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Models.Enums;
using DeliveryAppAPI.Models.Response;
using DeliveryAppAPI.Services.DishServices;
using DeliveryAppAPI.Services.RepositoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAppAPI.Controllers;

[ApiController]
[Route("api/dish")]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;
    private readonly IRepositoryService _repositoryService;

    public DishController(IDishService dishService, IRepositoryService repositoryService)
    {
        _dishService = dishService;
        _repositoryService = repositoryService;
    }

    /// <summary>
    /// Get a list of dishes (menu)
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">InternalServerError</response>
    [HttpGet]
    [Produces(AppConfigurations.ResponseContentType)]
    [ProducesResponseType(typeof(DishPagedListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get([FromQuery] DishCategory[]? categories = null, bool vegetarian = false,
        DishSorting? sorting = null, int? page = 1)
    {
        return Ok(await _dishService.GetAllDishes(categories, sorting, page ?? 1, vegetarian));
    }

    /// <summary>
    /// Get information about concrete dish
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">InternalServerError</response>
    [HttpGet, Route("{id:guid}")]
    [Produces(AppConfigurations.ResponseContentType)]
    [ProducesResponseType(typeof(DishDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(Guid id)
    {
        var dish = await _repositoryService.GetDish(id);
        return Ok(await _dishService.GetDishDto(dish));
    }

    /// <summary>
    /// Checks if user is able to set rating of the dish
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">InternalServerError</response>
    [HttpGet, Authorize(AppConfigurations.ActiveTokenPolicy), Route("{id:guid}/rating/check")]
    [Produces(AppConfigurations.ResponseContentType)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public IActionResult CheckReviewAccess(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return Ok(_dishService.CheckReviewAccess(id, userId));
    }

    /// <summary>
    /// Set a rating for a dish
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">InternalServerError</response>
    [HttpPost, Authorize(AppConfigurations.ActiveTokenPolicy), Route("{id:guid}/rating")]
    [Produces(AppConfigurations.ResponseContentType)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SetReview(Guid id, [Range(1, 10)] int ratingScore)
    {
        var user = await _repositoryService.GetUser(User);
        var dish = await _repositoryService.GetDish(id);
        if (!_dishService.CheckReviewAccess(id, user.Id)) return Forbid();
        
        await _dishService.SetReview(dish, user, ratingScore);
        return Ok();
    }
}