using Scalar.AspNetCore;
using WebAPI.Services;

namespace WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            /*
             * Accessible at https://localhost:7001/scalar/v1
             */
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        /*
         Minimal API endpoint. We're going to take this and implement elsewhere.
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                        new WeatherForecast
                        {
                            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            TemperatureC = Random.Shared.Next(-20, 55),
                            Summary = summaries[Random.Shared.Next(summaries.Length)]
                        })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast");
            */

        app.MapControllers();
        app.Run();
    }

    /*
     * Moved all service registration to its own method. we're going to keep adding to this as the API functionality grows.
     */
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddAuthorization();

        /*
         * Registering HTTP context service. Letting .NET do the work for it.
         */
        builder.Services.AddHttpContextAccessor();

        /*
         * We need to manually add the controllers we are building.
         */
        builder.Services.AddControllers();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        /*
         * We're going to add our own services here... there's a few types of service lifecycles, and I'm going to
         * explain them as best as I can.
         * I'll give a few and explain how to register them based on the functionality and lifetime. Only scoped will
         * have multiple classes, but they can all be switched depending on what you need as long as the interface is
         * the same.
         */

        #region Services

        /*
         * The most common when building your own services is usually Scoped. The lifecycle of Scoped is that every
         * HTTP request gets its own fresh copy of the implementation that persists throughout the HTTP call, and it is
         * disposed by .NET at the end of the call.
         *
         * For example, assume John and Jane are calling https://localhost:7001/api/Counter/AddToCountScoped, or any of the
         * other methods for that matter, at the exact same time even down to the nanosecond. Each of them would get
         * their own fresh copy of the service throughout the HTTP call and no matter where and how many times the
         * service is injected in the stack it will always be the same instance holding the same information. This
         * applies to this specific example since we are not using any other services that might contain shared state in
         * IScopedCounterService.
         *
         * To see this in action, call https://localhost:7001/api/Counter/AddToCountScoped and see your console output.
         * Notice that it is injected twice, and it will still produce the correct AddToCount() sum even though it is
         * being called from two different injections.
         *
         * Note: Comment out builder.Services.AddScoped<IScopedCounterService, ScopedCounterServiceB>(); if you want to
         * use builder.Services.AddScoped<IScopedCounterService, ScopedCounterServiceA>();. It won't throw an error if
         * both are not commented out, but ScopedCounterServiceB will overwrite ScopedCounterServiceA or vice versa
         * depending on order of declaration.
         *
         * See ScopedCounterService.cs for how to build a simple Scoped service.
         * See CounterController.cs for implementation.
         */
        builder.Services.AddScoped<IScopedCounterService, ScopedCounterServiceA>();
        // builder.Services.AddScoped<IScopedCounterService, ScopedCounterServiceB>();

        /*
         * Transient is almost the completely opposite of Scoped: instead of being persistent throughout the HTTP call
         * it gives you a fresh copy of the service every time it is injected at any point of the stack. Of course this
         * implies that it is a fresh instance for John and Jane when they make the call.
         *
         * For example, if you inject ITransientCounterService twice and call AddToCount() on both, you will see that
         * they will both act independently of each other and each print the same thing since they're both acting on
         * their own instance. Give this a try by calling https://localhost:7001/api/Counter/AddToCountTransient and
         * seeing the output in your console.
         *
         * Implementation and build are the same as IScopedCounterService.
         */

        builder.Services.AddTransient<ITransientCounterService, TransientCounterService>();

        /*
         * The last lifetime is Singleton. Singleton is one single instance throughout the application's lifetime no
         * matter how many times it is injected or who calls it. So John and Jane get a single shared instance.
         *
         * For example, if John adds to the count in one HTTP call and then Jane adds to the count again then Jane is
         * adding to the exact same count that John added to. This is true no matter how many times the service is
         * injected.
         *
         * Give it a try by calling https://localhost:7001/api/Counter/AddToCountSingleton and see how the call
         * increases with every HTTP call in the console output. Since it is called twice per call, it will increase by
         * two. See AddToCountSingleton in CounterController for more details.
         *
         * Note: Singleton is NOT thread safe so we add Interlocked.Increment. See SingletonCounterService.cs for more
         * details.
         */

        builder.Services.AddSingleton<ISingletonCounterService, SingletonCounterService>();

        #endregion
    }
}