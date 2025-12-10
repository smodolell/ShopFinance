// IFrequencyService.cs
using ShopFinance.Application.Services.Frequencies.DTOs;

namespace ShopFinance.Application.Services.Frequencies;

public interface IFrequencyService
{
    Task<Result> Delete(int id);
    Task<Result<FrequencyEditDto>> GetById(int id);
    Task<Result<int>> Save(FrequencyEditDto model);
    Task<Result> ChangeActive(int id, bool isActive);
    Task<PagedResult<List<FrequencyListItemDto>>> GetPaginated(FrequencyFilterDto? filter);
}
