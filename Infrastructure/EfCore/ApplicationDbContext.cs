using Microsoft.EntityFrameworkCore;
using TaskAggregator.Domain.Aggregates;

namespace TaskAggregator.Infrastructure.EfCore
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<RequestSource> RequestSources { get; set; }
        public DbSet<Request> Requests { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<Enum>().HaveConversion<string>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention();
        }
    }
}
