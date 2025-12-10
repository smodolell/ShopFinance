using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShopFinance.Application.Features.Products.DTOs;
using ShopFinance.Application.Features.Products.Queries;

namespace ShopFinance.WebApp.Components.Shared.Autocompletes;

public class ProductOrderAutocomplete<T> : MudAutocomplete<ProductForSaleDto>
{
    [Inject]
    private IQueryMediator QueryMediator { get; set; } = default!;

    public ProductOrderAutocomplete()
    {
        SearchFunc = SearchFunc_;
        ToStringFunc = dto => $"{dto?.CodeSku} {(dto == null ? "" : " - ")} {dto?.Name}";
        Clearable = true;
        Dense = true;
        Variant = Variant.Outlined;
        Margin = Margin.Dense;
        Label="Buscar Producto";
        Placeholder = "Buscar Cliente";
        ResetValueOnEmptyText = true;
    }



    private async Task<IEnumerable<ProductForSaleDto>> SearchFunc_(string? value, CancellationToken cancellation = default)
    {

        if (string.IsNullOrEmpty(value))
            return Enumerable.Empty<ProductForSaleDto>();

        var result = await QueryMediator.QueryAsync(new GetProductsForSaleQuery
        {
            SearchText = value,
        }, cancellation);

        return result;
    }
}