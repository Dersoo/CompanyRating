using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using CompaniesRatingWebApi.Models.Nested;

namespace CompaniesRatingWebApi.Models;

[BsonIgnoreExtraElements]
public class Review
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = String.Empty;
    
    [BsonElement("userId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = String.Empty;
    
    [BsonElement("companyId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CompanyId { get; set; } = String.Empty;
    
    [BsonElement("dateOfReview")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime DateOfReview { get; set; }

    [BsonElement("assessments")]
    public Assessments? Assessments { get; set; }
    
    [BsonElement("comment")]
    public string Comment { get; set; } = String.Empty;
    
    [BsonElement("scores")]
    public List<ScoreOfReview>? Scores { get; set; }
    
    [BsonElement("countOfLikes")]
    public int CountOfLikes { get; set; }
    
    [BsonElement("countOfDislikes")]
    public int CountOfDislikes { get; set; }
    
    [BsonElement("isDisabled")]
    public bool IsDisabled { get; set; }
}