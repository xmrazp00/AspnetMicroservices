﻿using Dapper;
using Discount.Grpc.Entities;
using Npgsql;

namespace Discount.Grpc.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;

    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        await using var connection =
            new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
            "select * from coupon where ProductName = @ProductName", new { ProductName = productName });

        if (coupon == null)
        {
            return new Coupon()
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount Desc"
            };

        }

        return coupon;
    }

    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        await using var connection = new NpgsqlConnection
            (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affected =
            await connection.ExecuteAsync
            ("INSERT INTO coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

        if (affected == 0)
            return false;

        return true;
    }


    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affected = await connection.ExecuteAsync
        ("UPDATE coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
            new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

        if (affected == 0)
            return false;

        return true;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affected = await connection.ExecuteAsync("DELETE FROM coupon WHERE ProductName = @ProductName",
            new { ProductName = productName });

        if (affected == 0)
            return false;

        return true;
    }
}