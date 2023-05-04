using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interfaces.Service
{
    public class ParametersService
    {
        private readonly ParametersStore _paramsStore;

        public ParametersService(ParametersStore store)
        {
            _paramsStore = store;
        }


        public async Task Insert(IEnumerable<Parameter> parameters)
        {
            foreach (var param in parameters)
                if(await Get(param.Name) == null && await GetByDescription(param.Description) == null)
                    await _paramsStore.Insert(param);            
        }

        public async Task<Parameter> Get(string name) => await _paramsStore.Get(x => x.Name == name);


        public async Task<Parameter> GetByDescription(string description) => await _paramsStore.Get(x => x.Description == description);


        public async Task<IEnumerable<Parameter>> GetAll() => await _paramsStore.All();  
    }
}
