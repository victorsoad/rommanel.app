using MediatR;

namespace Rommanel.Application.Commands
{
    public class DeleteCustomerCommand : IRequest
    {
        public Guid Id { get; }

        public DeleteCustomerCommand(Guid id)
        {
            Id = id;
        }
    }
}
