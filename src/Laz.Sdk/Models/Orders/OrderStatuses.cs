namespace Laz.Sdk.Models.Orders;

/// <summary>
/// Well-known wire values for the <c>status</c> filter on <c>/orders/get</c>.
/// String-typed because Lazada's set grows over time; use these constants
/// for compile-time safety against typos.
/// </summary>
public static class OrderStatuses
{
    public const string Unpaid             = "unpaid";
    public const string Pending            = "pending";
    public const string Packed             = "packed";
    public const string Canceled           = "canceled";
    public const string ReadyToShip        = "ready_to_ship";
    public const string Delivered          = "delivered";
    public const string Returned           = "returned";
    public const string Shipped            = "shipped";
    public const string Failed             = "failed";
    public const string ToPack             = "topack";
    public const string ToShip             = "toship";
    public const string Shipping           = "shipping";
    public const string Lost               = "lost";
    public const string LostBy3pl          = "lost_by_3pl";
    public const string DamagedBy3pl       = "damaged_by_3pl";
    public const string FailedDelivery     = "failed_delivery";
    public const string ShippedBack        = "shipped_back";
    public const string ShippedBackSuccess = "shipped_back_success";
    public const string ShippedBackFailed  = "shipped_back_failed";
    public const string PackageScrapped    = "package_scrapped";
}
