using Agents.API.Entities;
using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Query
{
    public class GetAllInfluencesQueryHandler : IRequestHandler<GetAllInfluencesQuery, List<Influence>>
    {
        private readonly IWebRequester webRequester;

        public GetAllInfluencesQueryHandler(IWebRequester webRequester)
        {
            this.webRequester = webRequester;
        }

        public async Task<List<Influence>> Handle(GetAllInfluencesQuery request, CancellationToken cancellationToken)
        {
            string url = $"https://host.docker.internal:8004/influences/";
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(new DateTime[2] { request.StartTimestamp, request.EndTimestamp});
            return await webRequester.GetResponse<List<Influence>>(url, "POST", body);
        }
    }
}
