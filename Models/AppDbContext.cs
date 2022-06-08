using Microsoft.EntityFrameworkCore;

namespace StoredProcedureApi.Models
{
     public class AppDbContext : DbContext
     {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<UserProfile> UserProfiles {get; set;}

        // protected override void OnModelCreating(ModelBuilder builder)
        // {
        //     base.OnModelCreating(builder);
        // }
    }
}