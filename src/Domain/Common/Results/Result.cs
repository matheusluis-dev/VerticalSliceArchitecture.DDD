using System.Diagnostics.CodeAnalysis;

namespace Domain.Common.Results;

public sealed class Result
{
    public bool Succeed { get; }

    public bool Failed => !Succeed;

    public IEnumerable<Error> Errors { get; }

    private Result(bool succeed, params IEnumerable<Error> errors)
    {
        Succeed = succeed;
        Errors = errors;
    }

    public static Result Success()
    {
        return new Result(true, Error.None);
    }

    public static Result Failure(params IEnumerable<Error> errors)
    {
        return new Result(false, errors);
    }
}

[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates")]
[SuppressMessage("Naming", "CA1720:Identifier contains type name")]
public sealed class Result<TObject>
    where TObject : class
{
    public bool Succeed => _result.Succeed;
    public bool Failed => _result.Failed;
    public IEnumerable<Error> Errors => _result.Errors;
    public TObject? Object { get; }

    private readonly Result _result;

    private Result(TObject? @object, bool succeed, params IEnumerable<Error> errors)
    {
        Object = @object;
        _result = succeed ? Result.Success() : Result.Failure(errors);
    }

    private static Result<TObject> Success(TObject value)
    {
        return new Result<TObject>(value, true, Error.None);
    }

    private static Result<TObject> Failure(params IEnumerable<Error> errors)
    {
        return new Result<TObject>(null, false, errors);
    }

    public static implicit operator Result(Result<TObject> someValue)
    {
        ArgumentNullException.ThrowIfNull(someValue);

        return someValue._result;
    }

    public static implicit operator TObject(Result<TObject> someValue)
    {
        ArgumentNullException.ThrowIfNull(someValue);

        return someValue.Object!;
    }

    public static implicit operator Result<TObject>(TObject someValue)
    {
        ArgumentNullException.ThrowIfNull(someValue);

        return Success(someValue);
    }

    public static implicit operator Result<TObject>(Result someValue)
    {
        ArgumentNullException.ThrowIfNull(someValue);

        return someValue.Failed ? Failure(someValue.Errors) : Success(null!);
    }
}
