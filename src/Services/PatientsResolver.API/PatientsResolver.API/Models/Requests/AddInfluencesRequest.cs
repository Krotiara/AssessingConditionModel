using PatientsResolver.API.Entities.Mongo;

namespace PatientsResolver.API.Models.Requests
{
    public class AddInfluencesRequest
    {
        public List<Influence> Influences { get; set; }
    }
}
