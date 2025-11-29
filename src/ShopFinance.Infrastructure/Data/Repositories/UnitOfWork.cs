using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using ShopFinance.Domain.Common.Interfaces;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Repositories;

namespace ShopFinance.Infrastructure.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public ICategoryRepository Categories { get; }
    public ICreditRepository Credits { get; }
    public ICustomerRepository Customers { get; }

    public IFrequencyRepository Frequencies { get; }
    public IRoleRepository Roles { get; }
    public IProductRepository Products { get; }

    public IUserRepository Users { get; }

    public ISettingRepository Settings { get; }

    public IOrderRepository Orders { get; }

    public ISaleRepository Sales { get; }

    public IOrderItemRepository OrderItems { get; }

    public IStockMovementRepository StockMovements { get; }

    public IWarehouseProductRepository WarehouseProducts { get; }

    public IStockTransferRepository StockTransfers { get; }

    public IWarehouseRepository Warehouses { get; }
    public UnitOfWork(ApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        Categories = new CategoryRepository(context);
        Products = new ProductRepository(context);
        Credits = new CreditRepository(context);
        Customers = new CustomerRepository(context);
        Frequencies = new FrequencyRepository(_context);
        Settings = new SettingRepository(_context);
        Roles = new RoleRepository(context,roleManager);
        Users = new UserRepository(context, userManager,signInManager);
        Orders = new OrderRepository(context);
        Sales = new SaleRepository(context);
        OrderItems = new OrderItemRepository(context);
        StockMovements = new StockMovementRepository(context);
        WarehouseProducts = new WarehouseProductRepository(context);
        StockTransfers = new StockTransferRepository(context);
        Warehouses = new WarehouseRepository(context);
    }

    public async Task BeginTransactionAsync()
    {
        if (_transaction == null)
            _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
                await _transaction.CommitAsync();
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
        }
        await DisposeTransactionAsync();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task DisposeTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context?.Dispose();
        GC.SuppressFinalize(this);
    }
}