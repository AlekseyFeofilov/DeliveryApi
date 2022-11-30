using DeliveryAppAPI.Models;
using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Models.Response;
using DeliveryAppAPI.Services.JwtService;
using DeliveryAppAPI.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAppAPI.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/account")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;

    public UserController(IUserService userService, IJwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    /// <summary>
    /// Register new user
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">InternalServerError</response>
    [HttpPost, Route("/api/account/register")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register(UserRegisterModel model)
    {
        if (await _userService.IsRegistered(model.Email)) return BadRequest("Email is already in registered"); //todo: Add to custom validator
        _userService.Register(model);
        return await GetToken(new LoginCredentials(model.Email, model.Password));
    }
    
    /// <summary>
    /// Log in to the system
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">InternalServerError</response>
    [HttpPost, Route("login")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login(LoginCredentials credentials)
    {
        return await GetToken(credentials);
    }
    
    /// <summary>
    /// Log out system user
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="500">InternalServerError</response>
    [HttpPost, Authorize, Route("logout")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public IActionResult Logout()
    {
        return Ok();
        //Todo: add token blacklist
    }
    
    /// <summary>
    /// Get user profile
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="500">InternalServerError</response>
    [HttpGet, Authorize, Route("profile")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProfileInfo()
    {
        return Ok(_userService.GetProfileInfo(await _userService.GetUser(Request)));
    }
    
    /// <summary>
    /// Edit user Profile
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="500">InternalServerError</response>
    [HttpPut, Authorize, Route("profile")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditProfileInfo(UserEditModel model)
    {
        _userService.EditProfileInfo(model, await _userService.GetUser(Request));
        return Ok();
    }

    private async Task<IActionResult> GetToken(LoginCredentials credentials)
    {
        var identity = await _jwtService.GetIdentity(credentials);
        if (identity == null) return BadRequest("Invalid username or password");

        return Ok(new TokenResponse(_jwtService.GetToken(identity)));
    }
}