using Interfaces;
using WebMVC.Models.MetaModels;

namespace WebMVC.Services
{
    public interface IParametersMetaService
    {
        public ParameterMeta GetParameterMeta(ParameterNames paramName);
    }
}
