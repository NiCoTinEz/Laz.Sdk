using System.Globalization;
using System.Text;
using Laz.Sdk.Models.Reviews;
using Laz.Sdk.Util;

namespace Laz.Sdk.Services;

internal sealed class ReviewsService(ILazClient client) : IReviewsService
{
    private readonly ILazClient _client = client;

    public async Task<GetHistoryReviewIdListResponse> GetHistoryReviewIdListAsync(
        long itemId,
        long startTime,
        long endTime,
        int current,
        long? orderId = null,
        string accessToken = "",
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);

        var lazRequest = new LazRequest("/review/seller/history/list") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("item_id", itemId.ToString(CultureInfo.InvariantCulture));
        lazRequest.AddApiParameter("start_time", startTime.ToString(CultureInfo.InvariantCulture));
        lazRequest.AddApiParameter("end_time", endTime.ToString(CultureInfo.InvariantCulture));
        lazRequest.AddApiParameter("current", current.ToString(CultureInfo.InvariantCulture));
        if (orderId.HasValue)
            lazRequest.AddApiParameter("order_id", orderId.Value.ToString(CultureInfo.InvariantCulture));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetHistoryReviewIdListResponse>();
    }

    public async Task<GetReviewListByIdListResponse> GetReviewListByIdListAsync(
        long[] idList,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(idList);
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        if (idList.Length == 0)
            throw new ArgumentException("idList must contain at least one id.", nameof(idList));
        if (idList.Length > 10)
            throw new ArgumentException("idList cannot exceed 10 ids per call.", nameof(idList));

        var lazRequest = new LazRequest("/review/seller/list/v2") { HttpMethod = Constants.METHOD_GET };
        lazRequest.AddApiParameter("id_list", FormatIdList(idList));

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<GetReviewListByIdListResponse>();
    }

    public async Task<SubmitSellerReplyResponse> SubmitSellerReplyAsync(
        long reviewId,
        string content,
        string accessToken,
        LazCredentials? credentials = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(content);

        var lazRequest = new LazRequest("/review/seller/reply/add");
        lazRequest.AddApiParameter("review_id", reviewId.ToString(CultureInfo.InvariantCulture));
        lazRequest.AddApiParameter("content", content);

        var response = await _client.ExecuteAsync(lazRequest, accessToken, credentials: credentials, cancellationToken: cancellationToken).ConfigureAwait(false);
        return response.DeserializeOrThrow<SubmitSellerReplyResponse>();
    }

    private static string FormatIdList(long[] ids)
    {
        var sb = new StringBuilder(2 + ids.Length * 8);
        sb.Append('[');
        for (var i = 0; i < ids.Length; i++)
        {
            if (i > 0) sb.Append(',');
            sb.Append(ids[i].ToString(CultureInfo.InvariantCulture));
        }
        sb.Append(']');
        return sb.ToString();
    }
}
