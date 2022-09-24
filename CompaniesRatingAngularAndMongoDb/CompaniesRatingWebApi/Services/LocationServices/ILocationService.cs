using CompaniesRatingWebApi.Models.Nested;

namespace CompaniesRatingWebApi.Services.LocationServices;

public interface ILocationService
{
    List<string> GetAllCountries();

    List<Location> GetAllLocationsOfTheCountry(string countryName);
}