# unity-simple-logging
A basic but flexible logging framework for use in Unity3d games

## Adding to your project via Unity Package Manager
Open **Window > Package Manager** in your editor.

Click the **+** in the top left and select **Add package from git URL...**

Paste *https://github.com/ThisIsNoZaku/unity-simple-logging.git?path=Assets* and click **Add**.

## Usage
Create a logger with `new LoggingModule()`.

By default, everything is logged.

## Actually Logging Something
To log a message, call `Log("some message")` on the logger. Simple methods for levels `Error("some message")` and `Warning("some message")` exist.

## Contexts and Configuration
You can filter your messages via ` ConfigureLogging`. This allows you to specify a context, optionally add a log level (if not specified, Info level is used) and optionally specify if logging for that level and context will be enabled or disabled (if not specified, it enables).

Whenever you log a message, you can specify a context. The logger will check to ensure that logging is enabled for the specified level and context before actually logging the message. When no context is specified, a message is automatically given the catch-all context of `*`. This allows you to separate your logging into different domains and specify how verbose your logs in that domain will be; for example, if you're debugging issues in your combat logic, you can log all of the nitty-gritty in their without clogging the logs with the trace-level logging of every part of the application.

### Lazy Evaluation
Instead of passing a string message to the logging methods, you can provide a `Func<string>` to generate them. This has the advantage of not actually creating the string until it is actually needed. This is an advantage for when you use string formatting to create the message, as this requires memory allocation. Lazy evaluation saves this usage if the message wouldn't actually end up being used.
