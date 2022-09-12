﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IPatientParameter
    {
        public int PatientId { get; set; }

        public DateTime Timestamp { get; set; }

        public string Name { get; set; }

        public object Value { get;  set; }

        public object DynamicValue { get; set; }

        /// <summary>
        /// Коэффициент направления улучшения показателя. 
        /// Если улучшение показателя характеризуется его повышением, то значение должно быть = 1; 
        /// Если в меньшую сторону, то значение должно быть = -1.
        /// </summary>
        public int PositiveDynamicCoef { get; set; }

    }
}
