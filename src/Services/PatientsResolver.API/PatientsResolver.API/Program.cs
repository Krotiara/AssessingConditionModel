using Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PatientsResolver.API.Data;
using PatientsResolver.API.Data.Repository;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Messaging.Receive.Receiver;
using PatientsResolver.API.Messaging.Send.Configurations;
using PatientsResolver.API.Messaging.Send.Sender;
using PatientsResolver.API.Models;
using PatientsResolver.API.Service.Command;
using PatientsResolver.API.Service.Query;
using PatientsResolver.API.Service.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Version = "v1",
        Title = "Управляющий данными пациентов",
        Description = "Микросервис отвечает за хранение и предоставление данных о пациентах и воздействиях на них",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
        {
            Name = "Lisovenko Anton",
            Email = String.Empty
        }
    });
});

//builder.Services.AddHttpsRedirection(options =>
//{
//    options.HttpsPort = 443;
//});

string connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<PatientsDataDbContext>(options => options.UseNpgsql(connectionString, builder =>
{
    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(2), null);
}), ServiceLifetime.Singleton); // Registration dbContext as service.


builder.Services.AddScoped<IPatientParameter, PatientParameter>();
builder.Services.AddScoped<IPatient, Patient>();
builder.Services.AddScoped<IFileData, FileData>();
builder.Services.AddScoped<IInfluence<Patient, PatientParameter>, Influence>();


builder.Services.AddTransient<IAddInfluencesDataFromSourceService, AddInfluencesDataFromSourceService>();
builder.Services.AddSingleton<IPatientFileDataSender, PatientFileDataSender>();
builder.Services.AddSingleton<IUpdatePatientsSender, UpdatePatientsSender>();
builder.Services.AddSingleton<IPatientsSender, PatientsSender>();
builder.Services.AddTransient<IInfluenceRepository, InfluenceRepository>();
builder.Services.AddSingleton<InfluenceRepository>();
builder.Services.AddSingleton<PatientsRepository>(); //МБ это криво
builder.Services.AddSingleton<PatientParametersRepository>();
builder.Services.AddOptions();

#region rabbitMQ
#warning Скорее всего плохоже решение с доп секцией.
var serviceClientSettingsConfigData = builder.Configuration.GetSection("RabbitMq1");
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.Configure<PatientsResolver.API.Messaging.Receive.RabbitMqConfiguration>(serviceClientSettingsConfigData);
builder.Services.Configure<RabbitMqUpdateInfoConfig>(builder.Configuration.GetSection("RabbitMqSendUpdateInfo"));
builder.Services.Configure<RabbitMqAddInfoConfig>(builder.Configuration.GetSection("RabbitMqSendAddInfo"));


var serviceClientSettings = serviceClientSettingsConfigData.Get<PatientsResolver.API.Messaging.Send.Configurations.RabbitMqConfiguration>();
if (serviceClientSettings.Enabled)
{
    builder.Services.AddHostedService<AddPatientsDataFromSourceReceiver>();
}
#endregion

/*Теперь вы можете выполнять ваши запросы. Для этого вам потребуется получить экземпляр интерфейса IMediator. Он регистрируется в вашем контейнере зависимостей той же командой AddMediatR.*/
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddTransient<IRequestHandler<GetPatientQuery, Patient>,
    GetPatientQueryHandler>();
builder.Services.AddTransient<IRequestHandler<AddInfluenceDataCommand, List<Influence>>,
    AddInfluenceDataCommandHandler>();
builder.Services.AddTransient<IRequestHandler<SendPatientDataFileSourceCommand, bool>,
    SendPatientDataFileSourceCommandHandler>();
builder.Services.AddTransient<IRequestHandler<SendPatientsCommand, Unit>,
    SendPatientsCommandHandler>();
builder.Services.AddTransient<IRequestHandler<AddNotExistedPatientsCommand, IList<Patient>>,
    AddNotExistedPatientsCommandHandler>();
builder.Services.AddTransient<IRequestHandler<AddPatientCommand, bool>,
    AddPatientCommandHandler>();
builder.Services.AddTransient<IRequestHandler<AddInfluenceDataCommand, List<Influence>>,
    AddInfluenceDataCommandHandler>();
builder.Services.AddTransient<IRequestHandler<GetPatientInfluencesQuery, List<Influence>>,
    GetPatientInfluencesQueryHandler>();
builder.Services.AddTransient<IRequestHandler<SendUpdatePatientsInfoCommand, Unit>,
    SendUpdatePatientsInfoCommandHandler>();
builder.Services.AddTransient<IRequestHandler<GetLatesPatientParametersQuery, List<PatientParameter>>,
    GetLatesPatientParametersQueryHandler>();
builder.Services.AddTransient<IRequestHandler<GetInfluencesQuery, List<Influence>>,
    GetInfluencesQueryHandler>();
builder.Services.AddTransient<IRequestHandler<UpdatePatientCommand, Patient>,
    UpdatePatientCommandHandler>();
builder.Services.AddTransient<IRequestHandler<DeletePatientCommand, bool>,
    DeletePatientCommandHandler>();
builder.Services.AddTransient<IRequestHandler<GetPatientInfluenceByIdQueue, Influence>,
    GetPatientInfluenceByIdQueueHandler>();

var app = builder.Build();

//Для избежания ошибки Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp with time zone', only UTC is supported.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

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

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patients Resolver API");
    c.RoutePrefix = string.Empty;
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
