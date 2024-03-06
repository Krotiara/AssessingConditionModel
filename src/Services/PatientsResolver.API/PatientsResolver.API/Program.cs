using Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PatientsResolver.API;
using PatientsResolver.API.Data;
using PatientsResolver.API.Data.Store;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Entities.Mongo;
using PatientsResolver.API.Middlewares;
using PatientsResolver.API.Models;
using PatientsResolver.API.Service.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

bool isRabbitMq = builder.Configuration.GetSection("RabbitMQSettings").Exists();

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

//https://stackoverflow.com/questions/50774060/asp-net-core-mediatr-error-register-your-handlers-with-the-container
//Для scoped requesthandler-ов
builder.Host.ConfigureDefaults(args)
    .UseDefaultServiceProvider(options =>options.ValidateScopes = false); // needed for mediatr DI

services.AddMongoService(builder.Configuration);
IConfigurationSection section = builder.Configuration.GetSection("PostgreSQLDbSettings");
services.Configure<PostgreSQLDbSettings>(section);

services.AddTransient<IFileData, FileData>();
services.AddTransient<IInfluence, Influence>();
services.AddOptions();

services.AddMediatR(Assembly.GetExecutingAssembly());

services.AddDbContextFactory<PostgreSQLParametersDbContext>();

services.AddTransient<IPatientsStore, MongoPatientsStore>();
services.AddTransient<IParametersStore, PostgreSQLParametersStore>();
services.AddTransient<IPatientsMetaStore, MongoPatientsMetaStore>();
services.AddTransient<MongoInfluencesStore>();

services.AddSingleton<PatientsDataService>();
services.AddSingleton<InfluencesDataService>();

if (isRabbitMq)
    services.AddRabbitMQEventBus(builder.Configuration);

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

app.UseMiddleware<ErrorHandlerMiddleware>();

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
