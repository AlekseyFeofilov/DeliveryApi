using AutoMapper;
using DeliveryAppAPI.Configurations;
using DeliveryAppAPI.DbContexts;
using DeliveryAppAPI.Exceptions;
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
    private readonly IMapper _mapper;
    private const int PageSize = AppConfigurations.PageSize;

    public DishServices(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DishPagedListDto> GetAllDishes(DishCategory[]? category, DishSorting? sorting, int page,
        bool vegetarian = false)
    {
        var dishes = _context.Dishes.AsQueryable();
        dishes = GetVegetarian(dishes, vegetarian);
        dishes = GetCategory(dishes, category);
        dishes = Sort(dishes, sorting);

        var dishPage = await GetDishPage(dishes, page);
        return GetDishPagedList(dishes, dishPage, page);
    }

    private static IQueryable<Dish> GetVegetarian(IQueryable<Dish> dishes, bool available = true)
    {
        return available ? dishes.Where(x => x.Vegetarian) : dishes;
    }

    private static IQueryable<Dish> GetCategory(IQueryable<Dish> dishes, DishCategory[]? category = null)
    {
        return !category.IsNullOrEmpty()
            ? dishes.Where(x => category != null && category.Contains(x.Category))
            : dishes;
    }

    private static IQueryable<Dish> Sort(IQueryable<Dish> dishes, DishSorting? sorting = null)
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

    private static async Task<IEnumerable<DishDto>> GetDishPage(IQueryable<Dish> dishes, int page)
    {
        var dishesPageCount = GetDishPageCount(dishes, page);

        return await dishes
            .Skip((page - 1) * PageSize)
            .Take(dishesPageCount)
            .Select(x => new DishDto(
                x.Id,
                x.Name,
                x.Description,
                x.Price,
                x.Image,
                x.Vegetarian,
                x.Reviews.Average(r => r.Rating),
                x.Category
            ))
            .ToListAsync();
    }

    private static int GetDishPageCount(IQueryable<Dish> dishes, int page)
    {
        if (dishes.Count() < PageSize * page)
        {
            return Math.Max(0, dishes.Count() - PageSize * (page - 1));
        }

        return PageSize;
    }

    private static DishPagedListDto GetDishPagedList(IQueryable<Dish> dishes, IEnumerable<DishDto> dishPage, int page)
    {
        var pageCount = (int)Math.Ceiling(dishes.Count() * 1.0 / PageSize);
        var dishCount = page < pageCount ? PageSize : Math.Max(0, dishes.Count() - PageSize * (page - 1));
        if (dishCount == 0) throw new NotFoundException();

        return new DishPagedListDto(
            dishPage,
            new PageInfoModel(
                dishCount,
                pageCount,
                page
            )
        );
    }

    public async Task<DishDto> GetDishDto(Dish dish)
    {
        var reviews = await GetReviews(dish.Id);

        return new DishDto(
            dish.Id,
            dish.Name,
            dish.Description,
            dish.Price,
            dish.Image,
            dish.Vegetarian,
            reviews.IsNullOrEmpty() ? null : reviews.Average(r => r.Rating),
            dish.Category
        );
    }

    public bool CheckReviewAccess(Guid dishId, Guid userId)
    {
        return _context.Orders
            .Where(x => x.User.Id == userId)
            .Any(order => order.DishBaskets
                .Any(dishBasket => dishBasket.Dish.Id == dishId)
            );
    }

    public async Task SetReview(Dish dish, User user, int rating)
    {
        SetReview(await GetReview(dish, user), dish, user, rating);
    }

    private async Task<Review?> GetReview(Dish dish, User user)
    {
        return await _context.Reviews
            .SingleOrDefaultAsync(x =>
                x.User != null
                && x.Dish.Id == dish.Id
                && x.User.Id == user.Id
            );
    }


    private async Task<IEnumerable<Review>> GetReviews(Guid dishId)
    {
        return await _context.Reviews.Where(x => x.Dish.Id == dishId).ToListAsync();
    }

    private void SetReview(Review? review, Dish dish, User user, int rating)
    {
        if (review != null)
        {
            review.Rating = rating;
        }
        else
        {
            _context.Reviews.Add(CreateReview(dish, user, rating));
        }

        _context.SaveChangesAsync();
    }

    private Review CreateReview(Dish dish, User user, int rating)
    {
        return new Review
        {
            Id = Guid.NewGuid(),
            Dish = dish,
            User = user,
            Rating = rating
        };
    }
}