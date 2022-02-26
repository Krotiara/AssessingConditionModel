using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models.PatientModel
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
        [DisplayName("Дата снятия показателей")]
        public DateTime Date { get; set; }

        [Column("Gender")]
        [DisplayName("Пол")]
        public string Gender { get; set; }

        [Column("Age")]
        [DisplayName("Возраст")]
        public double Age { get; set; }

        [Column("BioAge")]
        [DisplayName("Биовозраст")]
        public double BioAge { get; set; }

        [Column("Weight")]
        [DisplayName("Масса тела")]
        public double Weight { get; set; }

        [Column("SystolicBloodPressure")]
        [DisplayName("Артериальное давление (систолическое)")]
        public double SystolicBloodPressure { get; set; }

        [Column("DiastolicBloodPressure")]
        [DisplayName("Артериальное давление (диастолическое)")]
        public double DiastolicBloodPressure { get; set; }

        [Column("PulseBloodPressure")]
        [DisplayName("Артериальное давление (пульсовое)")]
        public double PulseBloodPressure { get; set; }

        [Column("LungCapacity")]
        [DisplayName("Жизненная емкость легких")]
        public double LungCapacity { get; set; }

        [Column("InhaleBreathHolding")]
        [DisplayName("Задержка дыхания (на вдохе)")]
        public double InhaleBreathHolding { get; set; }

        [Column("OuthaleBreathHolding")]
        [DisplayName("Задержка дыхания (на выдохе)")]
        public double OuthaleBreathHolding { get; set; }

    }
}
