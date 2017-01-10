# Log
*Core* provides a logging interface and extensions. This allows you to implement your own logger.

## Getting Started
First, you have to implement `ILogger`. There is a simple implementation, `ConsoleLogger`, but it most likely will not fit your use in production.

```csharp
var logger = new ConsoleLogger();
logger.d("Logger is created.");
```

## Log Level
There are 6 log level: verbose, debug, info, warn,  error, and assert.
