using Agents.API.Service.Services;
using Quartz;

namespace Agents.API.Jobs
{
    public class InitSettingsJob : IJob
    {
        private readonly SettingsService _setsService;

        public InitSettingsJob(SettingsService setsService)
        {
            _setsService = setsService;
        }


        public static void Schedule(IServiceCollectionQuartzConfigurator q)
        {
            q.ScheduleJob<InitSettingsJob>(j => j.StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(5)
                .RepeatForever()));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            bool isInit = await _setsService.Init();
            if (isInit)
                await context.Scheduler.PauseJob(context.JobDetail.Key);
        }
    }
}
