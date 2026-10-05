namespace WebAPI.Services;

/*
 * For more information on building a service please see ScopedCounterService.cs
 * See CounterController to see it in action.
 */

public interface ISingletonCounterService
{
    Task<string> GetClassName(string declarationName);
    Task AddToCount(string declarationName);
}

public class SingletonCounterService : ISingletonCounterService
{
    private int Count;

    public Task<string> GetClassName(string declarationName)
    {
        Console.WriteLine($"{declarationName} {nameof(GetClassName)} -");
        Console.WriteLine($"{declarationName} {nameof(GetClassName)} +");
        return Task.FromResult(nameof(ScopedCounterServiceA));
    }

    public Task AddToCount(string declarationName)
    {
        Console.WriteLine($"{declarationName} {nameof(AddToCount)} +");

        /*
         * To keep this singleton service thread safe, we add Interlocked.Increment(ref Count), wich increases by 1.
         * Interlocked.Increment(ref Count, 2) would increase by 2, etc.
         */
        var newCount = Interlocked.Increment(ref Count);
        Console.WriteLine($"{declarationName} {nameof(newCount)}: {newCount}");
        Console.WriteLine($"{declarationName} {nameof(AddToCount)} -");
        return Task.CompletedTask;
    }
}