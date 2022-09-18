using Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PatientsResolver.API.Data;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Messaging.Send;
using PatientsResolver.API.Models;
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

string connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<PatientsDataDbContext>(options => options.UseNpgsql(connectionString)); // Registration dbContext as service.

builder.Services.AddScoped<IPatientData, PatientData>();
builder.Services.AddScoped<IPatientParameter, PatientParameter>();
builder.Services.AddScoped<IPatient, Patient>();

builder.Services.AddOptions();

//#region rabbitMQ
///*Теперь вы можете выполнять ваши запросы. Для этого вам потребуется получить экземпляр интерфейса IMediator. Он регистрируется в вашем контейнере зависимостей той же командой AddMediatR.*/
//builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

//var serviceClientSettingsConfig = builder.Configuration.GetSection("RabbitMq");
//builder.Services.Configure<RabbitMqConfiguration>(serviceClientSettingsConfig);

//bool.TryParse(builder.Configuration["BaseServiceSettings:UserabbitMq"], out var useRabbitMq);

//if(useRabbitMq)
//{
//    builder.Services.AddSingleton<IPatientDataUpdateSender, PatientDatasUpdateSender>();
//    builder.Services.AddSingleton<IPatientsDataFilePathSender, PatientsDataFilePathSender>();
//}

//builder.Services.AddTransient<IRequestHandler<UpdatePatientDataCommand, IPatientData>,
//    UpdatePatientDataCommandHandler>();
//builder.Services.AddTransient<IRequestHandler<CreatePatientDatasCommand, List<PatientData>>,
//    CreatePatientDatasCommandHandler>();
//builder.Services.AddTransient<IRequestHandler<ParsePatientsDataCommand, IList<IPatientData>>,
//    ParsePatientsDataCommandHandler>();
//#endregion

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
