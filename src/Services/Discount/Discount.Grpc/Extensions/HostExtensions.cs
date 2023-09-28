using Npgsql;

namespace Discount.Grpc.Extensions;

public static class HostExtensions
{
    public static IHost MigrateDatabase<TContext>(this IHost host, int retry = 0)
    {
        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;
        var configuration = services.GetRequiredService<IConfiguration>();
        var logger = services.GetRequiredService<ILogger<TContext>>();

        try
        {
            logger.LogInformation("Migrating postgresql database ...");
            using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "DROP TABLE IF EXISTS Coupons";
            command.ExecuteNonQuery();

            command.CommandText = @"CREATE TABLE Coupons(Id SERIAL PRIMARY KEY,
                                                        ProductName VARCHAR(24) NOT NULL,
                                                        Description TEXT,
                                                        Amount INT)";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Coupons (ProductName, Description, Amount) VALUES ('Product 1', 'Desc', 150)";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Coupons (ProductName, Description, Amount) VALUES ('Product 2', 'Desc', 100)";
            command.ExecuteNonQuery();

            logger.LogInformation("Migrating postgresql database done ...");
        }
        catch (NpgsqlException ex)
        {
            logger.LogError(ex, "An error occurred while migrating the postgres database.");

            if(retry < 5)
            {
                retry++;
                System.Threading.Thread.Sleep(2000);
                MigrateDatabase<TContext>(host, retry);
            }
        }

        return host;
    }
}
