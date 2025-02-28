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
public sealed class Result<T>
    where T : class
{
    public bool Succeed => _result.Succeed;
    public bool Failed => _result.Failed;
    public IEnumerable<Error> Errors => _result.Errors;
    public T? Value { get; }

    private readonly Result _result;

    private Result(T? value, bool succeed, params IEnumerable<Error> errors)
    {
        Value = value;
        _result = succeed ? Result.Success() : Result.Failure(errors);
    }

    private static Result<T> Success(T value)
    {
        return new Result<T>(value, true, Error.None);
    }

    private static Result<T> Failure(params IEnumerable<Error> errors)
    {
        return new Result<T>(null, false, errors);
    }

    public static implicit operator Result(Result<T> someValue)
    {
        ArgumentNullException.ThrowIfNull(someValue);

        return someValue._result;
    }

    public static implicit operator T(Result<T> someValue)
    {
        ArgumentNullException.ThrowIfNull(someValue);

        return someValue.Value!;
    }

    public static implicit operator Result<T>(T someValue)
    {
        ArgumentNullException.ThrowIfNull(someValue);

        return Success(someValue);
    }

    public static implicit operator Result<T>(Result someValue)
    {
        ArgumentNullException.ThrowIfNull(someValue);

        return someValue.Failed ? Failure(someValue.Errors) : Success(null!);
    }
}
