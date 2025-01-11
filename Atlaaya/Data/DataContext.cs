using Microsoft.EntityFrameworkCore;

namespace Atlaaya.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<Projects> Projects { get; set; }
    }
}
