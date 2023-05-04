using Agents.API.Messaging.Send;
using Quartz;

namespace Agents.API.Jobs
{
    public class InitJob : IJob
    {
        private readonly InitServiceSender _initServiceSender;

        public InitJob(InitServiceSender initServiceSender)
        {
            _initServiceSender = initServiceSender;
        }

        public static void Schedule(IServiceCollectionQuartzConfigurator q)
        {
            q.ScheduleJob<InitJob>(j => j.StartNow());
        }


        public Task Execute(IJobExecutionContext context) => _initServiceSender.Send();
    }
}
