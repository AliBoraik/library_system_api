namespace Library.Domain;

public readonly struct Result<TValue, TError>
{
    public readonly TValue Value;
    public readonly TError Error;

    private Result(TValue v, TError e, bool success)
    {
        Value = v;
        Error = e;
        IsOk = success;
    }

    public bool IsOk { get; }

    public static Result<TValue, TError> Ok(TValue v)
    {
        return new Result<TValue, TError>(v, default!, true);
    }

    public static Result<TValue, TError> Err(TError e)
    {
        return new Result<TValue, TError>(default!, e, false);
    }

    public static implicit operator Result<TValue, TError>(TValue v)
    {
        return new Result<TValue, TError>(v, default!, true);
    }

    public static implicit operator Result<TValue, TError>(TError e)
    {
        return new Result<TValue, TError>(default!, e, false);
    }

    public TR Match<TR>(
        Func<TValue, TR> success,
        Func<TError, TR> failure)
    {
        return IsOk ? success(Value) : failure(Error);
    }
}