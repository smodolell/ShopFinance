using Microsoft.AspNetCore.Http;

namespace ShopFinance.Application.Features.Categories.Commands;

public class CreateCategoryFromExcelCommand : ICommand<Result<CreateCategoryFromExcelCommandResult>>
{
    public IFormFile File { get; set; } = null!;

  
}
