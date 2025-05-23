using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;
using System.Reflection.Emit;

namespace Rommanel.Domain.Entities
{
    public class CustomerEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; } = string.Empty;
        public string CpfCnpj { get; private set; } = string.Empty;
        public DateTime BirthDate { get; private set; }
        public string Phone { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;

        public bool IsIndividual { get; private set; }
        public string? StateRegistration { get; private set; }
        public bool IsStateRegistrationExempt { get; private set; }
        public AddressEntity Address { get; private set; } = new AddressEntity();
        
        protected CustomerEntity() { }

        public CustomerEntity(string name, string cpfCnpj, DateTime birthDate, string phone, string email,
                        bool isIndividual, string? stateRegistration, bool isStateRegistrationExempt, AddressEntity address)
        {
            //Id = Guid.NewGuid();
            Name = name;
            CpfCnpj = cpfCnpj;
            BirthDate = birthDate;
            Phone = phone;
            Email = email;
            IsIndividual = isIndividual;
            StateRegistration = stateRegistration;
            IsStateRegistrationExempt = isStateRegistrationExempt;
            Address = address;
        }

        public void Update(string name, string cpfCnpj, DateTime birthDate, string phone, string email,
                        bool isIndividual, string? stateRegistration, bool isStateRegistrationExempt,
                        string zipCode, string street, string number, string neighborhood, string city, string state)
        {
            Name = name;
            CpfCnpj = cpfCnpj;
            BirthDate = birthDate;
            Phone = phone;
            Email = email;
            IsIndividual = isIndividual;
            StateRegistration = stateRegistration;
            IsStateRegistrationExempt = isStateRegistrationExempt;

            Address.Update(zipCode, street, number, neighborhood, city, state);
        }        
    }
}
