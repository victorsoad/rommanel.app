using MediatR;
using Rommanel.Application.Customer.Queries.GetCustomer;

namespace Rommanel.Application.Queries
{
    public class GetCustomerQuery : IRequest<GetCustomerQueryResponse>
    {
        public Guid Id { get; }

        public GetCustomerQuery(Guid id)
        {
            Id = id;
        }
    }
}
