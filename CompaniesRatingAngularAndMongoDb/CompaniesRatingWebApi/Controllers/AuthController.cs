using System.Security.Claims;
using CompaniesRatingWebApi.Models;
using CompaniesRatingWebApi.Services.UserServices;
using CompaniesRatingWebApi.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompaniesRatingWebApi.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthController(IUserService userService, IAuthService authService, IHttpContextAccessor httpContextAccessor)
    {
        this._userService = userService;
        this._authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    [HttpPost, Route("login")]
    public IActionResult Login([FromBody] Login? userToLogin)
    {
        if (userToLogin == null)
        {
            return BadRequest("Invalid client request");
        }

        AuthenticatedResponse authenticatedResponse = _authService.Login(userToLogin);
        
        if (authenticatedResponse != null)
        {
            return Ok(authenticatedResponse);
        }

        return Unauthorized();
    }
    
    [HttpGet, Route("loginasguest")]
    public IActionResult LoginAsGuest()
    {
        AuthenticatedResponse authenticatedResponse = _authService.RegisterAndLoginAsGuest();
        
        if (authenticatedResponse != null)
        {
            return Ok(authenticatedResponse);
        }

        return Unauthorized();
    }
    
    [HttpPost, Route("register")]
    public IActionResult Register([FromBody] User? user)
    {
        if (user == null)
        {
            return BadRequest("Invalid client request");
        }

        return Created("success", _authService.Register(user));
    }

    [HttpGet, Route("user")]
    [Authorize]
    public IActionResult GetAuthorizedUser()
    {
        try
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            
            var user = _userService.GetByLogin(userName);
    
            return Ok(user);
        }
        catch (Exception ex)
        {
            return Unauthorized();
        }
    }

    [HttpPost, Route("isloginexisting")]
    public bool IsLoginExisting([FromBody] LoginName login)
    {
        var dbUser = _userService.GetByLogin(login.Login);
        
        return (dbUser != null);
    }
}