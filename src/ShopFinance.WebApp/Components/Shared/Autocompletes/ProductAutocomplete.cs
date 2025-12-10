using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShopFinance.Application.Features.Products.DTOs;
using ShopFinance.Application.Features.Products.Queries;

namespace ShopFinance.WebApp.Components.Shared.Autocompletes;


public class ProductAutocomplete<T> : MudAutocomplete<ProductSearchDto>
{
    [Inject]
    private IQueryMediator QueryMediator { get; set; } = default!;

    public ProductAutocomplete()
    {
        SearchFunc = SearchFunc_;
        ToStringFunc = dto => $"{dto?.CodeSku} {(dto == null ? "" : " - ")} {dto?.Name}";
        Clearable = true;
        Dense = true;
        Variant = Variant.Outlined;
        Margin = Margin.Dense;
        Label = "Buscar Producto";
        Placeholder = "Buscar Productos";
        ResetValueOnEmptyText = true;
    }



    private async Task<IEnumerable<ProductSearchDto>> SearchFunc_(string? value, CancellationToken cancellation = default)
    {

        if (string.IsNullOrEmpty(value))
            return Enumerable.Empty<ProductSearchDto>();

        var result = await QueryMediator.QueryAsync(new GetProductsSearchQuery
        {
            SearchText = value,
        }, cancellation);

        return result;
    }
}
