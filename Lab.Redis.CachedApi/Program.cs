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

        string pwd = "X2sNOeh0vFZAU5ZLhOLjI9lXKJQ7MVOh";


        IConnectionMultiplexer connectionMultiplexer = 
            ConnectionMultiplexer.Connect($"redis-11078.c256.us-east-1-2.ec2.cloud.redislabs.com:11078,password={pwd}");

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