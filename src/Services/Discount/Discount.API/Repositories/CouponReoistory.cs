using Discount.API.Entities;
using Npgsql;
using Dapper;

namespace Discount.API.Repositories;

public class CouponReoistory : ICouponReoistory
{
    private readonly IConfiguration _configuration;

    public CouponReoistory(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        using var connection = new NpgsqlConnection(
            _configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affected = await connection.ExecuteAsync(
            "INSERT INTO Coupons (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
            new { coupon.ProductName, coupon.Description, coupon.Amount });

        if(affected == 0) return false;

        return true;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        using var connection = new NpgsqlConnection(
            _configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affected = await connection.ExecuteAsync(
            $"DELETE FROM Coupons WHERE ProductName = @ProductName",
            new { ProductName = productName });

        if (affected == 0) return false;

        return true;
    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        using var connection = new NpgsqlConnection(
            _configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
            $"SELECT * FROM Coupons WHERE ProductName = @ProductName",
            new { ProductName = productName });

        if(coupon is null)
        {
            return new()
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount Discription"
            };
        }

        return coupon;
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        using var connection = new NpgsqlConnection(
            _configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affected = await connection.ExecuteAsync(
            "UPDATE Coupons SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
            new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });

        if (affected == 0) return false;

        return true;
    }
}
