using InfluenceCalculator.API.Models;
using Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Influence Calculator Microservice",
        Version = "v1",
        Description = "–асчет вли€ни€ воздействий на пациента",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Lisovenko Anton",
            Email = String.Empty
        }
    });
});

builder.Services.AddScoped<IPatientParameter, PatientParameter>();
builder.Services.AddScoped<IInfluenceResult, InfluenceResult>();
builder.Services.AddScoped<IInfluenceEffectivenessCalculator, InfluenceModel>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

string connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<InfluenceContext>(options => options.UseNpgsql(connectionString)); // Registration dbContext as service.

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();
// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Influence Calculator API");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
