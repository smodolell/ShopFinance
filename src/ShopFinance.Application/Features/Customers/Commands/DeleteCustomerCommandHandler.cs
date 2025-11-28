namespace ShopFinance.Application.Features.Customers.Commands;

internal class DeleteCustomerCommandHandler : ICommandHandler<DeleteCustomerCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteCustomerCommand> _logger;
    public DeleteCustomerCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteCustomerCommand> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<Result> HandleAsync(DeleteCustomerCommand command, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(command.CustomerId, cancellationToken);
            if (customer == null)
            {
                _logger.LogWarning("Intento de eliminar cliente inexistente - ID: {CustomerId}", command.CustomerId);
                return Result.Error("Cliente no encontrado");
            }
            await _unitOfWork.Customers.DeleteAsync(customer);
            await _unitOfWork.CommitAsync();
            _logger.LogInformation("Cliente eliminado exitosamente - ID: {CustomerId}", command.CustomerId);
            return Result.SuccessWithMessage("Cliente eliminado exitosamente");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error al eliminar el cliente - ID: {CustomerId}", command.CustomerId);
            return Result.Error($"Error al eliminar el cliente: {ex.Message}");
        }
    }
}