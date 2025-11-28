namespace ShopFinance.Application.Features.Categories.Commands;

public class CreateCategoryFromExcelCommandResult
{

    public int TotalRecords { get; set; }
    public int SuccessCount { get; set; }
    public int ErrorCount { get; set; }
    public List<string> Errors { get; set; } = new();
    public bool HasErrors => Errors.Any();
}
