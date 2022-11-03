using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using Interfaces;
using Agents.API.Entities;
using Agents.API.Messaging.Receive.Receiver;
using Microsoft.EntityFrameworkCore;
using Agents.API.Data.Database;
using Agents.API.Service.Services;
using Agents.API.Messaging.Receive.Configs;
using Agents.API.Service.Query;
using Agents.API.Data.Repository;
using Agents.API.Service.Command;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 443;
});

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
if(builder.Configuration.GetSection("RabbitMqAddInfo").Get<AddDataConfig>().Enabled)
    builder.Services.AddHostedService<AddPatientsReceiver>();
#endregion

string connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<AgentsDbContext>(options => options.UseNpgsql(connectionString, builder =>
{
    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(2), null);
}), ServiceLifetime.Singleton); // Registration dbContext as service.
//Для избежания ошибки Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp with time zone', only UTC is supported.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddScoped<IUpdatePatientsDataInfo, UpdatePatientsInfo>();
builder.Services.AddTransient<IWebRequester, RestWebRequester>();
builder.Services.AddTransient<IInitPatientAgentsService, InitPatientAgentsService>();
builder.Services.AddTransient<IUpdatePatientAgentsService, UpdatePatientAgentsService>();
builder.Services.AddSingleton<IAgentPatientsRepository, AgentPatientsRepository>();
builder.Services.AddSingleton<IAgingStatesRepository, AgingStatesRepository>();
builder.Services.AddTransient<IAgingDynamics<AgingPatientState>, AgingDynamics>();

builder.Services.AddScoped<IRequestHandler<GetAgingStateQuery, AgingPatientState>, 
    GetAgingStateQueryHandler>();
builder.Services.AddScoped<IRequestHandler<GetPatientInfluencesQuery, List<Influence>>, 
    GetPatientInfluencesQueryHandler>();
builder.Services.AddScoped<IRequestHandler<GetAgingDynamicsQuery, List<IAgingDynamics<AgingPatientState>>>, 
    GetAgingDynamicsQueryHandler>();
builder.Services.AddScoped<IRequestHandler<GetAllPatientsAgingDynamicsQuery, List<IAgingDynamics<AgingPatientState>>>,
    GetAllPatientsAgingDynamicsQueryHandler>();
builder.Services.AddScoped<IRequestHandler<GetAgingStateQueryDb, AgingState>, 
    GetAgingStateQueryDbHandler>();
builder.Services.AddScoped<IRequestHandler<AddAgingStateCommand, AgingState>, 
    AddAgingStateCommandHandler>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();



app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patient Data Handler API");
    c.RoutePrefix = string.Empty;
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
