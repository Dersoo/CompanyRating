namespace CompaniesRatingWebApi.Models;

public class Login
{
    public string UserLogin { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    
    public string? RefreshToken { get; set; }
    
    public DateTime RefreshTokenExpiryTime { get; set; }
}