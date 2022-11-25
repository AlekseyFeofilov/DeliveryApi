using DeliveryAppAPI.DbContexts;
using DeliveryAppAPI.Models;
using DeliveryAppAPI.Models.DbSets;
using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DeliveryAppAPI.Services.DishServices;

public class DishServices : IDishService
{
    private readonly ApplicationDbContext _context;
    private const int PageSize = 5;

    public DishServices(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DishPagedListDto> GetAllDishes(DishCategory? category, DishSorting? sorting, int? page,
        bool vegetarian = false)
    {
        var dishes = _context.Dishes.AsQueryable();
        dishes = GetVegetarian(dishes, vegetarian);
        dishes = GetCategory(dishes, category);
        dishes = Sort(dishes, sorting);

        var notNUllPage = page ?? 1;
        var dishPage = await GetDishPage(dishes, notNUllPage);
        return GetDishPagedList(dishes, dishPage, notNUllPage);
    }

    public async Task<DishDto?> GetDish(Guid id)
    {
        var dish = await _context.Dishes
            .Include(x => x.Reviews)
            .SingleOrDefaultAsync(x => x.Id == id);
        
        if (dish == null) return null;
        return new DishDto(dish.Id, dish.Name, dish.Description, dish.Price, dish.Image, dish.Vegetarian,
            dish.Reviews.Average(r => r.Rating), dish.Category);
    }

    public Task<bool> CheckReviewAccess(Guid dishId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetReview(Guid dishId, Guid userId, int rating)
    {
        throw new NotImplementedException();
    }


    private DishPagedListDto GetDishPagedList(IQueryable<Dish> dishes, IEnumerable<DishDto> dishPage, int page)
    {
        return new DishPagedListDto(
            dishPage,
            new PageInfoModel(
                PageSize,
                (int)Math.Ceiling(dishes.Count() * 1.0 / PageSize),
                page
            )
        );
    }

    private async Task<IEnumerable<DishDto>> GetDishPage(IQueryable<Dish> dishes, int page)
    {
        var dishesPageCount = GetDishPageCount(dishes, page);

        return await dishes
            .Skip((page - 1) * PageSize)
            .Take(dishesPageCount)
            .Select(x =>
                new DishDto(x.Id, x.Name, x.Description, x.Price, x.Image, x.Vegetarian,
                    x.Reviews.Average(r => r.Rating), x.Category)
            )
            .ToListAsync();
    }

    private int GetDishPageCount(IQueryable<Dish> dishes, int page)
    {
        if (dishes.Count() < PageSize * page)
        {
            return Math.Max(0, dishes.Count() - PageSize * (page - 1));
        }

        return PageSize;
    }

    private IQueryable<Dish> GetVegetarian(IQueryable<Dish> dishes, bool available = true)
    {
        return available ? dishes.Where(x => x.Vegetarian) : dishes;
    }

    private IQueryable<Dish> GetCategory(IQueryable<Dish> dishes, DishCategory? category = null)
    {
        return category.HasValue ? dishes.Where(x => x.Category == category) : dishes;
    }

    private IQueryable<Dish> Sort(IQueryable<Dish> dishes, DishSorting? sorting = null)
    {
        return sorting switch
        {
            DishSorting.NameAsc => dishes.OrderBy(x => x.Name),
            DishSorting.NameDesc => dishes.OrderByDescending(x => x.Name),
            DishSorting.PriceAsc => dishes.OrderBy(x => x.Price),
            DishSorting.PriceDesc => dishes.OrderByDescending(x => x.Price),
            DishSorting.RatingAsc => dishes.OrderBy(x => x.Reviews.Average(r => r.Rating)),
            DishSorting.RatingDesc => dishes.OrderByDescending(x => x.Reviews.Average(r => r.Rating)),
            null => dishes,
            _ => throw new ArgumentOutOfRangeException(nameof(sorting), sorting, null)
        };
    }
}