#pragma warning disable RCS1259 // Remove empty syntax
#pragma warning disable S3261 // Namespaces should not be empty
#pragma warning disable IDE0130 // Namespace does not match folder structure
#pragma warning disable CA1040 // Avoid empty interfaces
#pragma warning disable CA1715 // Identifiers should have correct prefix
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable S101 // Types should be named in PascalCase
#pragma warning disable RCS1251 // Remove unnecessary braces from record declaration
#pragma warning disable S2094 // Classes should not be empty
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable RCS1213 // Remove unused member declaration
#pragma warning disable S1144 // Unused private types or members should be removed
#pragma warning disable S1186 // Methods should not be empty
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
#pragma warning disable MA0048 // File name must match type name
#pragma warning disable MA0036

namespace Application
{
    using System;
    using Microsoft.AspNetCore.Authentication;

    public interface interfacesShouldBePascalCased { }

    public interface InterfaceSecondCharShouldBeUpperCased { }

    public class NonAbstractNonStaticNonPartialClassesShouldBeSealed { }

    public class classesShouldBePascalCased
    {
        private string camelCased;
        private string _PascalCased;
        private string _camelCased;
        public string Public;
        public const string PublicConst = "A";
        public readonly string PublicReadonly = "B";
        protected string Protected;
        internal string Internal;

        public void Http()
        {
            var httpClient = new HttpClient();
        }
    }

    public sealed class MethodsClass
    {
        public const string camel_cased = "A";

        private void camelCasedMethod()
        {
            const string insideMethod = "B";
            const int intConst = 10;
            var varInsideMethod = "B";
        }
    }

    public sealed class AllMethodsStatic()
    {
        public static void Bla() { }

        public static void Ble() { }
    }

    public sealed class NotAnEndpoint() { }

    public sealed class NullObjectWithoutImplementedInterfaceNullObject
    {
        public void EmptyMethod() { }

        public void EmptyMethodWithReturn()
        {
            return;
        }

        public void NonEmptyMethod()
        {
            var a = 5;
            var b = 1;
            var sum = a + b;
        }
    }

    public sealed class NullObjectImplementingOneInterfaceNullObject : ISystemClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.MinValue;
    }

    public sealed class NullObjectImplementingTwoInterfacesNullObject : ISystemClock, ILogger
    {
        public DateTimeOffset UtcNow => DateTimeOffset.MinValue;

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter
        )
        {
            throw new NotImplementedException();
        }
    }

    public sealed class SystemClockNullObject : ISystemClock
    {
        public DateTimeOffset UtcNow => throw new NotImplementedException();
    }
}

namespace Application.Services
{
    public sealed class OnNamespaceServicesWithoutSuffix() { }
}

#pragma warning restore IDE0130 // Namespace does not match folder structure
#pragma warning restore S3261 // Namespaces should not be empty
#pragma warning restore RCS1259 // Remove empty syntax
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
#pragma warning restore S1186 // Methods should not be empty
#pragma warning restore S1144 // Unused private types or members should be removed
#pragma warning restore RCS1213 // Remove unused member declaration
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore S2094 // Classes should not be empty
#pragma warning restore RCS1251 // Remove unnecessary braces from record declaration
#pragma warning restore S101 // Types should be named in PascalCase
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CA1715 // Identifiers should have correct prefix
#pragma warning restore CA1040 // Avoid empty interfaces
#pragma warning restore MA0048 // File name must match type name
#pragma warning restore MA0036
