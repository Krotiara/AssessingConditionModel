using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AssessingConditionModel.Models
{
    public class PatientsContext: DbContext
    {
        public DbSet<Patient> Patients { get; set; }

        public DbSet<ClinicalParameters> ClinicalParameters { get; set; }

        public DbSet<FunctionalParameters> FunctionalParameters { get; set; }

        public DbSet<InstrumentalParameters> InstrumentalParameters { get; set; }


        public PatientsContext(DbContextOptions<PatientsContext> options) : base(options)
        {
           
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasOne<ClinicalParameters>(p => p.ClinicalParameters)
                .WithOne()
                .HasPrincipalKey<Patient>(p=>p.MedicalHistoryNumber)
                .HasForeignKey<ClinicalParameters>(p => p.PatientId)
                .IsRequired();

            modelBuilder.Entity<Patient>()
                .HasOne<FunctionalParameters>(p => p.FunctionalParameters)
                .WithOne()
                .HasPrincipalKey<Patient>(p => p.MedicalHistoryNumber)
                .HasForeignKey<FunctionalParameters>(p => p.PatientId)
                .IsRequired();

            modelBuilder.Entity<Patient>()
                .HasOne<InstrumentalParameters>(p => p.InstrumentalParameters)
                .WithOne()
                .HasPrincipalKey<Patient>(p => p.MedicalHistoryNumber)
                .HasForeignKey<InstrumentalParameters>(p => p.PatientId)
                .IsRequired();

            modelBuilder.Entity<Patient>().Ignore(b => b.ParametersNorms);
        }
    }
}
