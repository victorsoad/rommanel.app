namespace Rommanel.Domain.Entities
{
    public class AddressEntity
    {
        public string ZipCode { get; private set; } = string.Empty;
        public string Street { get; private set; } = string.Empty;
        public string Number { get; private set; } = string.Empty;
        public string Neighborhood { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string State { get; private set; } = string.Empty;

        public AddressEntity() { }

        public AddressEntity(string zipCode, string street, string number, string neighborhood, string city, string state)
        {
            ZipCode = zipCode;
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            City = city;
            State = state;
        }

        public void Update(string zipCode, string street, string number, string neighborhood, string city, string state)
        {
            ZipCode = zipCode;
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            City = city;
            State = state;
        }
    }
}
