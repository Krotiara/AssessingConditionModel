using Agents.API.Entities;
using Agents.API.Entities.Requests;
using Agents.API.Interfaces;
using Interfaces;
using Interfaces.DynamicAgent;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    //public class CommandActionsProvider
    //{

    //    private readonly IMediator _mediator;
    //    private readonly IWebRequester _webRequester;
    //    private readonly Dictionary<SystemCommands, Delegate> _delegates;
    //    private readonly string _patientsResolverApiUrl;
    //    private readonly string _modelsApiUrl;

    //    public CommandActionsProvider(IMediator mediator, IWebRequester webRequester)
    //    {
    //        this._webRequester = webRequester;
    //        this._mediator = mediator;
    //        _patientsResolverApiUrl = Environment.GetEnvironmentVariable("PATIENTRESOLVER_API_URL");
    //        _modelsApiUrl = Environment.GetEnvironmentVariable("MODELS_API_URL"); //TODO - в отдельный сервис
    //        _delegates = new Dictionary<SystemCommands, Delegate>();
    //        InitDelegates();
    //    }
    //}
}
