using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DigitekShop.Persistence.Contexts
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DigitekShopDBContext>
    {
        public DigitekShopDBContext CreateDbContext(string[] args)
        {
            // Try to get configuration from local appsettings.json first
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DigitekDBConnection");

            // If not found locally, try to get from API project
            if (string.IsNullOrEmpty(connectionString))
            {
                var apiProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "DigitekShop.Api");
                
                // If we're in the Persistence project, go up one more level
                if (!Directory.Exists(apiProjectPath))
                {
                    apiProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "DigitekShop.Api");
                }

                configuration = new ConfigurationBuilder()
                    .SetBasePath(apiProjectPath)
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddJsonFile("appsettings.Development.json", optional: true)
                    .Build();

                connectionString = configuration.GetConnectionString("DigitekDBConnection");
            }

            var optionsBuilder = new DbContextOptionsBuilder<DigitekShopDBContext>();
            optionsBuilder.UseSqlServer(connectionString, options =>
            {
                options.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });

            return new DigitekShopDBContext(optionsBuilder.Options);
        }
    }
} 