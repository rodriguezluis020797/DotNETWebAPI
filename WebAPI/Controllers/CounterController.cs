using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers;

/*
 * To see details on controller setup see WeatherController.
 */
[Route("api/[controller]/[action]")]
[ApiController]
public class CounterController(
    /*
     * Injecting services.
     * See Program.cs's Services region for information on registration and lifecycle.
     * See *CounterService.cs files for implementation.
     */
    IScopedCounterService _scopedCounterService1,
    IScopedCounterService _scopedCounterService2,
    ITransientCounterService _transientCounterService1,
    ITransientCounterService _transientCounterService2,
    ISingletonCounterService _singletonCounterService1,
    ISingletonCounterService _singletonCounterService2
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> AddToCountScoped()
    {
        /*
         * Using IScopedCounterService. It is injected in the constructor.
         */
        Console.WriteLine($"{nameof(AddToCountScoped)} +");

        var className = await _scopedCounterService1
            .GetClassName(nameof(_scopedCounterService1));
        Console.WriteLine($"{nameof(className)}: {className}");

        /*
         * Notice that Even though _scopedCounterService1 is used and then _scopedCounterService2, the count carries
         * over because both are the same instance for this HTTP call. The next HTTP call starts fresh.
         */
        await _scopedCounterService1
            .AddToCount(nameof(_scopedCounterService1));
        await _scopedCounterService2
            .AddToCount(nameof(_scopedCounterService2));

        Console.WriteLine($"{nameof(AddToCountScoped)} -");
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> AddToCountTransient()
    {
        Console.WriteLine($"{nameof(AddToCountTransient)} +");
        /*
         * Using ITransientCounterService. It is injected in the constructor.
         */
        var className = await _transientCounterService1
            .GetClassName(nameof(_transientCounterService1));
        Console.WriteLine($"{nameof(className)}: {className}");

        /*
         * Notice that _transientCounterService1 is used and then _transientCounterService2 but the count is not
         * persistent. This is because it is transient and therefore NOT the same instance.
         */
        await _transientCounterService1
            .AddToCount(nameof(_transientCounterService1));
        await _transientCounterService2
            .AddToCount(nameof(_transientCounterService2));
        Console.WriteLine($"{nameof(AddToCountTransient)} -");
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> AddToCountSingleton()
    {
        Console.WriteLine($"{nameof(AddToCountSingleton)} +");
        /*
         * Using ISingletonCounterService. It is injected in the constructor.
         */
        var className = await _singletonCounterService1
            .GetClassName(nameof(_singletonCounterService1));
        Console.WriteLine($"{nameof(className)}: {className}");

        /*
         * Notice that even though _singletonCounterService1 is used and then _singletonCounterService2 the count is
         * still persistent not only for this HTTP call, but for all in the duration of the application's lifetime. This
         * is because it is a singleton and therefore the same instance only for the application's lifetime.
         */
        await _singletonCounterService1
            .AddToCount(nameof(_singletonCounterService1));
        await _singletonCounterService2
            .AddToCount(nameof(_singletonCounterService2));
        Console.WriteLine($"{nameof(AddToCountSingleton)} -");
        return Ok();
    }
}