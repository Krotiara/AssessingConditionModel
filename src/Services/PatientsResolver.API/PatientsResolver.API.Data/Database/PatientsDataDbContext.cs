﻿using Microsoft.EntityFrameworkCore;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Entities.Mongo;

namespace PatientsResolver.API.Data
{
    public class PatientsDataDbContext: DbContext
    {

        public virtual DbSet<PatientParameter> PatientsParameters { get; set; }

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
            modelBuilder.Entity<Influence>()
                .HasOne<Patient>(x=>x.Patient);
            modelBuilder.Entity<Patient>().HasKey(x => new { x.Id, x.MedicalOrganization });
            //Чтобы не пытался обновить ключ при update, иначе ошибка.
            //modelBuilder.Entity<Patient>()
            //    .Property(x => x.Id).Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);  
            modelBuilder.Entity<Influence>()
                .Property(x => x.Id).Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Throw);
            modelBuilder.Entity<PatientParameter>()
                .Property(x=>x.Id).Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Throw);
            base.OnModelCreating(modelBuilder);
        }
    }
}
