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
using PatientsResolver.API.Service.Store;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddControllers();

services.AddSwaggerGen(c =>
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

//services.AddHttpsRedirection(options =>
//{
//    options.HttpsPort = 443;
//});

string connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

services.AddDbContextFactory<PatientsDataDbContext>(options => options.UseNpgsql(connectionString, builder =>
{
    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(2), null);
}), ServiceLifetime.Scoped);


//https://stackoverflow.com/questions/50774060/asp-net-core-mediatr-error-register-your-handlers-with-the-container
//Для scoped requesthandler-ов
builder.Host.ConfigureDefaults(args)
    .UseDefaultServiceProvider(options =>options.ValidateScopes = false); // needed for mediatr DI

services.AddMongoService(builder.Configuration);

services.AddScoped<IPatientParameter, PatientParameter>();
services.AddScoped<IPatient, Patient>();
services.AddScoped<IFileData, FileData>();
services.AddScoped<IInfluence<Patient, PatientParameter>, Influence>();


services.AddTransient<IAddInfluencesDataFromSourceService, AddInfluencesDataFromSourceService>();
services.AddSingleton<IPatientFileDataSender, PatientFileDataSender>();

services.AddScoped<IInfluenceRepository, InfluenceRepository>();
services.AddScoped<InfluenceRepository>();
services.AddScoped<PatientsRepository>(); //МБ это криво
services.AddScoped<PatientParametersRepository>();
services.AddOptions();

#region rabbitMQ
#warning Скорее всего плохоже решение с доп секцией.
var serviceClientSettingsConfigData = builder.Configuration.GetSection("RabbitMq1");
services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMq"));
services.Configure<PatientsResolver.API.Messaging.Receive.RabbitMqConfiguration>(serviceClientSettingsConfigData);
services.Configure<RabbitMqUpdateInfoConfig>(builder.Configuration.GetSection("RabbitMqSendUpdateInfo"));
services.Configure<RabbitMqAddInfoConfig>(builder.Configuration.GetSection("RabbitMqSendAddInfo"));


var serviceClientSettings = serviceClientSettingsConfigData.Get<PatientsResolver.API.Messaging.Send.Configurations.RabbitMqConfiguration>();
if (serviceClientSettings.Enabled)
{
    services.AddHostedService<AddPatientsDataFromSourceReceiver>();
}
#endregion

/*Теперь вы можете выполнять ваши запросы. Для этого вам потребуется получить экземпляр интерфейса IMediator. Он регистрируется в вашем контейнере зависимостей той же командой AddMediatR.*/
services.AddMediatR(Assembly.GetExecutingAssembly());

services.AddScoped<IRequestHandler<GetPatientQuery, Patient>,
    GetPatientQueryHandler>();
services.AddScoped<IRequestHandler<AddInfluenceDataCommand, List<Influence>>,
    AddInfluenceDataCommandHandler>();
services.AddScoped<IRequestHandler<SendPatientDataFileSourceCommand, bool>,
    SendPatientDataFileSourceCommandHandler>();
services.AddScoped<IRequestHandler<AddNotExistedPatientsCommand, IList<Patient>>,
    AddNotExistedPatientsCommandHandler>();
services.AddScoped<IRequestHandler<AddPatientCommand, bool>,
    AddPatientCommandHandler>();
services.AddScoped<IRequestHandler<AddInfluenceDataCommand, List<Influence>>,
    AddInfluenceDataCommandHandler>();
services.AddScoped<IRequestHandler<GetPatientInfluencesQuery, List<Influence>>,
    GetPatientInfluencesQueryHandler>();
services.AddScoped<IRequestHandler<GetLatesPatientParametersQuery, List<PatientParameter>>,
    GetLatesPatientParametersQueryHandler>();
services.AddScoped<IRequestHandler<GetInfluencesQuery, List<Influence>>,
    GetInfluencesQueryHandler>();
services.AddScoped<IRequestHandler<UpdatePatientCommand, Patient>,
    UpdatePatientCommandHandler>();
services.AddScoped<IRequestHandler<DeletePatientCommand, bool>,
    DeletePatientCommandHandler>();
services.AddScoped<IRequestHandler<GetPatientInfluenceByIdQueue, Influence>,
    GetPatientInfluenceByIdQueueHandler>();
services.AddScoped<IRequestHandler<AddPatientInfluenceCommand, bool>,
    AddPatientInfluenceCommandHandler>();
services.AddScoped<IRequestHandler<UpdateInfluenceCommand, Influence>, 
    UpdateInfluenceCommandHandler>();
services.AddScoped<IRequestHandler<DeleteInfluenceCommand, bool>, 
    DeleteInfluenceCommandHandler>();

services.AddSingleton<PatientsStore>();
services.AddSingleton<PatientsDataService>();

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
