using Catalog.API.Entities;
using Catalog.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.API.Data;

public class CatalogContext : ICatalogContext
{
    private readonly DatabaseSettings _databaseSettings;

    public CatalogContext(IOptions<DatabaseSettings> option)
    {
        _databaseSettings = option.Value;

        var client = new MongoClient(_databaseSettings.ConnectionString);
        var database = client.GetDatabase(_databaseSettings.DatabaseName);

        Products = database.GetCollection<Product>(_databaseSettings.CollectionName);

        DataSeed.Seed(Products);
    }

    public IMongoCollection<Product> Products { get; }
}
