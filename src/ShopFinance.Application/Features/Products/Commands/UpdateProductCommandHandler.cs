namespace ShopFinance.Application.Features.Products.Commands;

internal class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateProductCommand> _validator;
    private readonly ILogger<UpdateProductCommandHandler> _logger;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IValidator<UpdateProductCommand> validator, ILogger<UpdateProductCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }
    public async Task<Result> HandleAsync(UpdateProductCommand command, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
    
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Invalid(validationResult.AsErrors());
            }



            var product = await _unitOfWork.Products.GetByIdAsync(command.ProductId);
            if (product == null)
            {
                return Result.NotFound("no existe");
            }

            command.Adapt(product);

            await _unitOfWork.Products.UpdateAsync(product, cancellationToken);

            await _unitOfWork.CommitAsync();

            return Result.SuccessWithMessage("Producto actualizaod");

        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();

            _logger.LogError("Error :{0}", ex.Message);

            return Result.Error("Erro en la actualizacion de datos");
        }


    }
}

