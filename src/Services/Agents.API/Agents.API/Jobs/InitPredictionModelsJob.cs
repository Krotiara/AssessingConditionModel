using Agents.API.Service.Services;
using Quartz;

namespace Agents.API.Jobs
{
    public class InitPredictionModelsJob : IJob
    {
        private readonly PredictionRequestsService _predcitionModelsService;

        public InitPredictionModelsJob(PredictionRequestsService predcitionModelsService)
        {
            _predcitionModelsService = predcitionModelsService;
        }

        public static void Schedule(IServiceCollectionQuartzConfigurator q)
        {
            q.ScheduleJob<InitPredictionModelsJob>(j => j.StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(5)
                .RepeatForever()));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            bool isInit = await _predcitionModelsService.Init();
            if (isInit)
                await context.Scheduler.PauseJob(context.JobDetail.Key);
        }
    }
}
