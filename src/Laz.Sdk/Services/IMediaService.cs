using Laz.Sdk.Models.Media;
using Laz.Sdk.Util;

namespace Laz.Sdk.Services;

/// <summary>
/// Wraps the Media Center / Video API endpoints (<c>/media/video/*</c>, <c>/image/*</c>).
/// Access via <see cref="ILazClient.Media"/>.
/// </summary>
public interface IMediaService
{
    /// <summary>
    /// Initialize a video upload session. Calls <c>/media/video/block/create</c>.
    /// </summary>
    /// <param name="fileName">Video file name.</param>
    /// <param name="fileBytes">Total file size in bytes.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<InitCreateVideoResponse> InitCreateVideoAsync(
        string fileName,
        long fileBytes,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Upload a video block. Calls <c>/media/video/block/upload</c>.
    /// </summary>
    /// <param name="uploadId">Upload ID from <see cref="InitCreateVideoAsync"/>.</param>
    /// <param name="blockNo">Current block number (0-based).</param>
    /// <param name="blockCount">Total number of blocks.</param>
    /// <param name="file">Video block file content.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<UploadVideoBlockResponse> UploadVideoBlockAsync(
        string uploadId,
        int blockNo,
        int blockCount,
        FileItem file,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Complete a video upload session. Calls <c>/media/video/block/commit</c>.
    /// </summary>
    /// <param name="uploadId">Upload ID from <see cref="InitCreateVideoAsync"/>.</param>
    /// <param name="parts">JSON string with partNumber/eTag array, e.g. <c>[{"partNumber":1,"eTag":"abc"}]</c>.</param>
    /// <param name="title">Video title.</param>
    /// <param name="coverUrl">Optional cover image URL.</param>
    /// <param name="videoUsage">Optional video usage type.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<CompleteCreateVideoResponse> CompleteCreateVideoAsync(
        string uploadId,
        string parts,
        string title,
        string? coverUrl = null,
        string? videoUsage = null,
        string accessToken = "",
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get video details. Calls <c>/media/video/get</c>.
    /// </summary>
    /// <param name="videoId">Video ID.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<GetVideoResponse> GetVideoAsync(
        long videoId,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove a video. Calls <c>/media/video/remove</c>.
    /// </summary>
    /// <param name="videoId">Video ID to remove.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<RemoveVideoResponse> RemoveVideoAsync(
        long videoId,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the seller's video quota. Calls <c>/media/video/quota/get</c>.
    /// </summary>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<GetVideoQuotaResponse> GetVideoQuotaAsync(
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Upload an image. Calls <c>/image/upload</c>.
    /// </summary>
    /// <param name="image">Image file to upload.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<UploadImageResponse> UploadImageAsync(
        FileItem image,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);
}
