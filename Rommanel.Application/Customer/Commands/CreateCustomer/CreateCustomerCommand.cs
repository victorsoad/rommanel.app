using MediatR;

namespace Rommanel.Application.Commands
{
    public class CreateCustomerCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string CpfCnpj { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public bool IsIndividual { get; set; }
        public string? StateRegistration { get; set; }
        public bool IsStateRegistrationExempt { get; set; }

        public string ZipCode { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string Neighborhood { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}
