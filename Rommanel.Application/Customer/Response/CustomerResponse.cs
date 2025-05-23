namespace Rommanel.Application.Responses
{
    public class CustomerResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CpfCnpj { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }


        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }


        public bool IsIndividual { get; set; }
        public string StateRegistration { get; set; }
        public bool IsStateRegistrationExempt { get; set; }
    }
}
