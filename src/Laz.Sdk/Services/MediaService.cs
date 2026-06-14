using System.Globalization;
using Laz.Sdk.Models.Media;
using Laz.Sdk.Util;

namespace Laz.Sdk.Services;

internal sealed class MediaService(ILazClient client) : IMediaService
{
    private readonly ILazClient _client = client;

    public async Task<InitCreateVideoResponse> InitCreateVideoAsync(
        string fileName,
        long fileBytes,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(fileName);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/media/video/block/create");
        lazRequest.AddApiParameter("file_name", fileName);
        lazRequest.AddApiParameter("file_bytes", fileBytes.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<InitCreateVideoResponse>();
    }

    public async Task<UploadVideoBlockResponse> UploadVideoBlockAsync(
        string uploadId,
        int blockNo,
        int blockCount,
        FileItem file,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(uploadId);
        ArgumentNullException.ThrowIfNull(file);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/media/video/block/upload");
        lazRequest.AddApiParameter("upload_id", uploadId);
        lazRequest.AddApiParameter("block_no", blockNo.ToString(CultureInfo.InvariantCulture));
        lazRequest.AddApiParameter("block_count", blockCount.ToString(CultureInfo.InvariantCulture));
        lazRequest.AddFileParameter("file", file);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<UploadVideoBlockResponse>();
    }

    public async Task<CompleteCreateVideoResponse> CompleteCreateVideoAsync(
        string uploadId,
        string parts,
        string title,
        string? coverUrl = null,
        string? videoUsage = null,
        string accessToken = "",
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(uploadId);
        ArgumentException.ThrowIfNullOrEmpty(parts);
        ArgumentException.ThrowIfNullOrEmpty(title);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/media/video/block/commit");
        lazRequest.AddApiParameter("upload_id", uploadId);
        lazRequest.AddApiParameter("parts", parts);
        lazRequest.AddApiParameter("title", title);
        if (!string.IsNullOrEmpty(coverUrl))
            lazRequest.AddApiParameter("cover_url", coverUrl);
        if (!string.IsNullOrEmpty(videoUsage))
            lazRequest.AddApiParameter("video_usage", videoUsage);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<CompleteCreateVideoResponse>();
    }

    public async Task<GetVideoResponse> GetVideoAsync(
        long videoId,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/media/video/get") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("video_id", videoId.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetVideoResponse>();
    }

    public async Task<RemoveVideoResponse> RemoveVideoAsync(
        long videoId,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/media/video/remove");
        lazRequest.AddApiParameter("video_id", videoId.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<RemoveVideoResponse>();
    }

    public async Task<GetVideoQuotaResponse> GetVideoQuotaAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/media/video/quota/get") { HttpMethod = Constants.METHOD_GET };

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetVideoQuotaResponse>();
    }

    public async Task<UploadImageResponse> UploadImageAsync(
        FileItem image,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(image);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/image/upload");
        lazRequest.AddFileParameter("image", image);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<UploadImageResponse>();
    }
}
