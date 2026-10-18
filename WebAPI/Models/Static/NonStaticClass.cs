namespace WebAPI.Models.Static;

/*
 * We're not actually using this class. It's just here as an example. Injecting ISharedMutableStateService as a
 * singleton creates the same effect to demonstrate a race condition and thread safety.
 *
 * We can see here that the class is non-static, but Count is. This means that Count will be the same instance for all
 * the instances of the class that are created. In contrast, Name will be its own instance for every instance of the
 * class.
 */
public class NonStaticClass
{
    public static int Count { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
}