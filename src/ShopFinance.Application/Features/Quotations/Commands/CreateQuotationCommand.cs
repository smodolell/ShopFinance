using ShopFinance.Domain.Entities;

namespace ShopFinance.Application.Features.Quotations.Commands;

public class CreateQuotationByOrderIdCommand :ICommand<Result<Guid>>
{
    public Guid OrderId { get; set; }
}

internal class CreateQuotationByOrderIdCommandHandler : ICommandHandler<CreateQuotationByOrderIdCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateQuotationByOrderIdCommandHandler> _logger;

    public CreateQuotationByOrderIdCommandHandler(IUnitOfWork unitOfWork,ILogger<CreateQuotationByOrderIdCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<Result<Guid>> HandleAsync(CreateQuotationByOrderIdCommand message, CancellationToken cancellationToken = default)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(message.OrderId);
        if(order is null)
        {
            _logger.LogWarning("Order with ID {OrderId} not found.", message.OrderId);
            return Result.NotFound("Order not found.");
        }
        var quotation = new Quotation
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            CreatedAt = DateTime.UtcNow,
            State = Domain.Enums.QuotationState.Draft
        };
        return Result.Success(quotation.Id);
    }
}

