using FluentValidation;
using MediatR;
using Rommanel.Application.Commands;
using Rommanel.Domain.Repositories;

namespace Rommanel.Application.Handlers
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IValidator<DeleteCustomerCommand> _validator;

        public DeleteCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IValidator<DeleteCustomerCommand> validator)
        {
            _customerRepository = customerRepository;
            _validator = validator;
        }

        public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);
            if (customer == null)
                throw new KeyNotFoundException("Cliente não encontrado.");

            await _customerRepository.DeleteAsync(customer, cancellationToken);
        }
    }

}
