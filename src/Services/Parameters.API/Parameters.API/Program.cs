using Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Parameters.API.Service;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

//services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(o =>
//    {
//        o.Events.OnRedirectToLogin = c =>
//        {
//            c.Response.StatusCode = StatusCodes.Status401Unauthorized;
//            return Task.CompletedTask;
//        };
//        o.Events.OnRedirectToAccessDenied = c =>
//        {
//            c.Response.StatusCode = StatusCodes.Status403Forbidden;
//            return Task.CompletedTask;
//        };
//    });

services.AddControllers();

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Version = "v1",
        Title = "Микросервис параметров системы",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
        {
            Name = "Lisovenko Anton",
            Email = String.Empty
        }
    });
});

services.AddMongoService(builder.Configuration);

//store
services.AddTransient<ParametersStore>();
//service
services.AddSingleton<ParametersService>();

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

//app.UseAuthentication();
//app.UseAuthorization();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agents API");
    c.RoutePrefix = string.Empty;
});

app.MapControllers();

app.Run();
