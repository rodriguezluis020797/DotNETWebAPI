namespace WebAPI.Services;

/*
 * Let's build a very simple service so we can call it directly from the controller. This works for scoped, transient,
 * and singleton.
 * See CounterController to see it in action.
 */
public interface IScopedCounterService
{
    Task<string> GetClassName(string declarationName);
    Task AddToCount(string declarationName);
}

/*
 * Notice how both ScopedCounterServiceA and ScopedCounterServiceB implement IScopedCounterService. This will allow us to
 * interchangeably switch between ScopedCounterServiceA and ScopedCounterServiceB by declaring the desired class alongside the
 * interface in Program.cs. When we want to use the functionality, we inject the interface into the class instead of
 * ScopedCounterServiceA or ScopedCounterServiceB and since we have declared what class we want to use in Program.cs .NET
 * automatically resolves it for us at runtime.
 *
 * How does this help?
 * We can switch between classes simply by switching the declared class alongside the interface.
 *
 * To see this in action, go to Program.cs for more information.
 *
 * See Program.cs's Services region for information on registration and lifecycle.
 * See CounterController.cs for implementation.
 */

/*
 * Must inherit the interface.
 */
public class ScopedCounterServiceA : IScopedCounterService
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
        /*
         * CounterA adds to count by 1
         */

        Console.WriteLine($"{declarationName} {nameof(AddToCount)} +");
        Count++;
        Console.WriteLine($"{declarationName} {nameof(Count)}: {Count}");
        Console.WriteLine($"{declarationName} {nameof(AddToCount)} -");
        return Task.CompletedTask;
    }
}

public class ScopedCounterServiceB : IScopedCounterService
{
    private int Count;

    public Task<string> GetClassName(string declarationName)
    {
        Console.WriteLine($"{declarationName} {nameof(GetClassName)} -");
        Console.WriteLine($"{declarationName} {nameof(GetClassName)} +");
        return Task.FromResult(nameof(ScopedCounterServiceB));
    }

    public Task AddToCount(string declarationName)
    {
        /*
         * CounterServiceB adds to count by 2
         */
        Console.WriteLine($"{declarationName} {nameof(AddToCount)} +");
        Count += 2;
        Console.WriteLine($"{declarationName} {nameof(Count)}: {Count}");
        Console.WriteLine($"{declarationName} {nameof(AddToCount)} -");
        return Task.CompletedTask;
    }
}