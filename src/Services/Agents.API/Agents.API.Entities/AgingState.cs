using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    [Table("AgingStates")]
    public class AgingState : IAgingState
    {
        [NotNull]
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [NotNull]
        [Required]
        [Column("PatientId")]
        public int PatientId { get; set; }
        [NotNull]
        [Required]
        [Column("Timestamp")]
        public DateTime Timestamp { get; set; }
        [NotNull]
        [Required]
        [Column("Age")]
        public double Age { get; set; }
        [NotNull]
        [Required]
        [Column("BioAge")]
        public double BioAge { get; set; }
        [NotNull]
        [Required]
        [Column("State")]
        public AgentBioAgeStates BioAgeState { get; set; }
    }
}
