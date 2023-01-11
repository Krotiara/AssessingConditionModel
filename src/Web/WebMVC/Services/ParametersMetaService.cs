using Interfaces;
using WebMVC.Models.MetaModels;

namespace WebMVC.Services
{
    public class ParametersMetaService : IParametersMetaService
    {

        private readonly Dictionary<ParameterNames, ParameterMeta> metas;

        public ParametersMetaService()
        {
            metas = new Dictionary<ParameterNames, ParameterMeta>()
            {
                
            };
        }


        public ParameterMeta GetParameterMeta(ParameterNames paramName)
        {
            throw new NotImplementedException();
        }
    }
}
