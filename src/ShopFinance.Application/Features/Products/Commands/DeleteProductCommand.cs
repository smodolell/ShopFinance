namespace ShopFinance.Application.Features.Products.Commands;

public class DeleteProductCommand : ICommand<Result>
{
    public Guid ProductId { get; set; }
}


