namespace ShopFinance.Application.Features.Products.Commands;

internal class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteProductCommandHandler> _logger;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteProductCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<Result> HandleAsync(DeleteProductCommand message, CancellationToken cancellationToken = default)
    {
        var productExists = await _unitOfWork.Products.AnyAsync(r => r.Id == message.ProductId);
        if (!productExists) return Result.NotFound("NO existe");

        try
        {
            await _unitOfWork.Products.DeleteAsync(message.ProductId);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Producto eliminado{message.ProductId}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"error al eliminar producto:{0}", ex.Message);
            return Result.Error($"Error al eliminar el producto: {message.ProductId}");
        }



        return Result.SuccessWithMessage("Producto Eliminado");
    }
}


