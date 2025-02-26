using IResult = Ardalis.Result.IResult;

namespace Application.Extensions;

public static class IResultExtensions
{
    public static bool WasFound(this IResult result)
    {
        return !result.IsNotFound();
    }
}
