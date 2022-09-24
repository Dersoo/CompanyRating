using CompaniesRatingWebApi.Models.DatabaseSettingsForStores;
using CompaniesRatingWebApi.Models.Nested;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CompaniesRatingWebApi.Services.LocationServices;

public class LocationService : ILocationService
{
    private readonly IMongoCollection<Location> _locations;
    
    public LocationService(IStoresDatabaseSettings settings, IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        _locations = database.GetCollection<Location>(settings.LocationCollectionName);
    }
    
    public List<string> GetAllCountries()
    {
        //return _locations.Find(location => location.Country).ToList();
        //return _locations.Find(location => true).Project(x => x.Country).ToList();
        
        var query = (from p in _locations.AsQueryable()
            select p.Country).Distinct().OrderBy(x => x);
        
        return query.ToList();
    }

    public List<Location> GetAllLocationsOfTheCountry(string countryName)
    {
        //return _locations.Find(location => location.Country == countryName).ToList();
        
        var query = (from p in _locations.AsQueryable()
            where p.Country == countryName
            orderby p.City
            select p);
        
        return query.ToList();
    }
}