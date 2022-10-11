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

        public int PatientDataId { get; set; }

        public int PatientId { get; set; }

        public DateTime Timestamp { get; set; }

        public Parameters ParameterName { get; set; }

        public string NameTextDescription { get; set; }

        public string Value { get;  set; }

        public string DynamicValue { get; set; }

        /// <summary>
        /// Коэффициент направления улучшения показателя. 
        /// Если улучшение показателя характеризуется его повышением, то значение должно быть = 1; 
        /// Если в меньшую сторону, то значение должно быть = -1.
        /// </summary>
        public int PositiveDynamicCoef { get; set; }

    }
}
