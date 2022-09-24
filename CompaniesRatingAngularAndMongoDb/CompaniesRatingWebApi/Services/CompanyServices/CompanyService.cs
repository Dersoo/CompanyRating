using CompaniesRatingWebApi.Models;
using CompaniesRatingWebApi.Models.DatabaseSettingsForStores;
using CompaniesRatingWebApi.Models.Nested;
using MongoDB.Driver;

namespace CompaniesRatingWebApi.Services.CompanyServices;

public class CompanyService : ICompanyService
{
    private readonly IMongoCollection<Company> _companies;

    public CompanyService(IStoresDatabaseSettings settings, IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        _companies = database.GetCollection<Company>(settings.CompanyCollectionName);

        //GenerateCompanies("YCompany", 1, 100000);
    }

    public List<Company> Get()
    {
        return _companies.Find(company => true).SortByDescending(company=>company.Rating).ToList();
    }

    public Company Get(string id)
    {
        return _companies.Find(company => company.Id == id).FirstOrDefault();
    }

    public Company Create(Company company)
    {
        _companies.InsertOne(company);

        return company;
    }

    public void Update(string id, Company company)
    {
        _companies.ReplaceOne(company => company.Id == id, company);
    }

    public void Remove(string id)
    {
        _companies.DeleteOne(company => company.Id == id);
    }
    
    /*--Utility--*/
    private void GenerateCompanies(string nameRoot, int startIndex, int count)
    {
        for (var i = startIndex; i <= count; i++)
        {
            _companies.InsertOne(
            new Company {
                Name = nameRoot + '_' + i,
                Rating = i / 10,
                Location = new Location{
                    City = nameRoot + "_city",
                    Country = nameRoot + "_country"
                }
            });
        }
    }
}