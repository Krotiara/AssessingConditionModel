using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Entities
{
    public class UpdatePatientsInfo : IUpdatePatientsInfo
    {
        public HashSet<int> UpdatedIds { get ; set ; }
    }
}
