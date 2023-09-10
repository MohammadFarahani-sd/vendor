using System.Data;
using System.Reflection.Emit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OFood.Shop.Domain.AggregatesModel.AreaCityAggregate;
using OFood.Shop.Domain.AggregatesModel.CustomerAggregate;
using OFood.Shop.Domain.SeedWork;
using OFood.Shop.Infrastructure.EntityConfigurations.Customers;

namespace OFood.Shop.Infrastructure.Persistence;

public class InfrastructureDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    private IDbContextTransaction _currentTransaction;

    public InfrastructureDbContext(DbContextOptions<InfrastructureDbContext> options) : base(options)
    {
    }

    protected InfrastructureDbContext(DbContextOptions options) : base(options)
    {
    }

    public InfrastructureDbContext(IMediator mediator, bool ctor) : base()
    {
        _mediator = mediator;
    }

    public bool HasActiveTransaction => _currentTransaction != null;


    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerAddress> CustomerAddresses { get; set; }
    public DbSet<AreaCity> AreaCities { get; set; }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEventsAsync(this);

        await base.SaveChangesAsync(cancellationToken);

        return true;
    }

    public IDbContextTransaction GetCurrentTransaction()
    {
        return _currentTransaction;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null)
            return null;

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    private void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction)
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            transaction.Commit();
        }
        catch (Exception ex)
        {
            RollbackTransaction();
            throw ex;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CustomerAddressEntityTypeConfiguration());
        builder.ApplyConfiguration(new CustomerEntityTypeConfiguration());

    }
}

internal static class MediatorExtension
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, InfrastructureDbContext ctx)
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());


        var tasks = domainEvents.Select(async x => { await mediator.Publish(x); });

        await Task.WhenAll(tasks);
    }
}