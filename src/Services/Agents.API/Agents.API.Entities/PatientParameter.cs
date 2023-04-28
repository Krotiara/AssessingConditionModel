using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class PatientParameter : IPatientParameter
    {
        public PatientParameter() { }

        public int Id { get; set; }
        public int InfluenceId { get; set; }
        public int PatientId { get; set; }
        public DateTime Timestamp { get; set; }
        public string ParameterName { get; set; }
        public string NameTextDescription { get; set; }
        public string Value { get; set; }

        public int PositiveDynamicCoef { get; set; }
        public bool IsDynamic { get; set; }
        public string MedicalOrganization { get; set; }


#warning Опасно менят ьвообще дял всех типов запятые на точки.
        public T ConvertValue<T>() => (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(Value.Replace(",", "."));
    }
}
