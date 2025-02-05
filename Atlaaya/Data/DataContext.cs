namespace Atlaaya.Data
{
	public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<ProjectImagesMapping> ProjectImagesMapping { get; set; }
        public DbSet<ProjectDocMapping> ProjectDocMapping { get; set; }
        public DbSet<Enquire> Enquire { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<Testimonial> Testimonial { get; set; }
    }
}
