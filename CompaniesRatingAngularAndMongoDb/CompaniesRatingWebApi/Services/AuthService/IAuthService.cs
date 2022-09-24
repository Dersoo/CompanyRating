using CompaniesRatingWebApi.Models;

namespace CompaniesRatingWebApi.Services.AuthService;

public interface IAuthService
{
   AuthenticatedResponse Login (Login userToLogin);
   User Register (User user);
   AuthenticatedResponse RegisterAndLoginAsGuest();
}