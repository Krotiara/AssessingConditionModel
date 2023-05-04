using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using Interfaces;
using Agents.API.Entities;
using Agents.API.Messaging.Receive.Receiver;
using Microsoft.EntityFrameworkCore;
using Agents.API.Service.Services;
using Agents.API.Messaging.Receive.Configs;
using Agents.API.Service.Query;
using Agents.API.Data.Store;
using Agents.API.Interfaces;
using Interfaces.DynamicAgent;
using Agents.API.Service.Command;
using Agents.API;
using Agents.API.Entities.AgentsSettings;
using Quartz;
using Agents.API.Jobs;
using Agents.API.Messaging.Send;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddControllers().AddNewtonsoftJson();

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Version = "v1",
        Title = "Микросервис агентов системы",
        Description = "Отвечает за отслеживание и прогнозирование состояния пациентов. " +
        "\r\nОтслеживание также подразумевает под собой оповещение об изменении " +
        "состояния пациента путем сообщений пользователю врачу." +
        "\r\nНа программном уровне отвечает за работу агентов системы.\r\n.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
        {
            Name = "Lisovenko Anton",
            Email = String.Empty
        }
    });
});

services.Configure<TempModelSettings>(builder.Configuration.GetSection("Models"));
services.Configure<EnvSettings>(builder.Configuration.GetSection("EnvSettings"));
CommandsDependensyRegistrator.RegisterDependencies(services);
services.AddMediatR(Assembly.GetExecutingAssembly());

#region rabbitMQ
services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMq"));
services.Configure<AddDataConfig>(builder.Configuration.GetSection("RabbitMqAddInfo"));
services.Configure<InitServiceRabbitConfig>(builder.Configuration.GetSection("InitServiceRabbitConfig"));
#endregion

string connectionString = builder.Configuration.GetConnectionString("PostgresConnection");


//Для избежания ошибки Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp with time zone', only UTC is supported.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

services
    .AddTransient<IWebRequester, HttpWebRequester>()
    .AddTransient<IAgingDynamics<AgingState>, AgingDynamics>()
    .AddTransient<IProperty, Property>()
    .AddTransient<IAgentState, AgentState>();

services
    .AddTransient<IRequestHandler<GetPatientInfoQuery, HttpResponseMessage>, GetPatientInfoQueryHandler>()
    .AddTransient<IRequestHandler<GetAgentStateQuery, IAgentState>, GetAgentStateQueryHandler>()
    .AddTransient<IRequestHandler<GetCommandTypesMetaQueue, ICommandArgsTypesMeta>, GetCommandTypesMetaQueueHandler>()
    .AddTransient<IRequestHandler<GetCommandArgsValuesQueue, List<object>>, GetCommandArgsValuesQueueHandler>()
    .AddTransient<IRequestHandler<ParseCodeLineCommand, ICommand>, ParseCodeLineCommandHandler>()
    .AddTransient<IRequestHandler<GetCommandNameCommand, string>, GetCommandNameCommandHandler>()
    .AddTransient<IRequestHandler<ExecuteCodeLineCommand, Unit>, ExecuteCodeLineCommandHandler>()
    .AddTransient<IRequestHandler<ConvertArgsCommand, object[]>, ConvertArgsCommandHandler>()
    .AddTransient<IMetaStorageService, InternalMetaStorageService>()
    .AddTransient<ICodeResolveService, CodeResolveService>();

services
    .AddSingleton<SettingsStore>()
    .AddSingleton<IAgentsStore, AgentsStore>();

services
    .AddSingleton<SettingsService>()
    .AddSingleton<PredcitionModelsService>()
    .AddSingleton<AgentsService>()
    .AddSingleton<InitServiceSender>()
    .AddSingleton<ICodeExecutor, CodeExecutorService>()
    .AddSingleton<PredcitionModelsService>()
    .AddSingleton<PatientParametersService>();

services.AddQuartz(q =>
{
    q.SchedulerId = "Scheduler-Core";
    q.UseMicrosoftDependencyInjectionJobFactory();

    q.UseSimpleTypeLoader();
    q.UseInMemoryStore();

    InitPredictionModelsJob.Schedule(q);
    InitJob.Schedule(q);
});
services.AddQuartzHostedService();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();



app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agents API");
    c.RoutePrefix = string.Empty;
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
