using Agents.API.Service.AgentCommand;
using Interfaces;

namespace Agents.API
{
    public static class CommandsDependensyRegistrator
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddTransient<GetAgeCommand>();
            services.AddTransient<PredictCommand>();
            services.AddTransient<GetInfluencesCommand>();

            services.AddTransient<CommandServiceResolver>(serviceProvider => (command, vars, properties, commonPropsNames) =>
            {
                IAgentCommand? res = command switch
                {
                    SystemCommands.GetAge => serviceProvider.GetService<GetAgeCommand>(),
                    SystemCommands.Predict => serviceProvider.GetService<PredictCommand>(),
                    SystemCommands.GetInfluences => serviceProvider.GetService<GetInfluencesCommand>(),
                    _ => null
                };

                if (res != null)
                {
                    res.Variables = vars;
                    res.Properties = properties;
                    res.PropertiesNamesSettings = commonPropsNames;
                }
                return res;
            });

        }
    }
}
