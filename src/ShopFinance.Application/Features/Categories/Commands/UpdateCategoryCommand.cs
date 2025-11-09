namespace ShopFinance.Application.Features.Categories.Commands;

public class UpdateCategoryCommand : ICommand<Result>
{
    public int CategoryId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
