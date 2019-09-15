using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess.DbContext
{
    public class DbContextFactory : IDesignTimeDbContextFactory<DataDbContext>
    {
        public DataDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<DataDbContext> builder = new DbContextOptionsBuilder<DataDbContext>();

            IConfigurationRoot configuration = Configuration.AppConfigurations.Get(Configuration.WebContentDirectoryFinder.CalculateContentRootFolder());

            DbContextConfigurer.Configure(builder, configuration.GetConnectionString("Connection"));

            return new DataDbContext(builder.Options);
        }
    }
}