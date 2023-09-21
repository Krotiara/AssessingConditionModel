using Agents.API.Entities.Documents;
using Agents.API.Entities.Requests;
using Interfaces.Mongo;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Data.Store
{
    public class UsersStore : MongoBaseService<UserDocument>
    {
        public UsersStore(MongoService mongo) : base(mongo, "Users")
        {
        }


        public async Task<UserDocument> UpdateUser(UpdateUserRequest request)
        {
            var filter = Builders<UserDocument>.Filter.Eq(x => x.Id, request.UserId);
            var updateFilter = Builders<UserDocument>.Update
                .Set(x => x.Name, request.Name)
                .Set(x => x.IsBan, request.IsBan)
                .Set(x => x.Role, request.Role);
            var user = await Collection.FindOneAndUpdateAsync(filter, updateFilter,
                new FindOneAndUpdateOptions<UserDocument>()
                {
                    ReturnDocument = ReturnDocument.After
                });
            return user;
        }


        public async Task<bool> IsUserExist(string login)
        {
            var filter = Builders<UserDocument>.Filter.Eq(x => x.Login, login);
            var count = await Collection.CountDocumentsAsync(filter);
            return count != 0;
        }
    }
}
