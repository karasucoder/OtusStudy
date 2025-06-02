using Dapper;
using Npgsql;

namespace DapperPracticeConsoleApp
{
    public class OrdersRepository
    {
        public List<Order> GetOrders()
        {
            using (var connection = new NpgsqlConnection(AppConfiguration.DefaultConnection))
            {
                return connection.Query<Order>("SELECT * FROM Orders").ToList();
            }
        }

        public List<Order> GetOrdersByQuantityRange(int minQuantity, int maxQuantity)
        {
            using (var connection = new NpgsqlConnection(AppConfiguration.DefaultConnection))
            {
                return connection.Query<Order>(
                    "SELECT * FROM Orders WHERE Quantity BETWEEN @MinQantity AND @MaxQuantity",
                    new 
                    { 
                        MinQantity = minQuantity, 
                        MaxQuantity = maxQuantity 
                    }
                ).ToList();
            }
        }

        // обработка запроса из задания "Кластерный индекс"
        public List<OrderDetails> GetOrdersByAgeAndProduct(int minAge, int productId)
        {
            using (var connection = new NpgsqlConnection(AppConfiguration.DefaultConnection))
            {
                var query = @"SELECT 
                              c.Id as CustomerId,
                              c.FirstName,
                              c.LastName,
                              p.Id as ProductId,
                              o.Quantity,
                              p.Price
                            FROM Customers c
                            JOIN Orders o ON c.Id = o.CustomerId
                            JOIN Products p ON o.ProductId = p.Id
                            WHERE c.Age >= @MinAge 
                            AND p.Id = @ProductId";

                return connection.Query<OrderDetails>(query, new
                {
                    MinAge = minAge,
                    ProductId = productId
                }).ToList();
            }
        }
    }
}