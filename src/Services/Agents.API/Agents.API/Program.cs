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
        Title = "����������� ������� �������",
        Description = "�������� �� ������������ � ��������������� ��������� ���������. " +
        "\r\n������������ ����� ������������� ��� ����� ���������� �� ��������� " +
        "��������� �������� ����� ��������� ������������ �����." +
        "\r\n�� ����������� ������ �������� �� ������ ������� �������.\r\n.",
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
/*������ �� ������ ��������� ���� �������. ��� ����� ��� ����������� �������� ��������� ���������� IMediator. �� �������������� � ����� ���������� ������������ ��� �� �������� AddMediatR.*/
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
