using CompaniesRatingWebApi.Models.Nested;
using CompaniesRatingWebApi.Services.LocationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompaniesRatingWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;
    
    public LocationController(ILocationService locationService)
    {
        this._locationService = locationService;
    }

    [HttpGet]
    [Authorize]
    public ActionResult<List<string>> Get()
    {
        return _locationService.GetAllCountries();
    }
    
    [HttpGet("{countryName}")]
    [Authorize]
    public ActionResult<List<Location>> Get(string countryName)
    {
        return _locationService.GetAllLocationsOfTheCountry(countryName);
    }
}