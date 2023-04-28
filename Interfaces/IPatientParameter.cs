using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IPatientParameter
    {

        public int Id { get; set; }

        public int InfluenceId { get; set; }

        public int PatientId { get; set; }

        public string MedicalOrganization { get; set; }

        public DateTime Timestamp { get; set; }

        public string ParameterName { get; set; }

        public string NameTextDescription { get; set; }

        public string Value { get;  set; }

        public bool IsDynamic { get; set; }

        //public string DynamicValue { get; set; }

        /// <summary>
        /// Коэффициент направления улучшения показателя. 
        /// Если улучшение показателя характеризуется его повышением, то значение должно быть = 1; 
        /// Если в меньшую сторону, то значение должно быть = -1.
        /// </summary>
        public int PositiveDynamicCoef { get; set; }

    }
}
