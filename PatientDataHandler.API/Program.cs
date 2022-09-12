using Interfaces;
using Microsoft.EntityFrameworkCore;
using PatientDataHandler.API.Models;

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

builder.Services.AddScoped<IPatientData, PatientData>();
builder.Services.AddScoped<IPatientParameter, PatientParameter>();
builder.Services.AddScoped<IInfluence, Influence>();

builder.Services.AddScoped<ExcelDataProvider>();

builder.Services.AddTransient<Func<DataParserTypes, IDataProvider>>(serviceProvider => serviceTypeName =>
{
    switch (serviceTypeName)
    {
        case DataParserTypes.TestVahitova:
            return serviceProvider.GetService<ExcelDataProvider>();
        default:
            return null;
    }
});

string connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<PatientsDataDbContext>(options => options.UseNpgsql(connectionString)); // Registration dbContext as service.

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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patient Data Handler API");
    c.RoutePrefix = string.Empty;
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
