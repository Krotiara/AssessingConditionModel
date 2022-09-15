using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IPatient
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public long MedicalHistoryNumber { get; set; }
    }
}
