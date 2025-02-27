namespace Application.Models;

public abstract record PagedRequest
{
    public required int Page { get; init; }
    public required int Size { get; init; }

    protected PagedRequest(int page, int size)
    {
        PagedRequestException.ThrowIfInvalid(page, size);

        Page = page;
        Size = size;
    }
}

public sealed class PagedRequestException : Exception
{
    public PagedRequestException() { }

    public PagedRequestException(string message)
        : base(message) { }

    public PagedRequestException(string message, Exception innerException)
        : base(message, innerException) { }

    public static void ThrowIfInvalid(int page, int size)
    {
        if (page < 0)
            throw new PagedRequestException("Page index must be greater than zero");

        if (size < 0)
            throw new PagedRequestException("Page size must be greater than zero");
    }
}
