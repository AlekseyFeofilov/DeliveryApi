using System.Security.Claims;
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

    /// <summary>
    /// Register new user
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">InternalServerError</response>
    [HttpPost]
    [Route("/api/account/register")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register(UserRegisterModel model)
    {
        if (!await _userService.Register(model))
        {
            return BadRequest("Email is already in registered"); //todo: Add to custom validator
        }

        return await GetToken(new LoginCredentials(model.Email, model.Password));
    }
    
    /// <summary>
    /// Log in to the system
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">InternalServerError</response>
    [HttpPost]
    [Route("/api/account/login")]
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
    /// <remarks>**Need Authorization**</remarks>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">InternalServerError</response>
    [HttpPost]
    [Authorize]
    [Route("/api/account/logout")]
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
    /// <remarks>**Need Authorization**</remarks>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">InternalServerError</response>
    [HttpGet]
    [Authorize]
    [Route("/api/account/profile")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
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
    
    /// <summary>
    /// Edit user Profile
    /// </summary>
    /// <remarks>**Need Authorization**</remarks>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">InternalServerError</response>
    [HttpPut]
    [Authorize]
    [Route("/api/account/profile")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
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