using MediatR;
using Rommanel.Application.Responses;

namespace Rommanel.Application.Queries
{
    public record GetAllCustomersQuery(int PageNumber = 1, int PageSize = 10) : IRequest<GetAllCustomersQueryResponse>;
}
