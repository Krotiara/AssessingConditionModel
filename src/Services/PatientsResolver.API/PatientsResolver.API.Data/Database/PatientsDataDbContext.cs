using Microsoft.EntityFrameworkCore;
using PatientsResolver.API.Entities;

namespace PatientsResolver.API.Data
{
    public class PatientsDataDbContext: DbContext
    {

        public virtual DbSet<PatientParameter> PatientsParameters { get; set; }

        public virtual DbSet<PatientData> PatientDatas { get; set; }

        public virtual DbSet<Patient> Patients { get; set; }

        public virtual DbSet<Influence> Influences { get; set; }


        public PatientsDataDbContext():base()
        {

        }

        public PatientsDataDbContext(DbContextOptions<PatientsDataDbContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {      
            modelBuilder.Entity<PatientData>()
                .HasMany<PatientParameter>(x=>x.Parameters)
                .WithOne()
                .HasForeignKey(x => x.PatientDataId)
                .IsRequired();
            modelBuilder.Entity<PatientData>()
                .HasOne<Patient>(x=>x.Patient);
            modelBuilder.Entity<PatientData>()
                .HasOne<Influence>(x=>x.Influence);
            base.OnModelCreating(modelBuilder);
        }
    }
}
