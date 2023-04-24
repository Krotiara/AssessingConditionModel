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

builder.Services.Configure<TempModelSettings>(builder.Configuration.GetSection("Models"));
builder.Services.Configure<EnvSettings>(builder.Configuration.GetSection("EnvSettings"));
CommandsDependensyRegistrator.RegisterDependencies(builder.Services);

/*������ �� ������ ��������� ���� �������. ��� ����� ��� ����������� �������� ��������� ���������� IMediator. �� �������������� � ����� ���������� ������������ ��� �� �������� AddMediatR.*/
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

#region rabbitMQ
//var configReceiveAddData = builder.Configuration.GetSection("RabbitMqAddData");
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.Configure<AddDataConfig>(builder.Configuration.GetSection("RabbitMqAddInfo"));
/*������ �� ������ ��������� ���� �������. ��� ����� ��� ����������� �������� ��������� ���������� IMediator. �� �������������� � ����� ���������� ������������ ��� �� �������� AddMediatR.*/
//if (builder.Configuration.GetSection("RabbitMq").Get<RabbitMqConfiguration>().Enabled)
//    builder.Services.AddHostedService<UpdatePatientsDataReceiver>();
//if(builder.Configuration.GetSection("RabbitMqAddInfo").Get<AddDataConfig>().Enabled)
//    builder.Services.AddHostedService<AddPatientsReceiver>();
#endregion

string connectionString = builder.Configuration.GetConnectionString("PostgresConnection");


//��� ��������� ������ Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp with time zone', only UTC is supported.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services
    .AddSingleton<ICodeExecutor, CodeExecutorService>()
    .AddTransient<IWebRequester, HttpWebRequester>();
builder.Services
    .AddTransient<IAgingDynamics<AgingState>, AgingDynamics>()
    .AddSingleton<IAgentsService, AgentsService>();

builder.Services
    .AddTransient<IRequestHandler<GetAgentStateQuery, IAgentState>, GetAgentStateQueryHandler>();

builder.Services.AddSingleton<IAgentsStore, AgentsStore>();

builder.Services.AddTransient<IProperty, Property>();
builder.Services.AddTransient<IAgentState, AgentState>();
builder.Services
    .AddTransient<IRequestHandler<GetCommandTypesMetaQueue, ICommandArgsTypesMeta>, GetCommandTypesMetaQueueHandler>()
    .AddTransient<IRequestHandler<GetCommandArgsValuesQueue, List<object>>, GetCommandArgsValuesQueueHandler>()
    .AddTransient<IRequestHandler<ParseCodeLineCommand, ICommand>, ParseCodeLineCommandHandler>()
    .AddTransient<IRequestHandler<GetCommandNameCommand, string>, GetCommandNameCommandHandler>()
    .AddTransient<IRequestHandler<ExecuteCodeLineCommand, Unit>, ExecuteCodeLineCommandHandler>()
    .AddTransient<IRequestHandler<ConvertArgsCommand, object[]>, ConvertArgsCommandHandler>()
    .AddTransient<IMetaStorageService, InternalMetaStorageService>()
    .AddTransient<ICodeResolveService, CodeResolveService>();

builder.Services.AddSingleton<SettingsStore>();
builder.Services.AddSingleton<SettingsService>();

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
