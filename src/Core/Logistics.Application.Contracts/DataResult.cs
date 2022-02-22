namespace Logistics.Application.Contracts;

public class DataResult
{
    private static readonly DataResult success = new();

    public string? Error { get; init; }
    public bool Success => string.IsNullOrEmpty(Error);

    public static DataResult CreateSuccess() => success;
}
    
public class DataResult<T> : DataResult
{
    public T? Value { get; set; }

    public static DataResult<T> CreateSuccess(T result) => new()
    {
        Value = result
    };
}