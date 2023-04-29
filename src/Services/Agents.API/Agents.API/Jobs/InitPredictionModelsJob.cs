using Agents.API.Service.Services;
using Quartz;

namespace Agents.API.Jobs
{
    public class InitPredictionModelsJob : IJob
    {
        private readonly PredcitionModelsService _predcitionModelsService;

        public InitPredictionModelsJob(PredcitionModelsService predcitionModelsService)
        {
            _predcitionModelsService = predcitionModelsService;
        }

        public static void Schedule(IServiceCollectionQuartzConfigurator q)
        {
            q.ScheduleJob<InitPredictionModelsJob>(j => j.StartNow());
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _predcitionModelsService.Init();
        }
    }
}
