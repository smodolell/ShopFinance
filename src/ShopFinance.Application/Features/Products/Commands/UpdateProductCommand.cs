namespace ShopFinance.Application.Features.Products.Commands;

public class UpdateProductCommand : ICommand<Result>
{
    public Guid ProductId { get; set; }
    public int? CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int StockMin { get; set; }
}

