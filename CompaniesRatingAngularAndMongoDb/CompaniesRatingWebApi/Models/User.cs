using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompaniesRatingWebApi.Models;

[BsonIgnoreExtraElements]
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = String.Empty;
    
    [BsonElement("firstName")]
    public string FirstName { get; set; } = String.Empty;
    
    [BsonElement("lastName")]
    public string LastName { get; set; } = String.Empty;
    
    [BsonElement("email")]
    public string Email { get; set; } = String.Empty;
    
    [BsonElement("age")]
    public int Age { get; set; }
    
    [BsonElement("login")]
    public string Login { get; set; } = String.Empty;
    
    [BsonElement("password")]
    public string Password { get; set; } = String.Empty;
    
    [BsonElement("isAdmin")]
    public bool IsAdmin { get; set; }
    
    public string? RefreshToken { get; set; }
    
    public DateTime RefreshTokenExpiryTime { get; set; }
}