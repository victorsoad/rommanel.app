using Rommanel.Application.Commands;
using Rommanel.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;
using System.Numerics;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace Rommanel.Application.Customer.Factories
{
    public static class CustomerFactory
    {
        public static CustomerEntity Create(CreateCustomerCommand request)
        {
            var AddressEntity = new AddressEntity(request.ZipCode, request.Street, request.Number, request.Neighborhood, request.City, request.State);

            return new CustomerEntity(request.Name, request.CpfCnpj, request.BirthDate, request.Phone, request.Email,
                request.IsIndividual, request.StateRegistration, request.IsStateRegistrationExempt, AddressEntity);
        }
    }
}
