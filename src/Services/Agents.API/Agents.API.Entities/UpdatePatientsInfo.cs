using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class UpdatePatientsInfo : IUpdatePatientsDataInfo
    {
        public HashSet<(int, DateTime)> UpdateInfo { get; set; }
    }
}
