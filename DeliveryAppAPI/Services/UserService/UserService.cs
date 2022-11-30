using System.Security.Claims;
using DeliveryAppAPI.DbContexts;
using DeliveryAppAPI.Exceptions;
using DeliveryAppAPI.Models;
using DeliveryAppAPI.Models.DbSets;
using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Services.JwtService;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAppAPI.Services.UserService;

public class UserService : IUserService //todo separate service into a few interfaces
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtClaimService _jwtClaimService;

    public UserService(ApplicationDbContext context, IJwtClaimService jwtClaimService)
    {
        _context = context;
        _jwtClaimService = jwtClaimService;
    }

    public void Register(UserRegisterModel model)
    {
        _context.Users.Add(new User
        {
            FullName = model.FullName,
            BirthDate = model.BirthDate,
            Gender = model.Gender,
            Address = model.Address,
            Email = model.Email,
            Password = model.Password,
            PhoneNumber = model.PhoneNumber
        });

        _context.SaveChangesAsync();
    }

    public UserDto GetProfileInfo(User user)
    {
        return new UserDto(user.Id, user.FullName, user.BirthDate, user.Gender, user.Address, user.Email,
            user.PhoneNumber);
    }

    public void EditProfileInfo(UserEditModel model, User user)
    {
        user.Address = model.Address;
        user.Gender = model.Gender;
        user.BirthDate = model.BirthDate;
        user.FullName = model.FullName;
        user.PhoneNumber = model.PhoneNumber;

        _context.SaveChangesAsync();
    }

    public async Task<User?> GetUser(Guid id)
    {
        return await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User> GetUser(HttpRequest request)
    {
        var userId = _jwtClaimService.GetIdClaim(request);
        var user = await GetUser(userId);
        
        if (user == null) throw new UnauthorizedException();
        return user;
    }

    public async Task<bool> IsRegistered(string email)
    {
        return await _context.Users.SingleOrDefaultAsync(x => x.Email == email) != null;
    }
}