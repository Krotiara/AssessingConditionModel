using Microsoft.Extensions.Options;

namespace Agents.API.Entities
{
    public class EnvSettings
    {
        public EnvSettings()
        {
            //TempModelSettings = opts.Value;
        }


        public string ModelsApiUrl { get; set; }

        public string PatientsResolverApiUrl { get; set; }

        //public TempModelSettings TempModelSettings { get; set; }
    }
}
