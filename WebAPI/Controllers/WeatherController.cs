using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

/*
 * Tells us where in the URL this is available. In this case it is available at
 * https://localhost:7001/api/weather/
 * Notice that [controller]  marks the name of the controller without the "Controller" string
 * [action] is the name of the method
 */
[Route("api/[controller]/[action]")]
/*
 * Tells .NET that this is an API controller
 */
[ApiController]
/*
 * We are injecting HttpContext as a service (look up '.net dependency injection').
 * Notice it's IHttpContextAccessor and not HttpContextAccessor: we are injecting the interface definition, not the class itself
 * HttpContext specifically lets us see HTTP specific values.
 * See below.
 */
public class WeatherController(IHttpContextAccessor httpContext) : Controller
{
    /*
     * [HttpGet] is the HTTP method. Gets are best used when there is no JSON body for the API to read
     * (i.e. just getting info with or without URL parameters)
     */
    [HttpGet]
    /*
     * Use async only for when there are asynchronous operations in your method.
     * In this case I am using it for demonstration.
     * [FromQuery] will pick up parameters from your query string.
     * Example use here is https://localhost:7001/api/weather/getweather?city=Puruandiro&neighborhood=Los_Angeles
     */
    public async Task<IActionResult> GetWeather([FromQuery] string city, [FromQuery] string neighborhood)
    {
        {
            /*
             * Replacing the underscore with a space. If you enter 'neighborhood=Los Angeles' with a space it will
             * work as expected since it gets URL encoded, but for now this is easier so you can just click the link.
             */
            neighborhood = neighborhood.Replace("_", " ");

            /*
             * logging to console for visual confirmation
             */
            Console.WriteLine($"{nameof(city)}: {city}");
            Console.WriteLine($"{nameof(neighborhood)}: {neighborhood}");

            /*
             * Logging some of the HttpContext values
             */
            try
            {
                /*
                 * check to see that it is not null
                 */
                if (httpContext.HttpContext == null)
                    /*
                     * throw exception if it is
                     */
                    throw new Exception($"{nameof(httpContext)} is null");

                /*
                 * Gets us the HTTP method
                 */
                Console.WriteLine(
                    $"{nameof(httpContext.HttpContext.Request.Method)}: {httpContext.HttpContext.Request.Method}");

                /*
                 * Gets us if its HTTPS
                 */
                Console.WriteLine(
                    $"{nameof(httpContext.HttpContext.Request.IsHttps)}: {httpContext.HttpContext.Request.IsHttps}");
            }
            catch (Exception ex)
            {
                /*
                 * Catch exception but only log it. no need to bubble up the exception here.
                 */
                Console.WriteLine(ex.ToString());
            }


            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                .ToArray();

            /*
             * Return an OK status, meaning 200 status.
             * Can also return StatusCode(200,  forecast), StatusCode(500,  forecast) for internal server error,
             * or StatusCode({any int},  forecast) for any status code you'd like
             */
            return Ok(forecast);
        }
    }
}