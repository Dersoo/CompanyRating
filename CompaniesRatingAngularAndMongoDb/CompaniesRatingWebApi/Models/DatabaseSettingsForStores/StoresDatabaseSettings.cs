namespace CompaniesRatingWebApi.Models.DatabaseSettingsForStores;

public class StoresDatabaseSettings : IStoresDatabaseSettings
{
    public string CompanyCollectionName { get; set; } = String.Empty;
    public string UserCollectionName { get; set; } = String.Empty;
    public string ReviewCollectionName { get; set; } = String.Empty;
    public string LocationCollectionName { get; set; } = String.Empty;
    public string ConnectionString { get; set; } = String.Empty;
    public string DatabaseName { get; set; } = String.Empty;
}