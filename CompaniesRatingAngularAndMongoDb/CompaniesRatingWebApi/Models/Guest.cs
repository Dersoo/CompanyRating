using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompaniesRatingWebApi.Models;

[BsonIgnoreExtraElements]
public class Guest
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = String.Empty;

    [BsonElement("login")]
    public string Login { get; set; } = "Guest";
}