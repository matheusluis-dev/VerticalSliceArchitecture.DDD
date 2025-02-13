namespace Application.Endpoints._Common;

public abstract record PagedRequest
{
    public required int Page { get; init; }
    public required int Size { get; init; }

    protected PagedRequest(int page, int size)
    {
        if (page < 0)
            throw new Exception("TODO");

        if (size < 0)
            throw new Exception("TODO");

        Page = page;
        Size = size;
    }
}
