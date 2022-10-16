using BioAge.API.Entites;
using BioAge.API.Service.Command;
using BioAge.API.Service.Services;
using Interfaces;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 443;
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Version = "v1",
        Title = "ћикросервис расчета биологического возраста",
        Description = "ќтвечает за вычисление биовозраста пациента.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
        {
            Name = "Lisovenko Anton",
            Email = String.Empty
        }
    });
});

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IBioAgeCalculationParameters<PatientParameter>, BioAgeCalculationParameters>();
builder.Services.AddScoped<IPatientParameter, PatientParameter>();

builder.Services.AddTransient<IBioAgeCalculationService, BioAgeCalculationService>();

builder.Services.AddTransient<IRequestHandler<CalculateBioAgeByFunctionalParamsCommand, double>, CalculateBioAgeByFunctionalParamsCommandHandler>();

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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bio Age API");
    c.RoutePrefix = string.Empty;
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
