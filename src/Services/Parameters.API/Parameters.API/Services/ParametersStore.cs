using Interfaces.Mongo;
using Parameters.API.Models;

namespace Parameters.API.Service
{
    public class ParametersStore : MongoBaseService<ACParameter>
    {
        public ParametersStore(MongoService mongo) : base(mongo, "Parameters") { }
    }
}
