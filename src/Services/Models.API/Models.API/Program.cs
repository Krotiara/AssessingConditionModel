using MediatR;
using Models.API;
using Models.API.Data;
using Models.API.Entities;
using Models.API.Service.Command;
using Models.API.Service.Query;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

services.AddControllers();
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

services.AddS3ClientService(builder.Configuration);
services.AddPostgresService(builder.Configuration);

services.AddTransient<ModelsStore>();
services
    .AddTransient<IRequestHandler<InsertModelCommand, Unit>, InsertModelCommandHandler>()
    .AddTransient<IRequestHandler<GetModelMetaQuery, ModelMeta>, GetModelMetaQueryHandler>()
    .AddTransient<IRequestHandler<PredictModelCommand, float[]>, PredictModelCommandHandler>();

//Swagger
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.Run();
