using Laz.Sdk.Models.Reviews;
using Laz.Sdk.Util;

namespace Laz.Sdk.Services;

/// <summary>
/// Wraps the Product Review API endpoints (<c>/review/seller/*</c>).
/// Access via <see cref="ILazClient.Reviews"/>.
/// </summary>
public interface IReviewsService
{
    /// <summary>
    /// Get a paginated list of review IDs for a product. Calls <c>/review/seller/history/list</c>.
    /// </summary>
    /// <param name="itemId">Product item ID.</param>
    /// <param name="startTime">Start time (Unix milliseconds).</param>
    /// <param name="endTime">End time (Unix milliseconds).</param>
    /// <param name="current">Current page number.</param>
    /// <param name="orderId">Optional order ID filter.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<GetHistoryReviewIdListResponse> GetHistoryReviewIdListAsync(
        long itemId,
        long startTime,
        long endTime,
        int current,
        long? orderId = null,
        string accessToken = "",
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get full review details by review IDs. Calls <c>/review/seller/list/v2</c>.
    /// Maximum 10 IDs per call.
    /// </summary>
    /// <param name="idList">Review IDs (max 10).</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<GetReviewListByIdListResponse> GetReviewListByIdListAsync(
        long[] idList,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Submit a seller reply to a review. Calls <c>/review/seller/reply/add</c>.
    /// </summary>
    /// <param name="reviewId">Review ID to reply to.</param>
    /// <param name="content">Reply text content.</param>
    /// <param name="accessToken">Seller access token.</param>
    /// <param name="credentials">Optional per-call credential override.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<SubmitSellerReplyResponse> SubmitSellerReplyAsync(
        long reviewId,
        string content,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default);
}
