namespace CompaniesRatingWebApi.Models.DatabaseSettingsForStores;

public interface IStoresDatabaseSettings
{
    string CompanyCollectionName { get; set; }
    string UserCollectionName { get; set; }
    string ReviewCollectionName { get; set; }
    string LocationCollectionName { get; set; }
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
}