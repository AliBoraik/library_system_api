namespace Library.Domain;

public readonly struct Result<TValue, TError> {
    private readonly bool _success;
    public readonly TValue Value;
    public readonly TError Error;

    private Result(TValue v, TError e, bool success)
    {
        Value = v;
        Error = e;
        _success = success;
    }

    public bool IsOk => _success;

    public static Result<TValue, TError> Ok(TValue v)
    {
        return new Result<TValue, TError>(v, default!, true);
    }

    public static Result<TValue, TError> Err(TError e)
    {
        return new Result<TValue, TError>(default!, e, false);
    }

    public static implicit operator Result<TValue, TError>(TValue v) => new(v, default!, true);
    public static implicit operator Result<TValue, TError>(TError e) => new(default!, e, false);

    public TR Match<TR>(
        Func<TValue, TR> success,
        Func<TError, TR> failure) =>
        _success ? success(Value) : failure(Error);
}