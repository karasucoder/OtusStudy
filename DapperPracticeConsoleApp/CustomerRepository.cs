using Dapper;
using Npgsql;

namespace DapperPracticeConsoleApp
{
    public class CustomerRepository
    {
        public List<Customer> GetCustomers()
        {
            using (var connection = new NpgsqlConnection(AppConfiguration.DefaultConnection))
            {
                return connection.Query<Customer>("SELECT * FROM Customers").ToList();
            }
        }

        public Customer? GetCustomer(int customerId)
        {
            using (var connection = new NpgsqlConnection(AppConfiguration.DefaultConnection))
            {
                return connection.QueryFirstOrDefault<Customer>("SELECT * FROM Customers WHERE Id = @Id",
                                                                new { Id = customerId });
            }
        }
    }
}
