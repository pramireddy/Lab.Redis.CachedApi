using Lab.Redis.CachedApi.Cache;
using StackExchange.Redis;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        string pwd = "xZyptv8D40MZOdxCX1mZiERNpz95mbBm";


        IConnectionMultiplexer connectionMultiplexer = 
            ConnectionMultiplexer.Connect($"redis-19035.c56.east-us.azure.cloud.redislabs.com:19035,password={pwd}");

        builder.Services.AddSingleton(connectionMultiplexer);
        builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}