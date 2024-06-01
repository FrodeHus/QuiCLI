using System.Diagnostics.Contracts;

namespace QuiCLI.Common;


public readonly struct Result<TValue> : IEquatable<Result<TValue>>, IComparable<Result<TValue>>
{
    public TValue Value { get; } = default!;
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    public Result(TValue value)
    {
        Value = value;
        IsSuccess = true;
    }

    public Result(Error error)
    {
        Value = default!;
        IsSuccess = false;
        Error = error;
    }

    public static Result<TValue> Success(TValue value) => new(value);

    public static Result<TValue> Failure(Error error) => new(error);

    [Pure]
    public bool Equals(Result<TValue> other)
    {
        return EqualityComparer<TValue>.Default.Equals(Value, other.Value);
    }

    [Pure]
    public int CompareTo(Result<TValue> other)
    {
        return Comparer<TValue>.Default.Compare(Value, other.Value);
    }

    [Pure]
    public static implicit operator TValue(Result<TValue> result) => result.Value;

    [Pure]
    public static implicit operator Error?(Result<TValue> result) => result.Error;

    [Pure]
    public static implicit operator Result<TValue>(Error error) => Failure(error);

    [Pure]
    public static implicit operator Result<TValue>(TValue value) => Success(value);

    [Pure]
    public override int GetHashCode()
    {
        return Value?.GetHashCode() ?? 0;
    }

    [Pure]
    public override bool Equals(object? obj)
    {
        return obj is Result<TValue> result && Equals(result);
    }

    [Pure]
    public static bool operator ==(Result<TValue> left, Result<TValue> right)
    {
        return left.Equals(right);
    }

    [Pure]
    public static bool operator !=(Result<TValue> left, Result<TValue> right)
    {
        return !(left == right);
    }

    [Pure]
    public override string ToString()
    {
        return IsSuccess ? Value?.ToString() ?? string.Empty : Error?.ToString() ?? string.Empty;
    }
}
