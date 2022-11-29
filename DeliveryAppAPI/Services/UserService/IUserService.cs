using DeliveryAppAPI.Models;
using DeliveryAppAPI.Models.DbSets;
using DeliveryAppAPI.Models.Dto;

namespace DeliveryAppAPI.Services.UserService;

public interface IUserService
{
    void Register(UserRegisterModel model);
    UserDto GetProfileInfo(User user);
    void EditProfileInfo(UserEditModel model, User user);
    Task<User?> GetUser(string email);
}