namespace ShopFinance.Application.Features.Categories.Commands;

public class DeleteCategoryCommand : ICommand<Result>
{
    public int CategoryId { get; set; }
}
