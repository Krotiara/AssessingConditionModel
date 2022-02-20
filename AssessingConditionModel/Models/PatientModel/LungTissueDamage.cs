using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models.PatientModel
{
    [Table("LungTissueDamages")]
    public class LungTissueDamage
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Column("IsRightHandDamage")]
        public bool IsRightHandDamage { get; set; }

        [Column("IsLeftHandDamage")]
        public bool IsLeftHandDamage { get; set; }

        [Column("IsTwoWayDamage")]
        public bool IsTwoWayDamage { get; set; }

        [Column("RightLungDamageDescription")]
        public string RightLungDamageDescription { get; set; }

        [Column("LeftLungDamageDescription")]
        public string LeftLungDamageDescription { get; set; }

        [Column("DamageVolumeDescription")]
        public string DamageVolumeDescription { get; set; }
    }
}
