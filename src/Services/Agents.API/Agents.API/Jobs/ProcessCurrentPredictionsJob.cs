using Agents.API.Service.Services;
using Quartz;

namespace Agents.API.Jobs
{
    public class ProcessCurrentPredictionsJob : IJob
    {
        private readonly AgentsService _agentsService;

        public ProcessCurrentPredictionsJob(AgentsService agentsService)
        {
            _agentsService = agentsService;
        }

        public static void Schedule(IServiceCollectionQuartzConfigurator q)
        {
            q.ScheduleJob<ProcessCurrentPredictionsJob>(j => j.StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(1)
                .RepeatForever()));
        }

        public Task Execute(IJobExecutionContext context) => _agentsService.ProcessCurrentPredictions();
    }
}
