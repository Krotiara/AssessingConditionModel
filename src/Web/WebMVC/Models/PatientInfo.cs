namespace WebMVC.Models
{
    public class PatientInfo
    {
        public PatientInfo() { }

        public Patient Patient { get; set; }

        public AgingState AgingPatientState { get; set; }
    }
}
