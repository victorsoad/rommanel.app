using FluentValidation;
using Rommanel.Application.Queries;

namespace Rommanel.Application.Validators
{
    public class GetCustomerQueryValidator : AbstractValidator<GetCustomerQuery>
    {
        public GetCustomerQueryValidator()
        {
            RuleFor(q => q.Id).NotEmpty().WithMessage("Id é obrigatório.");
        }
    }
}
