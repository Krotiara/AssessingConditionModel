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
        [Display(Name = "Правостороннее")]
        public bool IsRightHandDamage { get; set; }

        [Column("IsLeftHandDamage")]
        [Display(Name = "Левостороннее")]
        public bool IsLeftHandDamage { get; set; }

        [Column("IsTwoWayDamage")]
        [Display(Name = "Двустороннее")]
        public bool IsTwoWayDamage { get; set; }

        [Column("RightLungDamageDescription")]
        [Display(Name = "Правое легкое")]
        public string RightLungDamageDescription { get; set; }

        [Column("LeftLungDamageDescription")]
        [Display(Name = "Левое легкое")]
        public string LeftLungDamageDescription { get; set; }

        [Column("DamageVolumeDescription")]
        [Display(Name = "Объем поражения")]
        public string DamageVolumeDescription { get; set; }
    }
}
