namespace Application.Architecture.Tests.Common;

internal static class Namespaces
{
    public const string System = "System";

    internal static class ApplicationLayer
    {
        public const string Common = "Application.Common";
        public const string Services = "Application.Services";

        public const string Endpoints = "Application.Endpoints";
    }

    internal static class DomainLayer
    {
        public const string Features = "Application.Domain.Features";
    }

    internal static class Layer
    {
        public const string Application = "Application";
        public const string Domain = "Application.Domain";
        public const string Infrastructure = "Application.Infrastructure";
    }
}
