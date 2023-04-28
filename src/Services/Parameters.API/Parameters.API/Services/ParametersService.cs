using Parameters.API.Models.Documents;
using Parameters.API.Services.Store;
using System.Collections.Concurrent;

namespace Parameters.API.Services
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
                await InitDict();

            foreach (var param in parameters)
                if (!_currentParams.ContainsKey(param.Name))
                {
                    await _paramsStore.Insert(param);
                    _currentParams[param.Name] = param;
                }
        }


        private async Task InitDict()
        {
            var ps = await _paramsStore.All();
            foreach (var p in ps)
                _currentParams[p.Name] = p;
            isInit = true;
        }


        public async Task<IEnumerable<Parameter>> GetAll()
        {
            if (!isInit)
                await InitDict();
            return _currentParams.Values;
        }
    }
}
