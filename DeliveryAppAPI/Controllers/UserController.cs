using DeliveryAppAPI.Configurations;
using DeliveryAppAPI.Exceptions;
using DeliveryAppAPI.Models;
using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Models.Response;
using DeliveryAppAPI.Services.JwtService;
using DeliveryAppAPI.Services.RepositoryService;
using DeliveryAppAPI.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace DeliveryAppAPI.Controllers;

[ApiController]
[Produces(AppConfigurations.ResponseContentType)]
[Route("api/account")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IRepositoryService _repositoryService;
    private readonly IJwtService _jwtService;
    private readonly IDistributedCache _redis;

    public UserController(IUserService userService, IJwtService jwtService, IConnectionMultiplexer redis,
        IDistributedCache redis1, IRepositoryService repositoryService)
    {
        _userService = userService;
        _jwtService = jwtService;
        _redis = redis1;
        _repositoryService = repositoryService;
    }

    /// <summary>
    /// Register new user
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">InternalServerError</response>
    [HttpPost, Route("/api/account/register")]
    [Produces(AppConfigurations.ResponseContentType)]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register(UserRegisterModel model)
    {
        if (await _userService.IsRegistered(model.Email)) return BadRequest(ErrorMessage.RegisteredEmail);
        await _userService.Register(model);
        
        return await GetToken(new LoginCredentials(model.Email, model.Password));
    }

    /// <summary>
    /// Log in to the system
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">InternalServerError</response>
    [HttpPost, Route("login")]
    [Produces(AppConfigurations.ResponseContentType)]
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
    [HttpPost, Authorize, Authorize(AppConfigurations.ActiveTokenPolicy), Route("logout")]
    [Produces(AppConfigurations.ResponseContentType)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Logout()
    {
        string? authHeader = Request.Headers[StringConstance.Authorization];
        if (authHeader == null) throw new UnauthorizedException();

        await _redis.SetRecordAsync(authHeader, "sdfsdfdsf",//todo 
            new TimeSpan(0, JwtConfigurations.Lifetime, 0),
            new TimeSpan(0, JwtConfigurations.Lifetime, 0));
        return Ok();
    }

    /// <summary>
    /// Get user profile
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="500">InternalServerError</response>
    [HttpGet, Authorize, Authorize(AppConfigurations.ActiveTokenPolicy), Route("profile")]
    [Produces(AppConfigurations.ResponseContentType)]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProfileInfo()
    {
        return Ok(_userService.GetProfileInfo(await _repositoryService.GetUser(User)));
    }

    /// <summary>
    /// Edit user Profile
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="500">InternalServerError</response>
    [HttpPut, Authorize, Authorize(AppConfigurations.ActiveTokenPolicy), Route("profile")]
    [Produces(AppConfigurations.ResponseContentType)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditProfileInfo(UserEditModel model)
    {
        _userService.EditProfileInfo(model, await _repositoryService.GetUser(User));
        return Ok();
    }

    private async Task<IActionResult> GetToken(LoginCredentials credentials)
    {
        var identity = await _jwtService.GetIdentity(credentials);
        if (identity == null) return BadRequest(ErrorMessage.InvalidUserNameOrPassword);

        return Ok(new TokenResponse(_jwtService.GetToken(identity)));
    }
}