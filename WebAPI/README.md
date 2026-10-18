# DotNETWebAPI

A .NET Web API for learning and reference, built in public. The code is heavily commented, so reading the files is part
of the lesson.

## Topics

| Topic                                                                                     | Where in the code                                                                                        | Status  |
|-------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------|---------|
| Dependency injection and service lifetimes                                                | `Program.cs` (Services region), `Services/`, `Controllers/CounterController.cs`                          | Done    |
| Controller basics: routing, query parameters, `HttpContext` (no blog post for this topic) | `Controllers/WeatherController.cs`                                                                       | Done    |
| Shared mutable state: race conditions, `Interlocked`, and `lock`                          | `Services/SharedMutableStateService.cs`, `Models/Static/`, `Controllers/SharedMutableStateController.cs` | Done    |
| Thread safety across different scenarios                                                  | —                                                                                                        | Planned |
| Entity Framework Core                                                                     | —                                                                                                        | Planned |

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- A trusted HTTPS development certificate. If your browser complains, run `dotnet dev-certs https --trust` once.

### Run it

```bash
git clone https://github.com/rodriguezluis020797/DotNETWebAPI.git
cd DotNETWebAPI
dotnet run --project WebAPI
```

You can also open `DotNETWebAPI.slnx` in Visual Studio or Rider and run it from there.

The API runs at `https://localhost:7001`. In Development, interactive API docs (Scalar) are available at
`https://localhost:7001/scalar/v1`.

## Endpoints

| Endpoint                                                               | What it demonstrates                              |
|------------------------------------------------------------------------|---------------------------------------------------|
| `GET /api/Counter/AddToCountScoped`                                    | Scoped service lifetime                           |
| `GET /api/Counter/AddToCountTransient`                                 | Transient service lifetime                        |
| `GET /api/Counter/AddToCountSingleton`                                 | Singleton service lifetime (thread-safe)          |
| `GET /api/Weather/GetWeather?city=Puruandiro&neighborhood=Los_Angeles` | Routing, query parameters, `IHttpContextAccessor` |
| `GET /api/SharedMutableState/AddToCountNotThreadSafe`                  | Race condition on a counter (lost updates)        |
| `GET /api/SharedMutableState/AddToCountThreadSafe`                     | Counter fixed with `Interlocked`                  |
| `GET /api/SharedMutableState/WriteStringNotThreadSafe`                 | Race condition on a string (lost characters)      |
| `GET /api/SharedMutableState/WriteStringThreadSafe`                    | String fixed with `lock`                          |

Routes follow `api/[controller]/[action]`, so the controller and method names are the URL. The counter and shared
mutable state endpoints write their results to the console.

## Project structure

Key files shown:

```text
DotNETWebAPI/
├── DotNETWebAPI.slnx
└── WebAPI/
    ├── Program.cs                          # Service registrations live in the Services region
    ├── Controllers/
    │   ├── CounterController.cs            # Scoped, transient, and singleton demo endpoints
    │   ├── SharedMutableStateController.cs # Race condition and thread safety demo endpoints
    │   └── WeatherController.cs            # Controller basics
    ├── Models/
    │   └── Static/
    │       ├── StaticClass.cs              # Static class example
    │       └── NonStaticClass.cs           # Class with a static property example
    └── Services/
        ├── ScopedCounterService.cs         # IScopedCounterService + implementations A and B
        ├── TransientCounterService.cs
        ├── SingletonCounterService.cs
        └── SharedMutableStateService.cs    # Not thread-safe vs. thread-safe methods
```

## Roadmap

- [x] Dependency injection and service lifetimes
- [x] Shared mutable state
- [ ] Thread safety across different scenarios
- [ ] Entity Framework Core

## Author

Luis F. Rodriguez · [lrodriguez.puruandiroit.com](https://lrodriguez.puruandiroit.com)

## License

[MIT](LICENSE)