using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using Interfaces;
using Agents.API.Entities;
using Microsoft.EntityFrameworkCore;
using Agents.API.Service.Services;
using Agents.API.Data.Store;
using Agents.API.Interfaces;
using Interfaces.DynamicAgent;
using Agents.API.Service.Command;
using Agents.API;
using Agents.API.Entities.AgentsSettings;
using Quartz;
using Agents.API.Jobs;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;


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
        Title = "����������� ������� �������",
        Description = "�������� �� ������������ � ��������������� ��������� ���������. " +
        "\r\n������������ ����� ������������� ��� ����� ���������� �� ��������� " +
        "��������� �������� ����� ��������� ������������ �����." +
        "\r\n�� ����������� ������ �������� �� ������ ������� �������.\r\n.",
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


//��� ��������� ������ Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp with time zone', only UTC is supported.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//entities
services
    .AddTransient<IWebRequester, HttpWebRequester>()
    .AddTransient<IProperty, Property>()
    .AddTransient<IAgentState, AgentState>();

//store
services.AddTransient<UsersStore>();

services
    .AddSingleton<SettingsStore>()
    .AddSingleton<IAgentsStore, AgentsStore>();

//service
services
    .AddSingleton<SettingsService>()
    .AddSingleton<PredictionRequestsService>()
    .AddSingleton<AgentsService>()
    .AddSingleton<ICodeExecutor, CodeExecutorService>()
    .AddSingleton<PatientsService>();

services
    .AddTransient<IRequestHandler<ExecuteCodeLineCommand, Unit>, ExecuteCodeLineCommandHandler>()
    .AddTransient<IMetaStorageService, InternalMetaStorageService>()
    .AddTransient<ICodeResolveService, CodeResolveService>();

services.AddQuartz(q =>
{
    q.SchedulerId = "Scheduler-Core";
    q.UseMicrosoftDependencyInjectionJobFactory();

    q.UseSimpleTypeLoader();
    q.UseInMemoryStore();

    InitPredictionModelsJob.Schedule(q);
    InitUsersJob.Schedule(q);
    //InitJob.Schedule(q); ��� ���� ���?
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

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agents API");
    c.RoutePrefix = string.Empty;
});


app.MapControllers();

app.Run();
