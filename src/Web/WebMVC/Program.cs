using Interfaces;
using Microsoft.AspNetCore.Hosting;
using WebMVC.Models;
using WebMVC.Services;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add ToastNotification
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddTransient<IAgingState, AgingState>();
builder.Services.AddTransient<IAgingDynamics<AgingState>, AgingDynamics>();
builder.Services.AddTransient<IPatient, Patient>();
builder.Services.AddTransient<IWebRequester, HttpClientWebRequester>();
builder.Services.AddTransient<IAgingDynamicsSaveService, AgingDynamicsSaveService>();
builder.Services.AddScoped<IPatientService, PatientService>();

//builder.Services.AddHttpClientServices();

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
app.UseNotyf();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Medic}/{action=Index}/{id?}");

app.Run();