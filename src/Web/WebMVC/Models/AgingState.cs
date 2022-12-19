using Interfaces;
using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    public class AgingState : IAgingState
    {
        public int Id { get ; set ; }
        [Display(Name ="Идентификатор пациента")]
        public int PatientId { get ; set ; }

        [Display(Name = "Временная отметка")]
        public DateTime Timestamp { get ; set ; }

        [Display(Name = "Возраст")]
        public double Age { get ; set ; }

        [Display(Name = "Биовозраст")]
        public double BioAge { get ; set ; }

        [Display(Name = "Темп старения")]
        public AgentBioAgeStates BioAgeState { get ; set ; }
    }
}
