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


/*������ �� ������ ��������� ���� �������. ��� ����� ��� ����������� �������� ��������� ���������� IMediator. �� �������������� � ����� ���������� ������������ ��� �� �������� AddMediatR.*/
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

#region rabbitMQ
//var configReceiveAddData = builder.Configuration.GetSection("RabbitMqAddData");
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.Configure<AddDataConfig>(builder.Configuration.GetSection("RabbitMqAddInfo"));
/*������ �� ������ ��������� ���� �������. ��� ����� ��� ����������� �������� ��������� ���������� IMediator. �� �������������� � ����� ���������� ������������ ��� �� �������� AddMediatR.*/
if (builder.Configuration.GetSection("RabbitMq").Get<RabbitMqConfiguration>().Enabled)
    builder.Services.AddHostedService<UpdatePatientsDataReceiver>();
if(builder.Configuration.GetSection("RabbitMqAddInfo").Get<AddDataConfig>().Enabled)
    builder.Services.AddHostedService<AddPatientsReceiver>();
#endregion

string connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
//builder.Services.AddDbContext<AgentsDbContext>(options => options.UseNpgsql(connectionString, builder =>
//{
//    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(2), null);
//}), ServiceLifetime.Scoped); // Registration dbContext as service.

builder.Services.AddDbContextFactory<AgentsDbContext>(options => options.UseNpgsql(connectionString, builder =>
{
    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(2), null);
}), ServiceLifetime.Scoped);


//��� ��������� ������ Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp with time zone', only UTC is supported.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddScoped<IUpdatePatientsDataInfo, UpdatePatientsInfo>();
builder.Services.AddTransient<IWebRequester, HttpClientWebRequester>();
builder.Services.AddTransient<IInitPatientAgentsService, InitPatientAgentsService>();
builder.Services.AddTransient<IUpdatePatientAgentsService, UpdatePatientAgentsService>();
builder.Services.AddScoped<IAgentPatientsRepository, AgentPatientsRepository>();
builder.Services.AddScoped<IAgingStatesRepository, AgentPatientsRepository>();
builder.Services.AddTransient<IAgingDynamics<AgingState>, AgingDynamics>();

builder.Services.AddScoped<IRequestHandler<GetAgingStateQuery, AgingState>, 
    GetAgingStateQueryHandler>();
builder.Services.AddScoped<IRequestHandler<GetPatientInfluencesQuery, List<Influence>>, 
    GetPatientInfluencesQueryHandler>();
builder.Services.AddScoped<IRequestHandler<GetAgingDynamicsQuery, List<IAgingDynamics<AgingState>>>, 
    GetAgingDynamicsQueryHandler>();
builder.Services.AddScoped<IRequestHandler<GetAllPatientsAgingDynamicsQuery, List<IAgingDynamics<AgingState>>>,
    GetAllPatientsAgingDynamicsQueryHandler>();
builder.Services.AddScoped<IRequestHandler<GetAgingStateQueryDb, AgingState>, 
    GetAgingStateQueryDbHandler>();
builder.Services.AddScoped<IRequestHandler<AddAgingStateCommand, AgingState>, 
    AddAgingStateCommandHandler>();
builder.Services.AddScoped<IRequestHandler<GetAllInfluencesQuery, List<Influence>>, 
    GetAllInfluencesQueryHandler>();

builder.Services.AddSingleton<IDynamicAgentsRepository, DynamicAgentsRepository>();


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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patient Data Handler API");
    c.RoutePrefix = string.Empty;
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
