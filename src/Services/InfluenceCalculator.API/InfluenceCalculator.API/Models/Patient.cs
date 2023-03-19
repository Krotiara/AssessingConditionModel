using Interfaces;

namespace InfluenceCalculator.API.Models
{
    public class Patient : IPatient
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime Birthday { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        
        public GenderEnum Gender { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TreatmentType TreatmentType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string MedicalOrganization { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
