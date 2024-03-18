![Image](CodeCoverage/Report/badge_linecoverage.png)
# unity-simple-logging
A basic but flexible logging framework for use in Unity3d games

## Adding to your project via Unity Package Manager
Open **Window > Package Manager** in your editor.

Click the **+** in the top left and select **Add package from git URL...**

Paste *https://github.com/ThisIsNoZaku/unity-simple-logging.git?path=Assets* and click **Add**.

## Usage
Create a logger with `new LoggingModule()`.

To log a message, call `Log("some message")` on the logger. Simple methods for levels `Error("some message")` and `Warning("some message")` also exist.

## Contexts and Configuration
You can filter your messages via contexts.

By default, every context and every level is logged. To turn off this log-everything logic, use `logger.ConfigureLogging("*", LogLevel.Log)`. `*` is the "everything" context.

To configure a particular context, specify the context name and level to enable logging for that level (and higher) for the given context. You can specify a context and give no level, which will disable all logging for that context.

If you enable `Log` level logging, `Waring` and `Error` levels are also enabled. Enabling `Warning` also enables `Error` level. The `LogType` enum also specifies the `Assert` and `Exception` levels; this library treats these as equivalent to `Error`.

Whenever you log a message, you can specify a context as the final argument. The logger will check to ensure that logging is enabled for the specified level and context before actually logging the message. When no context is specified, a message is automatically given the catch-all context of `*`. 

This allows you to separate your logging into different domains and specify how verbose your logs in that domain will be; for example, if you're debugging issues in your combat logic, you can log all of the nitty-gritty in there without clogging the logs with the trace-level logging of every part of the application.

These contexts can also be hierarchical, using a dot notation like "foo.bar.baz". Each of these levels can be configured independently, allowing fine-grained configuration of logging subdomains. For example, you may configure the "foo" domain with a 'Error' level of logging; give the "foo.bar" a level of 'Info' and "foo.bar.baz" a level of 'Trace'. This will log the highest details for "foo.bar.baz", a moderate level for "foo.bar" and its children EXCEPT "foo.bar.baz" and log only the most serious messages for any remaining "foo" logs.

### Lazy Evaluation
Instead of passing a string message to the logging methods, you can provide a `Func<string>` to generate them. This means the string is only used when needed; the advantage to this is primarily for when you use string formatting to create the message, as this requires memory allocation. Lazy evaluation saves this usage if the message wouldn't actually end up being used.
