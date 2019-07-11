# Clean Architecture REST API ASP.NET Core 2.2

I came up with an idea to create this repository after watching the following videos and reading the following articles:
* Jason Taylor - Clean Architecture with ASP.NET Core 2.2 (https://youtu.be/Zygw4UAxCdg?list=PLZGVTBEOfzvXvw6bltS01s2SSd8EOy5dk)
*	Glenn Condron - APIs and Microservices in ASP.NET Core Today and Tomorrow (https://youtu.be/dUdGcogYkss)
*	What's new in ASP.NET Core 2.2 (https://docs.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-2.2?view=aspnetcore-2.2)

## Prerequisites
* [Visual Studio 2019](https://www.visualstudio.com/downloads/) & [.NET Core SDK 2.2](https://www.microsoft.com/net/download/dotnet-core/2.2)
* [Visual Studio Code](https://code.visualstudio.com) & [REST Client extension](https://github.com/Huachao/vscode-restclient)

## Key points of the application
*	Core
	*	Domain<br />
		_It contains base logic & types. But not information about DB and doesn’t have reference to an infrastructure._
		*	Domain objects should be without data annotations.
		*	All collections should be initialized.
	*	Application<br />
		_It contains business logic & types_
		*	Logic & Models
		*	Interfaces
		*	Exceptions
		*	CQRS ?!
*	Persistence<br />
	_It contains external concerns. As usually has reference to the domain._
	*	DbContext
		*	Interface
	*	Configurations – fluent configuration.
	*	Migrations
	*	Seeding – separate project for different purpose.
*	Infrastructure<br />
	_Basically it contains implementations for external API:_
	*	External API; Email; SMS
*	Presentation<br />
	_It can be any UI, or a list of UIs’. As usually depends on the application and the persistence._

## Best practices
### Health check
`SimpleHealthCheck` implements health check logic.

_See Query 1 in `testflight.http`_

### Swagger
Don't forget about _return codes_. See attributes `ApiConventionMethod` and `ProducesResponseType` in `ProductsController`. And check the result in swagger UI.

### Logging & Exception attributes
1. Define exceptions on the application level (`NotFoundException` and `ConflictException`).
2. Use a logging in your app (see `ILogger` in `ProductService`).
3. Catch exceptions on the presentation level and return `ProblemDetails` class (see `ExceptionAttribute`).

_See Query 2 in `testflight.http`_

## Tips & tricks
### Authorization & Policy
1. Define your policies and custom implementations (see `HasPermissionHandler`).
2. Parse custom headers, inject the result and use in your controller methods (see `HasCustomHeaderHandler`, `CustomHeaderModel` and `ValuesController.Delete()`).

_See Query 3 in `testflight.http`_

### Hangfire
1. Define a job with DI (see `HangfireWorkerJob`).
2. Create a backgound job on startup (see `Startup.Configure()`).
3. Create a backgound job in controller (see `HangfireTestController`).

_See Query 4 in `testflight.http`_
