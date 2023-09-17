using Agents.API.Data.Store;
using Agents.API.Entities.Mongo;
using MongoDB.Bson;
using Quartz;

namespace Agents.API.Jobs
{
    public class InitUsersJob : IJob
    {
        private readonly UsersStore _usersStore;

        public InitUsersJob(UsersStore usersStore = null)
        {
            _usersStore = usersStore;
        }

        public static void Schedule(IServiceCollectionQuartzConfigurator q)
        {
            q.ScheduleJob<InitUsersJob>(j => j.StartNow());
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if (_usersStore == null)
                return;

            bool isUsersExist = (await _usersStore.Collection.CountDocumentsAsync(new BsonDocument())) > 0;
            if (isUsersExist)
                return;
            var user = new UserDocument
            {
                Login = "admin",
                Role = UserDocument.ADMIN,
                RegistrationDate = DateTime.Today,
                Name = "Admin"
            };
            user.SetPassword("admin");
            await _usersStore.Insert(user);
        }
    }
}
