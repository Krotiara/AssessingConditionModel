using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interfaces.Service
{
    public class ParametersService
    {
        private readonly ParametersStore _paramsStore;
        private ConcurrentDictionary<string, Parameter> _currentParams;
        private bool isInit;

        public ParametersService(ParametersStore store)
        {
            _paramsStore = store;
            _currentParams = new();
        }


        public async Task Insert(IEnumerable<Parameter> parameters)
        {
            if (!isInit)
                await Init();

            foreach (var param in parameters)
                if (!_currentParams.ContainsKey(param.Name))
                {
                    await _paramsStore.Insert(param);
                    _currentParams[param.Name] = param;
                }
        }


        public async Task Init()
        {
            var ps = await _paramsStore.All();
            foreach (var p in ps)
                _currentParams[p.Name] = p;
            isInit = true;
        }


        public async Task<Parameter> Get(string name)
        {
            if (!isInit)
                await Init();

            return _currentParams.GetValueOrDefault(name);
        }

        public async Task<Parameter> GetByDescription(string description)
        {
            if (!isInit)
                await Init();

            return _currentParams.Values.FirstOrDefault(x => x.Description == description);
        }


        public async Task<IEnumerable<Parameter>> GetAll()
        {
            if (!isInit)
                await Init();
            return _currentParams.Values;
        }
    }
}
