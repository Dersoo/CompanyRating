using System;
using System.Collections.Generic;
using System.Data;
using CompaniesRatingWebApi.Controllers;
using CompaniesRatingWebApi.Models;
using CompaniesRatingWebApi.Models.Nested;
using CompaniesRatingWebApi.Services.CompanyServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace CompaniesRatingWebApiUnitTests.ControllerTests;

public class CompanyControllerTest
{
    private readonly Mock<ICompanyService> _companyServiceMock;
    private readonly Mock<ILogger<CompanyController>> _loggerMock;
    private readonly CompanyController _controller;

    public CompanyControllerTest()
    {
        _companyServiceMock = new Mock<ICompanyService>();
        _loggerMock = new Mock<ILogger<CompanyController>>();
        _controller = new CompanyController(_companyServiceMock.Object, _loggerMock.Object);
    }
    
    [Fact]
    public void Get_WhenCalled_ReturnsOkResult()
    {
        var okResult = _controller.Get();

        Assert.IsType<ActionResult<List<Company>>>(okResult);
    }
    
    [Fact]
    public void Get_ActionExecutes_ReturnsExactNumberOfCompanies()
    {
        _companyServiceMock.Setup(service => service.Get())
            .Returns(new List<Company>() { new Company(), new Company() });
        
        var result = _controller.Get();
        
        var actionResult = Assert.IsType<ActionResult<List<Company>>>(result);
        var companies = Assert.IsType<List<Company>>(actionResult.Value);
        
        Assert.Equal(2, companies.Count);
    }
    
    [Fact]
    public void GetByID_UnknownIdPassed_ReturnsNotFoundResult()
    {
        var companiesList = GetCompaniesData();

        _companyServiceMock.Setup(x => x.Get("11d006d011732i57960d0991"))
            .Returns((Company) null);
        
        var actionResult = _controller.Get("11d006d011732i57960d0991");
        
        Assert.IsType<NotFoundObjectResult>(actionResult.Result);
    }
    
    [Fact]
    public void GetByID_ExistingIdPassed_ReturnsRightItemAndOkResult()
    {
        var companiesList = GetCompaniesData();
        
        _companyServiceMock.Setup(x => x.Get("62d006d012732b57960d8991"))
            .Returns(companiesList[0]);

        var actionResult = _controller.Get("62d006d012732b57960d8991");
        
        Assert.NotNull(actionResult);
        Assert.Equal(companiesList[0].Id, actionResult.Value.Id);
        Assert.True(companiesList[0].Id == actionResult.Value.Id);
        Assert.IsType<ActionResult<Company>>(actionResult);
    }
    
    [Fact]
    public void Post_InvalidObjectPassed_ReturnsBadRequest()
    {
        var nameMissingItem = new Company()
        {
            Rating = 5.525,
            Location = new Location
            {
                Country  = "Country1",
                City = "City1"
            },
            Description = "Description for Company10"
        };
        
        _controller.ModelState.AddModelError("Name", "Required");
    
        var badResponse = _controller.Post(nameMissingItem);
    
        Assert.IsType<BadRequestObjectResult>(badResponse);
    }
    
    [Fact]
    public void Post_ValidObjectPassed_ReturnsCreatedResponse()
    {
        var testItem = new Company()
        {
            Name = "Company1",
            Rating = 5.525,
            Location = new Location
            {
                Country  = "Country1",
                City = "City1"
            },
            Description = "Description for Company10"
        };
        
        _companyServiceMock.Setup(x => x.Create(testItem))
            .Returns(new Company(){
                Id = "62d006d002732b57960d0911",
                Name = "Company25",
                Rating = 5.525,
                Location = new Location
                {
                    Country  = "Country1",
                    City = "City1"
                },
                Description = "Description for Company10"
            });
        _companyServiceMock.Setup(x => x.Get("62d006d002732b57960d0911"))
            .Returns(new Company(){
                Id = "62d006d002732b57960d0911",
                Name = "Company25",
                Rating = 5.525,
                Location = new Location
                {
                    Country  = "Country1",
                    City = "City1"
                },
                Description = "Description for Company10"
            });
        
        var createdResponse = _controller.Post(testItem);
        
        Assert.IsType<CreatedAtActionResult>(createdResponse);
    }
    
    [Fact]
    public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
    {
        var testItem = new Company()
        {
            Name = "Company25",
            Rating = 5.525,
            Location = new Location
            {
                Country  = "Country1",
                City = "City1"
            },
            Description = "Description for Company10"
        };
        
        _companyServiceMock.Setup(x => x.Create(testItem))
            .Returns(new Company(){
                Id = "62d006d002732b57960d0911",
                Name = "Company25",
                Rating = 5.525,
                Location = new Location
                {
                    Country  = "Country1",
                    City = "City1"
                },
                Description = "Description for Company10"
            });
        _companyServiceMock.Setup(x => x.Get("62d006d002732b57960d0911"))
            .Returns(new Company(){
                Id = "62d006d002732b57960d0911",
                Name = "Company25",
                Rating = 5.525,
                Location = new Location
                {
                    Country  = "Country1",
                    City = "City1"
                },
                Description = "Description for Company10"
            });

        var createdResponse = _controller.Post(testItem) as CreatedAtActionResult;
        
        var item = createdResponse.Value as Company;
        
        Assert.IsType<Company>(item);
        Assert.Equal("Company25", item.Name);
    }
    
    //Utility
    private List<Company> GetCompaniesData()
    {
        List<Company> companies = new List<Company>
        {
            new Company
            {
                Id = "62d006d012732b57960d8991",
                Name = "Company9",
                Rating = 2.125,
                Location = new Location
                {
                  Country  = "Country1",
                  City = "City1"
                },
                Description = "Description for Company9"
            },
            new Company
            {
                Id = "52d006d012732b59960d8991",
                Name = "Company10",
                Rating = 5.125,
                Location = new Location
                {
                    Country  = "Country1",
                    City = "City1"
                },
                Description = "Description for Company10"
            },
            new Company
            {
                Id = "62d006d012732b51960d8992",
                Name = "Company11",
                Rating = 1.121,
                Location = new Location
                {
                    Country  = "Country1",
                    City = "City1"
                },
                Description = "Description for Company11"
            },
        };
        
        return companies;
    }
}