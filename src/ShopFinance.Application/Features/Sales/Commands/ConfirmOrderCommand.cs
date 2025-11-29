using Ardalis.Result;
using ShopFinance.Domain.Enums;

namespace ShopFinance.Application.Features.Sales.Commands;

public class ConfirmOrderCommand : ICommand<Result<Guid>> // Retorna SaleId
{
    public Guid OrderId { get; set; }
    public string? InvoiceNumber { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
}
