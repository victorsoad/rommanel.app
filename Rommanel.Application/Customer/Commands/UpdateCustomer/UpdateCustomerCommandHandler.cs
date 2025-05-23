using FluentValidation;
using MediatR;
using Rommanel.Application.Commands;
using Rommanel.Domain.Entities;
using Rommanel.Domain.Repositories;

namespace Rommanel.Application.Handlers
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IValidator<UpdateCustomerCommand> _validator;

        public UpdateCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IValidator<UpdateCustomerCommand> validator)
        {
            _customerRepository = customerRepository;
            _validator = validator;
        }

        public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            CustomerEntity customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);
            if (customer == null)
                throw new KeyNotFoundException("Cliente não encontrado.");

            // 1. Verifica duplicidade de CPF/CNPJ
            if (!string.Equals(customer.CpfCnpj, request.CpfCnpj, StringComparison.OrdinalIgnoreCase))
            {
                var existsCpfCnpj = await _customerRepository.ExistsByCpfCnpjAsync(request.CpfCnpj, cancellationToken);
                if (existsCpfCnpj)
                    throw new ApplicationException("Já existe um cliente com este CPF/CNPJ");
            }

            // 2. Verifica duplicidade de Email
            if (!string.Equals(customer.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existsEmail = await _customerRepository.ExistsByEmailAsync(request.Email, cancellationToken);
                if (existsEmail)
                    throw new ApplicationException("Já existe um cliente com este Email.");
            }
                        
            customer.Update(
                request.Name,
                request.CpfCnpj,
                request.BirthDate,
                request.Phone,
                request.Email,     
                request.IsIndividual,
                request.StateRegistration,
                request.IsStateRegistrationExempt,
                request.ZipCode,
                request.Street,
                request.Number,
                request.Neighborhood,
                request.City,
                request.State
            );

            await _customerRepository.UpdateAsync(customer, cancellationToken);

            
        }

 
    }

}
