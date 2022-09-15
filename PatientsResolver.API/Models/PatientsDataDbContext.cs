using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PatientsResolver.API.Models
{
    public class PatientsDataDbContext: DbContext
    {

        public virtual DbSet<PatientParameter> PatientsParameters { get; set; }

        public virtual DbSet<PatientData> PatientDatas { get; set; }


        public PatientsDataDbContext():base()
        {

        }

        public PatientsDataDbContext(DbContextOptions<PatientsDataDbContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {      
            modelBuilder.Entity<PatientData>()
                .HasMany<PatientParameter>()
                .WithOne()
                .HasForeignKey(x => x.PatientDataId)
                .IsRequired();
            modelBuilder.Entity<PatientData>()
                .HasOne<Patient>()
                .WithMany()
                .HasForeignKey(x => x.PatientId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
