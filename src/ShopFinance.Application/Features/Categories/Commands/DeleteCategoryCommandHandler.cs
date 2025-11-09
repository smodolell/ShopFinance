namespace ShopFinance.Application.Features.Categories.Commands;

internal class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> HandleAsync(DeleteCategoryCommand message, CancellationToken cancellationToken = default)
    {
        var categoryExists = await _unitOfWork.Categories.AnyAsync(r => r.Id == message.CategoryId);
        if (!categoryExists)
        {
            return Result.NotFound("No existe");
        }

        await _unitOfWork.Categories.DeleteAsync(message.CategoryId);
        await _unitOfWork.SaveChangesAsync();

        return Result.SuccessWithMessage("Categoria eliminada");
    }
}