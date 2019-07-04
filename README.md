# Clean Architecture REST API ASP.NET Core 2.2

I came up with an idea to create this repository after watching the following videos and reading the following articles:
* Jason Taylor - Clean Architecture with ASP.NET Core 2.2 (https://youtu.be/Zygw4UAxCdg?list=PLZGVTBEOfzvXvw6bltS01s2SSd8EOy5dk)
*	Glenn Condron - APIs and Microservices in ASP.NET Core Today and Tomorrow (https://youtu.be/dUdGcogYkss)
*	What's new in ASP.NET Core 2.2 (https://docs.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-2.2?view=aspnetcore-2.2)

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
*	Health check
*	Swagger
	*	Return codes
*	Logging & Exception attributes
	*	Logging in an app
	*	Exception in an app
	*	Catch by ExceptionAttribute => ProblemDetails
*	Authorization & Policy
