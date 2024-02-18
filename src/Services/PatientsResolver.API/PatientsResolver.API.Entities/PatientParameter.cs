using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Entities
{
    public class PatientParameter : IPatientParameter
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string PatientId { get; set; }

        public string Name { get; set; }

        public DateTime Timestamp { get; set; }

        public double Value { get; set; }
    }
}
