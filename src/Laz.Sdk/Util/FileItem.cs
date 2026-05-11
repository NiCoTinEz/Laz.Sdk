namespace Laz.Sdk.Util;

/// <summary>
/// File payload for multipart uploads. Construct from a path, a <see cref="FileInfo"/>,
/// a byte array, or a <see cref="Stream"/>.
/// </summary>
public sealed class FileItem
{
    private readonly IFileContract _contract;

    public FileItem(FileInfo fileInfo) => _contract = new LocalContract(fileInfo);

    public FileItem(string filePath) : this(new FileInfo(filePath)) { }

    public FileItem(string fileName, byte[] content) : this(fileName, content, mimeType: null) { }

    public FileItem(string fileName, byte[] content, string? mimeType)
        => _contract = new ByteArrayContract(fileName, content, mimeType);

    public FileItem(string fileName, Stream stream) : this(fileName, stream, mimeType: null) { }

    public FileItem(string fileName, Stream stream, string? mimeType)
        => _contract = new StreamContract(fileName, stream, mimeType);

    public bool IsValid() => _contract.IsValid();
    public long GetFileLength() => _contract.GetFileLength();
    public string GetFileName() => _contract.GetFileName();
    public string GetMimeType() => _contract.GetMimeType();

    /// <summary>Open the underlying content as a <see cref="Stream"/>. Caller disposes.</summary>
    public Stream OpenRead() => _contract.OpenRead();

    private interface IFileContract
    {
        bool IsValid();
        string GetFileName();
        string GetMimeType();
        long GetFileLength();
        Stream OpenRead();
    }

    private sealed class LocalContract(FileInfo fileInfo) : IFileContract
    {
        bool IFileContract.IsValid() => fileInfo.Exists;
        long IFileContract.GetFileLength() => fileInfo.Length;
        string IFileContract.GetFileName() => fileInfo.Name;
        string IFileContract.GetMimeType() => Constants.CTYPE_DEFAULT;
        Stream IFileContract.OpenRead() => fileInfo.OpenRead();
    }

    private sealed class ByteArrayContract(string fileName, byte[] content, string? mimeType) : IFileContract
    {
        bool IFileContract.IsValid() => content is not null && !string.IsNullOrEmpty(fileName);
        long IFileContract.GetFileLength() => content.LongLength;
        string IFileContract.GetFileName() => fileName;
        string IFileContract.GetMimeType() => string.IsNullOrEmpty(mimeType) ? Constants.CTYPE_DEFAULT : mimeType;
        Stream IFileContract.OpenRead() => new MemoryStream(content, writable: false);
    }

    private sealed class StreamContract(string fileName, Stream stream, string? mimeType) : IFileContract
    {
        bool IFileContract.IsValid() => stream is not null && !string.IsNullOrEmpty(fileName);
        long IFileContract.GetFileLength() => stream.CanSeek ? stream.Length : 0L;
        string IFileContract.GetFileName() => fileName;
        string IFileContract.GetMimeType() => string.IsNullOrEmpty(mimeType) ? Constants.CTYPE_DEFAULT : mimeType;
        Stream IFileContract.OpenRead() => stream;
    }
}
