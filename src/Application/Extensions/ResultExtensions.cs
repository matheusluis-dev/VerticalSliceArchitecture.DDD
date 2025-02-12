namespace Application.Extensions;

using Ardalis.Result;
using FastEndpoints;
using LinqKit;
using Microsoft.AspNetCore.Http;

public static class ArdalisResultsExtensions
{
    public static bool WasFound(this Ardalis.Result.IResult result)
    {
        return !result.IsNotFound();
    }

    private static int ToStatusCode(this ResultStatus resultStatus)
    {
        return resultStatus switch
        {
            ResultStatus.Ok => StatusCodes.Status200OK,
            ResultStatus.Created => StatusCodes.Status201Created,
            ResultStatus.Error => StatusCodes.Status500InternalServerError,
            ResultStatus.Forbidden => StatusCodes.Status403Forbidden,
            ResultStatus.Unauthorized => StatusCodes.Status401Unauthorized,
            ResultStatus.Invalid => StatusCodes.Status400BadRequest,
            ResultStatus.NotFound => StatusCodes.Status404NotFound,
            ResultStatus.NoContent => StatusCodes.Status204NoContent,
            ResultStatus.Conflict => StatusCodes.Status409Conflict,
            ResultStatus.CriticalError => StatusCodes.Status500InternalServerError,
            ResultStatus.Unavailable => StatusCodes.Status503ServiceUnavailable,
            _ => throw new NotSupportedException(),
        };
    }

    public static async Task SendInvalidResponseAsync<TResult>(
        this IEndpoint ep,
        TResult result,
        CancellationToken ct = default
    )
        where TResult : Ardalis.Result.IResult
    {
        ArgumentNullException.ThrowIfNull(ep);

        if (result.Status is not ResultStatus.Invalid)
            throw new ArgumentException("TODO");

        result.ValidationErrors.ForEach(e =>
            ep.ValidationFailures.Add(new(e.Identifier, e.ErrorMessage))
        );

        await ep.HttpContext.Response.SendErrorsAsync(ep.ValidationFailures, cancellation: ct);
    }

    public static async Task SendResponseAsync<TResult, TResponse>(
        this IEndpoint ep,
        TResult result,
        Func<TResult, TResponse> mapper,
        CancellationToken ct = default
    )
        where TResult : Ardalis.Result.IResult
    {
        ArgumentNullException.ThrowIfNull(ep);
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(mapper);

        if (result.Status is ResultStatus.Invalid)
            throw new ArgumentException("TODO");

        await ep.HttpContext.Response.SendAsync(
            mapper(result),
            result.Status.ToStatusCode(),
            cancellation: ct
        );
    }
}
