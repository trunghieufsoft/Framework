using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DbContext
{
    public partial class DataDbContext
    {
        // DbSet
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<SystemConfiguration> SystemConfigurations { get; set; }
    }
}
