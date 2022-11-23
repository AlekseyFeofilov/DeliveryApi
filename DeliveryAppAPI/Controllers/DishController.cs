using System.Diagnostics;
using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Models.Enums;
using DeliveryAppAPI.Services.DishServices;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAppAPI.Controllers;

[ApiController]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishController(IDishService dishService)
    {
        _dishService = dishService;
    }

    [HttpGet]
    [Route("/api/dish")]
    public async Task<DishPagedListDto> Get(DishCategory? categories, DishSorting? sorting, int? page, bool vegetarian)
    {
        return await _dishService.GetAllDishes(categories, sorting, page, vegetarian);
    }
}