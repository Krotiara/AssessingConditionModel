using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Service.Command;
using Interfaces;
using Interfaces.DynamicAgent;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddTransient<IAgentProperty, AgentProperty>();
builder.Services.AddTransient<IExecutableAgentCodeSettings<AgentProperty>, ExecutableAgentCodeSettings>();

builder.Services
    .AddTransient<IRequestHandler<ExecuteCodeLinesCommand, Unit>, ExecuteCodeLinesCommandHandler>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
