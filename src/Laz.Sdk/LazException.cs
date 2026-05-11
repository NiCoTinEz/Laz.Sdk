namespace Laz.Sdk;

/// <summary>
/// Lazada Open Platform client exception.
/// </summary>
public sealed class LazException : Exception
{
    public string? ErrorCode { get; }
    public string? ErrorMsg { get; }

    public LazException() { }

    public LazException(string message) : base(message) { }

    public LazException(string message, Exception innerException) : base(message, innerException) { }

    public LazException(string errorCode, string errorMsg)
        : base($"{errorCode}:{errorMsg}")
    {
        ErrorCode = errorCode;
        ErrorMsg = errorMsg;
    }
}
