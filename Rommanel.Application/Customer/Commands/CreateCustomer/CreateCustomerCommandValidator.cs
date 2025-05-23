using Rommanel.Application.Commands;
using FluentValidation;

namespace Rommanel.Application.Validators
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Nome é obrigatório.");
            RuleFor(c => c.CpfCnpj).NotEmpty().WithMessage("CPF/CNPJ é obrigatório.");
            RuleFor(c => c.Email).NotEmpty().EmailAddress().WithMessage("Email valido é obrigatório.");
            RuleFor(c => c.Phone).NotEmpty().WithMessage("Telefone é obrigatório.");
            RuleFor(c => c.BirthDate).NotEmpty().WithMessage("Data de Nascimento é obrigatório.");

            // Se for pessoa física, a idade mínima deverá ser 18 anos.
            When(c => c.IsIndividual, () =>
            {
                RuleFor(c => c.BirthDate)
                    .Must(date => date <= DateTime.Today.AddYears(-18))
                    .WithMessage("O cliente deve ter pelo menos 18 anos de idade");
            });

            // Se for pessoa jurídica, é necessário que se cadastre o IE ou informe que é isento.
            When(c => !c.IsIndividual, () =>
            {
                RuleFor(c => c.StateRegistration)
                    .NotEmpty()
                    .Unless(c => c.IsStateRegistrationExempt)
                    .WithMessage("O IE é obrigatório, ao menos que seja isento.");
            });
                        
            RuleFor(c => c.ZipCode).NotEmpty().WithMessage("Cep é obrigatório.");
            RuleFor(c => c.Street).NotEmpty().WithMessage("Endereço é obrigatório.");
            RuleFor(c => c.Number).NotEmpty().WithMessage("Número é obrigatório.");
            RuleFor(c => c.Neighborhood).NotEmpty().WithMessage("Bairro é obrigatório.");
            RuleFor(c => c.City).NotEmpty().WithMessage("Cidade é obrigatório.");
            RuleFor(c => c.State).NotEmpty().WithMessage("Estado é obrigatório.");
        }
    }
}
