namespace ShopFinance.Application.Features.Warehouses.Commands;

public class DeleteWarehouseCommand : ICommand<Result>
{
    public Guid Id { get; set; }
    
}