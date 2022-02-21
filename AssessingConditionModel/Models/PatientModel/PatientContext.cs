using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssessingConditionModel.Models.PatientModel;
using Microsoft.EntityFrameworkCore;

namespace AssessingConditionModel.Models
{
    public class PatientsContext: DbContext
    {
        public DbSet<Patient> Patients { get; set; }

        public DbSet<ClinicalParameters> ClinicalParameters { get; set; }

        public DbSet<FunctionalParameters> FunctionalParameters { get; set; }

        public DbSet<InstrumentalParameters> InstrumentalParameters { get; set; }

        public DbSet<LungTissueDamage> LungTissueDamages { get; set; }

        public DbSet<GeneralBloodTest> GeneralBloodTests { get; set; }

        public DbSet<GeneralUrineAnalysis> GeneralUrineAnalysises { get; set; }


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

            modelBuilder.Entity<ClinicalParameters>()
                .HasOne<LungTissueDamage>(x => x.LungTissueDamage)
                .WithOne()
                .HasPrincipalKey<ClinicalParameters>(x => x.PatientId)
                .HasForeignKey<LungTissueDamage>(x => x.Id)
                .IsRequired();

            modelBuilder.Entity<ClinicalParameters>()
                .HasOne<GeneralBloodTest>(x => x.GeneralBloodTest)
                .WithOne()
                .HasPrincipalKey<ClinicalParameters>(x => x.PatientId)
                .HasForeignKey<GeneralBloodTest>(x => x.Id)
                .IsRequired();

            modelBuilder.Entity<ClinicalParameters>()
               .HasOne<GeneralUrineAnalysis>(x => x.GeneralUrineAnalysis)
               .WithOne()
               .HasPrincipalKey<ClinicalParameters>(x => x.PatientId)
               .HasForeignKey<GeneralUrineAnalysis>(x => x.Id)
               .IsRequired();

            modelBuilder.Entity<Patient>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();
        }
    }
}
