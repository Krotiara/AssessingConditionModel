using Interfaces.Mongo;

namespace Interfaces.Service
{
    public class ParametersStore : MongoBaseService<Parameter>
    {
        public ParametersStore(MongoService mongo) : base(mongo, "Parameters") { }
    }
}
