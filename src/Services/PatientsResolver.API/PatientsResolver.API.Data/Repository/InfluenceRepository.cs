using PatientsResolver.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Repository
{
    public class InfluenceRepository : Repository<Influence>
    {
        public InfluenceRepository(PatientsDataDbContext patientsDataDbContext): 
            base(patientsDataDbContext)
        {
            
        }
    }
}
