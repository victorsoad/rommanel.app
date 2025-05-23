using FluentValidation;
using MediatR;
using Rommanel.Application.Customer.Queries.GetCustomer;
using Rommanel.Domain.Repositories;

namespace Rommanel.Application.Queries
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, GetCustomerQueryResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IValidator<GetCustomerQuery> _validator;

        public GetCustomerQueryHandler(
            ICustomerRepository customerRepository,
            IValidator<GetCustomerQuery> validator)
        {
            _customerRepository = customerRepository;
            _validator = validator;
        }

        public async Task<GetCustomerQueryResponse> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);

            if (customer == null)
                throw new KeyNotFoundException("Cliente não encontrado.");

            return new GetCustomerQueryResponse
            {
                Id = customer.Id,
                Name = customer.Name,
                CpfCnpj = customer.CpfCnpj,
                BirthDate = customer.BirthDate,
                Phone = customer.Phone,
                Email = customer.Email,
                ZipCode = customer.Address.ZipCode,
                Street = customer.Address.Street,
                Number = customer.Address.Number,
                Neighborhood = customer.Address.Neighborhood,
                City = customer.Address.City,
                State = customer.Address.State,
                IsIndividual = customer.IsIndividual,
                StateRegistration = customer.StateRegistration,
                IsStateRegistrationExempt = customer.IsStateRegistrationExempt
            };
        }
    }

}

