using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IBioAgeCalculationParameters<T> where T: IPatientParameter
    {
        public BioAgeCalculationType CalculationType { get; set; }

        public Dictionary<string, T> Parameters { get; set; }
    }
}
