using System.Reflection;

using Device.Api.DTOs;
using Device.Api.Middleware;
using Device.Core.Repository;
using Device.Core.Services;
using Device.Infrastructure;
using Device.Infrastructure.Repository;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var useSqlServer = bool.TryParse(builder.Configuration["Database:UseSqlServer"], out var sqlServerResult) && sqlServerResult;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{

    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "Devices Api",
        Version = "v1"
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

if (useSqlServer)
    builder.Services.AddDbContext<DeviceContext>(x => x.UseSqlServer(builder.Configuration["Database:ConnectionString"]));
else
    builder.Services.AddDbContext<DeviceContext>(x => x.UseInMemoryDatabase("InMemoryDb"));

builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<Device.Core.Models.Device, DeviceDto>().ReverseMap();
    cfg.CreateMap<DeviceCreateUpdateDto, Device.Core.Models.Device>();
});

var app = builder.Build();

if (useSqlServer)
{
    //Not the best approach for migrations.
    using var serviceScope = app.Services.CreateScope();
    serviceScope.ServiceProvider.GetService<DeviceContext>().Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
