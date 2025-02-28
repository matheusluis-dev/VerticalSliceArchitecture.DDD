using FluentValidation.Results;
using LinqKit;

namespace Application.Extensions;

internal static class IEndpointExtensions
{
    internal static Task SendErrorResponseIfResultFailedAsync(
        this IEndpoint ep,
        Result result,
        CancellationToken ct = default
    )
    {
        if (result.Succeed)
            return Task.CompletedTask;

        result.Errors.ForEach(e => ep.ValidationFailures.Add(new ValidationFailure(e.Code, e.Description)));

        return ep.HttpContext.Response.SendErrorsAsync(ep.ValidationFailures, cancellation: ct);
    }
}
