using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShopFinance.Application.Features.Customers.DTOs;
using ShopFinance.Application.Features.Customers.Queries;
using ShopFinance.Application.Features.Products.DTOs;
using ShopFinance.Application.Features.Products.Queries;

namespace ShopFinance.WebApp.Components.Shared.Autocompletes;

public class CustomerAutocomplete<T> : MudAutocomplete<CustomerSearchDto>
{
    [Inject]
    private IQueryMediator QueryMediator { get; set; } = default!;

    public CustomerAutocomplete()
    {
        SearchFunc = SearchFunc_;
        ToStringFunc = dto => $"{dto?.FirstName} {dto?.LastName}{(dto == null ? "" : " - ")}{dto?.Identifier}";
        Clearable = true;
        Dense = true;
        Variant = Variant.Outlined;
        Margin = Margin.Dense;
        Placeholder = "Buscar Cliente";
        ResetValueOnEmptyText = true;
    }



    private async Task<IEnumerable<CustomerSearchDto>> SearchFunc_(string? value, CancellationToken cancellation = default)
    {

        if (string.IsNullOrEmpty(value))
            return Enumerable.Empty<CustomerSearchDto>();

        var result = await QueryMediator.QueryAsync(new GetCustomerSearchQuery
        {
            SearchText = value,
        }, cancellation);

        return result.Value;
    }
}


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