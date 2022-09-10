using Interfaces;

namespace AssessingConditionModel.Models.InfluenceModel
{
    public class PatientParameter : IPatientParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public object DynamicValue { get; set; }
    }
}
