using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Entities
{
    public class UpdatePatientsInfo : IUpdatePatientsDataInfo
    {
        public UpdatePatientsInfo()
        {
            UpdateInfo = new HashSet<(int, DateTime)>();
        }

        public HashSet<(int, DateTime)> UpdateInfo { get; set; }
    }
}
