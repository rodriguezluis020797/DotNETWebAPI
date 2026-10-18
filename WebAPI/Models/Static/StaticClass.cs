namespace WebAPI.Models.Static;

/*
 * We're not actually using this class. It's just here as an example. Injecting ISharedMutableStateService as a
 * singleton creates the same effect to demonstrate a race condition and thread safety.
 *
 * We can see here that the class itself is static, meaning that all members need to be static as well. We get an error
 * if that is not the case. This means that this will behave like a singleton registered service, except it won't be
 * injected. A static class can't be instantiated at all, so its members exist once for the entire application and are
 * shared by everything that uses them.
 */
public static class StaticClass
{
    public static int Count { get; set; } = 0;
    public static string Name { get; set; } = string.Empty;
}