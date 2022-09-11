﻿using Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfluenceCalculator.API.Models
{
    [Table("InfluenceResults")]
    public class InfluenceResult : IInfluenceResult
    {
        public InfluenceResult() { }

        [Key]
        public int Id { get; set; }

        [Column("Influence id")]
        public int InfluenceId { get; set; }

        public IInfluence Influence { get; set; }

        [NotMapped]
        public IEnumerable<IPatientParameter> TrackedParameters { get; set; }

        [Column("Influence effectiveness")]
        public double InfluenceEffectiveness { get; set; }

        [Column("Patient id")]
        public int PatientId { get; set; }
    }
}
