using Dapper;
using Discount.Grpc.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories {
  class DiscountRepository : IDiscountRepository {
    private readonly IConfiguration configuration;

    public DiscountRepository(IConfiguration configuration) {
      this.configuration = configuration;
    }

    public async Task<Coupon> GetDiscountAsync(string productName) {
      await using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
      
      var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductName = @ProductName",
                                                                     new { ProductName = productName }
                                                                    );

      return coupon ?? new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
    }

    public async Task<bool> CreateDiscountAsync(Coupon coupon) {
      await using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

      var affected = await connection.ExecuteAsync("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)", coupon);

      return affected != 0;
    }

    public async Task<bool> UpdateDiscountAsync(Coupon coupon) {
      await using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

      var affected = await connection.ExecuteAsync("UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id", coupon);

      return affected != 0;
    }

    public async Task<bool> DeleteDiscountAsync(string productName) {
      await using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

      var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

      return affected != 0;
    }
  }
}
