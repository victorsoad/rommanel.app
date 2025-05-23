using Rommanel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Rommanel.Infrastructure.Persistence
{
    public class CustomerDbContext : DbContext
    {
        public DbSet<CustomerEntity> Customers => Set<CustomerEntity>();

        public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerEntity>()
                .HasIndex(c => c.CpfCnpj)
                .IsUnique();

            modelBuilder.Entity<CustomerEntity>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<CustomerEntity>()
                .OwnsOne(c => c.Address);
        }
    }
}
