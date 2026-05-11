namespace Laz.Sdk.Models.Orders;

/// <summary>Sort column on <c>/orders/get</c>.</summary>
public enum OrderSortBy
{
    /// <summary>Wire value: <c>"created_at"</c>.</summary>
    CreatedAt,

    /// <summary>Wire value: <c>"updated_at"</c>.</summary>
    UpdatedAt,
}

/// <summary>Sort direction.</summary>
public enum OrderSortDirection
{
    /// <summary>Ascending. Wire value: <c>"ASC"</c>.</summary>
    Asc,

    /// <summary>Descending. Wire value: <c>"DESC"</c>.</summary>
    Desc,
}
