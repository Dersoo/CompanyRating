using CompaniesRatingWebApi.Models;

namespace CompaniesRatingWebApi.Services.UserServices;

public interface IUserService
{
    List<User> Get();

    User Get(string id);
    
    User GetByLogin(string login);

    User Create(User user);

    Guest CreateGuest();

    void Update(string id, User user);

    void Remove(string id);
}