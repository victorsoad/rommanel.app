using Rommanel.Domain.Entities;

namespace Rommanel.Domain.Repositories
{
    public interface ICustomerRepository : IRepository<CustomerEntity>
    {
        Task<bool> ExistsByCpfCnpjAsync(string cpfCnpj, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<IEnumerable<CustomerEntity>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<int> CountAsync(CancellationToken cancellationToken);
    }
}
