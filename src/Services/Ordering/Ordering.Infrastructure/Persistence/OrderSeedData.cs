using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderSeedData
{
    public static async Task SeedAsync(
        OrderDbContext dbContext,
        ILogger<OrderSeedData> logger)
    {
        if(!dbContext.Orders.Any())
        {
            dbContext.Orders.AddRange(GetPreconfiguredOrders());
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderDbContext).Name);
        }
    }

    private static IEnumerable<Order> GetPreconfiguredOrders()
    {
        return new List<Order>
            {
                new Order() 
                {
                    UserName = "swn", 
                    FirstName = "Mehmet", 
                    LastName = "Ozkaya", 
                    EmailAddress = "ezozkme@gmail.com", 
                    AddressLine = "Bahcelievler", 
                    Country = "Turkey", 
                    TotalPrice = 350 
                }
            };
    }
}
