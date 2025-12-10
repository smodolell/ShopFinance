using ShopFinance.Application.Services.InterestRates.DTOs;

namespace ShopFinance.Application.Services.InterestRates;

public interface IInterestRateService
{
    public Task<Result> Delete(int id);
    public Task<Result<InterestRateEditDto>> GetById(int id);
    public Task<Result<int>> Save(InterestRateEditDto model);
    public Task<Result> ChangeActive(int id, bool isActive);
    public Task<PagedResult<List<InterestRateListItemDto>>> GetPaginated(InterestRateFilterDto? filter);
}
