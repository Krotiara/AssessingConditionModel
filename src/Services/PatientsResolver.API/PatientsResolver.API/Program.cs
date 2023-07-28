using Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PatientsResolver.API.Data;
using PatientsResolver.API.Data.Store;
using PatientsResolver.API.Entities;
using PatientsResolver.API.Entities.Mongo;
using PatientsResolver.API.Models;
using PatientsResolver.API.Service.Services;
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
        Title = "����������� ������� ���������",
        Description = "����������� �������� �� �������� � �������������� ������ � ��������� � ������������ �� ���",
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
//��� scoped requesthandler-��
builder.Host.ConfigureDefaults(args)
    .UseDefaultServiceProvider(options =>options.ValidateScopes = false); // needed for mediatr DI

services.AddMongoService(builder.Configuration);

services.AddScoped<IPatientParameter, PatientParameter>();
services.AddScoped<IPatient, Patient>();
services.AddScoped<IFileData, FileData>();
services.AddTransient<IInfluence, Influence>();
services.AddOptions();

/*������ �� ������ ��������� ���� �������. ��� ����� ��� ����������� �������� ��������� ���������� IMediator. �� �������������� � ����� ���������� ������������ ��� �� �������� AddMediatR.*/
services.AddMediatR(Assembly.GetExecutingAssembly());

services.AddSingleton<PatientsStore>();
services.AddSingleton<InfluencesStore>();
services.AddSingleton<PatientsDataService>();
services.AddSingleton<InfluencesDataService>();

var app = builder.Build();

//��� ��������� ������ Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp with time zone', only UTC is supported.
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
