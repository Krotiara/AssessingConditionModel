using Interfaces;

namespace AssessingConditionModel.Models.InfluenceModel
{
    public class ParameterDynamic : IParameterDynamic
    {
        public IPatientParameter OldPatientParameter { get; set; }
        public IPatientParameter NewPatientParameter { get; set; }
    }
}
