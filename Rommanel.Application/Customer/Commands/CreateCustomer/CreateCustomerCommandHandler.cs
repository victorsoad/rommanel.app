using FluentValidation;
using MediatR;
using Rommanel.Application.Commands;
using Rommanel.Application.Customer.Factories;
using Rommanel.Domain.Repositories;

namespace CustomerApp.Application.Handlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IValidator<CreateCustomerCommand> _validator;

        public CreateCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IValidator<CreateCustomerCommand> validator)
        {
            _customerRepository = customerRepository;
            _validator = validator;
        }

        public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // 1. Verifica duplicidade de CPF/CNPJ
            var existsCpfCnpj = await _customerRepository.ExistsByCpfCnpjAsync(request.CpfCnpj, cancellationToken);
            if (existsCpfCnpj)
                throw new ApplicationException("Já existe um cliente com este CPF/CNPJ");

            // 2. Verifica duplicidade de Email
            var existsEmail = await _customerRepository.ExistsByEmailAsync(request.Email, cancellationToken);
            if (existsEmail)
                throw new ApplicationException("Já existe um cliente com este Email.");

            // Criação da entidade
            var cliente = CustomerFactory.Create(request);

            await _customerRepository.AddAsync(cliente, cancellationToken);

            return cliente.Id;
        }
    }
}
