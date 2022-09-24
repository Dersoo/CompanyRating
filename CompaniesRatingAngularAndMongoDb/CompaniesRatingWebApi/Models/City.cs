using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using CompaniesRatingWebApi.Models.Nested;

namespace CompaniesRatingWebApi.Models;

[BsonIgnoreExtraElements]
public class City
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = String.Empty;
    
    [BsonElement("name")]
    public string Name { get; set; } = String.Empty;
    
    [BsonElement("country")]
    public string Country { get; set; } = String.Empty;
}