using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDataHandler.API.Entities
{
    public class ParseDataSettings
    {
        public string Group { get; set; }

        public string Timestamp { get; set; }

        public string Dynamic { get; set; }

        public string Gender { get; set; }

        public string Birthday { get; set; }
    }
}
