using Agents.API.Service.AgentCommand;
using Interfaces;

namespace Agents.API
{

    public delegate IAgentCommand CommandServiceResolver(SystemCommands command);

    public static class CommandsDependensyRegistrator
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddTransient<GetAgeCommand>();
            services.AddTransient<GetAgeRangCommand>();
            services.AddTransient<GetBioageByFuncParamsCommand>();
            services.AddTransient<GetDentistSumCommand>();
            services.AddTransient<GetInfluencesCommand>();
            services.AddTransient<GetInfluencesWithoutParametersCommand>();
            services.AddTransient<GetLatestPatientParametersCommand>();

            services.AddTransient<CommandServiceResolver>(serviceProvider => command =>
            {
                return command switch
                {
                    SystemCommands.GetAge => serviceProvider.GetService<GetAgeCommand>(),
                    SystemCommands.GetAgeRangBy => serviceProvider.GetService<GetAgeRangCommand>(),
                    SystemCommands.GetBioageByFunctionalParameters => serviceProvider.GetService<GetBioageByFuncParamsCommand>(),
                    SystemCommands.GetDentistSum => serviceProvider.GetService<GetDentistSumCommand>(),
                    SystemCommands.GetInfluences => serviceProvider.GetService<GetInfluencesCommand>(),
                    SystemCommands.GetInfluencesWithoutParameters => serviceProvider.GetService<GetInfluencesWithoutParametersCommand>(),
                    SystemCommands.GetLatestPatientParameters => serviceProvider.GetService<GetLatestPatientParametersCommand>(),
                    _ => null
                };
            });

        }
    }
}
