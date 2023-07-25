using Interfaces.Mongo;
using PatientsResolver.API.Entities.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Service.Store
{
    public class InfluencesStore : MongoBaseService<Influence>
    {
        public InfluencesStore(MongoService mongo) : base(mongo, "Influences")
        {
        }
    }
}
