namespace WebAPI.Services;

/*
 * For more information on building a service please see ScopedCounterService.cs
 * See CounterController to see it in action.
 */
public interface ITransientCounterService
{
    Task<string> GetClassName(string declarationName);
    Task AddToCount(string declarationName);
}

public class TransientCounterService : ITransientCounterService
{
    private int Count;

    public Task<string> GetClassName(string declarationName)
    {
        Console.WriteLine($"{declarationName} {nameof(GetClassName)} -");
        Console.WriteLine($"{declarationName} {nameof(GetClassName)} +");
        return Task.FromResult(nameof(TransientCounterService));
    }

    public Task AddToCount(string declarationName)
    {
        Console.WriteLine($"{declarationName} {nameof(AddToCount)} +");
        Count++;
        Console.WriteLine($"{declarationName} {nameof(Count)}: {Count}");
        Console.WriteLine($"{declarationName} {nameof(AddToCount)} -");
        return Task.CompletedTask;
    }
}