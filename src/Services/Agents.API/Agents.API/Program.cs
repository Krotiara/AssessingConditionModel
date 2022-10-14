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
using Agents.API.Service.Command;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

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

#region rabbitMQ
//var configReceiveAddData = builder.Configuration.GetSection("RabbitMqAddData");

builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.Configure<AddDataConfig>(builder.Configuration.GetSection("RabbitMqAddInfo"));
/*������ �� ������ ��������� ���� �������. ��� ����� ��� ����������� �������� ��������� ���������� IMediator. �� �������������� � ����� ���������� ������������ ��� �� �������� AddMediatR.*/
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
if (builder.Configuration.GetSection("RabbitMq").Get<RabbitMqConfiguration>().Enabled)
    builder.Services.AddHostedService<UpdatePatientsDataReceiver>();
if(builder.Configuration.GetSection("RabbitMqAddInfo").Get<AddDataConfig>().Enabled)
    builder.Services.AddHostedService<AddPatientsReceiver>();
#endregion

/*������ �� ������ ��������� ���� �������. ��� ����� ��� ����������� �������� ��������� ���������� IMediator. �� �������������� � ����� ���������� ������������ ��� �� �������� AddMediatR.*/
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

string connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<AgentsDbContext>(options => options.UseNpgsql(connectionString, builder =>
{
    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(2), null);
}), ServiceLifetime.Singleton); // Registration dbContext as service.
//��� ��������� ������ Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp with time zone', only UTC is supported.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddScoped<IUpdatePatientsInfo, UpdatePatientsInfo>();
builder.Services.AddTransient<IWebRequester, RestWebRequester>();
builder.Services.AddTransient<IInitPatientAgentsService, InitPatientAgentsService>();
builder.Services.AddSingleton<IAgentPatientsRepository, AgentPatientsRepository>();

builder.Services.AddTransient<IRequestHandler<GetPatientDatasCommand, IList<IPatientData<IPatientParameter, IPatient, IInfluence>>>,
    GetPatientDatasCommandHandler>();


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
