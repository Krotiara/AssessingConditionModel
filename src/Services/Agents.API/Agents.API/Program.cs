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
using Agents.API.Data.Repository;
using Agents.API.Interfaces;
using Interfaces.DynamicAgent;
using Agents.API.Service.Command;
using Agents.API.Entities.DynamicAgent;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddHttpsRedirection(options =>
//{
//    options.HttpsPort = 443;
//});

builder.Services.AddSwaggerGen(c =>
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


/*Теперь вы можете выполнять ваши запросы. Для этого вам потребуется получить экземпляр интерфейса IMediator. Он регистрируется в вашем контейнере зависимостей той же командой AddMediatR.*/
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

#region rabbitMQ
//var configReceiveAddData = builder.Configuration.GetSection("RabbitMqAddData");
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.Configure<AddDataConfig>(builder.Configuration.GetSection("RabbitMqAddInfo"));
/*Теперь вы можете выполнять ваши запросы. Для этого вам потребуется получить экземпляр интерфейса IMediator. Он регистрируется в вашем контейнере зависимостей той же командой AddMediatR.*/
if (builder.Configuration.GetSection("RabbitMq").Get<RabbitMqConfiguration>().Enabled)
    builder.Services.AddHostedService<UpdatePatientsDataReceiver>();
//if(builder.Configuration.GetSection("RabbitMqAddInfo").Get<AddDataConfig>().Enabled)
//    builder.Services.AddHostedService<AddPatientsReceiver>();
#endregion

string connectionString = builder.Configuration.GetConnectionString("PostgresConnection");


//Для избежания ошибки Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp with time zone', only UTC is supported.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services
    .AddScoped<IUpdatePatientsDataInfo, UpdatePatientsInfo>()
    .AddScoped<ICodeExecutor, CodeExecutorService>()
    .AddTransient<IWebRequester, HttpClientWebRequester>();
builder.Services
    .AddTransient<IUpdatePatientAgentsService, UpdatePatientAgentsService>();
builder.Services
    .AddTransient<IAgingDynamics<AgingState>, AgingDynamics>()
    .AddTransient<IAgentInitSettingsProvider, AgentInitSettingsProvider>();

builder.Services
    .AddTransient<IRequestHandler<GetAgentStateQuery, IAgentState>, GetAgentStateQueryHandler>();

builder.Services.AddSingleton<IDynamicAgentsRepository, DynamicAgentsRepository>();

builder.Services.AddTransient<IProperty, AgentProperty>();
builder.Services.AddTransient<IExecutableAgentCodeSettings, ExecutableAgentCodeSettings>();
builder.Services
    .AddTransient<IRequestHandler<GetCommandTypesMetaQueue, ICommandArgsTypesMeta>, GetCommandTypesMetaQueueHandler>()
    .AddTransient<IRequestHandler<GetCommandArgsValuesQueue, List<object>>, GetCommandArgsValuesQueueHandler>()
    .AddTransient<IRequestHandler<ParseCodeLineCommand, ICommand>, ParseCodeLineCommandHandler>()
    .AddTransient<IRequestHandler<GetCommandNameCommand, string>, GetCommandNameCommandHandler>()
    .AddTransient<IRequestHandler<ExecuteCodeLineCommand, Unit>, ExecuteCodeLineCommandHandler>()
    .AddTransient<IRequestHandler<ConvertArgsCommand, object[]>, ConvertArgsCommandHandler>()
    .AddTransient<IWebRequester, HttpClientWebRequester>()
    .AddTransient<IMetaStorageService, InternalMetaStorageService>()
    .AddScoped<ICodeResolveService, CodeResolveService>()
    .AddSingleton<ICommandActionsProvider, CommandActionsProvider>();


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
