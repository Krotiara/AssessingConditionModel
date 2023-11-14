using Parameters.API.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parameters.API.Service
{
    public class ParametersService
    {
        private readonly ParametersStore _paramsStore;

        private readonly ConcurrentDictionary<string, ACParameter> _dictByNames;

        public ParametersService(ParametersStore store)
        {
            _paramsStore = store;
            _dictByNames = new();
        }


        public async Task<ACParameter> Insert(ACParameter param)
        {
            if (param.Description.Contains("."))
                param.Description = param.Description.Replace(".", "_");

            if (param.Id != null)
                await Update(param);
            else
            {
                await _paramsStore.Insert(param);
                _dictByNames[param.Name] = param;
            }

            return param;
        }


        public async Task<ACParameter> Get(string name)
        {
            if (_dictByNames.TryGetValue(name, out ACParameter val))
                return val;

            val = await _paramsStore.Get(x => x.Name == name);

            if (val != null)
                _dictByNames[val.Name] = val;

            return val;
        }


        public async Task<IEnumerable<ACParameter>> GetAll()
        {
            var ps = (await _paramsStore.All()).ToList();
            foreach (var param in ps)
                _dictByNames[param.Name] = param;
            return ps;
        }


        public async Task<ACParameter> Update(ACParameter parameter)
        {
            await _paramsStore.Update(x => x.Id == parameter.Id)
                .Set(x => x.Description, parameter.Description)
                .Set(x => x.Name, parameter.Name)
                .Execute();

            _dictByNames[parameter.Name] = parameter;

            return parameter;
        }


        public async Task Delete(string id)
        {
            await _paramsStore.Delete(x => x.Id == id);
            _dictByNames.TryRemove(id, out _);
        }

    }
}
