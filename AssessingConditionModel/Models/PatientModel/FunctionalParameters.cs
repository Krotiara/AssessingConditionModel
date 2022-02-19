using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models
{
    [Table("FunctionalParameters")]
    public class FunctionalParameters
    {

        public FunctionalParameters()
        {
            //suitable constructor for entity type for awoid EF error No suitable constructor found for entity type
        }

        [Key]
        [Required]
        public int PatientId { get; set; }

        [Column("Date")]
        [Display(Name = "Дата снятия показателей")]
        public DateTime Date { get; set; }

        [Column("Gender")]
        [Display(Name = "Пол")]
        public string Gender { get; set; }

        [Column("Age")]
        [Display(Name = "Возраст")]
        public double Age { get; set; }

        [Column("BioAge")]
        [Display(Name = "Биовозраст")]
        public double BioAge { get; set; }

        [Column("Weight")]
        [Display(Name = "Масса тела")]
        public double Weight { get; set; }

        [Column("SystolicBloodPressure")]
        [Display(Name = "Артериальное давление (систолическое)")]
        public double SystolicBloodPressure { get; set; }

        [Column("DiastolicBloodPressure")]
        [Display(Name = "Артериальное давление (диастолическое)")]
        public double DiastolicBloodPressure { get; set; }

        [Column("PulseBloodPressure")]
        [Display(Name = "Артериальное давление (пульсовое)")]
        public double PulseBloodPressure { get; set; }

        [Column("LungCapacity")]
        [Display(Name = "Жизненная емкость легких")]
        public double LungCapacity { get; set; }

        [Column("InhaleBreathHolding")]
        [Display(Name = "Задержка дыхания (на вдохе)")]
        public double InhaleBreathHolding { get; set; }

        [Column("OuthaleBreathHolding")]
        [Display(Name = "Задержка дыхания (на выдохе)")]
        public double OuthaleBreathHolding { get; set; }

    }
}
