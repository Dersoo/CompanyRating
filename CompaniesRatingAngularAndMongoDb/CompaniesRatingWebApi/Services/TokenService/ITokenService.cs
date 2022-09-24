using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CompaniesRatingWebApi.Services.TokenService;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    JwtSecurityToken Verify(string jwt);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}