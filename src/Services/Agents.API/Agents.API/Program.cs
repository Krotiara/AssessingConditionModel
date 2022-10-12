using Agents.API.Messaging.Receive;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using Interfaces;
using Agents.API.Entities;
using Agents.API.Messaging.Receive.Receiver;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Version = "v1",
        Title = "Микросервис агентов системы",
        Description = "Отвечает за отслеживание и прогнозирование состояния пациентов. " +
        "\r\nОтслеживание также подразумевает под собой оповещение об изменении " +
        "состояния пациента путем сообщений пользователю врачу." +
        "\r\nНа программном уровне отвечает за работу агентов системы.\r\n.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
        {
            Name = "Lisovenko Anton",
            Email = String.Empty
        }
    });
});

#region rabbitMQ
var configReceive = builder.Configuration.GetSection("RabbitMq");
var serviceClientReceiveSettings = configReceive.Get<RabbitMqConfiguration>();
builder.Services.Configure<RabbitMqConfiguration>(configReceive);
/*Теперь вы можете выполнять ваши запросы. Для этого вам потребуется получить экземпляр интерфейса IMediator. Он регистрируется в вашем контейнере зависимостей той же командой AddMediatR.*/
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
if (serviceClientReceiveSettings.Enabled)
{
    builder.Services.AddHostedService<UpdatePatientsDataReceiver>();
}
#endregion

builder.Services.AddScoped<IUpdatePatientsInfo, UpdatePatientsInfo>();

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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patient Data Handler API");
    c.RoutePrefix = string.Empty;
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
