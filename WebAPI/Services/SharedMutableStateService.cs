namespace WebAPI.Services;

/*
 * We're registering this as a singleton in Program.cs.
 */
public interface ISharedMutableStateService
{
    Task ResetCountAsync();
    Task<int> GetCountAsync();
    Task AddToCountNotThreadSafeAsync();
    Task AddToCountThreadSafeAsync();
    Task ResetStrAsync();
    Task<string> GetStr();
    Task WriteStringNotThreadSafeAsync(string str);
    Task WriteStringThreadSafeAsync(string str);
}

public class SharedMutableStateService : ISharedMutableStateService
{
    /*
     * Fields don't need to be static to achieve a race condition since we are injecting the service as a singleton.
     */
    private int _count;
    private readonly Lock _strLock = new();
    private string _str = string.Empty;

    public Task ResetCountAsync()
    {
        /*
         * Resetting _count to 0 at every Http call for demonstration purposes.
         * See AddToCountThreadSafeAsync
         */
        Interlocked.Exchange(ref _count, 0);
        return Task.CompletedTask;
    }

    public Task<int> GetCountAsync()
    {
        /*
         * Just getting the current count as it is in the heap.
         */
        return Task.FromResult(_count);
    }

    public Task AddToCountNotThreadSafeAsync()
    {
        /*
         * Not thread safe. Any thread can read, update, and write back their value at the heap position at any given
         * time and create a race condition.
         */
        _count++;
        return Task.CompletedTask;
    }

    public Task AddToCountThreadSafeAsync()
    {
        /*
         * Interlocked performs the increment as a single atomic operation, so no other thread can interrupt it. This is
         * much faster than creating a lock ourselves like in WriteStringThreadSafeAsync and should be used for simple
         * data types.
         */
        Interlocked.Increment(ref _count);
        return Task.CompletedTask;
    }

    public Task ResetStrAsync()
    {
        /*
         * Resetting _str to an empty string at every Http call for demonstration purposes.
         * See WriteStringThreadSafeAsync
         */

        lock (_strLock)
        {
            _str = string.Empty;
        }

        return Task.CompletedTask;
    }

    public Task<string> GetStr()
    {
        /*
         * Just getting the current string as it is in the heap.
         */
        return Task.FromResult(_str);
    }

    public Task WriteStringNotThreadSafeAsync(string str)
    {
        /*
         * Not thread safe. Any thread can read, update, and write their new value in a new heap position at any given
         * time and create a race condition.
         */
        _str += str;
        return Task.CompletedTask;
    }

    public Task WriteStringThreadSafeAsync(string str)
    {
        /*
         * We create a lock ourselves and implement it.
         * The lock forces threads to wait in line here and is only released once the lock condition has been completed.
         * A lock is slower than Interlocked, but it works on any data type, so we use it here since Interlocked has no
         * simple operation for appending to a string.
         * See AddToCountThreadSafeAsync
         */
        lock (_strLock)
        {
            _str += str;
        }

        return Task.CompletedTask;
    }
}