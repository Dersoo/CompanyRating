using System.Security.Claims;
using CompaniesRatingWebApi.Models;
using CompaniesRatingWebApi.Services.UserServices;
using CompaniesRatingWebApi.Services.TokenService;
using MongoDB.Bson;
using BCryptNet = BCrypt.Net.BCrypt;

namespace CompaniesRatingWebApi.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    
    public AuthService(IUserService userService, ITokenService tokenService)
    {
        this._userService = userService;
        this._tokenService = tokenService;
    }

    public AuthenticatedResponse Login(Login userToLogin)
    {
        var dbUser = _userService.GetByLogin(userToLogin.UserLogin);

        if (dbUser != null && BCryptNet.Verify(userToLogin.Password, dbUser.Password))
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, dbUser.Id),
                new Claim(ClaimTypes.Name, dbUser.Login),
                new Claim(ClaimTypes.Role, dbUser.IsAdmin? "Admin" : "User")
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            dbUser.RefreshToken = refreshToken;
            dbUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            
            _userService.Update(dbUser.Id, dbUser);

            return (new AuthenticatedResponse
            {
                Token = accessToken,
                RefreshToken = refreshToken
            });
        }

        return null;
    }

    public User Register(User user)
    {
        user.Password = BCryptNet.HashPassword(user.Password);

        return _userService.Create(user);
    }
    
    public AuthenticatedResponse RegisterAndLoginAsGuest()
    {
        var dbGuest = _userService.CreateGuest();

        if (dbGuest.Id != "")
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, dbGuest.Id),
                new Claim(ClaimTypes.Name, dbGuest.Login),
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);

            return (new AuthenticatedResponse
            {
                Token = accessToken,
                RefreshToken = null
            });
        }

        return null;
    }
}