using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    
    public class ModelKey
    {
        public string Id { get; set; }

        public string Version { get; set; }
    }

    public class TempModelSettings
    {
        public ModelKey BioAge { get; set; }

        public ModelKey Dentist_3_5 { get; set; }

        public ModelKey Dentist_6_9 { get; set; }

        public ModelKey Dentist_10_12 { get; set; }
    }
}
