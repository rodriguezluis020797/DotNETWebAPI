using System.Text;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class SharedMutableStateController(
    /*
     * We don't need to simulate thousands of calls using something like postman. We will queue thousands of tasks and
     * run them concurrently to mimic concurrent HTTP calls to the same service.
     */
    ISharedMutableStateService sharedMutableStateService
) : ControllerBase
{
    private const string CharacterPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const int IterationCount = 10000;

    [HttpGet]
    public async Task<IActionResult> AddToCountNotThreadSafe()
    {
        await sharedMutableStateService.ResetCountAsync();

        var tasks = new List<Task>();
        for (var i = 0; i < IterationCount; i++)
            /*
             * Task.Run queues this onto a thread pool thread
             */
            tasks.Add(Task.Run(sharedMutableStateService.AddToCountNotThreadSafeAsync));

        Console.WriteLine($"Method: {nameof(AddToCountNotThreadSafe)} - Expected Count: {IterationCount}");
        /*
         * Waiting for all the tasks to finish.
         */
        await Task.WhenAll(tasks);
        Console.WriteLine(
            $"Method: {nameof(AddToCountNotThreadSafe)} - Actual Count: {await sharedMutableStateService.GetCountAsync()}");
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> AddToCountThreadSafe()
    {
        await sharedMutableStateService.ResetCountAsync();

        var tasks = new List<Task>();
        for (var i = 0; i < IterationCount; i++)
            /*
             * Task.Run queues this onto a thread pool thread
             */
            tasks.Add(Task.Run(sharedMutableStateService.AddToCountThreadSafeAsync));

        Console.WriteLine($"Method: {nameof(AddToCountThreadSafe)} - Expected Count: {IterationCount}");
        /*
         * Waiting for all the tasks to finish.
         */
        await Task.WhenAll(tasks);
        Console.WriteLine(
            $"Method: {nameof(AddToCountThreadSafe)} - Actual Count: {await sharedMutableStateService.GetCountAsync()}");
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> WriteStringNotThreadSafe()
    {
        await sharedMutableStateService.ResetStrAsync();
        var strBuilder = new StringBuilder();

        var tasks = new List<Task>();
        for (var i = 0; i < IterationCount; i++)
        {
            var character = Random.Shared.GetItems<char>(CharacterPool, 1)[0].ToString();

            /*
             * For record keeping so we can compare what we wanted vs what we got.
             */
            strBuilder.Append(character);

            /*
             * Task.Run queues this onto a thread pool thread
             */
            tasks.Add(Task.Run(() => sharedMutableStateService.WriteStringNotThreadSafeAsync(character)));
        }

        Console.WriteLine(
            $"Method: {nameof(WriteStringNotThreadSafe)} - Expected Character Count: {strBuilder.ToString().Length}");
        /*
         * Waiting for all the tasks to finish.
         */
        await Task.WhenAll(tasks);
        var actualString = await sharedMutableStateService.GetStr();
        Console.WriteLine(
            $"Method: {nameof(WriteStringNotThreadSafe)} - Actual Character Count: {actualString.Length}");
        return Ok();
    }


    [HttpGet]
    public async Task<IActionResult> WriteStringThreadSafe()
    {
        await sharedMutableStateService.ResetStrAsync();
        var strBuilder = new StringBuilder();

        var tasks = new List<Task>();
        for (var i = 0; i < IterationCount; i++)
        {
            var character = Random.Shared.GetItems<char>(CharacterPool, 1)[0].ToString();

            /*
             * For record keeping so we can compare what we wanted vs what we got.
             */
            strBuilder.Append(character);

            /*
             * Task.Run queues this onto a thread pool thread
             */
            tasks.Add(Task.Run(() => sharedMutableStateService.WriteStringThreadSafeAsync(character)));
        }

        Console.WriteLine(
            $"Method: {nameof(WriteStringThreadSafe)} - Expected Character Count: {strBuilder.ToString().Length}");
        /*
         * Waiting for all the tasks to finish.
         */
        await Task.WhenAll(tasks);
        var actualString = await sharedMutableStateService.GetStr();
        Console.WriteLine($"Method: {nameof(WriteStringThreadSafe)} - Actual Character Count: {actualString.Length}");
        return Ok();
    }
}