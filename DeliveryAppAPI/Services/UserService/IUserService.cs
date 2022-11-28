using DeliveryAppAPI.Models;
using DeliveryAppAPI.Models.Dto;

namespace DeliveryAppAPI.Services.UserService;

public interface IUserService
{
    Task<bool> Register(UserRegisterModel model);
    Task<UserDto?> GetProfileInfo(string email);
    Task<bool> EditProfileInfo(UserEditModel model, string email);
}