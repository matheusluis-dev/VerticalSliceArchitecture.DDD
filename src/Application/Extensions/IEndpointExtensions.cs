using LinqKit;
using IResult = Ardalis.Result.IResult;

namespace Application.Extensions;

public static class IEndpointExtensions
{
    public static async Task SendInvalidResponseAsync<TResult>(
        this IEndpoint ep,
        TResult result,
        CancellationToken ct = default
    )
        where TResult : IResult
    {
        ArgumentNullException.ThrowIfNull(ep);

        if (result.Status is not ResultStatus.Invalid)
            throw new ArgumentException("TODO");

        result.ValidationErrors!.ForEach(e => ep.ValidationFailures.Add(new(e.Identifier!, e.ErrorMessage!)));

        await ep.HttpContext.Response.SendErrorsAsync(ep.ValidationFailures, cancellation: ct);
    }
}
