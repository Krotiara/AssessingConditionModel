using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PatientsResolver.API.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientsResolver.API.Data.Store
{
    public class PostgreSQLParametersDbContext : DbContext
    {
        private readonly PostgreSQLDbSettings _sets;
        private readonly ILogger<PostgreSQLParametersDbContext> _logger;

        public DbSet<PatientParameter> PatientParameters { get; set; }

        public PostgreSQLParametersDbContext(IOptions<PostgreSQLDbSettings> sets, ILogger<PostgreSQLParametersDbContext> logger)
        {
            _sets = sets.Value;
            _logger = logger;
            InitDbIfRequiered();
        }

        private void InitDbIfRequiered()
        {
            try
            {
                Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Skip ensure creating postgreSQL db: {ex.Message}.");
            }
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql($"Host={_sets.Host};" +
                $"Port={_sets.Port};" +
                $"Database={_sets.DatabaseName};" +
                $"Username={_sets.Username};" +
                $"Password={_sets.Password}");
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<PatientParameter>().HasKey(x => x.Id);
        //}
    }
}
