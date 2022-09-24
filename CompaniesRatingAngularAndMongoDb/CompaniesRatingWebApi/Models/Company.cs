using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using CompaniesRatingWebApi.Models.Nested;

namespace CompaniesRatingWebApi.Models;

[BsonIgnoreExtraElements]
public class Company
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = String.Empty;
    
    [BsonElement("name")]
    [Required]
    public string Name { get; set; } = String.Empty;
    
    [BsonElement("rating")]
    public double Rating { get; set; }
    
    [BsonElement("location")]
    public Location? Location { get; set; }
    
    [BsonElement("description")]
    public string Description { get; set; } = String.Empty;
}