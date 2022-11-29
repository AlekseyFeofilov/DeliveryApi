using DeliveryAppAPI.DbContexts;
using DeliveryAppAPI.Models;
using DeliveryAppAPI.Models.DbSets;
using DeliveryAppAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAppAPI.Services.UserService;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
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

        _context.SaveChanges();
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

        _context.SaveChanges();
    }

    public async Task<User?> GetUser(string email)
    {
        return await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
    }
}