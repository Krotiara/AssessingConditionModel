using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models
{
    public class FunctionalParameters
    {
        public int PatientId { get; set; }

        [Display(Name = "Дата снятия показателей")]
        public DateTime Date { get; set; }

        [Display(Name = "Возраст")]
        public int Age { get; set; }

        [Display(Name = "Биовозраст")]
        public double BioAge { get; set; }

        [Display(Name = "Масса тела")]
        public double Weight { get; set; }

        [Display(Name = "Артериальное давление (систолическое)")]
        public double SystolicBloodPressure { get; set; }

        [Display(Name = "Артериальное давление (диастолическое)")]
        public double DiastolicBloodPressure { get; set; }

        [Display(Name = "Артериальное давление (пульсовое)")]
        public double PulseBloodPressure { get; set; }

        [Display(Name = "Жизненная емкость легких")]
        public double LungCapacity { get; set; }

        [Display(Name = "Задержка дыхания (на вдохе)")]
        public double InhaleBreathHolding { get; set; }

        [Display(Name = "Задержка дыхания (на выдохе)")]
        public double OuthaleBreathHolding { get; set; }

    }
}
