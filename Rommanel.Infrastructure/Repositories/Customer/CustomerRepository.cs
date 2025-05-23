using Rommanel.Domain.Entities;
using Rommanel.Domain.Repositories;
using Rommanel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Rommanel.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<CustomerEntity>, ICustomerRepository
    {
        public CustomerRepository(CustomerDbContext context) : base(context) { }

        public async Task<bool> ExistsByCpfCnpjAsync(string cpfCnpj, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(c => c.CpfCnpj == cpfCnpj, cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(c => c.Email == email, cancellationToken);
        }

        public async Task<IEnumerable<CustomerEntity>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await _dbSet
                .OrderBy(c => c.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }
    }
}
