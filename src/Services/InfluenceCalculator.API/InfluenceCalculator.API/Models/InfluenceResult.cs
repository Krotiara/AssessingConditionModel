﻿using Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace InfluenceCalculator.API.Models
{
    [Table("InfluenceResults")]
    public class InfluenceResult : IInfluenceResult
    {
        public InfluenceResult() { }

#warning Пока убрано хранение в бд в силу принятого решения не хранить результаты вычислений, так как метод вычисления может меняться.
        //[Key]
        //[NotNull]
        //[Column("Id")]
        //[Required]
        public int Id { get; set; }

        //[NotNull]
        //[Column("Influence id")]
        //[Required]
        public int InfluenceId { get; set; }

        //[NotNull]
        //[Column("Influence effectiveness")]
        //[Required]
        public double InfluenceEffectiveness { get; set; }

        //[NotNull]
        //[Column("PatientDataId")]
        //[Required]
        public int PatientDataId { get; set; }
    }
}