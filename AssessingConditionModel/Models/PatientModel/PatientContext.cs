using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AssessingConditionModel.Models
{
    public class PatientContext: DbContext
    {
        public DbSet<Patient> Patients { get; set; }

        public DbSet<ClinicalParameters> ClinicalParameters { get; set; }

        public DbSet<FunctionalParameters> FunctionalParameters { get; set; }

        public DbSet<InstrumentalParameters> InstrumentalParameters { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasOne<ClinicalParameters>(p => p.ClinicalParameters)
                .WithOne()
                .HasForeignKey<ClinicalParameters>(p => p.PatientId)
                .IsRequired();

            modelBuilder.Entity<Patient>()
                .HasOne<FunctionalParameters>(p => p.FunctionalParameters)
                .WithOne()
                .HasForeignKey<FunctionalParameters>(p => p.PatientId)
                .IsRequired();

            modelBuilder.Entity<Patient>()
                .HasOne<InstrumentalParameters>(p => p.InstrumentalParameters)
                .WithOne()
                .HasForeignKey<InstrumentalParameters>(p => p.PatientId)
                .IsRequired();
        }
    }
}
