namespace Application.Endpoints.Orders.GetOrdersPaged;

public static partial class GetOrdersPagedEndpoint
{
    public sealed class Request
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
