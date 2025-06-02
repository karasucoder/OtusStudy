using Dapper;
using Npgsql;

namespace DapperPracticeConsoleApp
{
    public class ProductRepository
    {
        public List<Product> GetProducts()
        {
            using (var connection = new NpgsqlConnection(AppConfiguration.DefaultConnection))
            {
                return connection.Query<Product>("SELECT * FROM Products").ToList();
            }
        }

        public List<Product> GetProductsInStock(int productCount)
        {
            using (var connection = new NpgsqlConnection(AppConfiguration.DefaultConnection))
            {
                return connection.Query<Product>("SELECT * FROM Products WHERE StockQuantity > @MinStock",
                                                 new { MinStock = productCount })
                                                 .ToList();
            }
        }
    }
}
