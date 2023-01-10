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

builder.Services.AddDbContextFactory<PatientsDataDbContext>(options => options.UseNpgsql(connectionString, builder =>
{
    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(2), null);
}), ServiceLifetime.Scoped);


//https://stackoverflow.com/questions/50774060/asp-net-core-mediatr-error-register-your-handlers-with-the-container
//Для scoped requesthandler-ов
builder.Host.ConfigureDefaults(args)
    .UseDefaultServiceProvider(options =>options.ValidateScopes = false); // needed for mediatr DI

builder.Services.AddScoped<IPatientParameter, PatientParameter>();
builder.Services.AddScoped<IPatient, Patient>();
builder.Services.AddScoped<IFileData, FileData>();
builder.Services.AddScoped<IInfluence<Patient, PatientParameter>, Influence>();


builder.Services.AddTransient<IAddInfluencesDataFromSourceService, AddInfluencesDataFromSourceService>();
builder.Services.AddSingleton<IPatientFileDataSender, PatientFileDataSender>();
builder.Services.AddSingleton<IUpdatePatientsSender, UpdatePatientsSender>();
builder.Services.AddSingleton<IPatientsSender, PatientsSender>();
builder.Services.AddScoped<IInfluenceRepository, InfluenceRepository>();
builder.Services.AddScoped<InfluenceRepository>();
builder.Services.AddScoped<PatientsRepository>(); //МБ это криво
builder.Services.AddScoped<PatientParametersRepository>();
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

builder.Services.AddScoped<IRequestHandler<GetPatientQuery, Patient>,
    GetPatientQueryHandler>();
builder.Services.AddScoped<IRequestHandler<AddInfluenceDataCommand, List<Influence>>,
    AddInfluenceDataCommandHandler>();
builder.Services.AddScoped<IRequestHandler<SendPatientDataFileSourceCommand, bool>,
    SendPatientDataFileSourceCommandHandler>();
builder.Services.AddScoped<IRequestHandler<SendPatientsCommand, Unit>,
    SendPatientsCommandHandler>();
builder.Services.AddScoped<IRequestHandler<AddNotExistedPatientsCommand, IList<Patient>>,
    AddNotExistedPatientsCommandHandler>();
builder.Services.AddScoped<IRequestHandler<AddPatientCommand, bool>,
    AddPatientCommandHandler>();
builder.Services.AddScoped<IRequestHandler<AddInfluenceDataCommand, List<Influence>>,
    AddInfluenceDataCommandHandler>();
builder.Services.AddScoped<IRequestHandler<GetPatientInfluencesQuery, List<Influence>>,
    GetPatientInfluencesQueryHandler>();
builder.Services.AddScoped<IRequestHandler<SendUpdatePatientsInfoCommand, Unit>,
    SendUpdatePatientsInfoCommandHandler>();
builder.Services.AddScoped<IRequestHandler<GetLatesPatientParametersQuery, List<PatientParameter>>,
    GetLatesPatientParametersQueryHandler>();
builder.Services.AddScoped<IRequestHandler<GetInfluencesQuery, List<Influence>>,
    GetInfluencesQueryHandler>();
builder.Services.AddScoped<IRequestHandler<UpdatePatientCommand, Patient>,
    UpdatePatientCommandHandler>();
builder.Services.AddScoped<IRequestHandler<DeletePatientCommand, bool>,
    DeletePatientCommandHandler>();
builder.Services.AddScoped<IRequestHandler<GetPatientInfluenceByIdQueue, Influence>,
    GetPatientInfluenceByIdQueueHandler>();
builder.Services.AddScoped<IRequestHandler<AddPatientInfluenceCommand, bool>,
    AddPatientInfluenceCommandHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateInfluenceCommand, Influence>, 
    UpdateInfluenceCommandHandler>();
builder.Services.AddScoped<IRequestHandler<DeleteInfluenceCommand, bool>, 
    DeleteInfluenceCommandHandler>();

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
