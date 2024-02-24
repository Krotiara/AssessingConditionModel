using System.Reflection;
using MediatR;
using Interfaces;
using Agents.API.Entities;
using Microsoft.EntityFrameworkCore;
using Agents.API.Service.Services;
using Agents.API.Data.Store;
using Agents.API.Interfaces;
using Agents.API.Service.Command;
using Agents.API;
using Agents.API.Entities.AgentsSettings;
using Quartz;
using Agents.API.Jobs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Agents.API.Middlewares;
using ASMLib.DynamicAgent;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

bool isRabbitMq = builder.Configuration.GetSection("RabbitMQSettings").Exists();

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.Events.OnRedirectToLogin = c =>
        {
            c.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        o.Events.OnRedirectToAccessDenied = c =>
        {
            c.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });


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

services.AddMongoService(builder.Configuration);

services.Configure<EnvSettings>(builder.Configuration.GetSection("EnvSettings"));
CommandsDependensyRegistrator.RegisterDependencies(services);
services.AddMediatR(Assembly.GetExecutingAssembly());

string connectionString = builder.Configuration.GetConnectionString("PostgresConnection");


//Для избежания ошибки Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp with time zone', only UTC is supported.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//entities
services
    .AddTransient<IWebRequester, HttpWebRequester>()
    .AddTransient<IProperty, Property>()
    .AddTransient<IAgentState, AgentState>();

//store
services.AddTransient<UsersStore>();

services.AddSingleton<SettingsStore>();
services.AddSingleton<IAgentsStore, MemoryAgentsStore>();

//service
services
    .AddSingleton<PredictionRequestsService>()
    .AddSingleton<ICodeExecutor, CodeExecutorService>()
    .AddSingleton<AgentsService>()
    .AddSingleton<PatientsService>();

services
    .AddTransient<IRequestHandler<ExecuteCodeLineCommand, CommandResult>, ExecuteCodeLineCommandHandler>()
    .AddTransient<IMetaStorageService, InternalMetaStorageService>()
    .AddTransient<ICodeResolveService, CodeResolveService>();

if (isRabbitMq)
    services.AddRabbitMQEventBus(builder.Configuration);

services.AddQuartz(q =>
{
    q.SchedulerId = "Scheduler-Core";
    q.UseMicrosoftDependencyInjectionJobFactory();

    q.UseSimpleTypeLoader();
    q.UseInMemoryStore();

    InitPredictionModelsJob.Schedule(q);
    InitUsersJob.Schedule(q);
    ProcessCurrentPredictionsJob.Schedule(q);
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

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agents API");
    c.RoutePrefix = string.Empty;
});


IConfigurationSection authSettings = builder.Configuration.GetSection("AuthenticationSettings");
bool isUseAuth = authSettings.Exists() && authSettings.GetValue<bool>("IsUse");

if (isUseAuth)
    app.MapControllers();
else
    app.MapControllers().AllowAnonymous();

app.Run();
