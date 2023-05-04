using Agents.API.Service.AgentCommand;
using Interfaces;

namespace Agents.API
{
    public static class CommandsDependensyRegistrator
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddTransient<GetAgeCommand>();
            services.AddTransient<GetBioageByFuncParamsCommand>();
            services.AddTransient<GetDentistSumCommand>();
            services.AddTransient<GetInfluencesCommand>();
            services.AddTransient<GetInfluencesWithoutParametersCommand>();
            services.AddTransient<GetLatestPatientParametersCommand>();

            services.AddTransient<CommandServiceResolver>(serviceProvider => (command, vars, properties) =>
            {
                IAgentCommand res = command switch 
                {
                    SystemCommands.GetAge =>serviceProvider.GetService<GetAgeCommand>(),
                    SystemCommands.GetBioageByFunctionalParameters => serviceProvider.GetService<GetBioageByFuncParamsCommand>(),
                    SystemCommands.GetDentistSum => serviceProvider.GetService<GetDentistSumCommand>(),
                    SystemCommands.GetInfluences => serviceProvider.GetService<GetInfluencesCommand>(),
                    SystemCommands.GetInfluencesWithoutParameters => serviceProvider.GetService<GetInfluencesWithoutParametersCommand>(),
                    SystemCommands.GetLatestPatientParameters => serviceProvider.GetService<GetLatestPatientParametersCommand>(),
                    _ => null
                };
                if (res != null)
                {
                    res.Variables = vars;
                    res.Properties = properties;
                }
                return res;
            });

        }
    }
}
