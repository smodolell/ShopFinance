using ShopFinance.Application.Services.TaxRates.DTOs;

namespace ShopFinance.Application.Services.TaxRates;

public interface ITaxRateService
{
    Task<Result> Delete(int id);
    Task<Result<TaxRateEditDto>> GetById(int id);
    Task<Result<int>> Save(TaxRateEditDto model);
    Task<Result> ChangeActive(int id, bool isActive);
    Task<PagedResult<List<TaxRateListItemDto>>> GetPaginated(TaxRateFilterDto? filter);
}
