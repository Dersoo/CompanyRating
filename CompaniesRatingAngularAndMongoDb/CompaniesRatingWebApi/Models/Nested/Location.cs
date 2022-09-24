using MongoDB.Bson.Serialization.Attributes;

namespace CompaniesRatingWebApi.Models.Nested;

[BsonIgnoreExtraElements]
public class Location
{
    // [BsonId]
    // [BsonRepresentation(BsonType.ObjectId)]
    // public string Id { get; set; } = String.Empty;
    
    [BsonElement("city")]
    public string City { get; set; } = String.Empty;
    
    [BsonElement("country")]
    public string Country { get; set; } = String.Empty;
}