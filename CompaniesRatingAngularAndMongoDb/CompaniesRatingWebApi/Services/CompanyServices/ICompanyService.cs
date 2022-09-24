using CompaniesRatingWebApi.Models;

namespace CompaniesRatingWebApi.Services.CompanyServices;

public interface ICompanyService
{
    List<Company> Get();

    Company Get(string id);

    Company Create(Company company);

    void Update(string id, Company company);

    void Remove(string id);
}