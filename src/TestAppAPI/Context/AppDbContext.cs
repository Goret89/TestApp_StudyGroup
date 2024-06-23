using Microsoft.EntityFrameworkCore;
using TestApp;

namespace TestAppAPI.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<StudyGroup> StudyGroups { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Additional configuration here
        }
    }
}