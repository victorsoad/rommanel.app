using MediatR;
using Rommanel.Domain.Repositories;
using Rommanel.Application.Responses;

namespace Rommanel.Application.Queries
{
    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, GetAllCustomersQueryResponse>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetAllCustomersQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<GetAllCustomersQueryResponse> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var totalRecords = await _customerRepository.CountAsync(cancellationToken);

            var customers = await _customerRepository.GetAllAsync(
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            var response = new GetAllCustomersQueryResponse
            {
                Customers = customers.Select(c => new CustomerResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    CpfCnpj = c.CpfCnpj,
                    BirthDate = c.BirthDate,
                    Phone = c.Phone,
                    Email = c.Email,
                    ZipCode = c.Address.ZipCode,
                    Street = c.Address.Street,
                    Number = c.Address.Number,
                    Neighborhood = c.Address.Neighborhood,
                    City = c.Address.City,
                    State = c.Address.State,
                    IsIndividual = c.IsIndividual,
                    StateRegistration = c.StateRegistration,
                    IsStateRegistrationExempt = c.IsStateRegistrationExempt
                }).ToList(),
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalRecords = totalRecords
            };

            return response;
        }
    }
}
