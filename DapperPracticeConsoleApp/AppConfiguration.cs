using Microsoft.Extensions.Configuration;

namespace DapperPracticeConsoleApp
{
    internal class AppConfiguration
    {
        public static readonly string DefaultConnection;

        static AppConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            DefaultConnection = configuration.GetConnectionString("Default");
        }
    }
}
