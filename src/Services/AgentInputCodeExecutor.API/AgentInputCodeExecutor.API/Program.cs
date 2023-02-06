using AgentInputCodeExecutor.API.Entities;
using AgentInputCodeExecutor.API.Interfaces;
using AgentInputCodeExecutor.API.Service.Command;
using AgentInputCodeExecutor.API.Service.Queue;
using AgentInputCodeExecutor.API.Service.Service;
using Interfaces;
using Interfaces.DynamicAgent;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddTransient<IProperty, AgentProperty>();
builder.Services.AddTransient<IExecutableAgentCodeSettings, ExecutableAgentCodeSettings>();

builder.Services
    .AddTransient<IRequestHandler<ExecuteCodeLinesCommand, Unit>, ExecuteCodeLinesCommandHandler>()
    .AddTransient<IRequestHandler<GetCommandTypesMetaQueue, ICommandArgsTypesMeta>, GetCommandTypesMetaQueueHandler>()
    .AddTransient<IRequestHandler<ParseCodeLineCommand, ICommand>, ParseCodeLineCommandHandler>()
    .AddTransient<IRequestHandler<GetCommandNameCommand, string>, GetCommandNameCommandHandler>()
    .AddTransient<IRequestHandler<ExecuteCodeLinesCommand, Unit>, ExecuteCodeLinesCommandHandler>()
    .AddTransient<IRequestHandler<ExecuteCodeLineCommand, Unit>, ExecuteCodeLineCommandHandler>()
    .AddTransient<IWebRequester, HttpClientWebRequester>()
    .AddTransient<IMetaStorageService, InternalMetaStorageService>()
    .AddScoped<ICodeResolveService, CodeResolveService>()
    .AddSingleton<ICommandActionsProvider, CommandActionsProvider>();

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
