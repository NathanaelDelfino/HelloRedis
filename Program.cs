using System.Reflection;
using HelloRedis.Data;
using HelloRedis.Infrastructure.Caching;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICachingService, CachingService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "HelloRedis";
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new() { Title = "Hello Redis", Version = "0.0.1" });
    });

builder.Services.AddDbContext<DbAppContext>();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.MapGet("/", context => Task.Run(() => context.Response.Redirect("/swagger/index.html")));
app.Run();