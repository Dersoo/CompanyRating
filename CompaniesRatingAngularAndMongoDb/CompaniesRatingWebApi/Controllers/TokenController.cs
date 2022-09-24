using CompaniesRatingWebApi.Models;
using CompaniesRatingWebApi.Services.TokenService;
using CompaniesRatingWebApi.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompaniesRatingWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    
    public TokenController(IUserService userService, ITokenService tokenService)
    {
        this._userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }
    
    [HttpPost]
    [Route("refresh")]
    public IActionResult Refresh(TokenApiModel tokenApiModel)
    {
        if (tokenApiModel is null)
        {
            return BadRequest("Invalid client request");
        }
            
        string accessToken = tokenApiModel.AccessToken;
        string refreshToken = tokenApiModel.RefreshToken;
        
        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        var username = principal.Identity.Name; //this is mapped to the Name claim by default
        var user = _userService.GetByLogin(username);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("Invalid client request");
        }
        
        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        
        user.RefreshToken = newRefreshToken;
        
        _userService.Update(user.Id, user);
        
        return Ok(new AuthenticatedResponse()
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }
    [HttpPost, Authorize]
    [Route("revoke")]
    public IActionResult Revoke()
    {
        var username = User.Identity.Name;
        
        var user = _userService.GetByLogin(username);

        if (user == null)
        {
            return BadRequest();
        }
        
        user.RefreshToken = null;
        
        _userService.Update(user.Id, user);
        
        return NoContent();
    }
}