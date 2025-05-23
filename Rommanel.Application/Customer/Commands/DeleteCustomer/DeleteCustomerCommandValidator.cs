using FluentValidation;
using Rommanel.Application.Commands;

namespace Rommanel.Application.Validators
{
    public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("Id é obrigatório.");
        }
    }

}
