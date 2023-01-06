using Interfaces;
using Microsoft.AspNetCore.Hosting;
using NToastNotify;
using WebMVC.Models;
using WebMVC.Services;


var builder = WebApplication.CreateBuilder(args);

// Add ToastNotification
builder.Services.AddRazorPages().AddNToastNotifyNoty(new NotyOptions
{
    ProgressBar = true,
    Timeout = 5000
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
app.UseNToastNotify();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Medic}/{action=Index}/{id?}");

app.Run();