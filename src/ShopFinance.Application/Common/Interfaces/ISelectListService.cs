using ShopFinance.Application.Common.Models;

namespace ShopFinance.Application.Common.Interfaces;

public interface ISelectListService
{
    Task<List<SelectListItemDto>> ProductState();
    Task<List<SelectListItemDto>> Categories();


}
