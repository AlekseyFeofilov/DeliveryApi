using System.Security.Claims;
using DeliveryAppAPI.Models;
using DeliveryAppAPI.Models.Response;
using DeliveryAppAPI.Services.JwtService;
using DeliveryAppAPI.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAppAPI.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;
    private readonly IJwtClaimService _jwtClaimService;

    public UserController(IUserService userService, IJwtService jwtService, IJwtClaimService jwtClaimService)
    {
        _userService = userService;
        _jwtService = jwtService;
        _jwtClaimService = jwtClaimService;
    }

    [HttpPost]
    [Route("/api/account/register")]
    public async Task<IActionResult> Register(UserRegisterModel model)
    {
        if (!await _userService.Register(model))
        {
            return BadRequest("Email is already in registered"); //todo: Add to custom validator
        }

        return await GetToken(new LoginCredentials(model.Email, model.Password));
    }
    
    [HttpPost]
    [Route("/api/account/login")]
    public async Task<IActionResult> Login(LoginCredentials credentials)
    {
        return await GetToken(credentials);
    }
    
    [HttpPost]
    [Authorize]
    [Route("/api/account/logout")]
    public IActionResult Logout()
    {
        return Ok();
        //Todo: add token blacklist
    }
    
    [HttpGet]
    [Authorize]
    [Route("/api/account/profile")]
    public async Task<IActionResult> GetProfileInfo()
    {
        var email = _jwtClaimService.GetClaimValue(ClaimTypes.Email, Request);
        var user = await _userService.GetProfileInfo(email);

        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(user);
    }
    
    [HttpPut]
    [Authorize]
    [Route("/api/account/profile")]
    public async Task<IActionResult> EditProfileInfo(UserEditModel model)
    {
        var email = _jwtClaimService.GetClaimValue(ClaimTypes.Email, Request);

        if (!await _userService.EditProfileInfo(model, email))
        {
            return StatusCode(500, Response);
        }

        return Ok();
    }

    private async Task<IActionResult> GetToken(LoginCredentials credentials)
    {
        var identity = await _jwtService.GetIdentity(credentials);
        
        if (identity == null)
        {
            return BadRequest(new { errorText = "Invalid username or password" });
        }
        
        return Ok(new TokenResponse(_jwtService.GetToken(identity)));
    }
}