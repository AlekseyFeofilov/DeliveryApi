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

    public async Task<bool> Register(UserRegisterModel model)
    {
        try
        {
            await _context.Users.AddAsync(new User
            {
                FullName = model.FullName,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                Address = model.Address,
                Email = model.Email,
                Password = model.Password,
                PhoneNumber = model.PhoneNumber
            });

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            return false;
        }
    }

    public async Task<UserDto?> GetProfileInfo(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

        if (user == null)
        {
            return null;
        }

        return new UserDto(user.Id, user.FullName, user.BirthDate, user.Gender, user.Address, user.Email,
            user.PhoneNumber);
    }

    public async Task<bool> EditProfileInfo(UserEditModel model, string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

        if (user == null)
        {
            return false;
        }
        
        user.Address = model.Address;
        user.Gender = model.Gender;
        user.BirthDate = model.BirthDate;
        user.FullName = model.FullName;
        user.PhoneNumber = model.PhoneNumber;

        await _context.SaveChangesAsync();
        return true;
    }
}