using CompaniesRatingWebApi.Models;
using CompaniesRatingWebApi.Services.CompanyServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompaniesRatingWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;
    private readonly ILogger<CompanyController> _logger;
    
    public CompanyController(ICompanyService companyService,
                             ILogger<CompanyController> logger)
    {
        this._companyService = companyService;
        this._logger = logger;
    }

    [HttpGet]
    [Authorize]
    public ActionResult<List<Company>> Get()
    {
        _logger.LogInformation("[Get] method of [CompanyController] executing...");
        
        return _companyService.Get();
    }
    
    [HttpGet("{id}")]
    [Authorize]
    public ActionResult<Company> Get(string id)
    {
        _logger.LogInformation("[Get by id] method of [CompanyController] executing...");
        
        var company = _companyService.Get(id);
        
        if (company == null)
        {
            return NotFound($"Company with id = {id} not found");
        }

        return company;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Post([FromBody]Company company)
    {
        _logger.LogInformation("[Post] method of [CompanyController] executing...");
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        _companyService.Create(company);

        return CreatedAtAction(nameof(Get), new { id = company.Id }, company);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult Put(string id, [FromBody]Company company)
    {
        _logger.LogInformation("[Put] method of [CompanyController] executing...");
        
        var existingCompany = _companyService.Get(id);

        if (existingCompany == null)
        {
            return NotFound($"Company with id = {id} not found");
        }
        
        _companyService.Update(id, company);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult Delete(string id)
    {
        _logger.LogInformation("[Delete] method of [CompanyController] executing...");
        
        var existingCompany = _companyService.Get(id);

        if (existingCompany == null)
        {
            return NotFound($"Company with id = {id} not found");
        }
        
        _companyService.Remove(existingCompany.Id);
        
        return Ok(existingCompany);
    }
}