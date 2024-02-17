using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PatientsResolver.API.Entities;

namespace PatientsResolver.API.Data.Store
{
    public class PostgreSQLParametersDbContext : DbContext
    {
        private readonly PostgreSQLDbSettings _sets;

        public DbSet<PatientParameter> PatientParameters { get; set; }

        public PostgreSQLParametersDbContext(IOptions<PostgreSQLDbSettings> sets)
        {
            _sets = sets.Value;
            Database.EnsureCreated();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql($"Host={_sets.Host};" +
                $"Port={_sets.Port};" +
                $"Database={_sets.DatabaseName};" +
                $"Username={_sets.Username};" +
                $"Password={_sets.Password}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientParameter>().HasKey(x => new
            {
                x.Name,
                x.Timestamp
            });
        }
    }
}
