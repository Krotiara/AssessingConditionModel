using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IAgingState
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public DateTime Timestamp { get; set; }

        public double Age { get; set; }

        public double BioAge { get; set; }
    }
}
