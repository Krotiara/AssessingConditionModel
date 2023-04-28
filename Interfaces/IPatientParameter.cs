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

        public string PatientAffiliation { get; set; }

        public DateTime Timestamp { get; set; }

        public string Name { get; set; }

        public string Value { get;  set; }

        public bool IsDynamic { get; set; }
    }
}
