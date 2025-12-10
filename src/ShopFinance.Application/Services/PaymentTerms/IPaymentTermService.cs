// IPaymentTermService.cs
using ShopFinance.Application.Services.PaymentTerms.DTOs;

namespace ShopFinance.Application.Services.PaymentTerms;

public interface IPaymentTermService
{
    Task<Result> Delete(int id);
    Task<Result<PaymentTermEditDto>> GetById(int id);
    Task<Result<int>> Save(PaymentTermEditDto model);
    Task<Result> ChangeActive(int id, bool isActive);
    Task<PagedResult<List<PaymentTermListItemDto>>> GetPaginated(PaymentTermFilterDto? filter);
    Task<Result<List<PaymentTermListItemDto>>> GetActivePaymentTerms();
}
