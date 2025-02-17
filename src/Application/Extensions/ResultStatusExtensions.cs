namespace Application.Extensions;

using Microsoft.AspNetCore.Http;

public static class ResultStatusExtensions
{
    public static int ToStatusCode(this ResultStatus resultStatus)
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
}
