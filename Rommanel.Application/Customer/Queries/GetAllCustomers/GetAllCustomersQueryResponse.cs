using Rommanel.Application.Responses;

namespace Rommanel.Application.Responses
{
    public class GetAllCustomersQueryResponse
    {
        public List<CustomerResponse> Customers { get; set; } = new();

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
    }
}
