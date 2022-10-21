﻿using Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PatientsResolver.API.Entities
{
    [Table("PatientParameters")]
    public class PatientParameter : IPatientParameter
    {
        public PatientParameter()
        {

        }

        [NotNull]
        [Column("Id")]
        [Key]
        public int Id { get; set; }


        [NotNull]
        [Column("PatientDataId")]
        public int InfluenceId { get; set; } 


        [NotNull]
        [Required]
        public int PatientId { get ; set ; }

        [NotNull]
        [Required]
        public DateTime Timestamp { get ; set ; }

        [NotNull]
        [Required]
        [Column("Name")]
        public string NameTextDescription { get ; set ; }

        [NotNull]
        [Required]
        public string Value { get ; set ; }
       
        
        [NotNull]
        [Required]
        public int PositiveDynamicCoef { get ; set ; }

        [NotMapped]
        public ParameterNames ParameterName { get; set ; }

        public bool IsDynamic { get; set; }
    }
}
