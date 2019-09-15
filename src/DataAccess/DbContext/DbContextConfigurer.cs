using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DbContext
{
    public static class DbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<DataDbContext> builder, string connectionString)
        {
            builder.EnableSensitiveDataLogging().UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<DataDbContext> builder, DbConnection connection)
        {
            builder.EnableSensitiveDataLogging().UseSqlServer(connection);
        }
    }
}