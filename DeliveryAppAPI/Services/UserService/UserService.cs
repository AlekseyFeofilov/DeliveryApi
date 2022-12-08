using AutoMapper;
using DeliveryAppAPI.DbContexts;
using DeliveryAppAPI.Helpers;
using DeliveryAppAPI.Models;
using DeliveryAppAPI.Models.DbSets;
using DeliveryAppAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAppAPI.Services.UserService;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UserService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task Register(UserRegisterModel model)
    {
        var password = model.Password;
        model.Password = HashHelper.HashPassword(model.Password);
        await _context.Users.AddAsync(_mapper.Map<User>(model));

        model.Password = password;
        await _context.SaveChangesAsync();
    }

    public UserDto GetProfileInfo(User user)
    {
        return _mapper.Map<UserDto>(user);
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

    public async Task<bool> IsRegistered(string email)
    {
        return await _context.Users.SingleOrDefaultAsync(x => x.Email == email) != null;
    }
}