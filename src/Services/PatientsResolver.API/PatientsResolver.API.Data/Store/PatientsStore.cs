using Interfaces.Mongo;
using PatientsResolver.API.Entities.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Store
{
    public class PatientsStore : MongoBaseService<Patient>
    {
        public PatientsStore(MongoService mongo) : base(mongo, "Patients")
        {
        }
    }
}
