namespace ShopFinance.Application.Features.Categories.Commands;

public class CreateCategoryCommand : ICommand<Result<int>>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
