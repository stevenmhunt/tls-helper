TLS Helper
==========

Provides a convenient mechanism for creating Thread Local Storage objects in .NET based on what type of application you are creating.

The library uses a provider model implemented using the ITlsProvider interface. The code ships with two implementations, ThreadStaticProvider which wraps a conventional [ThreadStatic] TLS implementation, and HttpContextProvider which handles TLS in an ASP .NET environment.

The advantage of this library over hand-coding these scenarios is that the library can determine which provider to use based on the environment of the application your code runs in. For instance, if there is a current http context, the HttpContextProvider will be used. The library will fall back on ThreadStaticProvider if no other providers are valid. Also, you can also implement your own version of ITlsProvider and register it using the static method TlsHelper.AddProvider(ITlsProvider<T>).

How To Use
----------

Here is a sample on how to use the code.

```C#

using System;

using StevenHunt.Tls;

...

var myvar = new TlsHelper<string>("myvar", () => "initial value");

Console.WriteLine(myvar.HasValue);
Console.WriteLine(myvar.Value);

myvar.Value = "new value";

Console.WriteLine(myvar.HasValue);
Console.WriteLine(myvar.Value);

```

TlsHelper is a generic class, so tell it what data type your variable should contain. The constructor takes two parameters: a unique name to identify your object by, and an initializer function which provides your object with a lazily-loaded value.

To get or set the value, use the Value property.
