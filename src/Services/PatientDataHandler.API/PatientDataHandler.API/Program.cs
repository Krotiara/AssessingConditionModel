using Interfaces;
using MediatR;
using PatientDataHandler.API.Entities;
using PatientDataHandler.API.Messaging.Receive.Receiver;
using PatientDataHandler.API.Service.Command;
using PatientDataHandler.API.Service.Services;
using PatientDataHandler.API_Messaging.Send.Sender;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Version = "v1",
        Title = "ћикросервис обработки данных пациентов",
        Description = "ћикросервис отвечает за обработку сырых данных пациента.",
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

builder.Services.AddScoped<IPatientParameter, PatientParameter>();
builder.Services.AddScoped<IInfluence<Patient, PatientParameter>, Influence>();

builder.Services.AddSingleton<TestDataProvider>();

builder.Services.AddTransient<Func<InputFileType, IDataProvider>>(serviceProvider => serviceTypeName =>
{
    switch (serviceTypeName)
    {
        case InputFileType.Test:
            return serviceProvider.GetService<TestDataProvider>();
        default:
            return null;
    }
});

#region RabbitMQ
var serviceClientSettingsConfigFile = builder.Configuration.GetSection("RabbitMq");
var serviceClientSettingsConfigData = builder.Configuration.GetSection("RabbitMq1");
var serviceClientSettings = serviceClientSettingsConfigFile.Get<PatientDataHandler.API.Messaging.Receive.RabbitMqConfiguration>();

builder.Services.Configure<PatientDataHandler.API.Messaging.Receive.RabbitMqConfiguration>(serviceClientSettingsConfigFile);
builder.Services.Configure<PatientDataHandler.API.Messaging.Send.RabbitMqConfiguration>(serviceClientSettingsConfigData);
builder.Services.Configure<ParseDataSettings>(builder.Configuration.GetSection("ParseDataSettings"));

builder.Services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(IParsePatientsDataService).Assembly);
builder.Services.AddTransient<IRequestHandler<SendPatientsDataFileCommand, Unit>, SendPatientsDataFileCommandHandler>();
builder.Services.AddTransient<IPatientsDataSender, PatientsDataSender>();
builder.Services.AddTransient<IParsePatientsDataService, ParsePatientsDataService>();

if (serviceClientSettings.Enabled)
{
    builder.Services.AddHostedService<ParsePatientsDataReceiver>();
}
#endregion

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

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patient Data Handler API");
    c.RoutePrefix = string.Empty;
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
