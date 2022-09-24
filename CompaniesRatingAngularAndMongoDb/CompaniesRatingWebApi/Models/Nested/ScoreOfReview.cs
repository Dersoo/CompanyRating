using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CompaniesRatingWebApi.Models.Nested;

public class ScoreOfReview
{
    [BsonElement("userId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = String.Empty;
    
    [BsonElement("isScorePositive")]
    public bool IsScorePositive { get; set; }
}