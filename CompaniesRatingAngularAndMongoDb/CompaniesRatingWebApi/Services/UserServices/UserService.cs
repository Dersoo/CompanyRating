using CompaniesRatingWebApi.Models;
using CompaniesRatingWebApi.Models.DatabaseSettingsForStores;
using MongoDB.Driver;

namespace CompaniesRatingWebApi.Services.UserServices;

public class UserService : IUserService
{
    private readonly IMongoCollection<User> _users;
    private readonly IMongoCollection<Guest> _guests;

    public UserService(IStoresDatabaseSettings settings, IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        _users = database.GetCollection<User>(settings.UserCollectionName);
        _guests = database.GetCollection<Guest>(settings.UserCollectionName);
    }

    public List<User> Get()
    {
        return _users.Find(user => true).ToList();
    }

    public User Get(string id)
    {
        return _users.Find(user => user.Id == id).FirstOrDefault();
    }
    
    public User GetByLogin(string login)
    {
        return _users.Find(user => user.Login == login).FirstOrDefault();
    }

    public User Create(User user)
    {
        if (_users.Find(dbUser => dbUser.Login == user.Login).FirstOrDefault() != null)
        {
            return null;
        }
        else
        {
            _users.InsertOne(user);

            return _users.Find(dbUser => dbUser.Id == user.Id).FirstOrDefault();
        }
    }
    
    public Guest CreateGuest()
    {
        var newGuest = new Guest();
        
        _guests.InsertOne(newGuest);

        return newGuest;
    }

    public void Update(string id, User user)
    {
        _users.ReplaceOne(user => user.Id == id, user);
    }

    public void Remove(string id)
    {
        _users.DeleteOne(user => user.Id == id);
    }
}