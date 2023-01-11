using Interfaces;

namespace WebMVC.Models.MetaModels
{
    public record ParameterMeta(ParameterNames param, Func<string, bool> isValidValue);
}
