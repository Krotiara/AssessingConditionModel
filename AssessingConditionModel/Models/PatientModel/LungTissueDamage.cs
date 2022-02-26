using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Правостороннее")]
        public bool IsRightHandDamage { get; set; }

        [Column("IsLeftHandDamage")]
        [DisplayName("Левостороннее")]
        public bool IsLeftHandDamage { get; set; }

        [Column("IsTwoWayDamage")]
        [DisplayName("Двустороннее")]
        public bool IsTwoWayDamage { get; set; }

        [Column("RightLungDamageDescription")]
        [DisplayName("Правое легкое")]
        public string RightLungDamageDescription { get; set; }

        [Column("LeftLungDamageDescription")]
        [DisplayName("Левое легкое")]
        public string LeftLungDamageDescription { get; set; }

        [Column("DamageVolumeDescription")]
        [DisplayName("Объем поражения")]
        public string DamageVolumeDescription { get; set; }
    }
}
