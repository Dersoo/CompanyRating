using MongoDB.Bson.Serialization.Attributes;

namespace CompaniesRatingWebApi.Models.Nested;

public class Assessments
{
    [BsonElement("salary")]
    public int Salary { get; set; }
    
    [BsonElement("office")]
    public int Office { get; set; }
    
    [BsonElement("education")]
    public int Education { get; set; }
    
    [BsonElement("career")]
    public int Career { get; set; }
    
    [BsonElement("community")]
    public int Community { get; set; }
}