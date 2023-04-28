using Parameters.API.Models.Documents;
using Parameters.API.Services.Mongo;

namespace Parameters.API.Services.Store
{
    public class ParametersStore : MongoBaseService<Parameter>
    {
        public ParametersStore(MongoService mongo) : base(mongo, "Parameters") { }
    }
}
